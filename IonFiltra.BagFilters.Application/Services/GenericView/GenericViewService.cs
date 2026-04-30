using IonFiltra.BagFilters.Application.Calculation_Rules;
using IonFiltra.BagFilters.Application.Interfaces.GenericView;
using IonFiltra.BagFilters.Core.Interfaces.GenericView;
using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.Services.GenericView
{
    public class GenericViewService : IGenericViewService
    {
        private readonly IGenericViewRepository _repository;

        public GenericViewService(IGenericViewRepository repository)
        {
            _repository = repository;
        }
        /// <summary> 
        /// Fetches GenericView by View Name. 
        /// </summary>
        /// <param name="View Name">TheView Name to search for.</param>
        /// <returns>Returns the Generic View details.</returns>
        public async Task<List<Dictionary<string, object>>> GetViewData(string viewName)
        {
            return await _repository.GetViewData(viewName);
        }
        /// <summary> 
        /// Fetches View Name by Project ID. 
        /// </summary>
        /// <param name="viewName">The View Name to search for.</param>
        /// <param name="parameters"></param>
        /// <returns>Returns the View Name details for the specified project.</returns>
        public async Task<List<Dictionary<string, object>>> GetViewDataWithParam(string viewName, Dictionary<string, object> parameters)
        {
            return await _repository.GetViewDataWithParam(viewName, parameters);
        }

        // ⭐ NEW METHODS ⭐
        //public Task<int> InsertAsync(string tableName, Dictionary<string, object> data)
        //    => _repository.InsertAsync(tableName, data);

        //public Task<int> UpdateAsync(string tableName, int id, Dictionary<string, object> data)
        //    => _repository.UpdateAsync(tableName, id, data);

        public async Task<int> InsertAsync(string tableName, Dictionary<string, object> data)
        {
            data = await ApplyCalculations(tableName, data);

            var id = await _repository.InsertAsync(tableName, data);

            if (tableName == "CageMaterialConfig" || tableName == "CageMiscellaneous")
            {
                await RecalculateStandardCage(id, data);
            }


            return id;
        }

        public async Task<int> UpdateAsync(string tableName, int id, Dictionary<string, object> data)
        {

            data = await ApplyCalculations(tableName, data);

            var result = await _repository.UpdateAsync(tableName, id, data);

            if (tableName == "CageMaterialConfig" || tableName == "CageMiscellaneousConfig")
            {
                await RecalculateStandardCage(id, data); // 👈 pass updated row
            }

            if (tableName == "PlatesMaterialConfig" || tableName == "StructuresMaterialConfig")
            {
                await ProcessCrossTableTriggers(tableName, data); // 👈 new generic trigger engine
            }

            return result;

        }

        public Task<int> DeleteAsync(string tableName, int id)
            => _repository.DeleteAsync(tableName, id);

        public Task ExecuteRawSqlAsync(string sql)
        {
            return _repository.ExecuteRawSqlAsync(sql);
        }




        //calculation methods

        private async Task<Dictionary<string, object>> BuildContext(
        Dictionary<string, object> data,
        List<Dependency> dependencies,
        Dictionary<string, List<Dictionary<string, object>>> cache)
        {
            var context = new Dictionary<string, object>();

            foreach (var dep in dependencies)
            {
                if (dep.Type == "field")
                {
                    context[dep.Name] = data.ContainsKey(dep.Name) && data[dep.Name] != null
                        ? Convert.ToDouble(data[dep.Name])
                        : 0;
                }

                else if (dep.Type == "external")
                {
                    List<Dictionary<string, object>> tableData;

                    if (cache != null && cache.ContainsKey(dep.Table))
                        tableData = cache[dep.Table];
                    else
                        tableData = await _repository.GetViewData(dep.Table);

                    var matchRow = tableData.FirstOrDefault(row =>
                        dep.Match.All(m =>
                            row[m.Key]?.ToString()
                                .Equals(m.Value, StringComparison.OrdinalIgnoreCase) == true
                        )
                    );

                    context[dep.Alias] = matchRow != null && matchRow[dep.Field] != null
                        ? Convert.ToDouble(matchRow[dep.Field])
                        : 0;
                }
            }

            return context;
        }


        private object EvaluateExpression(string expression, Dictionary<string, object> context)
        {
            var exp = new Expression(expression);

            foreach (var kv in context)
            {
                exp.Parameters[kv.Key] = kv.Value;
            }

            return Convert.ToDouble(exp.Evaluate());
        }

        private async Task<Dictionary<string, object>> ApplyCalculations(
        string tableName,
        Dictionary<string, object> data,
        Dictionary<string, List<Dictionary<string, object>>> cache = null)
        {
            if (!ConfigLoader.Configs.ContainsKey(tableName))
                return data;

            var config = ConfigLoader.Configs[tableName];

            if (config.calculations == null || !config.calculations.Any())
                return data;

            foreach (var calc in config.calculations)
            {
                var context = await BuildContext(data, calc.Dependencies, cache);

                var value = EvaluateExpression(calc.Expression, context);

                data[calc.Target] = value;
            }

            return data;
        }

        private async Task RecalculateStandardCage(int updatedId, Dictionary<string, object> updatedMaterial)
        {
           

            var cages = await _repository.GetViewData("StandardCageConfig");
           

            var materials = await _repository.GetViewData("CageMaterialConfig");

    

            for (int i = 0; i < materials.Count; i++)
            {
                if (Convert.ToInt32(materials[i]["id"]) == updatedId)
                {
                    materials[i] = new Dictionary<string, object>(materials[i]);

                    foreach (var kv in updatedMaterial)
                    {
                        materials[i][kv.Key] = kv.Value;
                    }
                    break;
                }
            }

            var cache = new Dictionary<string, List<Dictionary<string, object>>>
            {
                { "CageMaterialConfig", materials }
            };

            foreach (var cage in cages)
            {
       

                var updated = await ApplyCalculations("StandardCageConfig", cage, cache);

                var updatePayload = new Dictionary<string, object>();

                var config = ConfigLoader.Configs["StandardCageConfig"];

                foreach (var calc in config.calculations)
                {
                    updatePayload[calc.Target] = updated[calc.Target];
                }

                await _repository.UpdateAsync(
                    "StandardCageConfig",
                    Convert.ToInt32(cage["id"]),
                   updatePayload
                );
            }
        }




        // ─────────────────────────────────────────────────────────────────────
        // Cross-table cascade engine  (NEW — replaces RecalculateStandardCage)
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// After a row in <paramref name="sourceTable"/> is saved, checks whether
        /// the table's JSON config defines any <c>crossTableTriggers</c> and runs
        /// them generically — no table-specific code required.
        ///
        /// Two trigger patterns are supported:
        ///
        /// 1. <b>Formula cascade</b> (e.g. StandardCageConfig):
        ///    The trigger target has no SetField/ValueFrom — instead the engine
        ///    re-runs ApplyCalculations on every row of the target table using
        ///    the updated source row injected into the cache.
        ///
        /// 2. <b>Direct assignment</b> (e.g. BillOfMaterialRates):
        ///    The trigger target has SetField + ValueFrom + IncludeKeys / ExcludeKeys.
        ///    The engine writes newValue into SetField for the matching rows.
        /// </summary>
        private async Task ProcessCrossTableTriggers(
            string sourceTable,
            Dictionary<string, object> savedData)
        {
            if (!ConfigLoader.Configs.ContainsKey(sourceTable))
                return;

            var config = ConfigLoader.Configs[sourceTable];

            if (config.crossTableTriggers == null || !config.crossTableTriggers.Any())
                return;

            foreach (var trigger in config.crossTableTriggers)
            {
                // ── 1. Condition check (e.g. material == "IS2062") ───────────
                if (trigger.Condition != null)
                {
                    var actualValue = savedData.ContainsKey(trigger.Condition.Field)
                        ? savedData[trigger.Condition.Field]?.ToString()
                        : null;

                    if (!string.Equals(actualValue, trigger.Condition.Value,
                            StringComparison.OrdinalIgnoreCase))
                        continue;
                }

                // ── 2. TriggerFields guard (optional whitelist of changed fields) ─
                if (trigger.TriggerFields != null && trigger.TriggerFields.Any())
                {
                    bool anyPresent = trigger.TriggerFields.Any(f => savedData.ContainsKey(f));
                    if (!anyPresent) continue;
                }

                // ── 3. Process each target ────────────────────────────────────
                foreach (var target in trigger.Targets)
                {
                    if (!string.IsNullOrEmpty(target.SetField))
                    {
                        // Pattern 2 — Direct assignment with row filtering
                        await ApplyDirectAssignment(savedData, target);
                    }
                    else
                    {
                        // Pattern 1 — Formula cascade (StandardCage style)
                        await ApplyFormulaCascade(sourceTable, savedData, target);
                    }
                }
            }
        }

        // ── Pattern 2 helper ──────────────────────────────────────────────────

        /// <summary>
        /// Writes <c>savedData[target.ValueFrom]</c> into <c>target.SetField</c>
        /// for all rows in <c>target.Table</c> that match the IncludeKeys /
        /// ExcludeKeys filter on <c>target.FilterField</c>.
        ///
        /// Example: update BillOfMaterialRates.rate = 75
        ///          WHERE itemkey NOT IN ('structure','cage','scrap_holes')
        /// </summary>
        private async Task ApplyDirectAssignment(
            Dictionary<string, object> savedData,
            TriggerTarget target)
        {
            // Resolve the new value from the saved row
            if (!savedData.ContainsKey(target.ValueFrom))
                return;

            var newValue = savedData[target.ValueFrom];

            // Fetch all rows from the target table
            var allRows = await _repository.GetViewData(target.Table);

            foreach (var row in allRows)
            {
                // Apply IncludeKeys / ExcludeKeys filter
                if (!ShouldUpdateRow(row, target))
                    continue;

                var payload = new Dictionary<string, object>
                {
                    [target.SetField] = newValue
                };

                var rowId = Convert.ToInt32(row["id"]);
                await _repository.UpdateAsync(target.Table, rowId, payload);
            }
        }

        /// <summary>
        /// Returns true if the row should be updated based on the target's
        /// IncludeKeys / ExcludeKeys filter.
        /// </summary>
        private static bool ShouldUpdateRow(
            Dictionary<string, object> row,
            TriggerTarget target)
        {
            // No filter at all → update every row
            if (string.IsNullOrEmpty(target.FilterField))
                return true;

            var fieldValue = row.ContainsKey(target.FilterField)
                ? row[target.FilterField]?.ToString() ?? ""
                : "";

            // IncludeKeys has priority when both are somehow defined
            if (target.IncludeKeys != null && target.IncludeKeys.Any())
            {
                return target.IncludeKeys.Any(k =>
                    string.Equals(k, fieldValue, StringComparison.OrdinalIgnoreCase));
            }

            if (target.ExcludeKeys != null && target.ExcludeKeys.Any())
            {
                return !target.ExcludeKeys.Any(k =>
                    string.Equals(k, fieldValue, StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

        // ── Pattern 1 helper ──────────────────────────────────────────────────

        /// <summary>
        /// Re-runs ApplyCalculations on every row of the target table,
        /// injecting the updated source row into the cache so the formula
        /// picks up the latest values without an extra DB round-trip.
        ///
        /// This is the generic replacement for the old RecalculateStandardCage().
        /// </summary>
        private async Task ApplyFormulaCascade(
            string sourceTable,
            Dictionary<string, object> savedData,
            TriggerTarget target)
        {
            // Fetch all rows from the target table (e.g. StandardCageConfig)
            var targetRows = await _repository.GetViewData(target.Table);

            // Build a cache that includes the just-updated source row
            var sourceRows = await _repository.GetViewData(sourceTable);

            // Patch the in-memory source list so the cache reflects the new value
            var updatedSourceRows = sourceRows.Select(row =>
            {
                if (savedData.ContainsKey("id") &&
                    Convert.ToInt32(row["id"]) == Convert.ToInt32(savedData["id"]))
                {
                    var patched = new Dictionary<string, object>(row);
                    foreach (var kv in savedData)
                        patched[kv.Key] = kv.Value;
                    return patched;
                }
                return row;
            }).ToList();

            var cache = new Dictionary<string, List<Dictionary<string, object>>>
            {
                [sourceTable] = updatedSourceRows
            };

            // Also include any other cached tables already referenced by target's calculations
            // (target.Table's own config will resolve external deps via the cache)

            if (!ConfigLoader.Configs.ContainsKey(target.Table))
                return;

            var targetConfig = ConfigLoader.Configs[target.Table];

            if (targetConfig.calculations == null || !targetConfig.calculations.Any())
                return;

            foreach (var targetRow in targetRows)
            {
                var updated = await ApplyCalculations(target.Table, targetRow, cache);

                // Build an update payload containing only the recalculated fields
                var updatePayload = new Dictionary<string, object>();
                foreach (var calc in targetConfig.calculations)
                    updatePayload[calc.Target] = updated[calc.Target];

                await _repository.UpdateAsync(
                    target.Table,
                    Convert.ToInt32(targetRow["id"]),
                    updatePayload);
            }
        }
    }
}

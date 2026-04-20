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

    }
}

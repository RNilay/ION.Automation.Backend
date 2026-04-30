using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.Calculation_Rules
{
    public class CalculationConfig
    {
        public string Target { get; set; }
        public string Expression { get; set; }
        public List<Dependency> Dependencies { get; set; }
    }

    public class Dependency
    {
        public string Type { get; set; } // "field" or "external"

        // for field
        public string Name { get; set; }

        // for external
        public string Table { get; set; }
        public Dictionary<string, string> Match { get; set; }
        public string Field { get; set; }
        public string Alias { get; set; }
    }




    // ─── NEW: Cross-table trigger models ─────────────────────────────────────

    /// <summary>
    /// Root trigger block defined on a SOURCE table's JSON.
    /// When an update to the source table satisfies <see cref="Condition"/>
    /// (and optionally touches one of <see cref="TriggerFields"/>),
    /// the engine runs each entry in <see cref="Targets"/>.
    /// </summary>
    public class CrossTableTrigger
    {
        /// <summary>
        /// Optional. Only fire this trigger when the saved row has
        /// field == value (e.g. material == "IS2062").
        /// </summary>
        public TriggerCondition Condition { get; set; }

        /// <summary>
        /// Optional whitelist. If provided, the trigger only fires when
        /// at least one of these fields is present in the saved payload.
        /// Leave null to always fire (when Condition is met).
        /// </summary>
        public List<string> TriggerFields { get; set; }

        /// <summary>One or more target-table update rules.</summary>
        public List<TriggerTarget> Targets { get; set; }
    }

    /// <summary>
    /// A simple field == value equality check on the saved row.
    /// Comparison is case-insensitive.
    /// </summary>
    public class TriggerCondition
    {
        public string Field { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Describes which rows in which table to update, and what to set.
    /// </summary>
    public class TriggerTarget
    {
        /// <summary>The DB table / view name to update (e.g. "BillOfMaterialRates").</summary>
        public string Table { get; set; }

        /// <summary>The column in the target table that receives the new value.</summary>
        public string SetField { get; set; }

        /// <summary>
        /// The field name from the saved source-row whose value will be written
        /// into <see cref="SetField"/>.  E.g. "rateperkg".
        /// </summary>
        public string ValueFrom { get; set; }

        /// <summary>The column in the target table used for row-level filtering.</summary>
        public string FilterField { get; set; }

        /// <summary>
        /// If provided, update ONLY rows where FilterField IN IncludeKeys.
        /// Mutually exclusive with ExcludeKeys.
        /// </summary>
        public List<string> IncludeKeys { get; set; }

        /// <summary>
        /// If provided, update ALL rows where FilterField NOT IN ExcludeKeys.
        /// Mutually exclusive with IncludeKeys.
        /// </summary>
        public List<string> ExcludeKeys { get; set; }
    }






    public class TableConfig
    {
        public string lookup_name { get; set; }

        public List<CalculationConfig> calculations { get; set; }

        /// <summary>
        /// Cross-table cascade rules — applied AFTER a row in THIS table is successfully
        /// inserted/updated, to propagate changes into other tables.
        /// </summary>
        public List<CrossTableTrigger> crossTableTriggers { get; set; }
    }
}

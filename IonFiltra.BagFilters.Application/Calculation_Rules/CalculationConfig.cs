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

    public class TableConfig
    {
        public string lookup_name { get; set; }

        public List<CalculationConfig> calculations { get; set; }
    }
}

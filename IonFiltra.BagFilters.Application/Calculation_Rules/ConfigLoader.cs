using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IonFiltra.BagFilters.Application.Calculation_Rules
{
    public static class ConfigLoader
    {
        public static Dictionary<string, TableConfig> Configs { get; set; }

        static ConfigLoader()
        {
            Configs = new Dictionary<string, TableConfig>();

            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Configs");

            var files = Directory.GetFiles(basePath, "*.json");

            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var config = JsonConvert.DeserializeObject<TableConfig>(json);

                if (config != null && !string.IsNullOrEmpty(config.lookup_name))
                {
                    Configs[config.lookup_name] = config;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.Calculation_Rules
{
    public static class CalculationRules
    {
        public static Dictionary<string, List<CalculationRule>> Rules = new()
        {
            ["StandardCageConfig"] = new List<CalculationRule>
            {
                new CalculationRule
                {
                    Target = "rawmaterialcost",
                    Formula = async (data, repo, cache) =>
                    {
                        var weight = data.ContainsKey("weightkg") && data["weightkg"] != null
                            ? Convert.ToDouble(data["weightkg"])
                            : 0;

                        List<Dictionary<string, object>> materials;

                        // ✅ Use cache first
                        if (cache != null && cache.ContainsKey("CageMaterialConfig"))
                        {
                            materials = cache["CageMaterialConfig"];
                        }
                        else
                        {
                            materials = await repo.GetViewData("CageMaterialConfig");
                        }

                        var gi = materials.FirstOrDefault(x =>
                            x["material"]?.ToString()
                                .Equals("GI", StringComparison.OrdinalIgnoreCase) == true
                        );

                        var coilCost = gi != null && gi["coilcost"] != null
                            ? Convert.ToDouble(gi["coilcost"])
                            : 0;

                        return weight * coilCost;
                    }
                }
            }
        };
    }
}

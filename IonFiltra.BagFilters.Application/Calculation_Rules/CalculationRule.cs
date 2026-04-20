using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Interfaces.GenericView;

namespace IonFiltra.BagFilters.Application.Calculation_Rules
{
    public class CalculationRule
    {
        public string Target { get; set; }

        // Async is better (since repo calls DB)
        //public Func<Dictionary<string, object>, IGenericViewRepository, Task<object>> Formula { get; set; }
        public Func<
        Dictionary<string, object>,
        IGenericViewRepository,
        Dictionary<string, List<Dictionary<string, object>>>,
        Task<object>
        > Formula
            { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Interfaces.GenericView
{
    public interface IGenericViewRepository
    {
        Task<List<Dictionary<string, object>>> GetViewData(string viewName);

        Task<List<Dictionary<string, object>>> GetViewDataWithParam(string viewName, Dictionary<string, object> parameters);
    }
}

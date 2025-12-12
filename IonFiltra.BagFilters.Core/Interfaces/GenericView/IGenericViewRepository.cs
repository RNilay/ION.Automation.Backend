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

        Task<int> InsertAsync(string tableName, Dictionary<string, object> data);
        Task<int> UpdateAsync(string tableName, int id, Dictionary<string, object> data);
        Task<int> DeleteAsync(string tableName, int id);

        Task ExecuteRawSqlAsync(string sql);


    }
}

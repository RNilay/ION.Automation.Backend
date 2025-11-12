using IonFiltra.BagFilters.Application.Interfaces.GenericView;
using IonFiltra.BagFilters.Core.Interfaces.GenericView;
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.DuctEngineering;

namespace IonFiltra.BagFilters.Core.Interfaces.DuctEngineering
{
    public interface IEnquiryDuctEngineeringRepository
    {
        Task<int> SaveAsync(EnquiryDuctEngineering entity);
        Task<bool> UpdateAsync(int enquiryId, EnquiryDuctEngineering entity);
        Task<EnquiryDuctEngineering?> GetByEnquiryIdAsync(int enquiryId);
        Task<bool> ExistsByEnquiryIdAsync(int enquiryId);
    }
}

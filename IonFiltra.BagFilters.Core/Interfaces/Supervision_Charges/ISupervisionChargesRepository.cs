using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.Supervision_Charges;

namespace IonFiltra.BagFilters.Core.Interfaces.Supervision_Charges
{
    public interface ISupervisionChargesRepository
    {
        Task<int> SaveAsync(EnquirySupervisionCharges entity);
        Task<bool> UpdateAsync(int enquiryId, EnquirySupervisionCharges entity);
        Task<EnquirySupervisionCharges?> GetByEnquiryIdAsync(int enquiryId);
        Task<bool> ExistsByEnquiryIdAsync(int enquiryId);
    }
}

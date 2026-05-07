using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.Transportation_Settings;

namespace IonFiltra.BagFilters.Core.Interfaces.Transportation_Settings
{
    public interface ITransportationSettingsRepository
    {
        Task<int> SaveAsync(EnquiryTransportationSettings entity);
        Task<bool> UpdateAsync(int enquiryId, EnquiryTransportationSettings entity);
        Task<EnquiryTransportationSettings?> GetByEnquiryIdAsync(int enquiryId);
        Task<bool> ExistsByEnquiryIdAsync(int enquiryId);
    }
}

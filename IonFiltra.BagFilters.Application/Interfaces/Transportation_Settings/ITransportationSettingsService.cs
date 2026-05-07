using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.Transportation_Settings;

namespace IonFiltra.BagFilters.Application.Interfaces.Transportation_Settings
{
    public interface ITransportationSettingsService
    {
        Task<int> SaveAsync(SaveTransportationSettingsRequestDto dto);
        Task<bool> UpdateAsync(SaveTransportationSettingsRequestDto dto);
        Task<TransportationSettingsResponseDto?> GetByEnquiryIdAsync(int enquiryId);
        Task<bool> ExistsByEnquiryIdAsync(int enquiryId);
    }
}

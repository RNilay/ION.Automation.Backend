using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.Supervision_Charges;

namespace IonFiltra.BagFilters.Application.Interfaces.Supervision_Charges
{
    public interface ISupervisionChargesService
    {
        Task<int> SaveAsync(SaveSupervisionChargesRequestDto dto);
        Task<bool> UpdateAsync(SaveSupervisionChargesRequestDto dto);
        Task<SupervisionChargesResponseDto?> GetByEnquiryIdAsync(int enquiryId);
        Task<bool> ExistsByEnquiryIdAsync(int enquiryId);
    }
}

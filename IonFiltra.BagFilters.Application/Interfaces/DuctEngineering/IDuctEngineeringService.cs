using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.DuctEngineering;

namespace IonFiltra.BagFilters.Application.Interfaces.DuctEngineering
{
    public interface IDuctEngineeringService
    {
        Task<int> SaveAsync(SaveDuctEngineeringRequestDto dto);
        Task<bool> UpdateAsync(SaveDuctEngineeringRequestDto dto);
        Task<DuctEngineeringResponseDto?> GetByEnquiryIdAsync(int enquiryId);
        Task<bool> ExistsByEnquiryIdAsync(int enquiryId);
    }
}

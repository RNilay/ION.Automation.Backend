using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.Enquiry;

namespace IonFiltra.BagFilters.Application.Interfaces.Enquiry
{
    public interface IEnquiryService
    {
        Task<List<EnquiryMainDto>> GetByUserId(int userId);
        Task<(List<EnquiryMainDto> Items, int TotalCount)> GetByUserId(int userId, int pageNumber, int pageSize);
        Task<int> AddAsync(EnquiryMainDto dto);
        Task UpdateAsync(EnquiryMainDto dto);


        Task<bool> UpdateByEnquiryIdAsync(EnquiryMainDto dto);
    }
}

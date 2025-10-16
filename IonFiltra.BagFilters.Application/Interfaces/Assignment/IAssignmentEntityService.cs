using IonFiltra.BagFilters.Application.DTOs.Assignment;
using IonFiltra.BagFilters.Core.Entities.Assignment;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IAssignmentEntityService
    {
        Task<List<AssignmentMainDto>> GetByUserId(int userId);
        Task<(List<AssignmentMainDto> Items, int TotalCount)> GetByUserId(int userId, int pageNumber, int pageSize);
        Task<(List<AssignmentMainDto> Items, int TotalCount)> GetByEnquiryId(string enquiryId, int pageNumber, int pageSize);
        Task<List<AssignmentMainDto>> AddAsync(AssignmentRequest request);
        Task UpdateAsync(AssignmentMainDto dto);
    }
}
    
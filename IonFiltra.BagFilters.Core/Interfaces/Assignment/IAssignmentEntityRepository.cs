using IonFiltra.BagFilters.Core.Entities.Assignment;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Assignment
{
    public interface IAssignmentEntityRepository
    {
        Task<List<AssignmentEntity>> GetByUserId(int userId);
        Task<(List<AssignmentEntity> Items, int TotalCount)> GetByUserId(int userId, int pageNumber, int pageSize);
        Task<(List<AssignmentEntity> Items, int TotalCount)> GetByEnquiryId(string enquiryId, int pageNumber, int pageSize);
        Task<List<AssignmentEntity>> AddRangeAsync(List<AssignmentEntity> entities);
        Task UpdateAsync(AssignmentEntity entity);
    }
}
    
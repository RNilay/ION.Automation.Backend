using IonFiltra.BagFilters.Core.Entities.EnquiryEntity;

namespace IonFiltra.BagFilters.Core.Interfaces.EnquiryRep
{
    public interface IEnquiryRepository
    {
        Task<List<Enquiry>> GetByUserId(int userId);
        Task<(List<Enquiry> Items, int TotalCount)> GetByUserId(int userId, int pageNumber, int pageSize);
        Task<int> AddAsync(Enquiry entity);
        Task UpdateAsync(Enquiry entity);
    }
}
    
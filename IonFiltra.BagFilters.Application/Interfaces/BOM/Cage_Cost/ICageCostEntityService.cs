using IonFiltra.BagFilters.Application.DTOs.BOM.Cage_Cost;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface ICageCostEntityService
    {
        Task<CageCostMainDto> GetById(int id);
        Task<List<CageCostMainDto>> GetByEnquiryId(int enquiryId);
        Task<int> AddAsync(CageCostMainDto dto);
        Task UpdateAsync(CageCostMainDto dto);
    }
}
    
using IonFiltra.BagFilters.Application.DTOs.BOM.PaintingRates;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IPaintingCostConfigService
    {
        Task<PaintingCostConfigMainDto> GetById(int id);
        Task<IEnumerable<PaintingCostConfigMainDto>> GetAll();
        Task<int> AddAsync(PaintingCostConfigMainDto dto);
        Task UpdateAsync(PaintingCostConfigMainDto dto);
    }
}
    
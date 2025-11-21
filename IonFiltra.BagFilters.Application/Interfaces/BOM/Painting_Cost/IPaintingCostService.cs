using IonFiltra.BagFilters.Application.DTOs.BOM.Painting_Cost;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IPaintingCostService
    {
        Task<PaintingCostMainDto> GetById(int id);
        Task<int> AddAsync(PaintingCostMainDto dto);
        Task UpdateAsync(PaintingCostMainDto dto);
    }
}
    
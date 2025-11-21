using IonFiltra.BagFilters.Core.Entities.BOM.Painting_Cost;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Painting_Cost
{
    public interface IPaintingCostRepository
    {
        Task<PaintingCost?> GetById(int id);
        Task<int> AddAsync(PaintingCost entity);
        Task UpdateAsync(PaintingCost entity);
    }
}
    
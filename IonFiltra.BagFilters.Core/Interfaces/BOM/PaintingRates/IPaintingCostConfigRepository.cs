using IonFiltra.BagFilters.Core.Entities.BOM.PaintingRates;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.PaintingRates
{
    public interface IPaintingCostConfigRepository
    {
        Task<PaintingCostConfig?> GetById(int id);
        Task<IEnumerable<PaintingCostConfig>> GetAll();
        Task<int> AddAsync(PaintingCostConfig entity);
        Task UpdateAsync(PaintingCostConfig entity);
    }
}
    
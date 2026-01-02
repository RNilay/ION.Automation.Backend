using IonFiltra.BagFilters.Core.Entities.BOM.Damper_Cost;


namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Damper_Cost
{
    public interface IDamperCostEntityRepository
    {
        Task<DamperCostEntity?> GetById(int id);
        Task<int> AddAsync(DamperCostEntity entity);
        Task UpdateAsync(DamperCostEntity entity);

        Task ReplaceForMastersAsync(
        Dictionary<int, List<DamperCostEntity>> data,
        CancellationToken ct);
    }
}
    
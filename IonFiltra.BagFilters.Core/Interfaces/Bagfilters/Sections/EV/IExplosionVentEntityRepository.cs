using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.EV;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.EV
{
    public interface IExplosionVentEntityRepository
    {
        Task<ExplosionVentEntity?> GetById(int id);
        Task<int> AddAsync(ExplosionVentEntity entity);
        Task UpdateAsync(ExplosionVentEntity entity);

        Task UpsertRangeAsync(
    IEnumerable<ExplosionVentEntity> entities,
    CancellationToken ct = default);

        Task<Dictionary<int, ExplosionVentEntity>> GetByMasterIdsAsync(
    IEnumerable<int> bagfilterMasterIds,
    CancellationToken ct = default);
    }
}
    
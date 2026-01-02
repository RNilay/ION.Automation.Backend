using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.DamperSize;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.DamperSize
{
    public interface IDamperSizeInputsRepository
    {
        Task<DamperSizeInputs?> GetById(int id);
        Task<int> AddAsync(DamperSizeInputs entity);
        Task UpdateAsync(DamperSizeInputs entity);

        Task<Dictionary<int, DamperSizeInputs>> GetByMasterIdsAsync(
    IEnumerable<int> bagfilterMasterIds,
    CancellationToken ct = default);

        Task UpsertRangeAsync(
    IEnumerable<DamperSizeInputs> entities,
    CancellationToken ct = default);
    }
}
    
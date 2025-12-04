using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Hopper_Trough;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Hopper_Trough
{
    public interface IHopperInputsRepository
    {
        Task<HopperInputs?> GetById(int id);
        Task<int> AddAsync(HopperInputs entity);
        Task UpdateAsync(HopperInputs entity);

        Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default);

        Task<Dictionary<int, HopperInputs>> GetByMasterIdsAsync(
        IEnumerable<int> bagfilterMasterIds,
        CancellationToken ct = default);

        Task UpsertRangeAsync(
            IEnumerable<HopperInputs> entities,
            CancellationToken ct = default);

    }
}
    
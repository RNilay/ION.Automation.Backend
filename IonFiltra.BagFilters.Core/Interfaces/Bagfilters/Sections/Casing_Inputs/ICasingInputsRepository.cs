using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Casing_Inputs;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Casing_Inputs
{
    public interface ICasingInputsRepository
    {
        Task<CasingInputs?> GetById(int id);
        Task<int> AddAsync(CasingInputs entity);
        Task UpdateAsync(CasingInputs entity);

        Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default);

        Task<Dictionary<int, CasingInputs>> GetByMasterIdsAsync(
        IEnumerable<int> bagfilterMasterIds,
        CancellationToken ct = default);

        Task UpsertRangeAsync(
            IEnumerable<CasingInputs> entities,
            CancellationToken ct = default);

    }
}
    
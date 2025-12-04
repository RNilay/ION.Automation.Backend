using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Weight_Summary;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Cage_Inputs
{
    public interface ICageInputsRepository
    {
        Task<CageInputs?> GetById(int id);
        Task<int> AddAsync(CageInputs entity);
        Task UpdateAsync(CageInputs entity);

        Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default);

        Task<Dictionary<int, CageInputs>> GetByMasterIdsAsync(
        IEnumerable<int> bagfilterMasterIds,
        CancellationToken ct = default);

        Task UpsertRangeAsync(
            IEnumerable<CageInputs> entities,
            CancellationToken ct = default);

    }
}
    
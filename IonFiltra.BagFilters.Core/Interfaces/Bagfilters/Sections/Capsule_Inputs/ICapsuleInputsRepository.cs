using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Capsule_Inputs;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Capsule_Inputs
{
    public interface ICapsuleInputsRepository
    {
        Task<CapsuleInputs?> GetById(int id);
        Task<int> AddAsync(CapsuleInputs entity);
        Task UpdateAsync(CapsuleInputs entity);
        Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default);

        Task<Dictionary<int, CapsuleInputs>> GetByMasterIdsAsync(
        IEnumerable<int> bagfilterMasterIds,
        CancellationToken ct = default);

        Task UpsertRangeAsync(
            IEnumerable<CapsuleInputs> entities,
            CancellationToken ct = default);

    }
}
    
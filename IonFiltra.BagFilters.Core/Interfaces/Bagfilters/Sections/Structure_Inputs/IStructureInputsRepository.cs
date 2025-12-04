using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Structure_Inputs;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Structure_Inputs
{
    public interface IStructureInputsRepository
    {
        Task<StructureInputs?> GetById(int id);
        Task<int> AddAsync(StructureInputs entity);
        Task UpdateAsync(StructureInputs entity);
        Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default);

        Task<Dictionary<int, StructureInputs>> GetByMasterIdsAsync(
        IEnumerable<int> bagfilterMasterIds,
        CancellationToken ct = default);

        Task UpsertRangeAsync(
            IEnumerable<StructureInputs> entities,
            CancellationToken ct = default);

    }
}
    
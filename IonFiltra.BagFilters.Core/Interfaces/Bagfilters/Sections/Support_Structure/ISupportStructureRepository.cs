using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Support_Structure;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Support_Structure
{
    public interface ISupportStructureRepository
    {
        Task<SupportStructure?> GetById(int id);
        Task<int> AddAsync(SupportStructure entity);
        Task UpdateAsync(SupportStructure entity);
        Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default);

        Task<Dictionary<int, SupportStructure>> GetByMasterIdsAsync(
    IEnumerable<int> bagfilterMasterIds,
    CancellationToken ct = default);

        Task UpsertRangeAsync(
            IEnumerable<SupportStructure> entities,
            CancellationToken ct = default);

    }
}
    
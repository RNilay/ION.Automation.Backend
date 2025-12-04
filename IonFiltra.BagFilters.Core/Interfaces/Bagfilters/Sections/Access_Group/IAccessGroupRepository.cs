using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Access_Group;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Access_Group
{
    public interface IAccessGroupRepository
    {
        Task<AccessGroup?> GetById(int id);
        Task<int> AddAsync(AccessGroup entity);
        Task UpdateAsync(AccessGroup entity);

        Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default);

        Task<Dictionary<int, AccessGroup>> GetByMasterIdsAsync(
    IEnumerable<int> bagfilterMasterIds,
    CancellationToken ct = default);

        Task UpsertRangeAsync(
            IEnumerable<AccessGroup> entities,
            CancellationToken ct = default);

    }
}
    
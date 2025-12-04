using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Bag_Selection;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Bag_Selection
{
    public interface IBagSelectionRepository
    {
        Task<BagSelection?> GetById(int id);
        Task<int> AddAsync(BagSelection entity);
        Task UpdateAsync(BagSelection entity);

        Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default);

        Task<Dictionary<int, BagSelection>> GetByMasterIdsAsync(
        IEnumerable<int> bagfilterMasterIds,
        CancellationToken ct = default);

        Task UpsertRangeAsync(
            IEnumerable<BagSelection> entities,
            CancellationToken ct = default);

    }
}
    
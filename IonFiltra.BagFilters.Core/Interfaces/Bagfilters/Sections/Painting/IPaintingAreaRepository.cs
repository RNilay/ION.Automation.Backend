using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Painting;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Painting
{
    public interface IPaintingAreaRepository
    {
        Task<PaintingArea?> GetById(int id);
        Task<int> AddAsync(PaintingArea entity);
        Task UpdateAsync(PaintingArea entity);

        Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default);

        Task<Dictionary<int, PaintingArea>> GetByMasterIdsAsync(
    IEnumerable<int> bagfilterMasterIds,
    CancellationToken ct = default);

        Task UpsertRangeAsync(
            IEnumerable<PaintingArea> entities,
            CancellationToken ct = default);

    }
}
    
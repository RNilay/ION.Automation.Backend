using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Roof_Door;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Roof_Door
{
    public interface IRoofDoorRepository
    {
        Task<RoofDoor?> GetById(int id);
        Task<int> AddAsync(RoofDoor entity);
        Task UpdateAsync(RoofDoor entity);
        Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default);

        Task<Dictionary<int, RoofDoor>> GetByMasterIdsAsync(
    IEnumerable<int> bagfilterMasterIds,
    CancellationToken ct = default);

        Task UpsertRangeAsync(
            IEnumerable<RoofDoor> entities,
            CancellationToken ct = default);

    }
}
    
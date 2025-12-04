using IonFiltra.BagFilters.Core.Entities.BOM.Bill_Of_Material;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Bill_Of_Material
{
    public interface IBillOfMaterialRepository
    {
        Task<BillOfMaterial?> GetById(int id);
        Task<int> AddAsync(BillOfMaterial entity);
        Task AddRangeAsync(IEnumerable<BillOfMaterial> entities);
        Task UpdateAsync(BillOfMaterial entity);

        Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default);

        Task DeleteByMasterAsync(int bagfilterMasterId, CancellationToken ct = default);

        Task ReplaceForMasterAsync(int bagfilterMasterId, List<BillOfMaterial> newEntities, CancellationToken ct = default);

        Task<Dictionary<int, List<BillOfMaterial>>> GetByMasterIdsAsync(
        IEnumerable<int> bagfilterMasterIds,
        CancellationToken ct = default);

        // Replace the full BOM set for each masterId with the given collection
        Task ReplaceForMastersAsync(
            Dictionary<int, List<BillOfMaterial>> newDataByMaster,
            CancellationToken ct = default);



    }
}
    
using IonFiltra.BagFilters.Core.Entities.BOM.Bill_Of_Material;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Bill_Of_Material
{
    public interface IBillOfMaterialRepository
    {
        Task<BillOfMaterial?> GetById(int id);
        Task<int> AddAsync(BillOfMaterial entity);
        Task AddRangeAsync(IEnumerable<BillOfMaterial> entities);
        Task UpdateAsync(BillOfMaterial entity);
    }
}
    
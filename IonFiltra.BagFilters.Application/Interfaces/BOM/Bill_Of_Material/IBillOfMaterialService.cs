using IonFiltra.BagFilters.Application.DTOs.BOM.Bill_Of_Material;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IBillOfMaterialService
    {
        Task<BillOfMaterialMainDto> GetById(int id);
        Task<int> AddAsync(BillOfMaterialMainDto dto);
        Task UpdateAsync(BillOfMaterialMainDto dto);
    }
}
    
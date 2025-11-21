using IonFiltra.BagFilters.Application.DTOs.BOM.Rates;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IBillOfMaterialRatesService
    {
        Task<BillOfMaterialRatesMainDto> GetById(int id);
        Task<IEnumerable<BillOfMaterialRatesMainDto>> GetAll();
        Task<int> AddAsync(BillOfMaterialRatesMainDto dto);
        Task UpdateAsync(BillOfMaterialRatesMainDto dto);
    }
}
    
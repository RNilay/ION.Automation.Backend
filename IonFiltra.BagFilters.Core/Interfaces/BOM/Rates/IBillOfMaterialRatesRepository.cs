using IonFiltra.BagFilters.Core.Entities.BOM.Rates;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Rates
{
    public interface IBillOfMaterialRatesRepository
    {
        Task<BillOfMaterialRates?> GetById(int id);
        Task<IEnumerable<BillOfMaterialRates>> GetAll();
        Task<int> AddAsync(BillOfMaterialRates entity);
        Task UpdateAsync(BillOfMaterialRates entity);
    }
}
    
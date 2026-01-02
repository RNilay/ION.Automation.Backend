using IonFiltra.BagFilters.Application.DTOs.BOM.Damper_Cost;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IDamperCostEntityService
    {
        Task<DamperCostMainDto> GetById(int id);
        Task<int> AddAsync(DamperCostMainDto dto);
        Task UpdateAsync(DamperCostMainDto dto);
    }
}
    
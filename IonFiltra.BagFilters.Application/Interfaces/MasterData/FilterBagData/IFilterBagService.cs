using IonFiltra.BagFilters.Application.DTOs.MasterData.FilterBagData;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IFilterBagService
    {
        Task<FilterBagMainDto> GetById(int id);
        Task<int> AddAsync(FilterBagMainDto dto);
        Task UpdateAsync(FilterBagMainDto dto);
    }
}
    
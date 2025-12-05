using IonFiltra.BagFilters.Core.Entities.MasterData.FilterBagData;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.FilterBagData
{
    public interface IFilterBagRepository
    {
        Task<FilterBag?> GetById(int id);
        Task<int> AddAsync(FilterBag entity);
        Task UpdateAsync(FilterBag entity);
    }
}
    
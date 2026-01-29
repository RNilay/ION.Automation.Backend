using IonFiltra.BagFilters.Application.DTOs.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Core.Entities.MasterData.BoughtOutItems;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IBoughtOutItemSelectionService
    {
        Task<BoughtOutItemSelectionMainDto> GetById(int id);
        Task<int> AddAsync(BoughtOutItemSelectionMainDto dto);
        Task UpdateAsync(BoughtOutItemSelectionMainDto dto);

        Task<List<BoughtOutItemSelection>> GetByEnquiryAsync(int enquiryId);

        Task<List<SecondaryBoughtOutItem>> GetSecByEnquiryAsync(int enquiryId);
    }
}
    
using IonFiltra.BagFilters.Core.Entities.MasterData.BoughtOutItems;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.BoughtOutItems
{
    public interface ISecondaryBoughtOutItemRepository
    {
    
        Task<List<SecondaryBoughtOutItem>> GetByEnquiryAsync(int enquiryId);
        Task<List<SecondaryBoughtOutItem>> GetByEnquiryAndBagfiltersAsync(
            int enquiryId,
            IEnumerable<int> bagfilterMasterIds);

        Task UpsertRangeAsync(IEnumerable<SecondaryBoughtOutItem> items);

    }
}
    
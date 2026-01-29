using IonFiltra.BagFilters.Core.Entities.MasterData.BoughtOutItems;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.BoughtOutItems
{
    public interface ISecondaryBoughtOutItemRepository
    {
        Task<SecondaryBoughtOutItem?> GetById(int id);
        Task<int> AddAsync(SecondaryBoughtOutItem entity);
        Task UpdateAsync(SecondaryBoughtOutItem entity);


        Task<List<SecondaryBoughtOutItem>> GetByEnquiryAsync(int enquiryId);

        Task<List<SecondaryBoughtOutItem>> GetByEnquiryAndMastersAsync(
            int enquiryId,
            IEnumerable<int> bagfilterMasterIds);

        Task UpsertRangeAsync(IEnumerable<SecondaryBoughtOutItem> entities);

        Task<Dictionary<(int BagfilterMasterId, int MasterDefinitionId),SecondaryBoughtOutItem>> 
            GetByMasterIdsAsync(IEnumerable<int> masterIds, CancellationToken ct);

    }
}
    
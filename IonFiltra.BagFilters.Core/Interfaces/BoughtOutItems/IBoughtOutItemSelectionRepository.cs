using IonFiltra.BagFilters.Core.Entities.MasterData.BoughtOutItems;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.BoughtOutItems
{
    public interface IBoughtOutItemSelectionRepository
    {
        Task<BoughtOutItemSelection?> GetById(int id);
        Task<int> AddAsync(BoughtOutItemSelection entity);
        Task UpdateAsync(BoughtOutItemSelection entity);


        Task<List<BoughtOutItemSelection>> GetByEnquiryAsync(int enquiryId);

        Task<List<BoughtOutItemSelection>> GetByEnquiryAndMastersAsync(
            int enquiryId,
            IEnumerable<int> bagfilterMasterIds);

        Task UpsertRangeAsync(IEnumerable<BoughtOutItemSelection> entities);

        Task<Dictionary<(int BagfilterMasterId, int MasterDefinitionId),BoughtOutItemSelection>> 
            GetByMasterIdsAsync(IEnumerable<int> masterIds, CancellationToken ct);

    }
}
    
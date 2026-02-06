using IonFiltra.BagFilters.Core.Entities.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuestPDF.Infrastructure;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.MasterData.BoughtOutItems
{
    public class SecondaryBoughtOutItemRepository : ISecondaryBoughtOutItemRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<SecondaryBoughtOutItemRepository> _logger;

        public SecondaryBoughtOutItemRepository(TransactionHelper transactionHelper, ILogger<SecondaryBoughtOutItemRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }


        //new code
        public async Task<List<SecondaryBoughtOutItem>> GetByEnquiryAsync(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async db =>
            {
                _logger.LogInformation(
                    "Fetching SecondaryBoughtOutItems for EnquiryId {EnquiryId}",
                    enquiryId);

                return await db.SecondaryBoughtOutItems
                    .AsNoTracking()
                    .Where(x => x.EnquiryId == enquiryId)
                    .ToListAsync();
            });
        }

        public async Task<List<SecondaryBoughtOutItem>> GetByEnquiryAndBagfiltersAsync(
        int enquiryId,
        IEnumerable<int> bagfilterMasterIds)
        {
            var ids = bagfilterMasterIds?.Distinct().ToList() ?? new();

            if (!ids.Any())
                return new List<SecondaryBoughtOutItem>();

            return await _transactionHelper.ExecuteAsync(async db =>
            {
                _logger.LogInformation(
                    "Fetching SecondaryBoughtOutItems for EnquiryId {EnquiryId}, Bagfilters {@Bagfilters}",
                    enquiryId,
                    ids);

                return await db.SecondaryBoughtOutItems
                    .AsNoTracking()
                    .Where(x =>
                        x.EnquiryId == enquiryId &&
                        ids.Contains(x.BagfilterMasterId))
                    .ToListAsync();
            });
        }

    
        public async Task UpsertRangeAsync(IEnumerable<SecondaryBoughtOutItem> items)
        {
            var list = items?.ToList() ?? new();
            if (!list.Any())
                return;

            await _transactionHelper.ExecuteAsync(async db =>
            {
                var enquiryId = list.Select(x => x.EnquiryId).Distinct().Single();
                var bagfilterIds = list.Select(x => x.BagfilterMasterId).Distinct().ToList();
                var masterKeys = list.Select(x => x.MasterKey).Distinct().ToList();

                _logger.LogInformation(
                    "Upserting SecondaryBoughtOutItems for EnquiryId {EnquiryId}",
                    enquiryId);

                var existing = await db.SecondaryBoughtOutItems
                    .Where(x =>
                        x.EnquiryId == enquiryId &&
                        bagfilterIds.Contains(x.BagfilterMasterId) &&
                        masterKeys.Contains(x.MasterKey))
                    .ToListAsync();

                var existingMap = existing.ToDictionary(
                    x => (x.BagfilterMasterId, x.MasterKey),
                    x => x);

                foreach (var incoming in list)
                {
                    var key = (incoming.BagfilterMasterId, incoming.MasterKey);

                    if (existingMap.TryGetValue(key, out var row))
                    {
                        // UPDATE
                        row.Make = incoming.Make;
                        row.Cost = incoming.Cost;
                        row.Qty = incoming.Qty;
                        row.Rate = incoming.Rate;
                        row.Unit = incoming.Unit;
                        row.UpdatedAt = DateTime.UtcNow;
                    }
                    else
                    {
                        // INSERT
                        incoming.CreatedAt = DateTime.UtcNow;
                        await db.SecondaryBoughtOutItems.AddAsync(incoming);
                    }
                }

                await db.SaveChangesAsync();
            });
        }

    }


}
    
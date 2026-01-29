using IonFiltra.BagFilters.Core.Entities.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        public async Task<SecondaryBoughtOutItem?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching SecondaryBoughtOutItem for Id {Id}", id);
                return await dbContext.SecondaryBoughtOutItems
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(SecondaryBoughtOutItem entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new SecondaryBoughtOutItem for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.SecondaryBoughtOutItems.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(SecondaryBoughtOutItem entity)
        {
            _logger.LogInformation("Updating SecondaryBoughtOutItem for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.SecondaryBoughtOutItems.FindAsync(entity.Id);
                if (existingEntity != null)
                {
                    var createdAt = existingEntity.CreatedAt;
                    dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                    existingEntity.UpdatedAt= DateTime.Now; // Assuming UpdatedDate exists
                    existingEntity.CreatedAt = createdAt;
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning("SecondaryBoughtOutItem with Id {Id} not found", entity.Id);
                }
            });
        }





        //new code
        public async Task<List<SecondaryBoughtOutItem>> GetByEnquiryAsync(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching SecondaryBoughtOutItem for EnquiryId {EnquiryId}", enquiryId);

                return await dbContext.SecondaryBoughtOutItems
                    .AsNoTracking()
                    .Where(x => x.EnquiryId == enquiryId)
                    .ToListAsync();
            });
        }

        public async Task<List<SecondaryBoughtOutItem>> GetByEnquiryAndMastersAsync(
            int enquiryId,
            IEnumerable<int> bagfilterMasterIds)
        {
            var masterIds = bagfilterMasterIds?.Distinct().ToList() ?? new List<int>();
            if (!masterIds.Any())
                return new List<SecondaryBoughtOutItem>();

            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Fetching SecondaryBoughtOutItem for EnquiryId {EnquiryId} and bagfilters {@Ids}",
                    enquiryId,
                    masterIds);

                return await dbContext.SecondaryBoughtOutItems
                    .AsNoTracking()
                    .Where(x => x.EnquiryId == enquiryId && masterIds.Contains(x.BagfilterMasterId))
                    .ToListAsync();
            });
        }


        public async Task UpsertRangeAsync(IEnumerable<SecondaryBoughtOutItem> entities)
        {
            var list = entities?.ToList() ?? new List<SecondaryBoughtOutItem>();
            if (!list.Any())
                return;

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var enquiryId = list.Select(x => x.EnquiryId).Distinct().Single();
                var bagfilterMasterIds = list.Select(x => x.BagfilterMasterId).Distinct().ToList();
                var masterDefIds = list.Select(x => x.MasterDefinitionId).Distinct().ToList();

                _logger.LogInformation(
                    "Upserting SecondaryBoughtOutItem for EnquiryId {EnquiryId}, bagfilters {@Bagfilters}",
                    enquiryId,
                    bagfilterMasterIds);

                var existing = await dbContext.SecondaryBoughtOutItems
                    .Where(x =>
                        x.EnquiryId == enquiryId &&
                        bagfilterMasterIds.Contains(x.BagfilterMasterId) &&
                        masterDefIds.Contains(x.MasterDefinitionId))
                    .ToListAsync();

                var existingByKey = existing.ToDictionary(
                    x => (x.BagfilterMasterId, x.MasterDefinitionId),
                    x => x);

                foreach (var incoming in list)
                {
                    var key = (incoming.BagfilterMasterId, incoming.MasterDefinitionId);

                    if (existingByKey.TryGetValue(key, out var existingRow))
                    {
                        // Update
                        existingRow.SelectedRowId = incoming.SelectedRowId;
                        existingRow.MasterKey = incoming.MasterKey;
                        existingRow.UpdatedAt = DateTime.UtcNow;
                    }
                    else
                    {
                        incoming.CreatedAt = DateTime.UtcNow;
                        await dbContext.SecondaryBoughtOutItems.AddAsync(incoming);
                    }
                }

                await dbContext.SaveChangesAsync();
            });
        }

        public async Task<Dictionary<(int, int), SecondaryBoughtOutItem>> GetByMasterIdsAsync(IEnumerable<int> masterIds, CancellationToken ct)
        {
            var ids = masterIds.Distinct().ToList();

            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var rows = await dbContext.SecondaryBoughtOutItems
                    .Where(x => ids.Contains(x.BagfilterMasterId))
                    .ToListAsync(ct);

                return rows.ToDictionary(
                    x => (x.BagfilterMasterId, x.MasterDefinitionId.Value),
                    x => x
                );
            });
        }

    }
}
    
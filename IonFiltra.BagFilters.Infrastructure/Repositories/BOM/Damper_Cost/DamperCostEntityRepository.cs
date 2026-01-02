using IonFiltra.BagFilters.Core.Entities.BOM.Damper_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Damper_Cost;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.BOM.Damper_Cost
{
    public class DamperCostEntityRepository : IDamperCostEntityRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<DamperCostEntityRepository> _logger;

        public DamperCostEntityRepository(TransactionHelper transactionHelper, ILogger<DamperCostEntityRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<DamperCostEntity?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching DamperCostEntity for Id {Id}", id);
                return await dbContext.DamperCostEntitys
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(DamperCostEntity entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new DamperCostEntity for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.DamperCostEntitys.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(DamperCostEntity entity)
        {
            _logger.LogInformation("Updating DamperCostEntity for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.DamperCostEntitys.FindAsync(entity.Id);
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
                    _logger.LogWarning("DamperCostEntity with Id {Id} not found", entity.Id);
                }
            });
        }

        public async Task ReplaceForMastersAsync(
    Dictionary<int, List<DamperCostEntity>> newDataByMaster,
    CancellationToken ct = default)
        {
            if (newDataByMaster == null || newDataByMaster.Count == 0)
                return;

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var masterIds = newDataByMaster.Keys
                    .Where(id => id > 0)
                    .Distinct()
                    .ToList();

                if (masterIds.Count == 0)
                    return;

                // Load existing Damper Cost rows
                var existing = await dbContext.DamperCostEntitys
                    .Where(d =>
                        d.BagfilterMasterId.HasValue &&
                        masterIds.Contains(d.BagfilterMasterId.Value))
                    .ToListAsync(ct);

                var existingByMaster = existing
                    .GroupBy(d => d.BagfilterMasterId!.Value)
                    .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var masterId in masterIds)
                {
                    // Remove old rows
                    if (existingByMaster.TryGetValue(masterId, out var oldRows) &&
                        oldRows.Count > 0)
                    {
                        dbContext.DamperCostEntitys.RemoveRange(oldRows);
                    }

                    // Add new rows
                    if (newDataByMaster.TryGetValue(masterId, out var newRows) &&
                        newRows != null &&
                        newRows.Count > 0)
                    {
                        foreach (var row in newRows)
                        {
                            row.Id = 0;
                            row.BagfilterMasterId = masterId;
                            // ⚠️ IMPORTANT: EnquiryId must be set before insert
                            // row.EnquiryId = enquiryId; (set in service OR here)
                            row.CreatedAt = DateTime.UtcNow;
                            row.UpdatedAt = null;
                        }

                        await dbContext.DamperCostEntitys.AddRangeAsync(newRows, ct);
                    }
                }

                await dbContext.SaveChangesAsync(ct);
            });
        }

    }
}
    
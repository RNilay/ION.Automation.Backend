using IonFiltra.BagFilters.Core.Entities.BOM.Transp_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Transp_Cost;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.BOM.Transp_Cost
{
    public class TransportationCostEntityRepository : ITransportationCostEntityRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<TransportationCostEntityRepository> _logger;

        public TransportationCostEntityRepository(TransactionHelper transactionHelper, ILogger<TransportationCostEntityRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<TransportationCostEntity?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching TransportationCostEntity for Id {Id}", id);
                return await dbContext.TransportationCostEntitys
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(TransportationCostEntity entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new TransportationCostEntity for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.TransportationCostEntitys.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(TransportationCostEntity entity)
        {
            _logger.LogInformation("Updating TransportationCostEntity for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.TransportationCostEntitys.FindAsync(entity.Id);
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
                    _logger.LogWarning("TransportationCostEntity with Id {Id} not found", entity.Id);
                }
            });
        }

        public async Task ReplaceForMastersAsync(
    Dictionary<int, List<TransportationCostEntity>> newDataByMaster,
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

                // Load all existing Transportation Cost rows for these masters
                var existing = await dbContext.TransportationCostEntitys
                    .Where(t => masterIds.Contains((int)t.BagfilterMasterId))
                    .ToListAsync(ct);

                var existingByMaster = existing
                    .GroupBy(t => t.BagfilterMasterId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var masterId in masterIds)
                {
                    // Remove old rows
                    if (existingByMaster.TryGetValue(masterId, out var oldRows) &&
                        oldRows.Count > 0)
                    {
                        dbContext.TransportationCostEntitys.RemoveRange(oldRows);
                    }

                    // Add new rows (if any)
                    if (newDataByMaster.TryGetValue(masterId, out var newRows) &&
                        newRows != null && newRows.Count > 0)
                    {
                        foreach (var row in newRows)
                        {
                            row.Id = 0;
                            row.BagfilterMasterId = masterId;
                     
                            row.CreatedAt = DateTime.UtcNow;
                            row.UpdatedAt = null;
                        }

                        await dbContext.TransportationCostEntitys
                            .AddRangeAsync(newRows, ct);
                    }
                }

                await dbContext.SaveChangesAsync(ct);
            });
        }



    }
}
    
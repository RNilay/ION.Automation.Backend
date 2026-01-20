using IonFiltra.BagFilters.Core.Entities.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.BOM.Bill_Of_Material
{
    public class BillOfMaterialRepository : IBillOfMaterialRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<BillOfMaterialRepository> _logger;

        public BillOfMaterialRepository(TransactionHelper transactionHelper, ILogger<BillOfMaterialRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<BillOfMaterial?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching BillOfMaterial for Id {Id}", id);
                return await dbContext.BillOfMaterials
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<List<BillOfMaterial>> GetByEnquiryIdAsync(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Fetching BillOfMaterial list for EnquiryId {EnquiryId}",
                    enquiryId);

                return await dbContext.BillOfMaterials
                    .AsNoTracking()
                    .Where(x => x.EnquiryId == enquiryId)
                    .OrderBy(x => x.CreatedAt) // primary
                    .ThenBy(x => x.Id)                         // secondary safety
                    .ToListAsync();
            });
        }


        public async Task<int> AddAsync(BillOfMaterial entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new BillOfMaterial for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.BillOfMaterials.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task AddRangeAsync(IEnumerable<BillOfMaterial> entities)
        {
            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                foreach (var e in entities)
                {
                    e.CreatedAt = DateTime.UtcNow;
                }

                await dbContext.BillOfMaterials.AddRangeAsync(entities);
                await dbContext.SaveChangesAsync();
            });
        }


        public async Task UpdateAsync(BillOfMaterial entity)
        {
            _logger.LogInformation("Updating BillOfMaterial for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.BillOfMaterials.FindAsync(entity.Id);
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
                    _logger.LogWarning("BillOfMaterial with Id {Id} not found", entity.Id);
                }
            });
        }

        public async Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                return await dbContext.BillOfMaterials
                    .Where(w => w.BagfilterMasterId == bagfilterMasterId)
                    .Select(w => (int?)w.Id)
                    .FirstOrDefaultAsync(ct);
            });
        }

        public async Task DeleteByMasterAsync(int bagfilterMasterId, CancellationToken ct = default)
        {
            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                // Bulk delete via EF - fetch then remove (small number of rows, acceptable)
                var existing = await dbContext.BillOfMaterials
                    .Where(b => b.BagfilterMasterId == bagfilterMasterId)
                    .ToListAsync(ct);

                if (existing.Any())
                {
                    dbContext.BillOfMaterials.RemoveRange(existing);
                    await dbContext.SaveChangesAsync(ct);
                }
            });
        }

        public async Task ReplaceForMasterAsync(int bagfilterMasterId, List<BillOfMaterial> newEntities, CancellationToken ct = default)
        {
            if (newEntities == null) newEntities = new List<BillOfMaterial>();

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                // Delete existing (single SQL when supported)

                // EF Core 7: ExecuteDeleteAsync
                // await dbContext.BillOfMaterials.Where(b => b.BagfilterMasterId == bagfilterMasterId).ExecuteDeleteAsync(ct);
                // Fallback: fetch + remove
                var existing = await dbContext.BillOfMaterials
                    .Where(b => b.BagfilterMasterId == bagfilterMasterId)
                    .ToListAsync(ct);

                if (existing.Any())
                    dbContext.BillOfMaterials.RemoveRange(existing);

                // Add new items
                if (newEntities.Any())
                {
                    // set CreatedAt just in case
                    var now = DateTime.UtcNow;
                    foreach (var e in newEntities) e.CreatedAt = now;

                    await dbContext.BillOfMaterials.AddRangeAsync(newEntities, ct);
                }

                await dbContext.SaveChangesAsync(ct);
            });
        }


        public async Task<Dictionary<int, List<BillOfMaterial>>> GetByMasterIdsAsync(
    IEnumerable<int> bagfilterMasterIds,
    CancellationToken ct = default)
        {
            var masterIds = bagfilterMasterIds?
                .Where(id => id > 0)
                .Distinct()
                .ToList() ?? new List<int>();

            if (masterIds.Count == 0)
                return new Dictionary<int, List<BillOfMaterial>>();

            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var items = await dbContext.BillOfMaterials
                    .Where(b => masterIds.Contains(b.BagfilterMasterId))
                    .ToListAsync(ct);

                return items
                    .GroupBy(b => b.BagfilterMasterId)
                    .ToDictionary(g => g.Key, g => g.ToList());
            });
        }

        public async Task ReplaceForMastersAsync(
    Dictionary<int, List<BillOfMaterial>> newDataByMaster,
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

                // Load all existing BOM rows for these masters
                var existing = await dbContext.BillOfMaterials
                    .Where(b => masterIds.Contains(b.BagfilterMasterId))
                    .ToListAsync(ct);

                var existingByMaster = existing
                    .GroupBy(b => b.BagfilterMasterId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var masterId in masterIds)
                {
                    // Remove old rows
                    if (existingByMaster.TryGetValue(masterId, out var oldRows) && oldRows.Count > 0)
                    {
                        dbContext.BillOfMaterials.RemoveRange(oldRows);
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

                        await dbContext.BillOfMaterials.AddRangeAsync(newRows, ct);
                    }
                }

                await dbContext.SaveChangesAsync(ct);
            });
        }




    }
}
    
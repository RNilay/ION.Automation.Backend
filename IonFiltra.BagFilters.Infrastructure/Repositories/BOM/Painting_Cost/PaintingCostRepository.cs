using IonFiltra.BagFilters.Core.Entities.BOM.Painting_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Painting_Cost;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.BOM.Painting_Cost
{
    public class PaintingCostRepository : IPaintingCostRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<PaintingCostRepository> _logger;

        public PaintingCostRepository(TransactionHelper transactionHelper, ILogger<PaintingCostRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<PaintingCost?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching PaintingCost for Id {Id}", id);
                return await dbContext.PaintingCosts
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<List<PaintingCost>> GetByEnquiryId(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching PaintingCosts for EnquiryId {EnquiryId}", enquiryId);

                return await dbContext.PaintingCosts
                    .AsNoTracking()
                    .Where(x => x.EnquiryId == enquiryId)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();
            });
        }


        public async Task<int> AddAsync(PaintingCost entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new PaintingCost for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.PaintingCosts.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(PaintingCost entity)
        {
            _logger.LogInformation("Updating PaintingCost for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.PaintingCosts.FindAsync(entity.Id);
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
                    _logger.LogWarning("PaintingCost with Id {Id} not found", entity.Id);
                }
            });
        }

        public async Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                return await dbContext.PaintingCosts
                    .Where(w => w.BagfilterMasterId == bagfilterMasterId)
                    .Select(w => (int?)w.Id)
                    .FirstOrDefaultAsync(ct);
            });
        }

        public async Task<Dictionary<int, PaintingCost>> GetByMasterIdsAsync(
    IEnumerable<int> bagfilterMasterIds,
    CancellationToken ct = default)
        {
            var masterIdList = bagfilterMasterIds?
                .Where(id => id > 0)
                .Distinct()
                .ToList() ?? new List<int>();

            if (masterIdList.Count == 0)
                return new Dictionary<int, PaintingCost>();

            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var items = await dbContext.PaintingCosts
                    .Where(p => masterIdList.Contains(p.BagfilterMasterId))
                    .ToListAsync(ct);

                // assuming 1:1 (one PaintingCost per BagfilterMaster)
                return items.ToDictionary(p => p.BagfilterMasterId, p => p);
            });
        }

        public async Task UpsertRangeAsync(
            IEnumerable<PaintingCost> entities,
            CancellationToken ct = default)
        {
            if (entities == null) return;

            var list = entities
                .Where(e => e != null && e.BagfilterMasterId > 0)
                .ToList();

            if (list.Count == 0) return;

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var masterIds = list
                    .Select(e => e.BagfilterMasterId)
                    .Distinct()
                    .ToList();

                var existing = await dbContext.PaintingCosts
                    .Where(p => masterIds.Contains(p.BagfilterMasterId))
                    .ToListAsync(ct);

                var existingByMasterId = existing.ToDictionary(p => p.BagfilterMasterId, p => p);

                foreach (var incoming in list)
                {
                    if (existingByMasterId.TryGetValue(incoming.BagfilterMasterId, out var existingEntity))
                    {
                        // UPDATE existing row
                        var createdAt = existingEntity.CreatedAt;

                        dbContext.Entry(existingEntity).CurrentValues.SetValues(incoming);

                        existingEntity.Id = existingEntity.Id;   // keep PK
                        existingEntity.CreatedAt = createdAt;    // preserve CreatedAt
                        existingEntity.UpdatedAt = DateTime.Now;
                    }
                    else
                    {
                        // INSERT new row
                        incoming.Id = 0;               // let DB assign
                        incoming.CreatedAt = DateTime.Now;
                        incoming.UpdatedAt = null;

                        await dbContext.PaintingCosts.AddAsync(incoming, ct);
                    }
                }

                await dbContext.SaveChangesAsync(ct);
            });
        }

    }
}
    
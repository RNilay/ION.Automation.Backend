using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Painting
{
    public class PaintingAreaRepository : IPaintingAreaRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<PaintingAreaRepository> _logger;

        public PaintingAreaRepository(TransactionHelper transactionHelper, ILogger<PaintingAreaRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<PaintingArea?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching PaintingArea for Id {Id}", id);
                return await dbContext.PaintingAreas
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(PaintingArea entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new PaintingArea for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.PaintingAreas.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(PaintingArea entity)
        {
            _logger.LogInformation("Updating PaintingArea for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.PaintingAreas.FindAsync(entity.Id);
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
                    _logger.LogWarning("PaintingArea with Id {Id} not found", entity.Id);
                }
            });
        }

        public async Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                return await dbContext.PaintingAreas
                    .Where(w => w.BagfilterMasterId == bagfilterMasterId)
                    .Select(w => (int?)w.Id)
                    .FirstOrDefaultAsync(ct);
            });
        }

        public async Task<Dictionary<int, PaintingArea>> GetByMasterIdsAsync(
    IEnumerable<int> bagfilterMasterIds,
    CancellationToken ct = default)
        {
            var masterIdList = bagfilterMasterIds?
                .Where(id => id > 0)
                .Distinct()
                .ToList() ?? new List<int>();

            if (masterIdList.Count == 0)
                return new Dictionary<int, PaintingArea>();

            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var items = await dbContext.PaintingAreas
                    .Where(p => masterIdList.Contains(p.BagfilterMasterId))
                    .ToListAsync(ct);

                // assuming 1:1 (one PaintingArea per BagfilterMaster)
                return items.ToDictionary(p => p.BagfilterMasterId, p => p);
            });
        }

        public async Task UpsertRangeAsync(
            IEnumerable<PaintingArea> entities,
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

                var existing = await dbContext.PaintingAreas
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

                        await dbContext.PaintingAreas.AddAsync(incoming, ct);
                    }
                }

                await dbContext.SaveChangesAsync(ct);
            });
        }

    }
}
    
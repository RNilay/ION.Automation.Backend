using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.EV;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.EV;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.EV
{
    public class ExplosionVentEntityRepository : IExplosionVentEntityRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<ExplosionVentEntityRepository> _logger;

        public ExplosionVentEntityRepository(TransactionHelper transactionHelper, ILogger<ExplosionVentEntityRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<ExplosionVentEntity?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching ExplosionVentEntity for Id {Id}", id);
                return await dbContext.ExplosionVentEntitys
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(ExplosionVentEntity entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new ExplosionVentEntity for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.ExplosionVentEntitys.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(ExplosionVentEntity entity)
        {
            _logger.LogInformation("Updating ExplosionVentEntity for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.ExplosionVentEntitys.FindAsync(entity.Id);
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
                    _logger.LogWarning("ExplosionVentEntity with Id {Id} not found", entity.Id);
                }
            });
        }

        public async Task<Dictionary<int, ExplosionVentEntity>> GetByMasterIdsAsync(
    IEnumerable<int> bagfilterMasterIds,
    CancellationToken ct = default)
        {
            var masterIdList = bagfilterMasterIds?
                .Where(id => id > 0)
                .Distinct()
                .ToList() ?? new List<int>();

            if (masterIdList.Count == 0)
                return new Dictionary<int, ExplosionVentEntity>();

            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var items = await dbContext.ExplosionVentEntitys
                    .Where(e => masterIdList.Contains(e.BagfilterMasterId))
                    .ToListAsync(ct);

                return items.ToDictionary(e => e.BagfilterMasterId, e => e);
            });
        }

        public async Task UpsertRangeAsync(
    IEnumerable<ExplosionVentEntity> entities,
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

                var existing = await dbContext.ExplosionVentEntitys
                    .Where(e => masterIds.Contains(e.BagfilterMasterId))
                    .ToListAsync(ct);

                var existingByMasterId =
                    existing.ToDictionary(e => e.BagfilterMasterId, e => e);

                foreach (var incoming in list)
                {
                    if (existingByMasterId.TryGetValue(incoming.BagfilterMasterId, out var existingEntity))
                    {
                        var createdAt = existingEntity.CreatedAt;

                        dbContext.Entry(existingEntity).CurrentValues.SetValues(incoming);

                        existingEntity.CreatedAt = createdAt;
                        existingEntity.UpdatedAt = DateTime.Now;
                    }
                    else
                    {
                        incoming.Id = 0;
                        incoming.CreatedAt = DateTime.Now;
                        incoming.UpdatedAt = null;

                        await dbContext.ExplosionVentEntitys.AddAsync(incoming, ct);
                    }
                }

                await dbContext.SaveChangesAsync(ct);
            });
        }


    }
}
    
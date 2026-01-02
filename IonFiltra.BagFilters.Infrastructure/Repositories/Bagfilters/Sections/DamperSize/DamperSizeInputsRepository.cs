using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.DamperSize;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.DamperSize;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.DamperSize
{
    public class DamperSizeInputsRepository : IDamperSizeInputsRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<DamperSizeInputsRepository> _logger;

        public DamperSizeInputsRepository(TransactionHelper transactionHelper, ILogger<DamperSizeInputsRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<DamperSizeInputs?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching DamperSizeInputs for Id {Id}", id);
                return await dbContext.DamperSizeInputss
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(DamperSizeInputs entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new DamperSizeInputs for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.DamperSizeInputss.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(DamperSizeInputs entity)
        {
            _logger.LogInformation("Updating DamperSizeInputs for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.DamperSizeInputss.FindAsync(entity.Id);
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
                    _logger.LogWarning("DamperSizeInputs with Id {Id} not found", entity.Id);
                }
            });
        }


        public async Task<Dictionary<int, DamperSizeInputs>> GetByMasterIdsAsync(
    IEnumerable<int> bagfilterMasterIds,
    CancellationToken ct = default)
        {
            var masterIdList = bagfilterMasterIds?
                .Where(id => id > 0)
                .Distinct()
                .ToList() ?? new List<int>();

            if (masterIdList.Count == 0)
                return new Dictionary<int, DamperSizeInputs>();

            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var items = await dbContext.DamperSizeInputss
                    .Where(d => masterIdList.Contains(d.BagfilterMasterId))
                    .ToListAsync(ct);

                return items.ToDictionary(d => d.BagfilterMasterId, d => d);
            });
        }

        public async Task UpsertRangeAsync(
    IEnumerable<DamperSizeInputs> entities,
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

                var existing = await dbContext.DamperSizeInputss
                    .Where(d => masterIds.Contains(d.BagfilterMasterId))
                    .ToListAsync(ct);

                var existingByMasterId =
                    existing.ToDictionary(d => d.BagfilterMasterId, d => d);

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

                        await dbContext.DamperSizeInputss.AddAsync(incoming, ct);
                    }
                }

                await dbContext.SaveChangesAsync(ct);
            });
        }


    }
}
    
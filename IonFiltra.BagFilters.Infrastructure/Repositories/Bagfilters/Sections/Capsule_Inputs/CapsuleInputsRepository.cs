using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Capsule_Inputs
{
    public class CapsuleInputsRepository : ICapsuleInputsRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<CapsuleInputsRepository> _logger;

        public CapsuleInputsRepository(TransactionHelper transactionHelper, ILogger<CapsuleInputsRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<CapsuleInputs?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching CapsuleInputs for Id {Id}", id);
                return await dbContext.CapsuleInputss
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(CapsuleInputs entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new CapsuleInputs for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.CapsuleInputss.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(CapsuleInputs entity)
        {
            _logger.LogInformation("Updating CapsuleInputs for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.CapsuleInputss.FindAsync(entity.Id);
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
                    _logger.LogWarning("CapsuleInputs with Id {Id} not found", entity.Id);
                }
            });
        }


        public async Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                return await dbContext.CapsuleInputss
                    .Where(w => w.BagfilterMasterId == bagfilterMasterId)
                    .Select(w => (int?)w.Id)
                    .FirstOrDefaultAsync(ct);
            });
        }


        public async Task<Dictionary<int, CapsuleInputs>> GetByMasterIdsAsync(
    IEnumerable<int> bagfilterMasterIds,
    CancellationToken ct = default)
        {
            var masterIdList = bagfilterMasterIds?
                .Where(id => id > 0)
                .Distinct()
                .ToList() ?? new List<int>();

            if (masterIdList.Count == 0)
                return new Dictionary<int, CapsuleInputs>();

            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var items = await dbContext.CapsuleInputss
                    .Where(c => masterIdList.Contains(c.BagfilterMasterId))
                    .ToListAsync(ct);

                // assuming 1:1 (one CapsuleInputs per BagfilterMaster)
                return items.ToDictionary(c => c.BagfilterMasterId, c => c);
            });
        }

        public async Task UpsertRangeAsync(
            IEnumerable<CapsuleInputs> entities,
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

                var existing = await dbContext.CapsuleInputss
                    .Where(c => masterIds.Contains(c.BagfilterMasterId))
                    .ToListAsync(ct);

                var existingByMasterId = existing.ToDictionary(c => c.BagfilterMasterId, c => c);

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

                        await dbContext.CapsuleInputss.AddAsync(incoming, ct);
                    }
                }

                await dbContext.SaveChangesAsync(ct);
            });
        }


    }
}
    
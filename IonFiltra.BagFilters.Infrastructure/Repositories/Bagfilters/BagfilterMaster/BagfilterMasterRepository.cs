using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterMasterEntity;
using IonFiltra.BagFilters.Core.Interfaces.Bagfilters.BagfilterMasters;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.BagfilterMasters
{
    public class BagfilterMasterRepository : IBagfilterMasterRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<BagfilterMasterRepository> _logger;

        public BagfilterMasterRepository(TransactionHelper transactionHelper, ILogger<BagfilterMasterRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<BagfilterMaster?> GetByProjectId(int assignmentId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching BagfilterMaster for AssignmentId {AssignmentId}", assignmentId);
                return await dbContext.BagfilterMasters
                    .AsNoTracking()
                    .Where(x => x.AssignmentId == assignmentId)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(BagfilterMaster entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new BagfilterMaster for AssignmentId {AssignmentId}", entity.AssignmentId);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.BagfilterMasters.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.BagfilterMasterId; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(BagfilterMaster entity)
        {
            _logger.LogInformation("Updating BagfilterMaster for AssignmentId {AssignmentId}", entity.AssignmentId);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.BagfilterMasters.FindAsync(entity.BagfilterMasterId);
                if (existingEntity != null)
                {
                    var createdAt = existingEntity.CreatedAt;
                    dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                    existingEntity.UpdatedAt = DateTime.Now; // Assuming UpdatedDate exists
                    existingEntity.CreatedAt = createdAt;
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning("BagfilterMaster with AssignmentId {AssignmentId} not found", entity.AssignmentId);
                }
            });
        }

        public async Task<List<int>> AddMastersAsync(IEnumerable<BagfilterMaster> masters, CancellationToken ct = default)
        {
            var list = (masters ?? Enumerable.Empty<BagfilterMaster>()).ToList();
            if (!list.Any()) return new List<int>();

            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                // Defensive: set CreatedAt and any defaults before adding
                var now = DateTime.UtcNow;
                foreach (var m in list)
                {
                    // if the caller pre-set Id > 0, you may want to ignore or throw — here we ensure it's treated as new
                    m.BagfilterMasterId = 0; // ensure EF treats as new entity (optional; remove if you rely on caller)
                    m.CreatedAt = m.CreatedAt == default ? now : m.CreatedAt;
                }

                // Add all masters in a single batch
                await dbContext.BagfilterMasters.AddRangeAsync(list, ct);

                // Save once — this will populate the identity PKs on the tracked master entities
                await dbContext.SaveChangesAsync(ct);

                // Collect the generated IDs in the same order as the input list
                var createdIds = list.Select(m => m.BagfilterMasterId).ToList();
                _logger.LogInformation("Inserted {Count} BagfilterMaster(s). FirstId={FirstId}", createdIds.Count, createdIds.FirstOrDefault());
                return createdIds;
            });
        }

    }
}

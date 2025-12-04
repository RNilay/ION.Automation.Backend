using System.Linq;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Process_Info
{
    public class ProcessInfoRepository : IProcessInfoRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<ProcessInfoRepository> _logger;

        public ProcessInfoRepository(TransactionHelper transactionHelper, ILogger<ProcessInfoRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<ProcessInfo?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching ProcessInfo for Id {Id}", id);
                return await dbContext.ProcessInfos
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(ProcessInfo entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new ProcessInfo for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.ProcessInfos.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(ProcessInfo entity)
        {
            _logger.LogInformation("Updating ProcessInfo for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.ProcessInfos.FindAsync(entity.Id);
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
                    _logger.LogWarning("ProcessInfo with Id {Id} not found", entity.Id);
                }
            });
        }


        public async Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                return await dbContext.ProcessInfos
                    .Where(w => w.BagfilterMasterId == bagfilterMasterId)
                    .Select(w => (int?)w.Id)
                    .FirstOrDefaultAsync(ct);
            });
        }



        public async Task<Dictionary<int, ProcessInfo>> GetByMasterIdsAsync(
        IEnumerable<int> bagfilterMasterIds,
        CancellationToken ct = default)
        {
            var masterIdList = bagfilterMasterIds?
                .Where(id => id > 0)
                .Distinct()
                .ToList() ?? new List<int>();

            if (masterIdList.Count == 0)
                return new Dictionary<int, ProcessInfo>();

            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var items = await dbContext.ProcessInfos
                    .Where(w => masterIdList.Contains(w.BagfilterMasterId))
                    .ToListAsync(ct);

                // assuming 1:1 (one WeightSummary per BagfilterMaster)
                return items.ToDictionary(w => w.BagfilterMasterId, w => w);
            });
        }

        public async Task UpsertRangeAsync(
        IEnumerable<ProcessInfo> entities,
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

                var existing = await dbContext.ProcessInfos
                    .Where(w => masterIds.Contains(w.BagfilterMasterId))
                    .ToListAsync(ct);

                var existingByMasterId = existing.ToDictionary(w => w.BagfilterMasterId, w => w);

                foreach (var incoming in list)
                {
                    if (existingByMasterId.TryGetValue(incoming.BagfilterMasterId, out var existingEntity))
                    {
                        // UPDATE existing row
                        var createdAt = existingEntity.CreatedAt;

                        dbContext.Entry(existingEntity).CurrentValues.SetValues(incoming);

                        existingEntity.Id = existingEntity.Id; // keep PK
                        existingEntity.CreatedAt = createdAt;         // preserve CreatedAt
                        existingEntity.UpdatedAt = DateTime.Now;
                    }
                    else
                    {
                        // INSERT new row
                        incoming.Id = 0;               // let DB assign
                        incoming.CreatedAt = DateTime.Now;
                        incoming.UpdatedAt = null;

                        await dbContext.ProcessInfos.AddAsync(incoming, ct);
                    }
                }

                await dbContext.SaveChangesAsync(ct);
            });
        }

    }
}
    
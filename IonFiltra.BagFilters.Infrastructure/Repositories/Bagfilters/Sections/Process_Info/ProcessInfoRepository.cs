using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Process_Info;
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
    }
}
    
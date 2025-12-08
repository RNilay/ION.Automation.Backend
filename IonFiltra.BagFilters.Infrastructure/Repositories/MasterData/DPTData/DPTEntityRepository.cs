using IonFiltra.BagFilters.Core.Entities.MasterData.DPTData;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.DPTData;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.MasterData.DPTData
{
    public class DPTEntityRepository : IDPTEntityRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<DPTEntityRepository> _logger;

        public DPTEntityRepository(TransactionHelper transactionHelper, ILogger<DPTEntityRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<DPTEntity?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching DPTEntity for Id {Id}", id);
                return await dbContext.DPTEntitys
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }
        
        public async Task<IEnumerable<DPTEntity>> GetAll()
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching all DPTEntity rows.");

                return await dbContext.DPTEntitys
                    .Where(x => !x.IsDeleted)
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .ToListAsync();
            });
        }

        public async Task<int> AddAsync(DPTEntity entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new DPTEntity for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.DPTEntitys.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(DPTEntity entity)
        {
            _logger.LogInformation("Updating DPTEntity for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.DPTEntitys.FindAsync(entity.Id);
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
                    _logger.LogWarning("DPTEntity with Id {Id} not found", entity.Id);
                }
            });
        }
        
        public async Task SoftDeleteAsync(int id)
        {
            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var entity = await dbContext.DPTEntitys.FindAsync(id);

                if (entity != null)
                {
                    entity.IsDeleted = true;
                    entity.UpdatedAt = DateTime.UtcNow;

                    await dbContext.SaveChangesAsync();
                }
            });
        }
    }
}
    
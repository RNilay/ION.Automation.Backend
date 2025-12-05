using IonFiltra.BagFilters.Core.Entities.MasterData.SolenoidValveData;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.SolenoidValveData;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.MasterData.SolenoidValveData
{
    public class SolenoidValveRepository : ISolenoidValveRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<SolenoidValveRepository> _logger;

        public SolenoidValveRepository(TransactionHelper transactionHelper, ILogger<SolenoidValveRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<SolenoidValve?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching SolenoidValve for Id {Id}", id);
                return await dbContext.SolenoidValves
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(SolenoidValve entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new SolenoidValve for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.SolenoidValves.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(SolenoidValve entity)
        {
            _logger.LogInformation("Updating SolenoidValve for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.SolenoidValves.FindAsync(entity.Id);
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
                    _logger.LogWarning("SolenoidValve with Id {Id} not found", entity.Id);
                }
            });
        }

        public async Task<IEnumerable<SolenoidValve>> GetAll()
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                return await dbContext.SolenoidValves
                    .Where(x => !x.IsDeleted)        // NEW
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .ToListAsync();
            });
        }

        public async Task SoftDeleteAsync(int id)
        {
            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var entity = await dbContext.SolenoidValves.FindAsync(id);

                if (entity != null)
                {
                    entity.IsDeleted = true;               // MARK AS DELETED
                    entity.UpdatedAt = DateTime.UtcNow;

                    await dbContext.SaveChangesAsync();
                }
            });
        }

    }
}
    
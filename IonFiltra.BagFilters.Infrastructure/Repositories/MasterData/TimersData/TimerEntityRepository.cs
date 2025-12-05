using IonFiltra.BagFilters.Core.Entities.MasterData.TimerData;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.TimerData;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.MasterData.TimerData
{
    public class TimerEntityRepository : ITimerEntityRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<TimerEntityRepository> _logger;

        public TimerEntityRepository(TransactionHelper transactionHelper, ILogger<TimerEntityRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<TimerEntity?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching TimerEntity for Id {Id}", id);
                return await dbContext.TimerEntitys
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(TimerEntity entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new TimerEntity for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.TimerEntitys.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(TimerEntity entity)
        {
            _logger.LogInformation("Updating TimerEntity for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.TimerEntitys.FindAsync(entity.Id);
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
                    _logger.LogWarning("TimerEntity with Id {Id} not found", entity.Id);
                }
            });
        }

        public async Task<IEnumerable<TimerEntity>> GetAll()
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching all TimerEntity rows.");

                return await dbContext.TimerEntitys
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .ToListAsync();
            });
        }

    }
}
    
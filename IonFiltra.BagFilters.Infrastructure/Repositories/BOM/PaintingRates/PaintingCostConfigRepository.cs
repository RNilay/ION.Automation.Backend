using IonFiltra.BagFilters.Core.Entities.BOM.PaintingRates;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.PaintingRates;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.BOM.PaintingRates
{
    public class PaintingCostConfigRepository : IPaintingCostConfigRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<PaintingCostConfigRepository> _logger;

        public PaintingCostConfigRepository(TransactionHelper transactionHelper, ILogger<PaintingCostConfigRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<PaintingCostConfig?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching PaintingCostConfig for Id {Id}", id);
                return await dbContext.PaintingCostConfigs
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<IEnumerable<PaintingCostConfig>> GetAll()
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching all PaintingCostConfig rows.");
                return await dbContext.PaintingCostConfigs
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .ToListAsync();
            });
        }


        public async Task<int> AddAsync(PaintingCostConfig entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new PaintingCostConfig for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.PaintingCostConfigs.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(PaintingCostConfig entity)
        {
            _logger.LogInformation("Updating PaintingCostConfig for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.PaintingCostConfigs.FindAsync(entity.Id);
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
                    _logger.LogWarning("PaintingCostConfig with Id {Id} not found", entity.Id);
                }
            });
        }
    }
}
    
using IonFiltra.BagFilters.Core.Entities.BOM.Painting_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Painting_Cost;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.BOM.Painting_Cost
{
    public class PaintingCostRepository : IPaintingCostRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<PaintingCostRepository> _logger;

        public PaintingCostRepository(TransactionHelper transactionHelper, ILogger<PaintingCostRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<PaintingCost?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching PaintingCost for Id {Id}", id);
                return await dbContext.PaintingCosts
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(PaintingCost entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new PaintingCost for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.PaintingCosts.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(PaintingCost entity)
        {
            _logger.LogInformation("Updating PaintingCost for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.PaintingCosts.FindAsync(entity.Id);
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
                    _logger.LogWarning("PaintingCost with Id {Id} not found", entity.Id);
                }
            });
        }
    }
}
    
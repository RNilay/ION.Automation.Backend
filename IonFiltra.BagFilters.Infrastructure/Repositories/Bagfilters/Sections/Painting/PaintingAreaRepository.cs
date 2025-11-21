using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Painting
{
    public class PaintingAreaRepository : IPaintingAreaRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<PaintingAreaRepository> _logger;

        public PaintingAreaRepository(TransactionHelper transactionHelper, ILogger<PaintingAreaRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<PaintingArea?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching PaintingArea for Id {Id}", id);
                return await dbContext.PaintingAreas
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(PaintingArea entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new PaintingArea for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.PaintingAreas.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(PaintingArea entity)
        {
            _logger.LogInformation("Updating PaintingArea for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.PaintingAreas.FindAsync(entity.Id);
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
                    _logger.LogWarning("PaintingArea with Id {Id} not found", entity.Id);
                }
            });
        }
    }
}
    
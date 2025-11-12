using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Support_Structure
{
    public class SupportStructureRepository : ISupportStructureRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<SupportStructureRepository> _logger;

        public SupportStructureRepository(TransactionHelper transactionHelper, ILogger<SupportStructureRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<SupportStructure?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching SupportStructure for Id {Id}", id);
                return await dbContext.SupportStructures
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(SupportStructure entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new SupportStructure for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.SupportStructures.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(SupportStructure entity)
        {
            _logger.LogInformation("Updating SupportStructure for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.SupportStructures.FindAsync(entity.Id);
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
                    _logger.LogWarning("SupportStructure with Id {Id} not found", entity.Id);
                }
            });
        }
    }
}
    
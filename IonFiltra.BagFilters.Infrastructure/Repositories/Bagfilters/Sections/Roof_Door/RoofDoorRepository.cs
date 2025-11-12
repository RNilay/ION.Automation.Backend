using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Roof_Door
{
    public class RoofDoorRepository : IRoofDoorRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<RoofDoorRepository> _logger;

        public RoofDoorRepository(TransactionHelper transactionHelper, ILogger<RoofDoorRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<RoofDoor?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching RoofDoor for Id {Id}", id);
                return await dbContext.RoofDoors
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(RoofDoor entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new RoofDoor for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.RoofDoors.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(RoofDoor entity)
        {
            _logger.LogInformation("Updating RoofDoor for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.RoofDoors.FindAsync(entity.Id);
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
                    _logger.LogWarning("RoofDoor with Id {Id} not found", entity.Id);
                }
            });
        }
    }
}
    
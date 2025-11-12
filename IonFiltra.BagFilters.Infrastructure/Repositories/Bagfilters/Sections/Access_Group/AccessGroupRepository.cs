using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.Access_Group
{
    public class AccessGroupRepository : IAccessGroupRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<AccessGroupRepository> _logger;

        public AccessGroupRepository(TransactionHelper transactionHelper, ILogger<AccessGroupRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<AccessGroup?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching AccessGroup for Id {Id}", id);
                return await dbContext.AccessGroups
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(AccessGroup entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new AccessGroup for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.AccessGroups.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(AccessGroup entity)
        {
            _logger.LogInformation("Updating AccessGroup for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.AccessGroups.FindAsync(entity.Id);
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
                    _logger.LogWarning("AccessGroup with Id {Id} not found", entity.Id);
                }
            });
        }
    }
}
    
using IonFiltra.BagFilters.Core.Entities.Users.UserRoles;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Users.UserRoles;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Users.UserRoles
{
    public class ApplicationRolesRepository : IApplicationRolesRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<ApplicationRolesRepository> _logger;

        public ApplicationRolesRepository(TransactionHelper transactionHelper, ILogger<ApplicationRolesRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<ApplicationRoles?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching ApplicationRoles for Id {Id}", id);
                return await dbContext.Roles
                    .AsNoTracking()
                    .Where(x => x.RoleId == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<List<ApplicationRoles>> GetAllAsync()
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching all ApplicationRoles");
                return await dbContext.Roles
                    .AsNoTracking()
                    .Where(x => x.IsActive && !x.IsDeleted)                 // optional filtering
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();
            });
        }


        public async Task<int> AddAsync(ApplicationRoles entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new ApplicationRoles for Id {RoleId}", entity.RoleId);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.Roles.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.RoleId; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(ApplicationRoles entity)
        {
            _logger.LogInformation("Updating ApplicationRoles for Id {RoleId}", entity.RoleId);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.Roles.FindAsync(entity.RoleId);
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
                    _logger.LogWarning("ApplicationRoles with Id {RoleId} not found", entity.RoleId);
                }
            });
        }
    }
}
    
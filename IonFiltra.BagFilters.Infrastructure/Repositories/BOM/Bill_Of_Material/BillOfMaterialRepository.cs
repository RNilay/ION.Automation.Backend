using IonFiltra.BagFilters.Core.Entities.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.BOM.Bill_Of_Material
{
    public class BillOfMaterialRepository : IBillOfMaterialRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<BillOfMaterialRepository> _logger;

        public BillOfMaterialRepository(TransactionHelper transactionHelper, ILogger<BillOfMaterialRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<BillOfMaterial?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching BillOfMaterial for Id {Id}", id);
                return await dbContext.BillOfMaterials
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(BillOfMaterial entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new BillOfMaterial for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.BillOfMaterials.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task AddRangeAsync(IEnumerable<BillOfMaterial> entities)
        {
            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                foreach (var e in entities)
                {
                    e.CreatedAt = DateTime.UtcNow;
                }

                await dbContext.BillOfMaterials.AddRangeAsync(entities);
                await dbContext.SaveChangesAsync();
            });
        }


        public async Task UpdateAsync(BillOfMaterial entity)
        {
            _logger.LogInformation("Updating BillOfMaterial for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.BillOfMaterials.FindAsync(entity.Id);
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
                    _logger.LogWarning("BillOfMaterial with Id {Id} not found", entity.Id);
                }
            });
        }
    }
}
    
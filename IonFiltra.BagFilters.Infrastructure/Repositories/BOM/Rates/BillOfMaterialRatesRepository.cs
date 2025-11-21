using IonFiltra.BagFilters.Core.Entities.BOM.Rates;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Rates;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.BOM.Rates
{
    public class BillOfMaterialRatesRepository : IBillOfMaterialRatesRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<BillOfMaterialRatesRepository> _logger;

        public BillOfMaterialRatesRepository(TransactionHelper transactionHelper, ILogger<BillOfMaterialRatesRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<BillOfMaterialRates?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching BillOfMaterialRates for Id {Id}", id);
                return await dbContext.BillOfMaterialRatess
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<IEnumerable<BillOfMaterialRates>> GetAll()
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching all BillOfMaterialRates from DB.");
                return await dbContext.BillOfMaterialRatess
                    .AsNoTracking()
                    .OrderBy(x => x.CreatedAt)
                    .ToListAsync();
            });
        }


        public async Task<int> AddAsync(BillOfMaterialRates entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new BillOfMaterialRates for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.BillOfMaterialRatess.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(BillOfMaterialRates entity)
        {
            _logger.LogInformation("Updating BillOfMaterialRates for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.BillOfMaterialRatess.FindAsync(entity.Id);
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
                    _logger.LogWarning("BillOfMaterialRates with Id {Id} not found", entity.Id);
                }
            });
        }
    }
}
    
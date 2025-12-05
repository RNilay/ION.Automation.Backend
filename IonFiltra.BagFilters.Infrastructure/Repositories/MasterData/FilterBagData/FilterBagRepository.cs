using IonFiltra.BagFilters.Core.Entities.MasterData.FilterBagData;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.FilterBagData;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.MasterData.FilterBagData
{
    public class FilterBagRepository : IFilterBagRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<FilterBagRepository> _logger;

        public FilterBagRepository(TransactionHelper transactionHelper, ILogger<FilterBagRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<FilterBag?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching FilterBag for Id {Id}", id);
                return await dbContext.FilterBags
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(FilterBag entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new FilterBag for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.FilterBags.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(FilterBag entity)
        {
            _logger.LogInformation("Updating FilterBag for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.FilterBags.FindAsync(entity.Id);
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
                    _logger.LogWarning("FilterBag with Id {Id} not found", entity.Id);
                }
            });
        }
    }
}
    
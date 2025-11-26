using IonFiltra.BagFilters.Core.Entities.BagfilterDatabase.WithCanopy;
using IonFiltra.BagFilters.Core.Entities.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BagfilterDatabase.WithCanopy;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.BagfilterDatabase.WithCanopy
{
    public class IFI_Bagfilter_Database_With_CanopyRepository : IIFI_Bagfilter_Database_With_CanopyRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<IFI_Bagfilter_Database_With_CanopyRepository> _logger;

        public IFI_Bagfilter_Database_With_CanopyRepository(TransactionHelper transactionHelper, ILogger<IFI_Bagfilter_Database_With_CanopyRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<IFI_Bagfilter_Database_With_Canopy?> GetById(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching IFI_Bagfilter_Database_With_Canopy for Id {Id}", id);
                return await dbContext.IFI_Bagfilter_Database_With_Canopys
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(IFI_Bagfilter_Database_With_Canopy entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new IFI_Bagfilter_Database_With_Canopy for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.IFI_Bagfilter_Database_With_Canopys.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(IFI_Bagfilter_Database_With_Canopy entity)
        {
            _logger.LogInformation("Updating IFI_Bagfilter_Database_With_Canopy for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.IFI_Bagfilter_Database_With_Canopys.FindAsync(entity.Id);
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
                    _logger.LogWarning("IFI_Bagfilter_Database_With_Canopy with Id {Id} not found", entity.Id);
                }
            });
        }




        public async Task<IFI_Bagfilter_Database_With_Canopy?> GetByMatchAsync(string? processVolume, string? hopperType, decimal? numberOfColumns)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching IFI_Bagfilter by match criteria in repo.");

                // Start query
                var query = dbContext.IFI_Bagfilter_Database_With_Canopys.AsNoTracking().AsQueryable();

                // Only add conditions for provided values (AND semantics across provided fields)
                if (!string.IsNullOrWhiteSpace(processVolume))
                {
                    var pv = processVolume.Trim().ToLower();
                    query = query.Where(x => x.Process_Volume_m3hr != null && x.Process_Volume_m3hr.ToLower() == pv);
                }

                if (!string.IsNullOrWhiteSpace(hopperType))
                {
                    var ht = hopperType.Trim().ToLower();
                    query = query.Where(x => x.Hopper_type != null && x.Hopper_type.ToLower() == ht);
                }

                if (numberOfColumns.HasValue)
                {
                    query = query.Where(x => x.Number_of_columns == numberOfColumns.Value);
                }

                // return latest matching record if multiple exist
                return await query.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();
            });
        }




    }
}
    
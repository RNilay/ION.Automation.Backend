using IonFiltra.BagFilters.Core.Entities.EnquiryEntity;
using IonFiltra.BagFilters.Core.Interfaces.EnquiryRep;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.EnquiryRepo
{
    public class EnquiryRepository : IEnquiryRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<EnquiryRepository> _logger;

        public EnquiryRepository(TransactionHelper transactionHelper, ILogger<EnquiryRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<List<Enquiry>> GetByUserId(int userId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching Enquiries for UserId {userId}", userId);
                return await dbContext.Enquirys
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .ToListAsync();
            });
        }

        public async Task<(List<Enquiry> Items, int TotalCount)> GetByUserId(int userId, int pageNumber, int pageSize)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching Enquiries with pagination for UserId {userId}, Page {pageNumber}, Size {pageSize}",
                    userId, pageNumber, pageSize);

                var query = dbContext.Enquirys
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.CreatedAt);

                var totalCount = await query.CountAsync();

                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (items, totalCount);
            });
        }



        public async Task<int> AddAsync(Enquiry entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new Enquiry for ProjectId {ProjectId}", entity.UserId);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.Enquirys.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(Enquiry entity)
        {
            _logger.LogInformation("Updating Enquiry for ProjectId {ProjectId}", entity.UserId);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.Enquirys.FindAsync(entity.Id);
                if (existingEntity != null)
                {
                    var createdAt = existingEntity.CreatedAt;
                    dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                    existingEntity.UpdatedAt = DateTime.Now; // Assuming UpdatedDate exists
                    existingEntity.CreatedAt = createdAt;
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning("Enquiry with ProjectId {ProjectId} not found", entity.UserId);
                }
            });
        }

        public async Task<bool> UpdateByEnquiryIdAsync(
        string enquiryId,
        int userId,
        string customer,
        int requiredBagFilters
    )
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Updating Enquiry {EnquiryId} for User {UserId}",
                    enquiryId,
                    userId
                );

                var existing = await dbContext.Enquirys
                    .FirstOrDefaultAsync(e =>
                        e.EnquiryId == enquiryId &&
                        e.UserId == userId
                    );

                if (existing == null)
                {
                    _logger.LogWarning(
                        "Enquiry {EnquiryId} not found for User {UserId}",
                        enquiryId,
                        userId
                    );
                    return false;
                }

                // Only update allowed editable fields
                existing.Customer = customer;
                existing.RequiredBagFilters = requiredBagFilters;
                existing.UpdatedAt = DateTime.Now;

                await dbContext.SaveChangesAsync();
                return true;
            });
        }


    }
}

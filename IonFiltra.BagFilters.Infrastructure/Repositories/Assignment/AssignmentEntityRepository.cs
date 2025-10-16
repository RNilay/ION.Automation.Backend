using IonFiltra.BagFilters.Core.Entities.Assignment;
using IonFiltra.BagFilters.Core.Entities.EnquiryEntity;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Assignment;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Assignment
{
    public class AssignmentEntityRepository : IAssignmentEntityRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<AssignmentEntityRepository> _logger;

        public AssignmentEntityRepository(TransactionHelper transactionHelper, ILogger<AssignmentEntityRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<List<AssignmentEntity>> GetByUserId(int userId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching Assignments via UserId {UserId}", userId);

                var assignments = await (from a in dbContext.AssignmentEntitys
                                         join e in dbContext.Enquirys
                                             on a.EnquiryId equals e.EnquiryId
                                         where e.UserId == userId
                                         orderby a.CreatedAt descending // ✅ correct LINQ syntax
                                         select a)
                                        .AsNoTracking()
                                        .ToListAsync();

                return assignments;
            });
        }

        public async Task<(List<AssignmentEntity> Items, int TotalCount)> GetByUserId(int userId, int pageNumber, int pageSize)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching paginated Assignments via UserId {UserId}, Page {PageNumber}, Size {PageSize}",
                    userId, pageNumber, pageSize);

                var query = from a in dbContext.AssignmentEntitys
                            join e in dbContext.Enquirys on a.EnquiryId equals e.EnquiryId
                            where e.UserId == userId
                            select a;

                var totalCount = await query.CountAsync();

                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();

                return (items, totalCount);
            });
        }

        public async Task<(List<AssignmentEntity> Items, int TotalCount)> GetByEnquiryId(
    string enquiryId, int pageNumber, int pageSize)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                // Guard inputs
                pageNumber = pageNumber < 1 ? 1 : pageNumber;
                pageSize = pageSize < 1 ? 8 : pageSize;

                _logger.LogInformation(
                    "Fetching paginated Assignments for EnquiryId {EnquiryId}, Page {PageNumber}, Size {PageSize}",
                    enquiryId, pageNumber, pageSize
                );

                var query = dbContext.AssignmentEntitys
                    .AsNoTracking()
                    .Where(a => a.EnquiryId == enquiryId)
                    .OrderByDescending(a => a.CreatedAt);

                var totalCount = await query.CountAsync();

                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (items, totalCount);
            });
        }



        public async Task<List<AssignmentEntity>> AddRangeAsync(List<AssignmentEntity> entities)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding {Count} AssignmentEntities for EnquiryId {EnquiryId}",
                    entities.Count, entities.First().EnquiryId);

                await dbContext.AssignmentEntitys.AddRangeAsync(entities);
                await dbContext.SaveChangesAsync();

                return entities; // return the full list with IDs
            });
        }


        public async Task UpdateAsync(AssignmentEntity entity)
        {
            _logger.LogInformation("Updating AssignmentEntity for ProjectId {ProjectId}", entity.EnquiryId);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.AssignmentEntitys.FindAsync(entity.Id);
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
                    _logger.LogWarning("AssignmentEntity with ProjectId {ProjectId} not found", entity.EnquiryId);
                }
            });
        }
    }
}
    
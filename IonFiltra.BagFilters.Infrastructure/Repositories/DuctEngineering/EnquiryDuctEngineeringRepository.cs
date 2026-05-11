using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.DuctEngineering;
using IonFiltra.BagFilters.Core.Interfaces.DuctEngineering;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.DuctEngineering
{
    public class EnquiryDuctEngineeringRepository : IEnquiryDuctEngineeringRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<EnquiryDuctEngineeringRepository> _logger;

        public EnquiryDuctEngineeringRepository(
            TransactionHelper transactionHelper,
            ILogger<EnquiryDuctEngineeringRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        // ── SAVE ──────────────────────────────────────────────────────────────
        public async Task<int> SaveAsync(EnquiryDuctEngineering entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Saving duct engineering settings for EnquiryId {EnquiryId}",
                    entity.EnquiryId);

                entity.CreatedAt = DateTime.Now;
                await dbContext.EnquiryDuctEngineerings.AddAsync(entity);
                await dbContext.SaveChangesAsync();

                _logger.LogInformation(
                    "Duct engineering saved with Id {Id} for EnquiryId {EnquiryId}",
                    entity.Id, entity.EnquiryId);

                return entity.Id;
            });
        }

        // ── UPDATE ────────────────────────────────────────────────────────────
        public async Task<bool> UpdateAsync(int enquiryId, EnquiryDuctEngineering entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Updating duct engineering settings for EnquiryId {EnquiryId}", enquiryId);

                var existing = await dbContext.EnquiryDuctEngineerings
                    .FirstOrDefaultAsync(x => x.EnquiryId == enquiryId && !x.IsDeleted);

                if (existing == null)
                {
                    _logger.LogWarning(
                        "Duct engineering record not found for EnquiryId {EnquiryId}", enquiryId);
                    return false;
                }

                existing.IsDuctEngineering = entity.IsDuctEngineering;
                existing.Cost = entity.Cost;
                existing.UpdatedAt = DateTime.Now;

                await dbContext.SaveChangesAsync();

                _logger.LogInformation(
                    "Duct engineering updated for EnquiryId {EnquiryId}", enquiryId);

                return true;
            });
        }

        // ── GET ───────────────────────────────────────────────────────────────
        public async Task<EnquiryDuctEngineering?> GetByEnquiryIdAsync(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Fetching duct engineering settings for EnquiryId {EnquiryId}", enquiryId);

                return await dbContext.EnquiryDuctEngineerings
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.EnquiryId == enquiryId && !x.IsDeleted);
            });
        }

        // ── EXISTS ────────────────────────────────────────────────────────────
        public async Task<bool> ExistsByEnquiryIdAsync(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                return await dbContext.EnquiryDuctEngineerings
                    .AsNoTracking()
                    .AnyAsync(x => x.EnquiryId == enquiryId && !x.IsDeleted);
            });
        }
    }
}

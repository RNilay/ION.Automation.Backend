using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.Transportation_Settings;
using IonFiltra.BagFilters.Core.Interfaces.Transportation_Settings;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Transportation_Settings
{
    public class TransportationSettingsRepository : ITransportationSettingsRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<TransportationSettingsRepository> _logger;

        public TransportationSettingsRepository(
            TransactionHelper transactionHelper,
            ILogger<TransportationSettingsRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        // ── SAVE ──────────────────────────────────────────────────────
        public async Task<int> SaveAsync(EnquiryTransportationSettings entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Saving transportation settings for EnquiryId {EnquiryId}",
                    entity.EnquiryId);

                entity.CreatedAt = DateTime.Now;

                await dbContext.EnquiryTransportationSettings.AddAsync(entity);
                await dbContext.SaveChangesAsync();

                _logger.LogInformation(
                    "Transportation settings saved with Id {Id} for EnquiryId {EnquiryId}",
                    entity.Id, entity.EnquiryId);

                return entity.Id;
            });
        }

        // ── UPDATE ────────────────────────────────────────────────────
        public async Task<bool> UpdateAsync(
            int enquiryId,
            EnquiryTransportationSettings entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Updating transportation settings for EnquiryId {EnquiryId}",
                    enquiryId);

                var existing = await dbContext.EnquiryTransportationSettings
                    .FirstOrDefaultAsync(x => x.EnquiryId == enquiryId && !x.IsDeleted);

                if (existing == null)
                {
                    _logger.LogWarning(
                        "Transportation settings not found for EnquiryId {EnquiryId}",
                        enquiryId);
                    return false;
                }

                // Update only value fields — never touch Id, EnquiryId, CreatedAt, IsDeleted
                existing.DefaultMode = entity.DefaultMode;
                existing.DapFixedCost = entity.DapFixedCost;
                existing.UpdatedAt = DateTime.Now;

                await dbContext.SaveChangesAsync();

                _logger.LogInformation(
                    "Transportation settings updated for EnquiryId {EnquiryId}",
                    enquiryId);

                return true;
            });
        }

        // ── GET ───────────────────────────────────────────────────────
        public async Task<EnquiryTransportationSettings?> GetByEnquiryIdAsync(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Fetching transportation settings for EnquiryId {EnquiryId}",
                    enquiryId);

                return await dbContext.EnquiryTransportationSettings
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.EnquiryId == enquiryId && !x.IsDeleted);
            });
        }

        // ── EXISTS ────────────────────────────────────────────────────
        public async Task<bool> ExistsByEnquiryIdAsync(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                return await dbContext.EnquiryTransportationSettings
                    .AsNoTracking()
                    .AnyAsync(x => x.EnquiryId == enquiryId && !x.IsDeleted);
            });
        }
    }
}

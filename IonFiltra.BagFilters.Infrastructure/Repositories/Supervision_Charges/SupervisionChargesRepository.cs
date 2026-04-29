using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.Supervision_Charges;
using IonFiltra.BagFilters.Core.Interfaces.Supervision_Charges;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Supervision_Charges
{
    public class SupervisionChargesRepository : ISupervisionChargesRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<SupervisionChargesRepository> _logger;

        public SupervisionChargesRepository(
            TransactionHelper transactionHelper,
            ILogger<SupervisionChargesRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        // ── SAVE ──────────────────────────────────────────────────────
        public async Task<int> SaveAsync(EnquirySupervisionCharges entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Saving supervision charges for EnquiryId {EnquiryId}",
                    entity.EnquiryId);

                entity.CreatedAt = DateTime.Now;

                await dbContext.EnquirySupervisionCharges.AddAsync(entity);
                await dbContext.SaveChangesAsync();

                _logger.LogInformation(
                    "Supervision charges saved with Id {Id} for EnquiryId {EnquiryId}",
                    entity.Id, entity.EnquiryId);

                return entity.Id;
            });
        }

        // ── UPDATE ────────────────────────────────────────────────────
        public async Task<bool> UpdateAsync(int enquiryId, EnquirySupervisionCharges entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Updating supervision charges for EnquiryId {EnquiryId}", enquiryId);

                var existing = await dbContext.EnquirySupervisionCharges
                    .FirstOrDefaultAsync(x => x.EnquiryId == enquiryId && !x.IsDeleted);

                if (existing == null)
                {
                    _logger.LogWarning(
                        "Supervision charges not found for EnquiryId {EnquiryId}", enquiryId);
                    return false;
                }

                // Update all value fields — never touch Id, EnquiryId, CreatedAt, IsDeleted
                existing.VisitEngineeringCharges = entity.VisitEngineeringCharges;
                existing.FreeManDays = entity.FreeManDays;
                existing.FreeManDaysRate = entity.FreeManDaysRate;
                existing.FreeManDaysToAndFro = entity.FreeManDaysToAndFro;
                existing.FreeManDaysLodgingBoarding = entity.FreeManDaysLodgingBoarding;
                existing.ChargeableDays = entity.ChargeableDays;
                existing.ChargeableRate = entity.ChargeableRate;
                existing.ChargeableToAndFro = entity.ChargeableToAndFro;
                existing.ChargeableLodgingBoarding = entity.ChargeableLodgingBoarding;
                existing.UpdatedAt = DateTime.Now;

                await dbContext.SaveChangesAsync();

                _logger.LogInformation(
                    "Supervision charges updated for EnquiryId {EnquiryId}", enquiryId);

                return true;
            });
        }

        // ── GET ───────────────────────────────────────────────────────
        public async Task<EnquirySupervisionCharges?> GetByEnquiryIdAsync(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Fetching supervision charges for EnquiryId {EnquiryId}", enquiryId);

                return await dbContext.EnquirySupervisionCharges
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.EnquiryId == enquiryId && !x.IsDeleted);
            });
        }

        // ── EXISTS ────────────────────────────────────────────────────
        public async Task<bool> ExistsByEnquiryIdAsync(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                return await dbContext.EnquirySupervisionCharges
                    .AsNoTracking()
                    .AnyAsync(x => x.EnquiryId == enquiryId && !x.IsDeleted);
            });
        }
    }
}

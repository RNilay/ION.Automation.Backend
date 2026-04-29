using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.PaintCostSummary;
using IonFiltra.BagFilters.Core.Interfaces.Bagfilters.Sections.PaintCostSummary;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.Sections.PaintCostSummary
{
    public class BagfilterPaintingCostRepository : IBagfilterPaintingCostRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<BagfilterPaintingCostRepository> _logger;

        public BagfilterPaintingCostRepository(
            TransactionHelper transactionHelper,
            ILogger<BagfilterPaintingCostRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        // ── SAVE ─────────────────────────────────────────────────────────────
        public async Task SaveAsync(BagfilterPaintingCostSummary entity)
        {
            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Saving painting cost summary for BagfilterMasterId {Id}",
                    entity.BagfilterMasterId);

                entity.CreatedAt = DateTime.Now;
                entity.IsDeleted = false;
                await dbContext.BagfilterPaintingCostSummaries.AddAsync(entity);
                await dbContext.SaveChangesAsync();
            });
        }

        // ── UPDATE ────────────────────────────────────────────────────────────
        public async Task<bool> UpdateAsync(BagfilterPaintingCostSummary entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existing = await dbContext.BagfilterPaintingCostSummaries
                    .FirstOrDefaultAsync(x =>
                        x.BagfilterMasterId == entity.BagfilterMasterId &&
                        !x.IsDeleted);

                if (existing == null)
                {
                    _logger.LogWarning(
                        "Painting cost summary not found for BagfilterMasterId {Id}",
                        entity.BagfilterMasterId);
                    return false;
                }

                var createdAt = existing.CreatedAt;
                existing.SchemeName = entity.SchemeName;
                existing.GrandTotal = entity.GrandTotal;
                existing.EnquiryId = entity.EnquiryId;
                existing.UpdatedAt = DateTime.Now;
                existing.CreatedAt = createdAt;

                await dbContext.SaveChangesAsync();
                return true;
            });
        }

        // ── UPSERT ────────────────────────────────────────────────────────────
        public async Task UpsertAsync(BagfilterPaintingCostSummary entity)
        {
            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Upserting painting cost summary for BagfilterMasterId {Id}",
                    entity.BagfilterMasterId);

                var existing = await dbContext.BagfilterPaintingCostSummaries
                    .FirstOrDefaultAsync(x =>
                        x.BagfilterMasterId == entity.BagfilterMasterId &&
                        !x.IsDeleted);

                if (existing == null)
                {
                    entity.CreatedAt = DateTime.Now;
                    entity.IsDeleted = false;
                    await dbContext.BagfilterPaintingCostSummaries.AddAsync(entity);
                }
                else
                {
                    var createdAt = existing.CreatedAt;
                    existing.SchemeName = entity.SchemeName;
                    existing.GrandTotal = entity.GrandTotal;
                    existing.EnquiryId = entity.EnquiryId;
                    existing.UpdatedAt = DateTime.Now;
                    existing.CreatedAt = createdAt;
                }

                await dbContext.SaveChangesAsync();
            });
        }


        public async Task UpsertRangeAsync(List<BagfilterPaintingCostSummary> entities)
        {
            if (entities == null || entities.Count == 0)
                return;

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var masterIds = entities
                    .Select(e => e.BagfilterMasterId)
                    .Distinct()
                    .ToList();

                var existingList = await dbContext.BagfilterPaintingCostSummaries
                    .Where(x => masterIds.Contains(x.BagfilterMasterId) && !x.IsDeleted)
                    .ToListAsync();

                var existingMap = existingList
                    .ToDictionary(x => x.BagfilterMasterId, x => x);

                var toInsert = new List<BagfilterPaintingCostSummary>();

                foreach (var entity in entities)
                {
                    if (existingMap.TryGetValue(entity.BagfilterMasterId, out var existing))
                    {
                        // UPDATE
                        existing.SchemeName = entity.SchemeName;
                        existing.GrandTotal = entity.GrandTotal;
                        existing.EnquiryId = entity.EnquiryId;
                        existing.UpdatedAt = DateTime.Now;
                    }
                    else
                    {
                        // INSERT
                        entity.CreatedAt = DateTime.Now;
                        entity.IsDeleted = false;
                        toInsert.Add(entity);
                    }
                }

                if (toInsert.Count > 0)
                    await dbContext.BagfilterPaintingCostSummaries.AddRangeAsync(toInsert);

                await dbContext.SaveChangesAsync();
            });
        }

        // ── GET BY ENQUIRY ────────────────────────────────────────────────────
        public async Task<List<BagfilterPaintingCostSummary>> GetByEnquiryIdAsync(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                return await dbContext.BagfilterPaintingCostSummaries
                    .AsNoTracking()
                    .Where(x => x.EnquiryId == enquiryId && !x.IsDeleted)
                    .ToListAsync();
            });
        }

        // ── GET BY BF MASTER ──────────────────────────────────────────────────
        public async Task<BagfilterPaintingCostSummary?> GetByBagfilterMasterIdAsync(int bagfilterMasterId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                return await dbContext.BagfilterPaintingCostSummaries
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.BagfilterMasterId == bagfilterMasterId &&
                        !x.IsDeleted);
            });
        }
    }
}

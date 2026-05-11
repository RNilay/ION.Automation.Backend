using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.ApprovedMakes;
using IonFiltra.BagFilters.Core.Interfaces.ApprovedMakes;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.ApprovedMakes
{
    public class EnquiryApprovedMakesRepository : IEnquiryApprovedMakesRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<EnquiryApprovedMakesRepository> _logger;

        public EnquiryApprovedMakesRepository(
            TransactionHelper transactionHelper,
            ILogger<EnquiryApprovedMakesRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        // ── SAVE ──────────────────────────────────────────────────────────────
        public async Task<int> SaveAsync(ApprovedMakeGraph graph)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Saving approved makes for EnquiryId {EnquiryId}",
                    graph.Header.EnquiryId);

                // 1. Insert header
                graph.Header.CreatedAt = DateTime.Now;
                await dbContext.EnquiryApprovedMakes.AddAsync(graph.Header);
                await dbContext.SaveChangesAsync();
                // graph.Header.Id is now populated

                // 2. Insert items (link to header)
                foreach (var item in graph.Items)
                {
                    item.EnquiryApprovedMakeId = graph.Header.Id;
                    item.CreatedAt = DateTime.Now;
                }

                if (graph.Items.Any())
                    await dbContext.EnquiryApprovedMakeItems.AddRangeAsync(graph.Items);

                await dbContext.SaveChangesAsync();

                _logger.LogInformation(
                    "Approved makes saved with Id {Id} for EnquiryId {EnquiryId}",
                    graph.Header.Id, graph.Header.EnquiryId);

                return graph.Header.Id;
            });
        }

        // ── UPDATE (delete items + re-insert) ─────────────────────────────────
        public async Task<bool> UpdateAsync(int enquiryId, ApprovedMakeGraph graph)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Updating approved makes for EnquiryId {EnquiryId}", enquiryId);

                var existing = await dbContext.EnquiryApprovedMakes
                    .FirstOrDefaultAsync(x => x.EnquiryId == enquiryId && !x.IsDeleted);

                if (existing == null)
                {
                    _logger.LogWarning(
                        "Approved makes header not found for EnquiryId {EnquiryId}", enquiryId);
                    return false;
                }

                // Update header timestamp
                existing.UpdatedAt = DateTime.Now;

                // Delete all existing items for this header
                await dbContext.EnquiryApprovedMakeItems
                    .Where(i => i.EnquiryApprovedMakeId == existing.Id)
                    .ExecuteDeleteAsync();

                await dbContext.SaveChangesAsync();

                // Re-insert fresh items
                foreach (var item in graph.Items)
                {
                    item.EnquiryApprovedMakeId = existing.Id;
                    item.CreatedAt = DateTime.Now;
                }

                if (graph.Items.Any())
                    await dbContext.EnquiryApprovedMakeItems.AddRangeAsync(graph.Items);

                await dbContext.SaveChangesAsync();

                _logger.LogInformation(
                    "Approved makes updated for EnquiryId {EnquiryId} — {Count} items",
                    enquiryId, graph.Items.Count);

                return true;
            });
        }

        // ── GET ───────────────────────────────────────────────────────────────
        public async Task<ApprovedMakeGraph?> GetByEnquiryIdAsync(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Fetching approved makes for EnquiryId {EnquiryId}", enquiryId);

                var header = await dbContext.EnquiryApprovedMakes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.EnquiryId == enquiryId && !x.IsDeleted);

                if (header == null) return null;

                var items = await dbContext.EnquiryApprovedMakeItems
                    .AsNoTracking()
                    .Where(i => i.EnquiryApprovedMakeId == header.Id)
                    .ToListAsync();

                return new ApprovedMakeGraph
                {
                    Header = header,
                    Items = items
                };
            });
        }

        // ── EXISTS ────────────────────────────────────────────────────────────
        public async Task<bool> ExistsByEnquiryIdAsync(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                return await dbContext.EnquiryApprovedMakes
                    .AsNoTracking()
                    .AnyAsync(x => x.EnquiryId == enquiryId && !x.IsDeleted);
            });
        }
    }
}

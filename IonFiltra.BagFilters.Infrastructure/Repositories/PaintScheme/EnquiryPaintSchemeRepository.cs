using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.PaintScheme;
using IonFiltra.BagFilters.Core.Interfaces.PaintScheme;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.PaintScheme
{
    public class EnquiryPaintSchemeRepository : IEnquiryPaintSchemeRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<EnquiryPaintSchemeRepository> _logger;
        private record SchemeMasterLookup(decimal CostPerKg, string SchemeName);

        public EnquiryPaintSchemeRepository(
            TransactionHelper transactionHelper,
            ILogger<EnquiryPaintSchemeRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

   
        // ── SAVE ──────────────────────────────────────────────────────────────
        public async Task<int> SaveAsync(PaintSchemeGraph graph)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Saving paint scheme for EnquiryId {EnquiryId}",
                    graph.Header.EnquiryId);

                // 1. insert header
                graph.Header.CreatedAt = DateTime.Now;
                await dbContext.EnquiryPaintSchemes.AddAsync(graph.Header);
                await dbContext.SaveChangesAsync();
                // graph.Header.Id is now populated

                // 2. global sections
                foreach (var s in graph.Sections)
                {
                    s.EnquiryPaintSchemeId = graph.Header.Id;
                    s.CreatedAt = DateTime.Now;
                }
                if (graph.Sections.Any())
                    await dbContext.EnquiryPaintSchemeSections.AddRangeAsync(graph.Sections);

                // 3. BF assignments
                foreach (var a in graph.Assignments)
                {
                    a.EnquiryPaintSchemeId = graph.Header.Id;
                    a.CreatedAt = DateTime.Now;
                }
                if (graph.Assignments.Any())
                    await dbContext.EnquiryPaintSchemeBfAssignments.AddRangeAsync(graph.Assignments);

                await dbContext.SaveChangesAsync();
                // assignment Ids are now populated

                // 4. custom overrides
                foreach (var og in graph.Overrides)
                {
                    // link to its assignment row using the transient BfName
                    var matchedAssignment = graph.Assignments
                        .FirstOrDefault(a => a.BfName == og.Override.BfName);

                    if (matchedAssignment == null)
                    {
                        _logger.LogWarning(
                            "Override has no matching assignment for BfName {BfName}",
                            og.Override.BfName);
                        continue;
                    }

                    og.Override.BfAssignmentId = matchedAssignment.Id;
                    og.Override.CreatedAt = DateTime.Now;
                    await dbContext.EnquiryPaintSchemeOverrides.AddAsync(og.Override);
                    await dbContext.SaveChangesAsync();
                    // og.Override.Id is now populated

                    foreach (var os in og.Sections)
                    {
                        os.OverrideId = og.Override.Id;
                        os.CreatedAt = DateTime.Now;
                    }
                    if (og.Sections.Any())
                        await dbContext.EnquiryPaintSchemeOverrideSections.AddRangeAsync(og.Sections);

                    await dbContext.SaveChangesAsync();
                }

                _logger.LogInformation(
                    "Paint scheme saved with Id {Id} for EnquiryId {EnquiryId}",
                    graph.Header.Id, graph.Header.EnquiryId);

                return graph.Header.Id;
            });
        }

        // ── UPDATE (delete children + re-insert) ──────────────────────────────
        public async Task<bool> UpdateAsync(int enquiryId, PaintSchemeGraph graph)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Updating paint scheme for EnquiryId {EnquiryId}", enquiryId);

                var existing = await dbContext.EnquiryPaintSchemes
                    .FirstOrDefaultAsync(x => x.EnquiryId == enquiryId && !x.IsDeleted);

                if (existing == null)
                {
                    _logger.LogWarning(
                        "Paint scheme not found for EnquiryId {EnquiryId}", enquiryId);
                    return false;
                }

                // update header
                var createdAt = existing.CreatedAt;
                existing.PaintingSchemeId = graph.Header.PaintingSchemeId;
                existing.UpdatedAt = DateTime.Now;
                existing.CreatedAt = createdAt;

                // collect child Ids for deletion
                var existingAssignmentIds = await dbContext.EnquiryPaintSchemeBfAssignments
                    .Where(a => a.EnquiryPaintSchemeId == existing.Id)
                    .Select(a => a.Id)
                    .ToListAsync();

                var existingOverrideIds = await dbContext.EnquiryPaintSchemeOverrides
                    .Where(o => existingAssignmentIds.Contains(o.BfAssignmentId))
                    .Select(o => o.Id)
                    .ToListAsync();

                // delete in reverse FK order
                if (existingOverrideIds.Any())
                {
                    await dbContext.EnquiryPaintSchemeOverrideSections
                        .Where(os => existingOverrideIds.Contains(os.OverrideId))
                        .ExecuteDeleteAsync();

                    await dbContext.EnquiryPaintSchemeOverrides
                        .Where(o => existingOverrideIds.Contains(o.Id))
                        .ExecuteDeleteAsync();
                }

                if (existingAssignmentIds.Any())
                {
                    await dbContext.EnquiryPaintSchemeBfAssignments
                        .Where(a => existingAssignmentIds.Contains(a.Id))
                        .ExecuteDeleteAsync();
                }

                await dbContext.EnquiryPaintSchemeSections
                    .Where(s => s.EnquiryPaintSchemeId == existing.Id)
                    .ExecuteDeleteAsync();

                await dbContext.SaveChangesAsync();

                // re-insert using the same logic as SaveAsync but with existing.Id
                foreach (var s in graph.Sections)
                {
                    s.EnquiryPaintSchemeId = existing.Id;
                    s.CreatedAt = DateTime.Now;
                }
                if (graph.Sections.Any())
                    await dbContext.EnquiryPaintSchemeSections.AddRangeAsync(graph.Sections);

                foreach (var a in graph.Assignments)
                {
                    a.EnquiryPaintSchemeId = existing.Id;
                    a.CreatedAt = DateTime.Now;
                }
                if (graph.Assignments.Any())
                    await dbContext.EnquiryPaintSchemeBfAssignments.AddRangeAsync(graph.Assignments);

                await dbContext.SaveChangesAsync();

                foreach (var og in graph.Overrides)
                {
                    var matchedAssignment = graph.Assignments
                        .FirstOrDefault(a => a.BfName == og.Override.BfName);

                    if (matchedAssignment == null)
                    {
                        _logger.LogWarning(
                            "Override has no matching assignment for BfName {BfName}",
                            og.Override.BfName);
                        continue;
                    }

                    og.Override.BfAssignmentId = matchedAssignment.Id;
                    og.Override.CreatedAt = DateTime.Now;
                    await dbContext.EnquiryPaintSchemeOverrides.AddAsync(og.Override);
                    await dbContext.SaveChangesAsync();

                    foreach (var os in og.Sections)
                    {
                        os.OverrideId = og.Override.Id;
                        os.CreatedAt = DateTime.Now;
                    }
                    if (og.Sections.Any())
                        await dbContext.EnquiryPaintSchemeOverrideSections.AddRangeAsync(og.Sections);

                    await dbContext.SaveChangesAsync();
                }

                _logger.LogInformation(
                    "Paint scheme updated for EnquiryId {EnquiryId}", enquiryId);

                return true;
            });
        }

        // ── GET ───────────────────────────────────────────────────────────────
        // This method previously returned a nullable value tuple which caused
        // CS0029/CS4010. It now returns PaintSchemeGraph? — a plain class —
        // which TransactionHelper.ExecuteAsync<T> can infer without ambiguity.
        public async Task<PaintSchemeGraph?> GetByEnquiryIdAsync(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Fetching paint scheme for EnquiryId {EnquiryId}", enquiryId);

                var header = await dbContext.EnquiryPaintSchemes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.EnquiryId == enquiryId && !x.IsDeleted);


                // returning null (PaintSchemeGraph?) is now unambiguous for the compiler
                if (header == null) return null;

                // Resolve costPerKg from PaintingSchemeMaster (source of truth)
                decimal costPerKg = 0m;
                string schemeName = string.Empty;
                if (header.PaintingSchemeId.HasValue)
                {
                    var schemeMaster = await dbContext.PaintingSchemeMasters
                             .AsNoTracking()
                             .FirstOrDefaultAsync(m => m.Id == header.PaintingSchemeId.Value);
                    costPerKg = schemeMaster?.CostPerKg ?? 0m;
                    schemeName = schemeMaster?.SchemeName ?? string.Empty;
                }


                var sections = await dbContext.EnquiryPaintSchemeSections
                    .AsNoTracking()
                    .Where(s => s.EnquiryPaintSchemeId == header.Id)
                    .ToListAsync();

                var assignments = await dbContext.EnquiryPaintSchemeBfAssignments
                    .AsNoTracking()
                    .Where(a => a.EnquiryPaintSchemeId == header.Id)
                    .ToListAsync();

                var assignmentIds = assignments.Select(a => a.Id).ToList();

                var overrideHeaders = await dbContext.EnquiryPaintSchemeOverrides
                    .AsNoTracking()
                    .Where(o => assignmentIds.Contains(o.BfAssignmentId))
                    .ToListAsync();

                // Collect all unique PaintingSchemeIds referenced by overrides so we can
                // resolve their costPerKg in one query instead of N+1 queries
                var overrideSchemeIds = overrideHeaders
                     .Where(o => o.PaintingSchemeId.HasValue)
                     .Select(o => o.PaintingSchemeId!.Value)
                     .Distinct()
                     .ToList();

                var overrideSchemeMasters = overrideSchemeIds.Any()
                     ? await dbContext.PaintingSchemeMasters
                           .AsNoTracking()
                           .Where(m => overrideSchemeIds.Contains(m.Id))
                           //.ToDictionaryAsync(m => m.Id, m => m.CostPerKg)
                           .ToDictionaryAsync(m => m.Id, m => new SchemeMasterLookup(m.CostPerKg, m.SchemeName))
                     : new Dictionary<int, SchemeMasterLookup>();



                var overrideIds = overrideHeaders.Select(o => o.Id).ToList();

                var overrideSections = await dbContext.EnquiryPaintSchemeOverrideSections
                    .AsNoTracking()
                    .Where(os => overrideIds.Contains(os.OverrideId))
                    .ToListAsync();

                var overrideGraphs = overrideHeaders
                    .Select(o =>
                    {
                        var master = o.PaintingSchemeId.HasValue
                            ? overrideSchemeMasters.GetValueOrDefault(o.PaintingSchemeId.Value)
                            : null;
                        return new PaintSchemeOverrideGraph
                        {
                            Override = o,
                            //CostPerKg = o.PaintingSchemeId.HasValue
                            //    ? overrideSchemeMasters.GetValueOrDefault(o.PaintingSchemeId.Value, 0m)
                            //    : 0m,
                            CostPerKg = master?.CostPerKg ?? 0m,
                            SchemeName = master?.SchemeName ?? string.Empty,
                            Sections = overrideSections
                                .Where(os => os.OverrideId == o.Id)
                                .ToList()
                        };
                    })
                    .ToList();

                return new PaintSchemeGraph
                {
                    Header = header,
                    SchemeName = schemeName,
                    CostPerKg = costPerKg,
                    Sections = sections,
                    Assignments = assignments,
                    Overrides = overrideGraphs
                };
            });
        }

        // ── EXISTS ────────────────────────────────────────────────────────────
        public async Task<bool> ExistsByEnquiryIdAsync(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                return await dbContext.EnquiryPaintSchemes
                    .AsNoTracking()
                    .AnyAsync(x => x.EnquiryId == enquiryId && !x.IsDeleted);
            });
        }
    }
}

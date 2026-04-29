using IonFiltra.BagFilters.Core.Entities.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.BagfilterDatabase.WithoutCanopy
{
    public class IFI_Bagfilter_Database_Without_CanopyRepository : IIFI_Bagfilter_Database_Without_CanopyRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<IFI_Bagfilter_Database_Without_CanopyRepository> _logger;

        public IFI_Bagfilter_Database_Without_CanopyRepository(TransactionHelper transactionHelper, ILogger<IFI_Bagfilter_Database_Without_CanopyRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<IFI_Bagfilter_Database_Without_Canopy?> GetByProjectId(int id)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching IFI_Bagfilter_Database_Without_Canopy for Id {Id}", id);
                return await dbContext.IFI_Bagfilter_Database_Without_Canopys
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<int> AddAsync(IFI_Bagfilter_Database_Without_Canopy entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new IFI_Bagfilter_Database_Without_Canopy for Id {Id}", entity.Id);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.IFI_Bagfilter_Database_Without_Canopys.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.Id; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(IFI_Bagfilter_Database_Without_Canopy entity)
        {
            _logger.LogInformation("Updating IFI_Bagfilter_Database_Without_Canopy for Id {Id}", entity.Id);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.IFI_Bagfilter_Database_Without_Canopys.FindAsync(entity.Id);
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
                    _logger.LogWarning("IFI_Bagfilter_Database_Without_Canopy with Id {Id} not found", entity.Id);
                }
            });
        }


        //public async Task<IFI_Bagfilter_Database_Without_Canopy?> GetByMatchAsync(string? processVolume, string? hopperType, decimal? numberOfColumns)
        //{
        //    return await _transactionHelper.ExecuteAsync(async dbContext =>
        //    {
        //        _logger.LogInformation("Fetching IFI_Bagfilter by match criteria in repo.");

        //        // Start query
        //        var query = dbContext.IFI_Bagfilter_Database_Without_Canopys.AsNoTracking().AsQueryable();

        //        // Only add conditions for provided values (AND semantics across provided fields)
        //        if (!string.IsNullOrWhiteSpace(processVolume))
        //        {
        //            var pv = processVolume.Trim().ToLower();
        //            query = query.Where(x => x.Process_Volume_m3hr != null && x.Process_Volume_m3hr.ToLower() == pv);
        //        }

        //        if (!string.IsNullOrWhiteSpace(hopperType))
        //        {
        //            var ht = hopperType.Trim().ToLower();
        //            query = query.Where(x => x.Hopper_type != null && x.Hopper_type.ToLower() == ht);
        //        }

        //        if (numberOfColumns.HasValue)
        //        {
        //            query = query.Where(x => x.Number_of_columns == numberOfColumns.Value);
        //        }

        //        // return latest matching record if multiple exist
        //        return await query.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();
        //    });
        //}

        public async Task<IFI_Bagfilter_Database_Without_Canopy?> GetByMatchAsync(
         string? processVolume,
         string? hopperType,
         decimal? numberOfColumns)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching IFI_Bagfilter_Without_Canopy by match criteria (3-param).");

                // ── Step 1: exact volume + all filters ─────────────────────────────
                var exactResult = await BuildQueryWithoutCanopy(dbContext, processVolume, hopperType, numberOfColumns)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();

                if (exactResult != null)
                    return exactResult;

                // ── Step 2: fallback — resolve next-greater volume ──────────────────
                if (string.IsNullOrWhiteSpace(processVolume))
                    return null;

                if (!decimal.TryParse(
                        processVolume.Trim(),
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out var inputVolumeDecimal))
                {
                    _logger.LogWarning(
                        "Cannot parse processVolume '{Value}' as decimal. Skipping fallback.", processVolume);
                    return null;
                }

                var allRows = await dbContext.IFI_Bagfilter_Database_Without_Canopys
                    .AsNoTracking()
                    .Where(x => x.Process_Volume_m3hr != null)
                    .ToListAsync();

                var nextGreaterVolumeStr = allRows
                    .Select(x => new
                    {
                        VolumeStr = x.Process_Volume_m3hr!,
                        Parsed = decimal.TryParse(
                                        x.Process_Volume_m3hr,
                                        System.Globalization.NumberStyles.Any,
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        out var v) ? v : (decimal?)null
                    })
                    .Where(x => x.Parsed.HasValue && x.Parsed.Value > inputVolumeDecimal)
                    .OrderBy(x => x.Parsed!.Value)
                    .Select(x => x.VolumeStr)
                    .FirstOrDefault();

                if (nextGreaterVolumeStr == null)
                {
                    _logger.LogInformation(
                        "No next-greater volume found above '{Volume}' in Without_Canopy table.", processVolume);
                    return null;
                }

                _logger.LogInformation(
                    "Exact volume '{Exact}' not found. Attempting fallback with next-greater volume '{Next}'.",
                    processVolume, nextGreaterVolumeStr);

                // ── Step 3: next-greater volume + all filters ───────────────────────
                var result = await BuildQueryWithoutCanopy(dbContext, nextGreaterVolumeStr, hopperType, numberOfColumns)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
                if (result != null)
                {
                    _logger.LogInformation("Fallback matched on next-greater volume + all filters.");
                    return result;
                }

                // ── Step 4: next-greater volume + hopperType only ───────────────────
                result = await BuildQueryWithoutCanopy(dbContext, nextGreaterVolumeStr, hopperType, numberOfColumns: null)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
                if (result != null)
                {
                    _logger.LogInformation("Fallback matched on next-greater volume + hopperType only.");
                    return result;
                }

                // ── Step 5: next-greater volume + numberOfColumns only ──────────────
                result = await BuildQueryWithoutCanopy(dbContext, nextGreaterVolumeStr, hopperType: null, numberOfColumns)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
                if (result != null)
                {
                    _logger.LogInformation("Fallback matched on next-greater volume + numberOfColumns only.");
                    return result;
                }

                // ── Step 6: next-greater volume alone ───────────────────────────────
                result = await BuildQueryWithoutCanopy(dbContext, nextGreaterVolumeStr, hopperType: null, numberOfColumns: null)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();

                if (result != null)
                    _logger.LogInformation("Fallback matched on next-greater volume alone.");
                else
                    _logger.LogWarning(
                        "No row found even with next-greater volume '{Next}' and all filters relaxed.",
                        nextGreaterVolumeStr);

                return result;
            });
        }

        // shared query builder — pass null to skip a filter
        private static IQueryable<IFI_Bagfilter_Database_Without_Canopy> BuildQueryWithoutCanopy(
            AppDbContext dbContext,
            string? processVolume,
            string? hopperType,
            decimal? numberOfColumns)
        {
            var query = dbContext.IFI_Bagfilter_Database_Without_Canopys
                .AsNoTracking()
                .AsQueryable();

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
                query = query.Where(x => x.Number_of_columns == numberOfColumns.Value);

            return query;
        }

        public async Task<IFI_Bagfilter_Database_Without_Canopy?> GetByMatchAsync(
        string? processVolume,
        string? hopperType,
        decimal? numberOfColumns,
        decimal? baysX,
        decimal? baysZ)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var query = dbContext.IFI_Bagfilter_Database_Without_Canopys
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(processVolume))
                {
                    var pv = processVolume.Trim().ToLower();
                    query = query.Where(x => x.Process_Volume_m3hr != null &&
                                             x.Process_Volume_m3hr.ToLower() == pv);
                }

                if (!string.IsNullOrWhiteSpace(hopperType))
                {
                    var ht = hopperType.Trim().ToLower();
                    query = query.Where(x => x.Hopper_type != null &&
                                             x.Hopper_type.ToLower() == ht);
                }

                if (numberOfColumns.HasValue)
                    query = query.Where(x => x.Number_of_columns == numberOfColumns.Value);

                if (baysX.HasValue)
                    query = query.Where(x => x.Number_of_bays_in_X_direction == baysX.Value);

                if (baysZ.HasValue)
                    query = query.Where(x => x.Number_of_bays_in_Y_direction == baysZ.Value);

                return await query
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }



        /// <summary>
        /// Checks whether a row with the exact processVolume already exists in the table.
        /// Returns true if found, false otherwise.
        /// </summary>
        public async Task<bool> ExistsByProcessVolumeAsync(string processVolume)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Checking existence of Process_Volume_m3hr={ProcessVolume} in Without_Canopy table.", processVolume);

                if (string.IsNullOrWhiteSpace(processVolume))
                    return false;

                var pv = processVolume.Trim().ToLower();
                return await dbContext.IFI_Bagfilter_Database_Without_Canopys
                    .AsNoTracking()
                    .AnyAsync(x => x.Process_Volume_m3hr != null && x.Process_Volume_m3hr.ToLower() == pv);
            });
        }

        /// <summary>
        /// Returns the row whose Process_Volume_m3hr is the next greater value above the supplied
        /// processVolume (numeric comparison). Returns null if no greater value exists.
        /// </summary>
        public async Task<IFI_Bagfilter_Database_Without_Canopy?> GetByNextGreaterVolumeAsync(decimal processVolume)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation(
                    "Fetching next-greater Process_Volume row above {ProcessVolume} in Without_Canopy table.",
                    processVolume);

                // Retrieve all rows so we can do numeric parsing in memory.
                // The column is stored as string, so we pull the candidates and parse.
                var candidates = await dbContext.IFI_Bagfilter_Database_Without_Canopys
                    .AsNoTracking()
                    .Where(x => x.Process_Volume_m3hr != null)
                    .ToListAsync();

                // Parse each row's volume to decimal; keep only those strictly greater than input.
                var nextRow = candidates
                    .Select(x => new
                    {
                        Row = x,
                        Parsed = decimal.TryParse(
                            x.Process_Volume_m3hr,
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out var v) ? v : (decimal?)null
                    })
                    .Where(x => x.Parsed.HasValue && x.Parsed.Value > processVolume)
                    .OrderBy(x => x.Parsed!.Value)          // smallest of those that are greater
                    .ThenByDescending(x => x.Row.CreatedAt) // latest record if duplicates
                    .FirstOrDefault();

                return nextRow?.Row;
            });
        }
    }
}
    
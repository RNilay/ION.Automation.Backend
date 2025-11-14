using System.Linq.Expressions;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterMasterEntity;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using IonFiltra.BagFilters.Core.Entities.EnquiryEntity;


namespace IonFiltra.BagFilters.Infrastructure.Repositories.Bagfilters.BagfilterInputs
{
    public class BagfilterInputRepository : IBagfilterInputRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<BagfilterInputRepository> _logger;

        public BagfilterInputRepository(TransactionHelper transactionHelper, ILogger<BagfilterInputRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<BagfilterInput?> GetByMasterId(int masterId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching BagfilterInput for Master Id {Id}", masterId);
                return await dbContext.BagfilterInputs
                    .AsNoTracking()
                    .Where(x => x.BagfilterMasterId == masterId)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            });
        }

        public async Task<List<BagfilterInput>> GetAllByEnquiryId(int enquiryId)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching BagfilterInputs + BagfilterMaster for Enquiry Id {Id}", enquiryId);

                return await dbContext.BagfilterInputs
                    .AsNoTracking()
                    .Include(x => x.BagfilterMaster) // include master table
                    .Where(x => x.EnquiryId == enquiryId)
                    .OrderBy(x => x.BagfilterMaster.BagFilterName) // ensure order 001,002,003
                    .ToListAsync();
            });
        }


        public async Task<List<BagfilterInput>> GetByIdsAsync(IEnumerable<int> bagfilterInputIds)
        {
            if (bagfilterInputIds == null || !bagfilterInputIds.Any())
                return new List<BagfilterInput>();

            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching BagfilterInputs for Ids: {Ids}", string.Join(",", bagfilterInputIds));

                var inputs = await dbContext.BagfilterInputs
                    .AsNoTracking()
                    .Where(x => bagfilterInputIds.Contains(x.BagfilterInputId))
                    .Select(x => new BagfilterInput
                    {
                        BagfilterInputId = x.BagfilterInputId,
                        BagfilterMasterId = x.BagfilterMasterId,
                        EnquiryId = x.EnquiryId
                        // (optional: include other fields if needed)
                    })
                    .ToListAsync();

                return inputs;
            });
        }


        public async Task<int> AddAsync(BagfilterInput entity)
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Adding new BagfilterInput for Id {id}", entity.BagfilterMasterId);
                entity.CreatedAt = DateTime.Now;
                var addedEntity = await dbContext.BagfilterInputs.AddAsync(entity);
                await dbContext.SaveChangesAsync();
                return addedEntity.Entity.BagfilterInputId; // Assuming 'Id' is the primary key
            });
        }

        public async Task UpdateAsync(BagfilterInput entity)
        {
            _logger.LogInformation("Updating BagfilterInput for Id {Id}", entity.BagfilterInputId);

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var existingEntity = await dbContext.BagfilterInputs.FindAsync(entity.BagfilterInputId);
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
                    _logger.LogWarning("BagfilterInput with Id {Id} not found", entity.BagfilterInputId);
                }
            });
        }

        public async Task<List<int>> AddRangeAsync(IEnumerable<(BagfilterMaster Master, BagfilterInput Input)> pairs)
        {
            var pairsList = pairs.ToList();
            if (!pairsList.Any()) return new List<int>();

            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                // Prepare lists for EF
                var mastersToAdd = new List<BagfilterMaster>();
                var inputsToAdd = new List<BagfilterInput>();

                foreach (var (master, input) in pairsList)
                {
                    // If the master has an existing id (>0), we won't create a new master.
                    if (master.BagfilterMasterId > 0)
                    {
                        // Ensure input references existing master id
                        input.BagfilterMasterId = master.BagfilterMasterId;
                        inputsToAdd.Add(input);
                        continue;
                    }

                    // For new master + input pair: wire navigation so EF Core sets FK after SaveChanges
                    mastersToAdd.Add(master);
                    // set navigation so EF links them (important: don't set BagfilterMasterId here)
                    input.BagfilterMaster = master;
                    inputsToAdd.Add(input);
                }

                // Add all new masters + inputs to context
                if (mastersToAdd.Any())
                    await dbContext.BagfilterMasters.AddRangeAsync(mastersToAdd);

                if (inputsToAdd.Any())
                    await dbContext.BagfilterInputs.AddRangeAsync(inputsToAdd);

                // Single SaveChanges commit -> inserts masters and inputs, EF will populate FK automatically
                await dbContext.SaveChangesAsync();

                // Collect created input Ids (for any inputs that were new). For inputs that referenced pre-existing masterId,
                // their Id will be populated only if they were newly inserted (we added them above as well).
                var createdInputIds = inputsToAdd.Select(i => i.BagfilterInputId).ToList();

                return createdInputIds;
            });
        }

        public async Task<List<int>> AddRangeAsync(
            IEnumerable<(BagfilterMaster Master, BagfilterInput Input)> pairs,
            Dictionary<int, (int matchedBagfilterInputId, int matchedBagfilterMasterId)>? matchMappingByPairIndex = null)
        {
            var pairsList = pairs.ToList();
            if (!pairsList.Any()) return new List<int>();

            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var mastersToAdd = new List<BagfilterMaster>();
                var inputsToAdd = new List<BagfilterInput>();

                // Keep the inputsToAdd in same order as pairsList so pairIndex -> inputsToAdd index mapping holds
                foreach (var (master, input) in pairsList)
                {
                    if (master.BagfilterMasterId > 0)
                    {
                        input.BagfilterMasterId = master.BagfilterMasterId;
                        inputsToAdd.Add(input);
                        continue;
                    }

                    mastersToAdd.Add(master);
                    input.BagfilterMaster = master;
                    inputsToAdd.Add(input);
                }

                if (mastersToAdd.Any())
                    await dbContext.BagfilterMasters.AddRangeAsync(mastersToAdd);

                if (inputsToAdd.Any())
                    await dbContext.BagfilterInputs.AddRangeAsync(inputsToAdd);

                await dbContext.SaveChangesAsync();

                // inputsToAdd order corresponds to pairsList order
                var createdInputIds = inputsToAdd.Select(i => i.BagfilterInputId).ToList();

                // If caller provided match mapping (pairIndex -> matched ids), apply them to newly inserted inputs
                if (matchMappingByPairIndex != null && matchMappingByPairIndex.Count > 0)
                {
                    var now = DateTime.UtcNow;
                    // Prepare list of input ids that need update
                    var updateIds = matchMappingByPairIndex.Keys
                                        .Select(pairIndex => createdInputIds.ElementAt(pairIndex))
                                        .ToList();

                    // Bulk fetch inserted input entities to update
                    var insertedInputs = await dbContext.BagfilterInputs
                        .Where(i => updateIds.Contains(i.BagfilterInputId))
                        .ToListAsync();

                    // Map bagfilterInputId -> entity for quick lookup
                    var insertedMap = insertedInputs.ToDictionary(i => i.BagfilterInputId);

                    foreach (var kv in matchMappingByPairIndex)
                    {
                        var pairIndex = kv.Key;
                        var (matchedInputId, matchedMasterId) = kv.Value;

                        // created id at same position in createdInputIds
                        var createdId = createdInputIds.ElementAt(pairIndex);

                        if (insertedMap.TryGetValue(createdId, out var inputEntity))
                        {
                            inputEntity.IsMatched = true;
                            inputEntity.MatchedBagfilterInputId = matchedInputId;
                            inputEntity.MatchedBagfilterMasterId = matchedMasterId;
                            inputEntity.MatchedAt = now;
                            // optionally set EnquiryId? depends on your flow
                        }
                    }

                    // Save updates in same transaction
                    await dbContext.SaveChangesAsync();
                }

                return createdInputIds;
            });
        }



        public async Task<List<BagfilterInput>> FindCandidatesByGroupKeysAsync(IEnumerable<GroupKey> keys)
        {
            var keyList = keys?.ToList() ?? new List<GroupKey>();
            if (!keyList.Any()) return new List<BagfilterInput>();

            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                // Build predicate: ( (Location==k1.Location && No==k1.No && ... ) || (Location==k2.Location && ... ) || ... )
                ParameterExpression param = Expression.Parameter(typeof(BagfilterInput), "i");
                Expression combined = Expression.Constant(false);

                foreach (var k in keyList)
                {
                    // build equality sub-expression for this key
                    Expression exp = Expression.Constant(true);

                    // Location (string) - handle nulls
                    if (k.Location != null)
                    {
                        var left = Expression.PropertyOrField(param, nameof(BagfilterInput.Location));
                        var right = Expression.Constant(k.Location);
                        exp = Expression.AndAlso(exp, Expression.Equal(left, right));
                    }
                    else
                    {
                        var left = Expression.PropertyOrField(param, nameof(BagfilterInput.Location));
                        exp = Expression.AndAlso(exp, Expression.Equal(left, Expression.Constant(null, typeof(string))));
                    }

                    // Helper for decimals (nullable)
                    Expression MakeDecimalEquals(string propName, decimal? val)
                    {
                        var left = Expression.PropertyOrField(param, propName);
                        if (val == null)
                        {
                            return Expression.Equal(left, Expression.Constant(null, left.Type));
                        }
                        else
                        {
                            var right = Expression.Constant(val, left.Type);
                            return Expression.Equal(left, right);
                        }
                    }

                    exp = Expression.AndAlso(exp, MakeDecimalEquals(nameof(BagfilterInput.No_Of_Column), k.No_Of_Column));
                    exp = Expression.AndAlso(exp, MakeDecimalEquals(nameof(BagfilterInput.Ground_Clearance), k.Ground_Clearance));
                    exp = Expression.AndAlso(exp, MakeDecimalEquals(nameof(BagfilterInput.Bag_Per_Row), k.Bag_Per_Row));
                    exp = Expression.AndAlso(exp, MakeDecimalEquals(nameof(BagfilterInput.Number_Of_Rows), k.Number_Of_Rows));

                    combined = Expression.OrElse(combined, exp);
                }

                var lambda = Expression.Lambda<Func<BagfilterInput, bool>>(combined, param);

                // Query DB and include BagfilterMaster for details

                var candidates = await dbContext.BagfilterInputs
                    .AsNoTracking()
                    .Include(i => i.BagfilterMaster)
                    .Where(lambda)
                    .OrderBy(i => i.BagfilterInputId)
                    .ToListAsync();

                return candidates;
            });
        }

        // Infrastructure/Repository/BagfilterRepository.cs

        public async Task<Dictionary<int, Enquiry>> GetEnquiriesByIdsAsync(IEnumerable<int> enquiryIds)
        {
            var ids = (enquiryIds ?? Enumerable.Empty<int>()).Distinct().Where(id => id > 0).ToList();
            if (!ids.Any()) return new Dictionary<int, Enquiry>();

            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                var list = await dbContext.Enquirys
                    .AsNoTracking()
                    .Where(e => ids.Contains(e.Id))
                    .ToListAsync();

                return list.ToDictionary(e => e.Id);
            });
        }


        public async Task UpdateS3dModelAsync(int bagfilterInputId, string s3dModelJson, string? sessionId)
        {
            await _transactionHelper.ExecuteAsync(async db =>
            {
                var entity = await db.BagfilterInputs.FindAsync(bagfilterInputId);
                if (entity == null) return;
                entity.AnalysisResult = s3dModelJson;
                db.BagfilterInputs.Update(entity);
                await db.SaveChangesAsync();
            });
        }

    }
}
    
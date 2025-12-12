using System.Linq.Expressions;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterMasterEntity;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using IonFiltra.BagFilters.Core.Entities.EnquiryEntity;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterInputs;
using static IonFiltra.BagFilters.Application.Services.Bagfilters.BagfilterInputs.BagfilterInputService;


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

        public async Task UpdateRangeAsync(
        List<RepositoryInputUpdateDto> dtos,
        CancellationToken ct = default)
        {
            if (dtos == null || dtos.Count == 0) return;

            await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                // -------- 1) Prefetch all input entities to update ----------
                var inputIds = dtos
                    .Select(d => d.ExistingInputId)
                    .Distinct()
                    .ToList();

                var inputEntities = await dbContext.BagfilterInputs
                    .Where(i => inputIds.Contains(i.BagfilterInputId))
                    .ToListAsync(ct);

                var inputMap = inputEntities.ToDictionary(i => i.BagfilterInputId);

                // -------- 2) Prefetch master entities that might need updating ----------
                // We only update masters when:
                // - MasterEntityToUpdate != null
                // - ResolvedNewMasterId == 0   (we are *not* switching to a new master)
                // - ExistingMasterId > 0
                var masterIdsToUpdate = dtos
                    .Where(d => d.MasterEntityToUpdate != null
                                && d.ResolvedNewMasterId == 0
                                && d.ExistingMasterId > 0)
                    .Select(d => d.ExistingMasterId)
                    .Distinct()
                    .ToList();

                var masterMap = new Dictionary<int, BagfilterMaster>();
                if (masterIdsToUpdate.Any())
                {
                    var masters = await dbContext.BagfilterMasters
                        .Where(m => masterIdsToUpdate.Contains(m.BagfilterMasterId))
                        .ToListAsync(ct);

                    masterMap = masters.ToDictionary(m => m.BagfilterMasterId);
                }

                // -------- 3) Apply each DTO to the tracked entities ----------
                foreach (var dto in dtos)
                {
                    // 3a) Update BagfilterInput
                    if (!inputMap.TryGetValue(dto.ExistingInputId, out var existingInput))
                    {
                        _logger.LogWarning("BagfilterInput with Id {Id} not found during UpdateRangeAsync", dto.ExistingInputId);
                        continue;
                    }

                    // preserve CreatedAt, primary key
                    var createdAt = existingInput.CreatedAt;

                    // 🔹 Make sure the DTO's key + CreatedAt match the tracked entity
                    //    so SetValues does NOT try to change the PK.
                    if (dto.InputEntity == null)
                    {
                        _logger.LogWarning("InputEntity is null for ExistingInputId {Id} during UpdateRangeAsync", dto.ExistingInputId);
                        continue;
                    }

                    dto.InputEntity.BagfilterInputId = dto.ExistingInputId;
                    dto.InputEntity.CreatedAt = createdAt; // we don't want client overriding CreatedAt

                    // Copy scalar values from InputEntity → existingInput
                    var inputEntry = dbContext.Entry(existingInput);
                    inputEntry.CurrentValues.SetValues(dto.InputEntity);

                    // 🔹 Make absolutely sure EF does NOT treat PK as modified
                    inputEntry.Property(x => x.BagfilterInputId).IsModified = false;

                    // restore invariants
                    existingInput.CreatedAt = createdAt;
                    existingInput.UpdatedAt = DateTime.Now;

                    // Re-point master FK if a new master was created/resolved
                    if (dto.ResolvedNewMasterId > 0)
                    {
                        existingInput.BagfilterMasterId = dto.ResolvedNewMasterId;
                        existingInput.BagfilterMaster = null; // avoid EF trying to insert/attach from DTO
                    }
                    else if (dto.ExistingMasterId > 0)
                    {
                        // keep/restore the existing master FK
                        existingInput.BagfilterMasterId = dto.ExistingMasterId;
                        existingInput.BagfilterMaster = null;
                    }

                    // 3b) Optionally update the existing BagfilterMaster row
                    if (dto.MasterEntityToUpdate != null
                        && dto.ResolvedNewMasterId == 0
                        && dto.ExistingMasterId > 0)
                    {
                        if (masterMap.TryGetValue(dto.ExistingMasterId, out var existingMaster))
                        {
                            var masterCreatedAt = existingMaster.CreatedAt;

                            var masterEntry = dbContext.Entry(existingMaster);

                            // same pattern: never try to change PK / CreatedAt via SetValues
                            dto.MasterEntityToUpdate.BagfilterMasterId = dto.ExistingMasterId;
                            dto.MasterEntityToUpdate.CreatedAt = masterCreatedAt;

                            masterEntry.CurrentValues.SetValues(dto.MasterEntityToUpdate);
                            masterEntry.Property(x => x.BagfilterMasterId).IsModified = false;

                            existingMaster.CreatedAt = masterCreatedAt;
                            existingMaster.UpdatedAt = DateTime.Now;
                        }
                        else
                        {
                            _logger.LogWarning(
                                "BagfilterMaster with Id {Id} not found for update during UpdateRangeAsync",
                                dto.ExistingMasterId);
                        }
                    }
                }


                // -------- 4) Persist all changes in one shot ----------
                await dbContext.SaveChangesAsync(ct);
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

                if (createdInputIds.Count != pairsList.Count)
                    throw new InvalidOperationException("Repository must return one input id per pair in the same order.");

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
    
using System.Globalization;
using System.Linq;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Application.DTOs.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Application.DTOs.SkyCiv;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterInputs;

using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterMasterEntity;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Core.Entities.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Core.Entities.BOM.Painting_Cost;
using IonFiltra.BagFilters.Core.Entities.EnquiryEntity;
using IonFiltra.BagFilters.Core.Interfaces.Bagfilters.BagfilterMasters;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Painting_Cost;
using IonFiltra.BagFilters.Core.Interfaces.SkyCiv;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.BagfilterInputs
{
    public class BagfilterInputService : IBagfilterInputService
    {
        private readonly IBagfilterInputRepository _repository;
        private readonly IBagfilterMasterRepository _masterRepository;
        private readonly ISkyCivAnalysisService _skyCivService;
        private readonly ILogger<BagfilterInputService> _logger;
        // new: specific repo for weight summary (inject similar repos for other child entities)
        private readonly IWeightSummaryRepository _weightSummaryRepository;
        private readonly IProcessInfoRepository _processInfoRepository;
        private readonly ICageInputsRepository _cageInputsRepository;
        private readonly IBagSelectionRepository _bagSelectionRepository;
        private readonly IStructureInputsRepository _structureInputsRepository;
        private readonly ICapsuleInputsRepository _capsuleInputsRepository;
        private readonly ICasingInputsRepository _casingInputsRepository;
        private readonly IHopperInputsRepository _hopperInputsRepository;
        private readonly ISupportStructureRepository _supportStructureRepository;
        private readonly IAccessGroupRepository _accessGroupRepository;
        private readonly IRoofDoorRepository _roofDoorRepository;
        private readonly IPaintingAreaRepository _paintingAreaRepository;

        private readonly IBillOfMaterialRepository _billOfMaterialRepository;
        private readonly IPaintingCostRepository _paintingCostRepository;
        // new: registry of handlers keyed by DTO property name (case-insensitive)
        private readonly Dictionary<string, Func<BagfilterInputMainDto, int, CancellationToken, Task>> _childHandlers;
        private readonly Dictionary<string, Func<BagfilterInputMainDto, int, CancellationToken, Task>> _childUpdateHandlers;





        public BagfilterInputService(
            IBagfilterInputRepository repository,
            IBagfilterMasterRepository masterRepository,
            ILogger<BagfilterInputService> logger,
            ISkyCivAnalysisService skyCivService,
            IWeightSummaryRepository weightSummaryRepository,
            IProcessInfoRepository processInfoRepository,
            ICageInputsRepository cageInputsRepository,
            IBagSelectionRepository bagSelectionRepository,
            IStructureInputsRepository structureInputsRepository,
            ICapsuleInputsRepository capsuleInputsRepository,
            ICasingInputsRepository casingInputsRepository,
            IHopperInputsRepository hopperInputsRepository,
            ISupportStructureRepository supportStructureRepository,
            IAccessGroupRepository accessGroupRepository,
            IRoofDoorRepository roofDoorRepository,
            IPaintingAreaRepository paintingAreaRepository,
            IBillOfMaterialRepository billOfMaterialRepository,
            IPaintingCostRepository paintingCostRepository
        )
        {
            _repository = repository;
            _masterRepository = masterRepository;
            _logger = logger;
            _skyCivService = skyCivService;

            _weightSummaryRepository = weightSummaryRepository ?? throw new ArgumentNullException(nameof(weightSummaryRepository));
            _processInfoRepository = processInfoRepository ?? throw new ArgumentNullException(nameof(processInfoRepository));
            _cageInputsRepository = cageInputsRepository ?? throw new ArgumentNullException(nameof(cageInputsRepository));
            _bagSelectionRepository = bagSelectionRepository ?? throw new ArgumentNullException(nameof(bagSelectionRepository));
            _structureInputsRepository = structureInputsRepository ?? throw new ArgumentNullException(nameof(structureInputsRepository));
            _capsuleInputsRepository = capsuleInputsRepository ?? throw new ArgumentNullException(nameof(capsuleInputsRepository));
            _casingInputsRepository = casingInputsRepository ?? throw new ArgumentNullException(nameof(casingInputsRepository));
            _hopperInputsRepository = hopperInputsRepository ?? throw new ArgumentNullException(nameof(hopperInputsRepository));
            _supportStructureRepository = supportStructureRepository ?? throw new ArgumentNullException(nameof(supportStructureRepository));
            _accessGroupRepository = accessGroupRepository ?? throw new ArgumentNullException(nameof(accessGroupRepository));
            _roofDoorRepository = roofDoorRepository ?? throw new ArgumentNullException(nameof(roofDoorRepository));
            _paintingAreaRepository = paintingAreaRepository ?? throw new ArgumentNullException(nameof(paintingAreaRepository));
            _billOfMaterialRepository = billOfMaterialRepository ?? throw new ArgumentNullException(nameof(billOfMaterialRepository));
            _paintingCostRepository = paintingCostRepository ?? throw new ArgumentNullException(nameof(paintingCostRepository));

            //// initialize handler registry
            //_childHandlers = new Dictionary<string, Func<BagfilterInputMainDto, int, CancellationToken, Task>>(StringComparer.OrdinalIgnoreCase)
            //{
            //    // key is the DTO property name (e.g. "WeightSummary" or "weightSummary" - registry is case-insensitive)
            //    ["weightSummary"] = HandleWeightSummaryAsync,
            //    ["processInfo"] = HandleProcessInfoAsync,
            //    ["cageInputs"] = HandleCageInputsAsync,
            //    ["bagSelection"] = HandleBagSelectionAsync,
            //    ["structureInputs"] = HandleStructureInputsAsync,
            //    ["capsuleInputs"] = HandleCapsuleInputsAsync,
            //    ["casingInputs"] = HandleCasingInputsAsync,
            //    ["hopperInputs"] = HandleHopperInputsAsync,
            //    ["supportStructure"] = HandleSupportStructureAsync,
            //    ["accessGroup"] = HandleAccessGroupAsync,
            //    ["roofDoor"] = HandleRoofDoorAsync,
            //    ["paintingArea"] = HandlePaintingAreaAsync,
            //    ["billOfMaterial"] = HandleBillOfMaterialAsync,
            //    ["paintingCost"] = HandlePaintingCostAsync,
            //};

            _childHandlers = new Dictionary<string, Func<BagfilterInputMainDto, int, CancellationToken, Task>>(StringComparer.OrdinalIgnoreCase);
            _childUpdateHandlers = new Dictionary<string, Func<BagfilterInputMainDto, int, CancellationToken, Task>>(StringComparer.OrdinalIgnoreCase);

            // Register handlers. Use the helper to keep it clean.
            // For each area, we pass the existing Add handler and the corresponding Update handler.
            RegisterChildHandler("weightSummary", HandleWeightSummaryAsync);
            RegisterChildHandler("processInfo", HandleProcessInfoAsync);
            RegisterChildHandler("cageInputs", HandleCageInputsAsync);
            RegisterChildHandler("bagSelection", HandleBagSelectionAsync);
            RegisterChildHandler("structureInputs", HandleStructureInputsAsync);
            RegisterChildHandler("capsuleInputs", HandleCapsuleInputsAsync);
            RegisterChildHandler("casingInputs", HandleCasingInputsAsync);
            RegisterChildHandler("hopperInputs", HandleHopperInputsAsync);
            RegisterChildHandler("supportStructure", HandleSupportStructureAsync);
            RegisterChildHandler("accessGroup", HandleAccessGroupAsync);
            RegisterChildHandler("roofDoor", HandleRoofDoorAsync);
            RegisterChildHandler("paintingArea", HandlePaintingAreaAsync);
            RegisterChildHandler("billOfMaterial", HandleBillOfMaterialAsync);
            RegisterChildHandler("paintingCost", HandlePaintingCostAsync);

        }

        

        public async Task<BagfilterInputMainDto> GetByMasterId(int masterId)
        {
            _logger.LogInformation("Fetching BagfilterInput for master id {id}", masterId);
            var entity = await _repository.GetByMasterId(masterId);
            return BagfilterInputMapper.ToMainDto(entity);
        }

        public async Task<List<BagfilterInputMainDto>> GetAllByEnquiryId(int enquiryId)
        {
            var list = await _repository.GetAllByEnquiryId(enquiryId);
            return list.Select(BagfilterInputMapper.ToMainDto).ToList();
        }


        public async Task<int> AddAsync(BagfilterInputMainDto dto)
        {
            _logger.LogInformation("Adding BagfilterInput for Id {id}", dto.BagfilterMasterId);
            var entity = BagfilterInputMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.BagfilterInputId;
        }

        public async Task UpdateAsync(BagfilterInputMainDto dto)
        {
            _logger.LogInformation("Updating BagfilterInput for Id {id}", dto.BagfilterInputId);
            var entity = BagfilterInputMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }


        //new method 
        public async Task<AddRangeResultDto> AddRangeAsync(List<BagfilterInputMainDto> dtos, CancellationToken ct)
        {
            if (dtos == null || dtos.Count == 0) return new AddRangeResultDto();

            // 1) Map DTOs -> pairs (master + input) and collect group keys and mapping info
            var pairs = new List<(BagfilterMaster Master, BagfilterInput Input)>(dtos.Count);

            // keep parallel array to map pair index -> groupKey
            var pairGroupKeys = new List<string>(dtos.Count);

            // groupKey -> list of pairIndices (so we know which pairs belong to a group)
            var groups = new Dictionary<string, List<int>>();

            for (int idx = 0; idx < dtos.Count; idx++)
            {
                var dto = dtos[idx];

                if (dto.BagfilterMaster == null && dto.BagfilterMasterId <= 0)
                    throw new ArgumentException("Each item must include BagfilterMaster data or an existing BagfilterMasterId.");

                var masterEntity = dto.BagfilterMaster != null
                    ? new BagfilterMaster
                    {
                        BagFilterName = dto.BagfilterMaster.BagFilterName,
                        Status = dto.BagfilterMaster.Status,
                        Revision = dto.BagfilterMaster.Revision,
                        AssignmentId = dto.BagfilterMaster.AssignmentId, // if present
                        CreatedAt = DateTime.UtcNow,
                        EnquiryId = dto.BagfilterMaster.EnquiryId // if you put EnquiryId on master; otherwise dto may include EnquiryId on input
                    }
                    : new BagfilterMaster
                    {
                        BagfilterMasterId = dto.BagfilterMasterId,
                        CreatedAt = DateTime.UtcNow
                    };

                var inputDto = dto.BagfilterInput ?? throw new ArgumentException("BagfilterInput is required.");
                var inputEntity = MapBagfilterInputDtoToEntity(inputDto);
                inputEntity.CreatedAt = DateTime.UtcNow;

                if (masterEntity.BagfilterMasterId > 0)
                    inputEntity.BagfilterMasterId = masterEntity.BagfilterMasterId;
                else
                    inputEntity.BagfilterMaster = masterEntity;

                pairs.Add((masterEntity, inputEntity));

                // Build group key for grouping duplicates (Location + the 4 numeric fields)
                string groupKey = BuildGroupKey(inputEntity.Location, inputEntity.No_Of_Column, inputEntity.Ground_Clearance, inputEntity.Bag_Per_Row, inputEntity.Number_Of_Rows);

                pairGroupKeys.Add(groupKey);

                if (!groups.TryGetValue(groupKey, out var list)) { list = new List<int>(); groups[groupKey] = list; }
                list.Add(idx);
            }

            // 2) Prepare group representative list for database candidate lookup
            var groupKeys = groups.Keys
                .Select(k =>
                {
                    // parse one representative from groups: use first pair index
                    var firstIdx = groups[k].First();
                    var repInput = pairs[firstIdx].Input;
                    return new GroupKey
                    {
                        Location = repInput.Location,
                        No_Of_Column = repInput.No_Of_Column,
                        Ground_Clearance = repInput.Ground_Clearance,
                        Bag_Per_Row = repInput.Bag_Per_Row,
                        Number_Of_Rows = repInput.Number_Of_Rows
                    };
                })
                .ToList();

            // 3) Fetch candidate existing DB rows for all group keys (single DB query)
            var candidates = await _repository.FindCandidatesByGroupKeysAsync(groupKeys); // returns BagfilterInput with BagfilterMaster included

            // --- Build Group labels (Group 1, Group 2, ...) ---
            var groupKeysList = groups.Keys.ToList();
            var groupIdByKey = new Dictionary<string, string>(groupKeysList.Count);
            for (int i = 0; i < groupKeysList.Count; i++)
            {
                groupIdByKey[groupKeysList[i]] = $"Group {i + 1}";
            }

            // Prepare container for results and Enquiry ids to fetch
            var groupMatchMap = new Dictionary<string, BagfilterMatchDto>(groups.Count);
            var matchedEnquiryIds = new HashSet<int>();

            // For each group, create a placeholder DTO and try to find a DB match
            foreach (var groupKey in groupKeysList)
            {
                var representativePairIndex = groups[groupKey].First();
                var repInput = pairs[representativePairIndex].Input;
                var incomingEnquiryId = dtoHasEnquiryId(pairs[representativePairIndex]);

                // Placeholder for group — ensures we return every group even if unmatched
                var placeholder = new BagfilterMatchDto
                {
                    GroupId = groupIdByKey[groupKey],
                    Location = repInput.Location,
                    BagfilterInputId = 0,
                    BagfilterMasterId = 0,
                    AssignmentId = null,
                    EnquiryId = null,
                    BagFilterName = null,
                    CustomerName = null,
                    CreatedAt = null,
                    No_Of_Column = repInput.No_Of_Column,
                    Ground_Clearance = repInput.Ground_Clearance,
                    Bag_Per_Row = repInput.Bag_Per_Row,
                    Number_Of_Rows = repInput.Number_Of_Rows,
                    IsMatched = false
                };

                // Find first candidate match in DB (exclude same enquiry if incomingEnquiryId present)
                var matchCandidate = candidates
                    .FirstOrDefault(c =>
                        string.Equals(c.Location ?? "", repInput.Location ?? "", StringComparison.InvariantCultureIgnoreCase)
                        && EqualsNullable(c.No_Of_Column, repInput.No_Of_Column)
                        && EqualsNullable(c.Ground_Clearance, repInput.Ground_Clearance)
                        && EqualsNullable(c.Bag_Per_Row, repInput.Bag_Per_Row)
                        && EqualsNullable(c.Number_Of_Rows, repInput.Number_Of_Rows)
                        && (incomingEnquiryId == null || c.EnquiryId != incomingEnquiryId)
                    );

                if (matchCandidate != null)
                {
                    placeholder.IsMatched = true;
                    placeholder.BagfilterInputId = matchCandidate.BagfilterInputId;
                    placeholder.BagfilterMasterId = matchCandidate.BagfilterMasterId;
                    placeholder.AssignmentId = matchCandidate.BagfilterMaster?.AssignmentId;
                    placeholder.EnquiryId = matchCandidate.EnquiryId;
                    placeholder.BagFilterName = matchCandidate.BagfilterMaster?.BagFilterName;
                    // Collect enquiry ids for enrichment (only non-null)
                    if (matchCandidate.EnquiryId.HasValue)
                        matchedEnquiryIds.Add(matchCandidate.EnquiryId.Value);
                }

                groupMatchMap[groupKey] = placeholder;
            }

            // Fetch all Enquiries for matched groups in one DB call
            Dictionary<int, Enquiry> enquiryMap = new();
            if (matchedEnquiryIds.Any())
            {
                // ensure your repository method exists and returns a dictionary keyed by Enquiry.Id
                var enquiryDict = await _repository.GetEnquiriesByIdsAsync(matchedEnquiryIds);
                if (enquiryDict != null && enquiryDict.Any())
                    enquiryMap = enquiryDict;
            }

            // Enrich DTOs with CustomerName and CreatedAt where possible
            foreach (var kv in groupMatchMap.ToList())
            {
                var matchDto = kv.Value;
                if (matchDto.IsMatched && matchDto.EnquiryId.HasValue && enquiryMap.TryGetValue(matchDto.EnquiryId.Value, out var enq))
                {
                    matchDto.CustomerName = enq.Customer;
                    matchDto.CreatedAt = enq.CreatedAt;
                }
            }

            // Build matchMappingByPairIndex (pairIndex -> matched ids) only for groups that had a match
            var matchMappingByPairIndex = new Dictionary<int, (int matchedBagfilterInputId, int matchedBagfilterMasterId)>();
            foreach (var kv in groups)
            {
                var groupKey = kv.Key;
                if (!groupMatchMap.TryGetValue(groupKey, out var matchDto)) continue;
                if (!matchDto.IsMatched) continue; // only map when matched

                // For every pair index in this group, mark mapping to the matched candidate
                foreach (var pairIndex in kv.Value)
                {
                    matchMappingByPairIndex[pairIndex] = (matchDto.BagfilterInputId, matchDto.BagfilterMasterId);
                }
            }


            // 6) Insert all (masters + inputs) and apply match mapping inside repository in same transaction
            var createdInputIds = await _repository.AddRangeAsync(pairs, matchMappingByPairIndex);

            // Batch fetch all inputs (one DB call)
            var inputs = await _repository.GetByIdsAsync(createdInputIds);

            // Build a dictionary for quick lookup
            var masterMap = inputs.ToDictionary(x => x.BagfilterInputId, x => x.BagfilterMasterId);

           
            // 7) Build child tasks
            var childTasks = new List<Task>();

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];
                var createdInputId = createdInputIds.ElementAtOrDefault(i);
                var masterId = masterMap.TryGetValue(createdInputId, out var mid) ? mid : 0;

                foreach (var handlerKv in _childHandlers)
                {
                    var task = handlerKv.Value(dto, masterId, ct);
                    if (task != null)
                        childTasks.Add(task);
                }
            }

            // 8) WAIT for all child tasks BEFORE returning
            if (childTasks.Count > 0)
            {
                try
                {
                    await Task.WhenAll(childTasks);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Child handler failure.");
                    throw; // Required to avoid partial writes
                }
            }




            //8)
            var matchesList = groupKeysList.Select(k => groupMatchMap[k]).ToList();
            var matchedGroupsCount = matchesList.Count(m => m.IsMatched);
            var totalGroups = matchesList.Count;
            var matchedItemsCount = groups.Where(g => groupMatchMap[g.Key].IsMatched)
                                          .Sum(g => g.Value.Count); // sum of sizes of matched groups
                                                                    // 7) Prepare result DTO: created ids + matches summary
            var result = new AddRangeResultDto
            {
                CreatedBagfilterInputIds = createdInputIds,
                Matches = matchesList,
                Message = matchedGroupsCount > 0 ? "Bagfilter Match Found" : null,
                MatchedItemsCount = matchedItemsCount,
                TotalGroupsCount = totalGroups,            // optional but handy
                MatchedGroupsCount = matchedGroupsCount   // optional
            };

            var unmatchedPairIndices = new List<int>();
            for (int pairIndex = 0; pairIndex < pairs.Count; pairIndex++)
            {
                var groupKey = pairGroupKeys[pairIndex];
                if (!groupMatchMap.TryGetValue(groupKey, out var gm)) continue;
                if (!gm.IsMatched)
                {
                    unmatchedPairIndices.Add(pairIndex);
                }
            }

            // If nothing to run, skip
            //if (unmatchedPairIndices.Any())
            //{
            //    // limiter to avoid flooding SkyCiv — adjust concurrency as needed
            //    var maxConcurrency = 2; // or 1 if you want strictly sequential
            //    using var sem = new SemaphoreSlim(maxConcurrency);
            //    var tasks = new List<Task>();

            //    foreach (var pairIndex in unmatchedPairIndices)
            //    {
            //        await sem.WaitAsync(); // respect cancellation
            //        tasks.Add(Task.Run(async () =>
            //        {
            //            try
            //            {
            //                // get model to send
            //                var inputEntity = pairs[pairIndex].Input;
            //                // assume the DTO or entity stored s3dModel as JObject (if not, adapt to convert string->JObject)
            //                var s3dModel = inputEntity.S3dModel != null
            //                    ? JObject.Parse(inputEntity.S3dModel) // if stored as string
            //                    : null;

            //                if (s3dModel == null)
            //                {
            //                    _logger?.LogWarning("No S3D model present for pairIndex {idx}", pairIndex);
            //                    return;
            //                }

            //                // Call analysis service
            //                AnalysisResponseDto analysisResponse;
            //                try
            //                {
            //                    analysisResponse = await _skyCivService.RunAnalysisAsync(s3dModel, ct);
            //                }
            //                catch (Exception ex)
            //                {
            //                    _logger?.LogError(ex, "SkyCiv analysis failed for pairIndex {idx}", pairIndex);
            //                    return;
            //                }

            //                if (analysisResponse != null && analysisResponse.Status == "Succeeded")
            //                {
            //                    // Persist model data and session id to the DB row that was created earlier
            //                    var createdId = createdInputIds[pairIndex]; // createdInputIds indexes map to pairs
            //                    var modelJson = analysisResponse.ModelData?.ToString(Formatting.None) ?? string.Empty;
            //                    var sessionId = analysisResponse.SessionId;

            //                    // Call repository to update the row (you need to implement this)
            //                    await _repository.UpdateS3dModelAsync(createdId, modelJson, sessionId);

            //                    // Optionally update the groupMatchMap or matchesList for return DTOs
            //                    var groupKey = pairGroupKeys[pairIndex];
            //                    if (groupMatchMap.TryGetValue(groupKey, out var gm2))
            //                    {
            //                        // annotate placeholder to indicate analysis was run
            //                        gm2.IsMatched = false; // still unmatched, but tried analysis

            //                    }
            //                }
            //                else
            //                {
            //                    _logger?.LogWarning("Analysis didn't succeed for pairIndex {idx}. Status: {status}", pairIndex, analysisResponse?.Status);
            //                }
            //            }
            //            finally
            //            {
            //                sem.Release();
            //            }
            //        }));
            //    }

            //    // Wait for all tasks to complete
            //    await Task.WhenAll(tasks);
            //}

            if (unmatchedPairIndices.Any())
            {
                const int maxConcurrency = 2;
                using var sem = new SemaphoreSlim(maxConcurrency);
                var tasks = new List<Task>();

                foreach (var originalPairIndex in unmatchedPairIndices)
                {
                    // create a local copy to avoid closed-over loop variable bugs
                    var pairIndex = originalPairIndex;

                    // start the processing task directly (no Task.Run)
                    tasks.Add(ProcessPairAsync(pairIndex, pairs, pairGroupKeys, createdInputIds, groupMatchMap, sem, ct));
                }

                try
                {
                    await Task.WhenAll(tasks).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "One or more SkyCiv analysis tasks failed.");
                    throw; // or aggregate and return a partial result as your policy dictates
                }
            }

            


            return result;
        }




        //public async Task<UpdateRangeResultDto> UpdateRangeAsync(List<BagfilterInputMainDto> dtos, CancellationToken ct)
        //{
        //    if (dtos == null || dtos.Count == 0) return new UpdateRangeResultDto();

        //    // Step A: Map DTOs -> desired pair state, build grouping keys & groups (same as AddRange)
        //    var pairs = new List<(BagfilterMaster Master, BagfilterInput Input)>(dtos.Count);
        //    var pairGroupKeys = new List<string>(dtos.Count);
        //    var groups = new Dictionary<string, List<int>>();

        //    for (int idx = 0; idx < dtos.Count; idx++)
        //    {
        //        var dto = dtos[idx];

        //        if (dto.BagfilterMaster == null && dto.BagfilterMasterId <= 0)
        //            throw new ArgumentException("Each item must include BagfilterMaster data or an existing BagfilterMasterId.");

        //        var masterEntity = dto.BagfilterMaster != null
        //            ? new BagfilterMaster
        //            {
        //                BagFilterName = dto.BagfilterMaster.BagFilterName,
        //                Status = dto.BagfilterMaster.Status,
        //                Revision = dto.BagfilterMaster.Revision,
        //                AssignmentId = dto.BagfilterMaster.AssignmentId,
        //                CreatedAt = DateTime.UtcNow,
        //                EnquiryId = dto.BagfilterMaster.EnquiryId
        //            }
        //            : new BagfilterMaster
        //            {
        //                BagfilterMasterId = dto.BagfilterMasterId,
        //                CreatedAt = DateTime.UtcNow
        //            };

        //        var inputDto = dto.BagfilterInput ?? throw new ArgumentException("BagfilterInput is required.");
        //        var inputEntity = MapBagfilterInputDtoToEntity(inputDto);
        //        inputEntity.CreatedAt = DateTime.UtcNow;

        //        if (masterEntity.BagfilterMasterId > 0)
        //            inputEntity.BagfilterMasterId = masterEntity.BagfilterMasterId;
        //        else
        //            inputEntity.BagfilterMaster = masterEntity;

        //        pairs.Add((masterEntity, inputEntity));

        //        var groupKey = BuildGroupKey(inputEntity.Location, inputEntity.No_Of_Column, inputEntity.Ground_Clearance, inputEntity.Bag_Per_Row, inputEntity.Number_Of_Rows);
        //        pairGroupKeys.Add(groupKey);

        //        if (!groups.TryGetValue(groupKey, out var list)) { list = new List<int>(); groups[groupKey] = list; }
        //        list.Add(idx);
        //    }

        //    // Step B: Find candidates for groups
        //    var groupKeys = groups.Keys
        //        .Select(k =>
        //        {
        //            var firstIdx = groups[k].First();
        //            var repInput = pairs[firstIdx].Input;
        //            return new GroupKey
        //            {
        //                Location = repInput.Location,
        //                No_Of_Column = repInput.No_Of_Column,
        //                Ground_Clearance = repInput.Ground_Clearance,
        //                Bag_Per_Row = repInput.Bag_Per_Row,
        //                Number_Of_Rows = repInput.Number_Of_Rows
        //            };
        //        })
        //        .ToList();

        //    var candidates = await _repository.FindCandidatesByGroupKeysAsync(groupKeys); // BagfilterInput + Master included

        //    // Step C: For each group, create placeholder and decide match/update vs insert
        //    var groupKeysList = groups.Keys.ToList();
        //    var groupIdByKey = new Dictionary<string, string>(groupKeysList.Count);
        //    for (int i = 0; i < groupKeysList.Count; i++) groupIdByKey[groupKeysList[i]] = $"Group {i + 1}";

        //    var groupMatchMap = new Dictionary<string, BagfilterMatchDto>(groups.Count);
        //    var matchedEnquiryIds = new HashSet<int>();

        //    foreach (var groupKey in groupKeysList)
        //    {
        //        var repPairIdx = groups[groupKey].First();
        //        var repInput = pairs[repPairIdx].Input;
        //        var incomingEnquiryId = dtoHasEnquiryId(pairs[repPairIdx]);

        //        var placeholder = new BagfilterMatchDto
        //        {
        //            GroupId = groupIdByKey[groupKey],
        //            Location = repInput.Location,
        //            BagfilterInputId = 0,
        //            BagfilterMasterId = 0,
        //            AssignmentId = null,
        //            EnquiryId = null,
        //            BagFilterName = null,
        //            CustomerName = null,
        //            CreatedAt = null,
        //            No_Of_Column = repInput.No_Of_Column,
        //            Ground_Clearance = repInput.Ground_Clearance,
        //            Bag_Per_Row = repInput.Bag_Per_Row,
        //            Number_Of_Rows = repInput.Number_Of_Rows,
        //            IsMatched = false
        //        };

        //        var matchCandidate = candidates
        //            .FirstOrDefault(c =>
        //                string.Equals(c.Location ?? "", repInput.Location ?? "", StringComparison.InvariantCultureIgnoreCase)
        //                && EqualsNullable(c.No_Of_Column, repInput.No_Of_Column)
        //                && EqualsNullable(c.Ground_Clearance, repInput.Ground_Clearance)
        //                && EqualsNullable(c.Bag_Per_Row, repInput.Bag_Per_Row)
        //                && EqualsNullable(c.Number_Of_Rows, repInput.Number_Of_Rows)
        //                && (incomingEnquiryId == null || c.EnquiryId != incomingEnquiryId)
        //            );

        //        if (matchCandidate != null)
        //        {
        //            placeholder.IsMatched = true;
        //            placeholder.BagfilterInputId = matchCandidate.BagfilterInputId;
        //            placeholder.BagfilterMasterId = matchCandidate.BagfilterMasterId;
        //            placeholder.AssignmentId = matchCandidate.BagfilterMaster?.AssignmentId;
        //            placeholder.EnquiryId = matchCandidate.EnquiryId;
        //            placeholder.BagFilterName = matchCandidate.BagfilterMaster?.BagFilterName;
        //            if (matchCandidate.EnquiryId.HasValue) matchedEnquiryIds.Add(matchCandidate.EnquiryId.Value);
        //        }

        //        groupMatchMap[groupKey] = placeholder;
        //    }

        //    // Enrich enquiries if any (same as AddRange)
        //    Dictionary<int, Enquiry> enquiryMap = new();
        //    if (matchedEnquiryIds.Any())
        //    {
        //        var enquiryDict = await _repository.GetEnquiriesByIdsAsync(matchedEnquiryIds);
        //        if (enquiryDict != null && enquiryDict.Any()) enquiryMap = enquiryDict;
        //    }
        //    foreach (var kv in groupMatchMap.ToList())
        //    {
        //        var matchDto = kv.Value;
        //        if (matchDto.IsMatched && matchDto.EnquiryId.HasValue && enquiryMap.TryGetValue(matchDto.EnquiryId.Value, out var enq))
        //        {
        //            matchDto.CustomerName = enq.Customer;
        //            matchDto.CreatedAt = enq.CreatedAt;
        //        }
        //    }

        //    // Step D: Decide per-pair operations
        //    // We'll prepare:
        //    // - updates: pairIndex -> (existingInputId, existingMasterId, updatedInputEntity, updatedMasterEntity OR newMasterToInsert)
        //    // - inserts: list of pairs (master+input) to create (for unmatched groups or when new master requested)
        //    var updates = new Dictionary<int, UpdateIntent>(); // UpdateIntent is defined below
        //    var inserts = new List<(BagfilterMaster Master, BagfilterInput Input, int originalPairIndex)>();

        //    for (int pairIndex = 0; pairIndex < pairs.Count; pairIndex++)
        //    {
        //        var groupKey = pairGroupKeys[pairIndex];
        //        var pair = pairs[pairIndex];
        //        var matchDto = groupMatchMap.TryGetValue(groupKey, out var m) ? m : null;

        //        if (matchDto != null && matchDto.IsMatched && matchDto.BagfilterInputId > 0)
        //        {
        //            // Update path: we have an existing row to update
        //            var existingInputId = matchDto.BagfilterInputId;
        //            var existingMasterId = matchDto.BagfilterMasterId; // may be zero/nullable

        //            // Determine if incoming master info indicates a "new master" (distinct BagFilterName or explicit masterId mismatch)
        //            var incomingMasterName = pair.Master?.BagFilterName;
        //            var incomingMasterId = pair.Master?.BagfilterMasterId ?? 0;

        //            var requiresNewMaster = false;
        //            if (!string.IsNullOrWhiteSpace(incomingMasterName))
        //            {
        //                // if the incoming name differs from matched one -> create new master
        //                if (!string.Equals(incomingMasterName, matchDto.BagFilterName ?? "", StringComparison.InvariantCultureIgnoreCase))
        //                {
        //                    requiresNewMaster = true;
        //                }
        //            }
        //            // if incoming explicitly references a masterId that is not the matched master, treat as new (or verify existence)
        //            if (incomingMasterId > 0 && incomingMasterId != (matchDto.BagfilterMasterId))
        //            {
        //                // we should verify whether incomingMasterId exists; for simplicity treat it as "new master reference" (create new master copy)
        //                requiresNewMaster = true;
        //            }

        //            if (requiresNewMaster)
        //            {
        //                // Create a new master entity (copy incoming master data if provided; otherwise minimal)
        //                var newMaster = pair.Master ?? new BagfilterMaster
        //                {
        //                    BagFilterName = incomingMasterName ?? ("Master_For_Input_" + existingInputId),
        //                    CreatedAt = DateTime.UtcNow
        //                };

        //                // Prepare an insert for the new master and an update for input pointing to new master after insert.
        //                // We'll insert new master and new input OR update input to reference new master id after insertion.
        //                // Simpler path: insert a new master and then set input.BagfilterMasterId to new master id in a subsequent step.
        //                updates[pairIndex] = new UpdateIntent
        //                {
        //                    PairIndex = pairIndex,
        //                    ExistingInputId = existingInputId,
        //                    ExistingMasterId = existingMasterId,
        //                    InputToUpdate = pair.Input,
        //                    NewMasterToCreate = newMaster,
        //                    UpdateMaster = false // because we create new master
        //                };
        //            }
        //            else
        //            {
        //                // No new master required — maybe update existing master fields and/or input fields
        //                updates[pairIndex] = new UpdateIntent
        //                {
        //                    PairIndex = pairIndex,
        //                    ExistingInputId = existingInputId,
        //                    ExistingMasterId = existingMasterId,
        //                    InputToUpdate = pair.Input,
        //                    MasterToUpdate = pair.Master, // may be a shell with fields to update
        //                    UpdateMaster = pair.Master != null && pair.Master.BagfilterMasterId == 0 // flag to update existing master instead of creating new? keep conservative
        //                };
        //            }
        //        }
        //        else
        //        {
        //            // Insert path — either unmatched group or explicit new master requested; create new master+input pair
        //            inserts.Add((pairs[pairIndex].Master, pairs[pairIndex].Input, pairIndex));
        //        }
        //    }

        //    // Step E: Persist updates and inserts
        //    // Strategy:
        //    // 1) For updates that require new master creation, create those masters first (so we can attach their IDs to inputs)
        //    // 2) For updates that simply update existing records, call repository.UpdateRangeAsync (you must implement)
        //    // 3) For inserts, call repository.AddRangeAsync (existing method). Make sure to keep insertion order aligned with original pair indices.
        //    // We'll implement a simple, safe order.

        //    // --- E1: Create all new masters required by updates (so we can update input.masterId) ---
        //    var newMastersToCreate = new List<(int pairIndex, BagfilterMaster master)>();
        //    foreach (var kv in updates)
        //    {
        //        var intent = kv.Value;
        //        if (intent.NewMasterToCreate != null)
        //            newMastersToCreate.Add((kv.Key, intent.NewMasterToCreate));
        //    }

        //    // Persist new masters and collect mapping pairIndex -> createdMasterId
        //    var createdMasterIdsForUpdates = new Dictionary<int, int>();
        //    if (newMastersToCreate.Any())
        //    {
        //        // Create masters in a single transaction using repo helper (you should add repository method AddMastersAsync)
        //        var mastersOnly = newMastersToCreate.Select(x => x.master).ToList();
        //        var createdMasterIds = await _masterRepository.AddMastersAsync(mastersOnly, ct); // returns list aligned with mastersOnly
        //        for (int i = 0; i < newMastersToCreate.Count; i++)
        //        {
        //            createdMasterIdsForUpdates[newMastersToCreate[i].pairIndex] = createdMasterIds[i];
        //        }
        //    }

        //    // Apply created masterIds into update intents (so inputs point to new master)
        //    foreach (var kv in updates)
        //    {
        //        var intent = kv.Value;
        //        if (intent.NewMasterToCreate != null && createdMasterIdsForUpdates.TryGetValue(kv.Key, out var mid))
        //        {
        //            intent.ResolvedNewMasterId = mid;
        //            // also set the InputToUpdate's BagfilterMasterId so repository update knows how to set FK
        //            intent.InputToUpdate.BagfilterMasterId = mid;
        //            intent.InputToUpdate.BagfilterMaster = null; // avoid EF trying to insert the master again
        //        }
        //    }

        //    // --- E2: Execute updates against existing rows (batch)
        //    var updatesList = updates.Values.ToList();
        //    if (updatesList.Any())
        //    {
        //        // repository.UpdateRangeAsync should accept a list of update DTOs or entities and apply changes in a transaction.
        //        // We construct lightweight DTOs: (ExistingInputId, InputToUpdate, ExistingMasterId, MasterToUpdate, ResolvedNewMasterId)
        //        var updateDtos = updatesList.Select(u => new RepositoryInputUpdateDto
        //        {
        //            ExistingInputId = u.ExistingInputId,
        //            InputEntity = u.InputToUpdate,
        //            ExistingMasterId = u.ExistingMasterId,
        //            MasterEntityToUpdate = u.MasterToUpdate,
        //            ResolvedNewMasterId = u.ResolvedNewMasterId
        //        }).ToList();

        //        // Call repository to apply updates in a single transaction
        //        await _repository.UpdateRangeAsync(updateDtos);
        //    }

        //    // --- E3: Perform inserts for unmatched pairs (use your existing AddRangeAsync repo method)
        //    var insertPairsAligned = inserts.Select(t =>
        //    {
        //        // keep same master+input objects as earlier; repository.AddRangeAsync expects (Master, Input)
        //        return (Master: t.Master, Input: t.Input);
        //    }).ToList();

        //    // We need to know createdInputIds aligned with original pair indices.
        //    // We'll call AddRangeAsync with pairs for unmatched items, but we must keep alignment.
        //    var createdInputIdsForInserts = new Dictionary<int, int>(); // pairIndex -> createdInputId
        //    if (insertPairsAligned.Any())
        //    {
        //        // We will reuse AddRangeAsync but it expects enumeration matching its internal ordering.
        //        // So call with pairs in same order as 'inserts' list and then map back to original pair indices.
        //        var addPairsList = insertPairsAligned;
        //        var createdIds = await _repository.AddRangeAsync(addPairsList);

        //        for (int i = 0; i < inserts.Count; i++)
        //        {
        //            var originalIndex = inserts[i].originalPairIndex;
        //            createdInputIdsForInserts[originalIndex] = createdIds.ElementAtOrDefault(i);
        //        }
        //    }

        //    // After updates/inserts we should fetch latest inputs to build masterMap and determine which items need SkyCiv.
        //    // Collect all involved inputIds: updated existing ids + created ones
        //    var updatedExistingIds = updatesList.Select(u => u.ExistingInputId).ToList();
        //    var createdIdsAll = createdInputIdsForInserts.Values.Where(id => id > 0).ToList();
        //    var allIdsToFetch = updatedExistingIds.Concat(createdIdsAll).Distinct().ToList();

        //    var inputsAfterOps = await _repository.GetByIdsAsync(allIdsToFetch);

        //    var masterMap = inputsAfterOps.ToDictionary(x => x.BagfilterInputId, x => x.BagfilterMasterId);

        //    // Step F: Child handler calls (UpdateAsync for updated inputs, Add for newly created)
        //    var childTasks = new List<Task>();

        //    // Helper to check whether a pair was an insert or an update
        //    bool IsInserted(int pairIndex) => createdInputIdsForInserts.ContainsKey(pairIndex) && createdInputIdsForInserts[pairIndex] > 0;
        //    bool IsUpdated(int pairIndex) => updates.ContainsKey(pairIndex);

        //    for (int i = 0; i < dtos.Count; i++)
        //    {
        //        var dto = dtos[i];
        //        int inputId;
        //        int masterId;

        //        if (IsInserted(i))
        //        {
        //            inputId = createdInputIdsForInserts[i];
        //            _ = masterMap.TryGetValue(inputId, out masterId);
        //            // call Add style handler (same as your AddRange flow)
        //            foreach (var handlerKv in _childHandlers)
        //            {
        //                var task = handlerKv.Value(dto, masterId, ct); // existing add handler
        //                if (task != null) childTasks.Add(task);
        //            }
        //        }
        //        else if (IsUpdated(i))
        //        {
        //            var intent = updates[i];
        //            inputId = intent.ExistingInputId;
        //            masterId = intent.ResolvedNewMasterId > 0 ? intent.ResolvedNewMasterId : intent.ExistingMasterId;
        //            // call UpdateAsync on child handlers - you must expose these update delegates (see notes below)
        //            foreach (var handlerKv in _childUpdateHandlers)
        //            {
        //                var updateTask = handlerKv.Value(dto, masterId, ct); // signature: Func<dto, masterId, inputId, ct, Task>
        //                if (updateTask != null) childTasks.Add(updateTask);
        //            }
        //        }
        //        else
        //        {
        //            // neither inserted nor updated - this can happen for pairs that were matched but had no change.
        //            // Still, if you want to ensure child handlers are in sync, call UpdateAsync with existing ids.
        //            var groupKey = pairGroupKeys[i];
        //            var gm = groupMatchMap[groupKey];
        //            if (gm != null && gm.IsMatched && gm.BagfilterInputId > 0)
        //            {
        //                inputId = gm.BagfilterInputId;
        //                masterId = gm.BagfilterMasterId;
        //                foreach (var handlerKv in _childUpdateHandlers)
        //                {
        //                    var updateTask = handlerKv.Value(dto, masterId, ct);
        //                    if (updateTask != null) childTasks.Add(updateTask);
        //                }
        //            }
        //        }
        //    }

        //    // Wait for child handlers
        //    if (childTasks.Count > 0)
        //    {
        //        try
        //        {
        //            await Task.WhenAll(childTasks);
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Child handler failure during UpdateRangeAsync.");
        //            throw;
        //        }
        //    }

        //    // Step G: SkyCiv analysis
        //    // Re-run analysis for:
        //    // - newly inserted inputs
        //    // - updated inputs where S3D model differs from DB (we need to fetch previous model to determine if changed)
        //    var skycivCandidates = new List<int>(); // inputIds to run analysis for

        //    // Add all created ones
        //    skycivCandidates.AddRange(createdIdsAll);

        //    // For updates: detect changed S3D models (we need to fetch original DB inputs for comparison; use 'candidates' list earlier)
        //    // Build dictionary of candidate input id -> original S3dModel (if we have it from earlier candidate fetch)
        //    var candidateModelMap = candidates
        //        .Where(c => c.BagfilterInputId > 0)
        //        .ToDictionary(c => c.BagfilterInputId, c => c.S3dModel ?? string.Empty);

        //    foreach (var kv in updates)
        //    {
        //        var intent = kv.Value;
        //        var existingId = intent.ExistingInputId;
        //        // find the input after update to get current S3dModel
        //        var after = inputsAfterOps.FirstOrDefault(x => x.BagfilterInputId == existingId);
        //        var afterModel = after?.S3dModel ?? string.Empty;
        //        candidateModelMap.TryGetValue(existingId, out var beforeModel);

        //        if (!string.Equals(beforeModel ?? string.Empty, afterModel ?? string.Empty, StringComparison.Ordinal))
        //        {
        //            skycivCandidates.Add(existingId);
        //        }
        //    }

        //    // Run SkyCiv for the skycivCandidates (same ProcessPairAsync helper as AddRange, but we will pass the list)
        //    if (skycivCandidates.Any())
        //    {
        //        const int maxConcurrency = 2;
        //        using var sem = new SemaphoreSlim(maxConcurrency);
        //        var tasks = new List<Task>();
        //        foreach (var inputId in skycivCandidates.Distinct())
        //        {
        //            // find pairIndex for this inputId
        //            var pairIndex = -1;
        //            // created ones are in createdInputIdsForInserts mapping
        //            var kvCreated = createdInputIdsForInserts.FirstOrDefault(kv2 => kv2.Value == inputId);
        //            if (!kvCreated.Equals(default(KeyValuePair<int, int>))) pairIndex = kvCreated.Key;
        //            else
        //            {
        //                // updated ones: find matching updates entry
        //                var upd = updates.Values.FirstOrDefault(u => u.ExistingInputId == inputId);
        //                if (upd != null) pairIndex = upd.PairIndex;
        //            }

        //            if (pairIndex < 0) continue;

        //            tasks.Add(ProcessPairAsync(pairIndex, pairs, pairGroupKeys, allIdsToFetch.ToList(), groupMatchMap, sem, ct));
        //        }

        //        try
        //        {
        //            await Task.WhenAll(tasks).ConfigureAwait(false);
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger?.LogError(ex, "SkyCiv analysis failed during UpdateRangeAsync.");
        //            throw;
        //        }
        //    }

        //    // Step H: Prepare result DTO
        //    var result = new UpdateRangeResultDto
        //    {
        //        UpdatedBagfilterInputIds = updatedExistingIds,
        //        CreatedBagfilterInputIds = createdIdsAll,
        //        Matches = groupMatchMap.Values.ToList()
        //    };

        //    return result;
        //}


        public async Task<UpdateRangeResultDto> UpdateRangeAsync(List<BagfilterInputMainDto> dtos, CancellationToken ct)
        {
            if (dtos == null || dtos.Count == 0) return new UpdateRangeResultDto();

            // Step A: Map DTOs -> desired pair state, build grouping keys & groups (same as AddRange)
            var pairs = new List<(BagfilterMaster Master, BagfilterInput Input)>(dtos.Count);
            var pairGroupKeys = new List<string>(dtos.Count);
            var groups = new Dictionary<string, List<int>>();

            for (int idx = 0; idx < dtos.Count; idx++)
            {
                var dto = dtos[idx];

                if (dto.BagfilterMaster == null && dto.BagfilterMasterId <= 0)
                    throw new ArgumentException("Each item must include BagfilterMaster data or an existing BagfilterMasterId.");

                var masterEntity = dto.BagfilterMaster != null
                    ? new BagfilterMaster
                    {
                        BagFilterName = dto.BagfilterMaster.BagFilterName,
                        Status = dto.BagfilterMaster.Status,
                        Revision = dto.BagfilterMaster.Revision,
                        AssignmentId = dto.BagfilterMaster.AssignmentId,
                        CreatedAt = DateTime.UtcNow,
                        EnquiryId = dto.BagfilterMaster.EnquiryId
                    }
                    : new BagfilterMaster
                    {
                        BagfilterMasterId = dto.BagfilterMasterId,
                        CreatedAt = DateTime.UtcNow
                    };

                var inputDto = dto.BagfilterInput ?? throw new ArgumentException("BagfilterInput is required.");
                var inputEntity = MapBagfilterInputDtoToEntity(inputDto);
                inputEntity.CreatedAt = DateTime.UtcNow;

                if (masterEntity.BagfilterMasterId > 0)
                    inputEntity.BagfilterMasterId = masterEntity.BagfilterMasterId;
                else
                    inputEntity.BagfilterMaster = masterEntity;

                pairs.Add((masterEntity, inputEntity));

                var groupKey = BuildGroupKey(inputEntity.Location, inputEntity.No_Of_Column, inputEntity.Ground_Clearance, inputEntity.Bag_Per_Row, inputEntity.Number_Of_Rows);
                pairGroupKeys.Add(groupKey);

                if (!groups.TryGetValue(groupKey, out var list)) { list = new List<int>(); groups[groupKey] = list; }
                list.Add(idx);
            }

            // Step B: Find candidates for groups
            var groupKeys = groups.Keys
                .Select(k =>
                {
                    var firstIdx = groups[k].First();
                    var repInput = pairs[firstIdx].Input;
                    return new GroupKey
                    {
                        Location = repInput.Location,
                        No_Of_Column = repInput.No_Of_Column,
                        Ground_Clearance = repInput.Ground_Clearance,
                        Bag_Per_Row = repInput.Bag_Per_Row,
                        Number_Of_Rows = repInput.Number_Of_Rows
                    };
                })
                .ToList();

            var candidates = await _repository.FindCandidatesByGroupKeysAsync(groupKeys); // BagfilterInput + Master included

            // Step C: For each group, create placeholder and decide match/update vs insert
            var groupKeysList = groups.Keys.ToList();
            var groupIdByKey = new Dictionary<string, string>(groupKeysList.Count);
            for (int i = 0; i < groupKeysList.Count; i++) groupIdByKey[groupKeysList[i]] = $"Group {i + 1}";

            var groupMatchMap = new Dictionary<string, BagfilterMatchDto>(groups.Count);
            var matchedEnquiryIds = new HashSet<int>();

            foreach (var groupKey in groupKeysList)
            {
                var repPairIdx = groups[groupKey].First();
                var repInput = pairs[repPairIdx].Input;
                var incomingEnquiryId = dtoHasEnquiryId(pairs[repPairIdx]);

                var placeholder = new BagfilterMatchDto
                {
                    GroupId = groupIdByKey[groupKey],
                    Location = repInput.Location,
                    BagfilterInputId = 0,
                    BagfilterMasterId = 0,
                    AssignmentId = null,
                    EnquiryId = null,
                    BagFilterName = null,
                    CustomerName = null,
                    CreatedAt = null,
                    No_Of_Column = repInput.No_Of_Column,
                    Ground_Clearance = repInput.Ground_Clearance,
                    Bag_Per_Row = repInput.Bag_Per_Row,
                    Number_Of_Rows = repInput.Number_Of_Rows,
                    IsMatched = false
                };

                var matchCandidate = candidates
                    .FirstOrDefault(c =>
                        string.Equals(c.Location ?? "", repInput.Location ?? "", StringComparison.InvariantCultureIgnoreCase)
                        && EqualsNullable(c.No_Of_Column, repInput.No_Of_Column)
                        && EqualsNullable(c.Ground_Clearance, repInput.Ground_Clearance)
                        && EqualsNullable(c.Bag_Per_Row, repInput.Bag_Per_Row)
                        && EqualsNullable(c.Number_Of_Rows, repInput.Number_Of_Rows)
                    // no same-enquiry exclusion in UpdateRange
                    );


                if (matchCandidate != null)
                {
                    placeholder.IsMatched = true;
                    placeholder.BagfilterInputId = matchCandidate.BagfilterInputId;
                    placeholder.BagfilterMasterId = matchCandidate.BagfilterMasterId;
                    placeholder.AssignmentId = matchCandidate.BagfilterMaster?.AssignmentId;
                    placeholder.EnquiryId = matchCandidate.EnquiryId;
                    placeholder.BagFilterName = matchCandidate.BagfilterMaster?.BagFilterName;
                    if (matchCandidate.EnquiryId.HasValue) matchedEnquiryIds.Add(matchCandidate.EnquiryId.Value);
                }

                groupMatchMap[groupKey] = placeholder;
            }

            // Enrich enquiries if any (same as AddRange)
            Dictionary<int, Enquiry> enquiryMap = new();
            if (matchedEnquiryIds.Any())
            {
                var enquiryDict = await _repository.GetEnquiriesByIdsAsync(matchedEnquiryIds);
                if (enquiryDict != null && enquiryDict.Any()) enquiryMap = enquiryDict;
            }
            foreach (var kv in groupMatchMap.ToList())
            {
                var matchDto = kv.Value;
                if (matchDto.IsMatched && matchDto.EnquiryId.HasValue && enquiryMap.TryGetValue(matchDto.EnquiryId.Value, out var enq))
                {
                    matchDto.CustomerName = enq.Customer;
                    matchDto.CreatedAt = enq.CreatedAt;
                }
            }

            // Step D: Decide per-pair operations
            // We'll prepare:
            // - updates: pairIndex -> (existingInputId, existingMasterId, updatedInputEntity, updatedMasterEntity OR newMasterToInsert)
            // - inserts: list of pairs (master+input) to create (for unmatched groups or when new master requested)
            var updates = new Dictionary<int, UpdateIntent>(); // UpdateIntent is defined below
            var inserts = new List<(BagfilterMaster Master, BagfilterInput Input, int originalPairIndex)>();

            for (int pairIndex = 0; pairIndex < pairs.Count; pairIndex++)
            {
                var dto = dtos[pairIndex];
                var groupKey = pairGroupKeys[pairIndex];
                var pair = pairs[pairIndex];

                // 1) DIRECT UPDATE PATH: if DTO already has an existing BagfilterInputId,
                //    treat it as update and do NOT rely on group matching for identity.
                if (dto.BagfilterInputId > 0)
                {
                    var existingInputId = dto.BagfilterInputId;
                    var existingMasterId = dto.BagfilterMasterId;

                    // If masterId wasn't set for some reason, fall back to group match as a last resort
                    if (existingMasterId <= 0 &&
                        groupMatchMap.TryGetValue(groupKey, out var gm) &&
                        gm != null && gm.IsMatched && gm.BagfilterMasterId > 0)
                    {
                        existingMasterId = gm.BagfilterMasterId;
                    }

                    updates[pairIndex] = new UpdateIntent
                    {
                        PairIndex = pairIndex,
                        ExistingInputId = existingInputId,
                        ExistingMasterId = existingMasterId,
                        InputToUpdate = pair.Input,

                        // If you actually want to update master fields as well when BagfilterMaster is supplied,
                        // you can use the mapped Master from Step A:
                        MasterToUpdate = dto.BagfilterMaster != null ? pair.Master : null,
                        UpdateMaster = dto.BagfilterMaster != null
                    };

                    // IMPORTANT: skip match-based logic for this row
                    continue;
                }

                // 2) NEW ROWS (no BagfilterInputId) -> use existing group-match logic
                var matchDto = groupMatchMap.TryGetValue(groupKey, out var m) ? m : null;

                if (matchDto != null && matchDto.IsMatched && matchDto.BagfilterInputId > 0)
                {
                    // Update path: we have an existing row to update (matched by group)
                    var existingInputId = matchDto.BagfilterInputId;
                    var existingMasterId = matchDto.BagfilterMasterId; // may be zero/nullable

                    // Determine if incoming master info indicates a "new master" (distinct BagFilterName or explicit masterId mismatch)
                    var incomingMasterName = pair.Master?.BagFilterName;
                    var incomingMasterId = pair.Master?.BagfilterMasterId ?? 0;

                    var requiresNewMaster = false;
                    if (!string.IsNullOrWhiteSpace(incomingMasterName))
                    {
                        // if the incoming name differs from matched one -> create new master
                        if (!string.Equals(incomingMasterName, matchDto.BagFilterName ?? "", StringComparison.InvariantCultureIgnoreCase))
                        {
                            requiresNewMaster = true;
                        }
                    }
                    // if incoming explicitly references a masterId that is not the matched master, treat as new (or verify existence)
                    if (incomingMasterId > 0 && incomingMasterId != matchDto.BagfilterMasterId)
                    {
                        requiresNewMaster = true;
                    }

                    if (requiresNewMaster)
                    {
                        // Create a new master entity (copy incoming master data if provided; otherwise minimal)
                        var newMaster = pair.Master ?? new BagfilterMaster
                        {
                            BagFilterName = incomingMasterName ?? ("Master_For_Input_" + existingInputId),
                            CreatedAt = DateTime.UtcNow
                        };

                        // Prepare an update intent that will first create a new master and then
                        // point the input to that new master.
                        updates[pairIndex] = new UpdateIntent
                        {
                            PairIndex = pairIndex,
                            ExistingInputId = existingInputId,
                            ExistingMasterId = existingMasterId,
                            InputToUpdate = pair.Input,
                            NewMasterToCreate = newMaster,
                            UpdateMaster = false // we are creating new, not updating old
                        };
                    }
                    else
                    {
                        // No new master required — maybe update existing master fields and/or input fields
                        updates[pairIndex] = new UpdateIntent
                        {
                            PairIndex = pairIndex,
                            ExistingInputId = existingInputId,
                            ExistingMasterId = existingMasterId,
                            InputToUpdate = pair.Input,
                            MasterToUpdate = pair.Master, // may be a shell with fields to update
                            UpdateMaster = pair.Master != null && pair.Master.BagfilterMasterId == 0
                        };
                    }
                }
                else
                {
                    // 3) INSERT PATH — unmatched group or explicit new master requested;
                    //    create new master+input pair
                    inserts.Add((pairs[pairIndex].Master, pairs[pairIndex].Input, pairIndex));
                }
            }


            // Step E: Persist updates and inserts
            // Strategy:
            // 1) For updates that require new master creation, create those masters first (so we can attach their IDs to inputs)
            // 2) For updates that simply update existing records, call repository.UpdateRangeAsync (you must implement)
            // 3) For inserts, call repository.AddRangeAsync (existing method). Make sure to keep insertion order aligned with original pair indices.
            // We'll implement a simple, safe order.

            // --- E1: Create all new masters required by updates (so we can update input.masterId) ---
            var newMastersToCreate = new List<(int pairIndex, BagfilterMaster master)>();
            foreach (var kv in updates)
            {
                var intent = kv.Value;
                if (intent.NewMasterToCreate != null)
                    newMastersToCreate.Add((kv.Key, intent.NewMasterToCreate));
            }

            // Persist new masters and collect mapping pairIndex -> createdMasterId
            var createdMasterIdsForUpdates = new Dictionary<int, int>();
            if (newMastersToCreate.Any())
            {
                // Create masters in a single transaction using repo helper (you should add repository method AddMastersAsync)
                var mastersOnly = newMastersToCreate.Select(x => x.master).ToList();
                var createdMasterIds = await _masterRepository.AddMastersAsync(mastersOnly, ct); // returns list aligned with mastersOnly
                for (int i = 0; i < newMastersToCreate.Count; i++)
                {
                    createdMasterIdsForUpdates[newMastersToCreate[i].pairIndex] = createdMasterIds[i];
                }
            }

            // Apply created masterIds into update intents (so inputs point to new master)
            foreach (var kv in updates)
            {
                var intent = kv.Value;
                if (intent.NewMasterToCreate != null && createdMasterIdsForUpdates.TryGetValue(kv.Key, out var mid))
                {
                    intent.ResolvedNewMasterId = mid;
                    // also set the InputToUpdate's BagfilterMasterId so repository update knows how to set FK
                    intent.InputToUpdate.BagfilterMasterId = mid;
                    intent.InputToUpdate.BagfilterMaster = null; // avoid EF trying to insert the master again
                }
            }

            // --- E2: Execute updates against existing rows (batch)
            var updatesList = updates.Values.ToList();
            if (updatesList.Any())
            {
                // repository.UpdateRangeAsync should accept a list of update DTOs or entities and apply changes in a transaction.
                // We construct lightweight DTOs: (ExistingInputId, InputToUpdate, ExistingMasterId, MasterToUpdate, ResolvedNewMasterId)
                var updateDtos = updatesList.Select(u => new RepositoryInputUpdateDto
                {
                    ExistingInputId = u.ExistingInputId,
                    InputEntity = u.InputToUpdate,
                    ExistingMasterId = u.ExistingMasterId,
                    MasterEntityToUpdate = u.MasterToUpdate,
                    ResolvedNewMasterId = u.ResolvedNewMasterId
                }).ToList();

                // Call repository to apply updates in a single transaction
                await _repository.UpdateRangeAsync(updateDtos);
            }

            // --- E3: Perform inserts for unmatched pairs (use your existing AddRangeAsync repo method)
            var insertPairsAligned = inserts.Select(t =>
            {
                // keep same master+input objects as earlier; repository.AddRangeAsync expects (Master, Input)
                return (Master: t.Master, Input: t.Input);
            }).ToList();

            // We need to know createdInputIds aligned with original pair indices.
            // We'll call AddRangeAsync with pairs for unmatched items, but we must keep alignment.
            var createdInputIdsForInserts = new Dictionary<int, int>(); // pairIndex -> createdInputId
            if (insertPairsAligned.Any())
            {
                // We will reuse AddRangeAsync but it expects enumeration matching its internal ordering.
                // So call with pairs in same order as 'inserts' list and then map back to original pair indices.
                var addPairsList = insertPairsAligned;
                var createdIds = await _repository.AddRangeAsync(addPairsList);

                for (int i = 0; i < inserts.Count; i++)
                {
                    var originalIndex = inserts[i].originalPairIndex;
                    createdInputIdsForInserts[originalIndex] = createdIds.ElementAtOrDefault(i);
                }
            }

            // After updates/inserts we should fetch latest inputs to build masterMap and determine which items need SkyCiv.
            // Collect all involved inputIds: updated existing ids + created ones
            var updatedExistingIds = updatesList.Select(u => u.ExistingInputId).ToList();
            var createdIdsAll = createdInputIdsForInserts.Values.Where(id => id > 0).ToList();
            var allIdsToFetch = updatedExistingIds.Concat(createdIdsAll).Distinct().ToList();

            var inputsAfterOps = await _repository.GetByIdsAsync(allIdsToFetch);

            var masterMap = inputsAfterOps.ToDictionary(x => x.BagfilterInputId, x => x.BagfilterMasterId);


            //New code
            // === NEW: map pairIndex -> final BagfilterMasterId ===

            bool IsInserted(int pairIndex) =>
                createdInputIdsForInserts.ContainsKey(pairIndex) &&
                createdInputIdsForInserts[pairIndex] > 0;

            bool IsUpdated(int pairIndex) =>
                updates.ContainsKey(pairIndex);

            var masterIdByIndex = new int[dtos.Count];

            for (int i = 0; i < dtos.Count; i++)
            {
                int masterId = 0;

                if (IsInserted(i))
                {
                    var inputId = createdInputIdsForInserts[i];
                    masterMap.TryGetValue(inputId, out masterId);
                }
                else if (IsUpdated(i))
                {
                    var intent = updates[i];
                    masterId = intent.ResolvedNewMasterId > 0
                        ? intent.ResolvedNewMasterId
                        : intent.ExistingMasterId;
                }
                else
                {
                    var groupKey = pairGroupKeys[i];
                    if (groupMatchMap.TryGetValue(groupKey, out var gm) &&
                        gm != null && gm.IsMatched)
                    {
                        masterId = gm.BagfilterMasterId;
                    }
                }

                masterIdByIndex[i] = masterId;
            }

            // === Batched children BEFORE Step F ===

            // WeightSummary
            await BatchUpsertChildAsync<WeightSummary>(dtos,masterIdByIndex,hasChildDto: dto => dto.WeightSummary != null,
                getExistingByMasterIds: (masterIds, token) => _weightSummaryRepository.GetByMasterIdsAsync(masterIds, token),
                mapEntity: (dto, masterId, existing) =>MapWeightSummary(dto, masterId, existing),
                upsertRange: (entities, token) => _weightSummaryRepository.UpsertRangeAsync(entities, token),ct);

            await BatchUpsertChildAsync<ProcessInfo>(dtos, masterIdByIndex, hasChildDto: dto => dto.ProcessInfo != null,
                getExistingByMasterIds: (masterIds, token) => _processInfoRepository.GetByMasterIdsAsync(masterIds, token),
                mapEntity: (dto, masterId, existing) => MapProcessInfo(dto, masterId, existing),
                upsertRange: (entities, token) => _processInfoRepository.UpsertRangeAsync(entities, token), ct);

            await BatchUpsertChildAsync<CageInputs>(
            dtos,
            masterIdByIndex,
            hasChildDto: dto => dto.CageInputs != null,
            getExistingByMasterIds: (masterIds, token) => _cageInputsRepository.GetByMasterIdsAsync(masterIds, token),
            mapEntity: (dto, masterId, existing) => MapCageInputs(dto, masterId, existing),
            upsertRange: (entities, token) => _cageInputsRepository.UpsertRangeAsync(entities, token),
            ct);

            await BatchUpsertChildAsync<BagSelection>(dtos, masterIdByIndex,
            hasChildDto: dto => dto.BagSelection != null,
            getExistingByMasterIds: (masterIds, token) => _bagSelectionRepository.GetByMasterIdsAsync(masterIds, token),
            mapEntity: (dto, masterId, existing) => MapBagSelection(dto, masterId, existing),
            upsertRange: (entities, token) => _bagSelectionRepository.UpsertRangeAsync(entities, token),
            ct);

            await BatchUpsertChildAsync<StructureInputs>(dtos, masterIdByIndex,
            hasChildDto: dto => dto.StructureInputs != null,
            getExistingByMasterIds: (masterIds, token) => _structureInputsRepository.GetByMasterIdsAsync(masterIds, token),
            mapEntity: (dto, masterId, existing) => MapStructureInputs(dto, masterId, existing),
            upsertRange: (entities, token) => _structureInputsRepository.UpsertRangeAsync(entities, token),
            ct);

            await BatchUpsertChildAsync<CapsuleInputs>(dtos, masterIdByIndex,
            hasChildDto: dto => dto.CapsuleInputs != null,
            getExistingByMasterIds: (masterIds, token) => _capsuleInputsRepository.GetByMasterIdsAsync(masterIds, token),
            mapEntity: (dto, masterId, existing) => MapCapsuleInputs(dto, masterId, existing),
            upsertRange: (entities, token) => _capsuleInputsRepository.UpsertRangeAsync(entities, token),
            ct);

            await BatchUpsertChildAsync<CasingInputs>(dtos, masterIdByIndex,
            hasChildDto: dto => dto.CasingInputs != null,
            getExistingByMasterIds: (masterIds, token) => _casingInputsRepository.GetByMasterIdsAsync(masterIds, token),
            mapEntity: (dto, masterId, existing) => MapCasingInputs(dto, masterId, existing),
            upsertRange: (entities, token) => _casingInputsRepository.UpsertRangeAsync(entities, token),
            ct);

            await BatchUpsertChildAsync<HopperInputs>(dtos, masterIdByIndex,
            hasChildDto: dto => dto.HopperInputs != null,
            getExistingByMasterIds: (masterIds, token) => _hopperInputsRepository.GetByMasterIdsAsync(masterIds, token),
            mapEntity: (dto, masterId, existing) => MapHopperInputs(dto, masterId, existing),
            upsertRange: (entities, token) => _hopperInputsRepository.UpsertRangeAsync(entities, token),
            ct);

            await BatchUpsertChildAsync<SupportStructure>(dtos, masterIdByIndex,
            hasChildDto: dto => dto.SupportStructure != null,
            getExistingByMasterIds: (masterIds, token) => _supportStructureRepository.GetByMasterIdsAsync(masterIds, token),
            mapEntity: (dto, masterId, existing) => MapSupportStructure(dto, masterId, existing),
            upsertRange: (entities, token) => _supportStructureRepository.UpsertRangeAsync(entities, token),
            ct);

            await BatchUpsertChildAsync<AccessGroup>(dtos, masterIdByIndex,
            hasChildDto: dto => dto.AccessGroup != null,
            getExistingByMasterIds: (masterIds, token) => _accessGroupRepository.GetByMasterIdsAsync(masterIds, token),
            mapEntity: (dto, masterId, existing) => MapAccessGroup(dto, masterId, existing),
            upsertRange: (entities, token) => _accessGroupRepository.UpsertRangeAsync(entities, token),
            ct);

            await BatchUpsertChildAsync<RoofDoor>(dtos, masterIdByIndex,
            hasChildDto: dto => dto.RoofDoor != null,
            getExistingByMasterIds: (masterIds, token) => _roofDoorRepository.GetByMasterIdsAsync(masterIds, token),
            mapEntity: (dto, masterId, existing) => MapRoofDoor(dto, masterId, existing),
            upsertRange: (entities, token) => _roofDoorRepository.UpsertRangeAsync(entities, token),
            ct);

            await BatchUpsertChildAsync<PaintingArea>(dtos, masterIdByIndex,
            hasChildDto: dto => dto.PaintingArea != null,
            getExistingByMasterIds: (masterIds, token) => _paintingAreaRepository.GetByMasterIdsAsync(masterIds, token),
            mapEntity: (dto, masterId, existing) => MapPaintingArea(dto, masterId, existing),
            upsertRange: (entities, token) => _paintingAreaRepository.UpsertRangeAsync(entities, token),
            ct);

            await BatchUpsertChildAsync<PaintingCost>(dtos, masterIdByIndex,
            hasChildDto: dto => dto.PaintingCost != null,
            getExistingByMasterIds: (masterIds, token) => _paintingCostRepository.GetByMasterIdsAsync(masterIds, token),
            mapEntity: (dto, masterId, existing) => MapPaintingCost(dto, masterId, existing),
            upsertRange: (entities, token) => _paintingCostRepository.UpsertRangeAsync(entities, token),
            ct);


            // === Batched BillOfMaterial replace BEFORE SkyCiv ===

            var bomDataByMaster = new Dictionary<int, List<BillOfMaterial>>();

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];

                // decide when to touch BOM:
                //   - If you want "no change" when list is empty, require Count > 0
                if (dto.BillOfMaterial == null || dto.BillOfMaterial.Count == 0)
                    continue;

                var masterId = masterIdByIndex[i];
                if (masterId <= 0)
                    continue;

                var mappedRows = MapBillOfMaterialCollection(dto, masterId);
                if (mappedRows.Count == 0)
                    continue;

                if (!bomDataByMaster.TryGetValue(masterId, out var list))
                {
                    list = new List<BillOfMaterial>();
                    bomDataByMaster[masterId] = list;
                }

                // if you can have multiple dtos for same masterId, they’ll aggregate
                list.AddRange(mappedRows);
            }

            // Now replace BOM sets per master in one transactional call
            if (bomDataByMaster.Count > 0)
            {
                await _billOfMaterialRepository.ReplaceForMastersAsync(bomDataByMaster, ct);
            }


            // Step G: SkyCiv analysis
            // Re-run analysis for:
            // - newly inserted inputs
            // - updated inputs where S3D model differs from DB (we need to fetch previous model to determine if changed)
            var skycivCandidates = new List<int>(); // inputIds to run analysis for

            // Add all created ones
            skycivCandidates.AddRange(createdIdsAll);

            // For updates: detect changed S3D models (we need to fetch original DB inputs for comparison; use 'candidates' list earlier)
            // Build dictionary of candidate input id -> original S3dModel (if we have it from earlier candidate fetch)
            var candidateModelMap = candidates
                .Where(c => c.BagfilterInputId > 0)
                .ToDictionary(c => c.BagfilterInputId, c => c.S3dModel ?? string.Empty);

            foreach (var kv in updates)
            {
                var intent = kv.Value;
                var existingId = intent.ExistingInputId;
                // find the input after update to get current S3dModel
                var after = inputsAfterOps.FirstOrDefault(x => x.BagfilterInputId == existingId);
                var afterModel = after?.S3dModel ?? string.Empty;
                candidateModelMap.TryGetValue(existingId, out var beforeModel);

                if (!string.Equals(beforeModel ?? string.Empty, afterModel ?? string.Empty, StringComparison.Ordinal))
                {
                    skycivCandidates.Add(existingId);
                }
            }

            // Run SkyCiv for the skycivCandidates (same ProcessPairAsync helper as AddRange, but we will pass the list)
            if (skycivCandidates.Any())
            {
                const int maxConcurrency = 2;
                using var sem = new SemaphoreSlim(maxConcurrency);
                var tasks = new List<Task>();
                foreach (var inputId in skycivCandidates.Distinct())
                {
                    // find pairIndex for this inputId
                    var pairIndex = -1;
                    // created ones are in createdInputIdsForInserts mapping
                    var kvCreated = createdInputIdsForInserts.FirstOrDefault(kv2 => kv2.Value == inputId);
                    if (!kvCreated.Equals(default(KeyValuePair<int, int>))) pairIndex = kvCreated.Key;
                    else
                    {
                        // updated ones: find matching updates entry
                        var upd = updates.Values.FirstOrDefault(u => u.ExistingInputId == inputId);
                        if (upd != null) pairIndex = upd.PairIndex;
                    }

                    if (pairIndex < 0) continue;

                    tasks.Add(ProcessPairAsync(pairIndex, pairs, pairGroupKeys, allIdsToFetch.ToList(), groupMatchMap, sem, ct));
                }

                try
                {
                    await Task.WhenAll(tasks).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "SkyCiv analysis failed during UpdateRangeAsync.");
                    throw;
                }
            }

            // Step H: Prepare result DTO
            var result = new UpdateRangeResultDto
            {
                UpdatedBagfilterInputIds = updatedExistingIds,
                CreatedBagfilterInputIds = createdIdsAll,
                Matches = groupMatchMap.Values.ToList()
            };

            return result;
        }

        // ---- Helper types used in the method ----

        private class UpdateIntent
        {
            public int PairIndex { get; set; }
            public int ExistingInputId { get; set; }
            public int ExistingMasterId { get; set; }
            public BagfilterInput InputToUpdate { get; set; } = null!;
            public BagfilterMaster? MasterToUpdate { get; set; }
            public BagfilterMaster? NewMasterToCreate { get; set; }
            public int ResolvedNewMasterId { get; set; } = 0;
            public bool UpdateMaster { get; set; } = false;
        }

        // repository input DTO
        //public class RepositoryInputUpdateDto
        //{
        //    public int ExistingInputId { get; set; }
        //    public BagfilterInput InputEntity { get; set; } = null!;
        //    public int ExistingMasterId { get; set; }
        //    public BagfilterMaster? MasterEntityToUpdate { get; set; } // optional, update existing master fields
        //    public int ResolvedNewMasterId { get; set; } // optional - if set, attach to this master id
        //}





        private async Task ProcessPairAsync(
                int pairIndex,
                List<(BagfilterMaster Master, BagfilterInput Input)> pairs,
                List<string> pairGroupKeys,
                IList<int> createdInputIds,
                Dictionary<string, BagfilterMatchDto> groupMatchMap,
                SemaphoreSlim sem,
                CancellationToken ct)
        {
            await sem.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                // validate pairIndex bounds
                if (pairIndex < 0 || pairIndex >= pairs.Count)
                {
                    _logger?.LogWarning("Invalid pairIndex {pairIndex}. Skipping.", pairIndex);
                    return;
                }

                var inputEntity = pairs[pairIndex].Input;
                if (inputEntity == null)
                {
                    _logger?.LogWarning("No Input entity for pairIndex {pairIndex}. Skipping.", pairIndex);
                    return;
                }

                // safely parse the stored S3D model (not throwing on invalid JSON)
                JObject s3dModel = null;
                if (!string.IsNullOrWhiteSpace(inputEntity.S3dModel))
                {
                    try
                    {
                        s3dModel = JObject.Parse(inputEntity.S3dModel);
                    }
                    catch (Exception parseEx)
                    {
                        _logger?.LogWarning(parseEx, "Failed to parse S3D model for pairIndex {pairIndex}. Skipping analysis.", pairIndex);
                        return;
                    }
                }

                if (s3dModel == null)
                {
                    _logger?.LogInformation("No S3D model present for pairIndex {pairIndex}.", pairIndex);
                    return;
                }

                // call SkyCiv with a simple retry/backoff (3 attempts). Replace with Polly if you have it.
                AnalysisResponseDto analysisResponse = null;
                var attempts = 0;
                var maxAttempts = 3;
                var delayMs = 500;
                while (attempts < maxAttempts)
                {
                    ct.ThrowIfCancellationRequested();
                    attempts++;
                    try
                    {
                        analysisResponse = await _skyCivService.RunAnalysisAsync(s3dModel, ct).ConfigureAwait(false);
                        break; // success
                    }
                    catch (OperationCanceledException) { throw; }
                    catch (Exception ex) when (attempts < maxAttempts)
                    {
                        _logger?.LogWarning(ex, "SkyCiv attempt {attempt} failed for pairIndex {pairIndex}, retrying after {delay}ms", attempts, pairIndex, delayMs);
                        await Task.Delay(delayMs, ct).ConfigureAwait(false);
                        delayMs *= 2;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "SkyCiv analysis failed for pairIndex {pairIndex} on final attempt", pairIndex);
                        return;
                    }
                }

                if (analysisResponse == null || !string.Equals(analysisResponse.Status, "Succeeded", StringComparison.OrdinalIgnoreCase))
                {
                    _logger?.LogWarning("Analysis didn't succeed for pairIndex {pairIndex}. Status: {status}", pairIndex, analysisResponse?.Status);
                    return;
                }

                // safely obtain createdId (ElementAtOrDefault to avoid exceptions)
                var createdId = (pairIndex >= 0 && pairIndex < createdInputIds.Count) ? createdInputIds[pairIndex] : 0;
                if (createdId <= 0)
                {
                    _logger?.LogWarning("No created record id for pairIndex {pairIndex}. Skipping DB update.", pairIndex);
                    return;
                }

                var modelJson = analysisResponse.ModelData?.ToString(Formatting.None) ?? string.Empty;
                var sessionId = analysisResponse.SessionId;

                // Update repository with cancellation token (make repository support ct)
                try
                {
                    await _repository.UpdateS3dModelAsync(createdId, modelJson, sessionId).ConfigureAwait(false);
                }
                catch (OperationCanceledException) { throw; }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Failed to persist SkyCiv model for createdId {createdId} (pairIndex {pairIndex})", createdId, pairIndex);
                    // decide: rethrow to fail whole batch, or swallow and continue. Here we log and return.
                    return;
                }

                // annotate groupMatchMap if present (safe lookup)
                var groupKey = (pairIndex >= 0 && pairIndex < pairGroupKeys.Count) ? pairGroupKeys[pairIndex] : null;
                if (!string.IsNullOrEmpty(groupKey) && groupMatchMap.TryGetValue(groupKey, out var gm2))
                {
                    // mark that analysis was attempted; do not flip IsMatched to true unless intended
                    gm2.AnalysisAttempted = true; // add a property or similar to signal analysis ran
                }
            }
            finally
            {
                sem.Release();
            }
        }



        //Helpers
        private BagfilterInput MapBagfilterInputDtoToEntity(BagfilterInputDto dto)
        {
            if (dto == null) return new BagfilterInput();

            var e = new BagfilterInput
            {
                // Copy core values (you already had these)
                Process_Volume_M3h = dto.Process_Volume_M3h,
                Location = string.IsNullOrWhiteSpace(dto.Location) ? null : dto.Location.Trim(),
                Process_Dust = dto.Process_Dust,
                Process_Dustload_Gmspm3 = dto.Process_Dustload_Gmspm3,
                Process_Temp_C = dto.Process_Temp_C,
                Dew_Point_C = dto.Dew_Point_C,
                Outlet_Emission_Mgpm3 = dto.Outlet_Emission_Mgpm3,
                Process_Cloth_Ratio = dto.Process_Cloth_Ratio,
                Can_Correction = dto.Can_Correction,
                Customer_Equipment_Tag_No = dto.Customer_Equipment_Tag_No,
                Bagfilter_Cleaning_Type = dto.Bagfilter_Cleaning_Type,
                Offline_Maintainence = dto.Offline_Maintainence,
                Cage_Type = dto.Cage_Type,
                Cage_Sub_Type = dto.Cage_Sub_Type,
                Cage_Wire_Dia = dto.Cage_Wire_Dia,
                No_Of_Cage_Wires = dto.No_Of_Cage_Wires,
                Ring_Spacing = dto.Ring_Spacing,
                Cage_Diameter = dto.Cage_Diameter,
                Cage_Length = dto.Cage_Length,
                Cage_Configuration = dto.Cage_Configuration,
                Filter_Bag_Dia = dto.Filter_Bag_Dia,
                Fil_Bag_Length = dto.Fil_Bag_Length,
                Fil_Bag_Recommendation = dto.Fil_Bag_Recommendation,
                Gas_Entry = dto.Gas_Entry,
                Support_Structure_Type = dto.Support_Structure_Type,
                
                Valve_Size = dto.Valve_Size,
                Voltage_Rating = dto.Voltage_Rating,
                Capsule_Height = dto.Capsule_Height,
                Tube_Sheet_Thickness = dto.Tube_Sheet_Thickness,
                Capsule_Wall_Thickness = dto.Capsule_Wall_Thickness,
                Canopy = dto.Canopy,
                Solenoid_Valve_Maintainence = dto.Solenoid_Valve_Maintainence,
                Casing_Wall_Thickness = dto.Casing_Wall_Thickness,
                Hopper_Type = dto.Hopper_Type,
                Process_Compartments = dto.Process_Compartments,
                Tot_No_Of_Hoppers = dto.Tot_No_Of_Hoppers,
                Tot_No_Of_Trough = dto.Tot_No_Of_Trough,
                Plenum_Width = dto.Plenum_Width,
                Inlet_Height = dto.Inlet_Height,
                Hopper_Thickness = dto.Hopper_Thickness,
                Hopper_Valley_Angle = dto.Hopper_Valley_Angle,
                Access_Door_Type = dto.Access_Door_Type,
                Access_Door_Qty = dto.Access_Door_Qty,
                Rav_Maintainence_Pltform = dto.Rav_Maintainence_Pltform,
                Hopper_Access_Stool = dto.Hopper_Access_Stool,
                Is_Distance_Piece = dto.Is_Distance_Piece,
                Distance_Piece_Height = dto.Distance_Piece_Height,
                Stiffening_Factor = dto.Stiffening_Factor,
                Hopper = dto.Hopper,
                Discharge_Opening_Sqr = dto.Discharge_Opening_Sqr,
                Rav_Height = dto.Rav_Height, //new
                Material_Handling = dto.Material_Handling,
                Material_Handling_Qty = dto.Material_Handling_Qty,
                Trough_Outlet_Length = dto.Trough_Outlet_Length,
                Trough_Outlet_Width = dto.Trough_Outlet_Width,
                Material_Handling_XXX = dto.Material_Handling_XXX,
                Support_Struct_Type = dto.Support_Struct_Type,
                No_Of_Column = dto.No_Of_Column,
                Ground_Clearance = dto.Ground_Clearance,
                Slide_Gate_Height = dto.Slide_Gate_Height, //new
                Access_Type = dto.Access_Type,
                Cage_Weight_Ladder = dto.Cage_Weight_Ladder,
                Mid_Landing_Pltform = dto.Mid_Landing_Pltform,
                Platform_Weight = dto.Platform_Weight,
                Staircase_Height = dto.Staircase_Height,
                Staircase_Weight = dto.Staircase_Weight,
                Railing_Weight = dto.Railing_Weight,
                Maintainence_Pltform = dto.Maintainence_Pltform,
                Maintainence_Pltform_Weight = dto.Maintainence_Pltform_Weight,
                Blow_Pipe = dto.Blow_Pipe,
                Pressure_Header = dto.Pressure_Header,
                Distance_Piece = dto.Distance_Piece,
                Access_Stool_Size_Mm = dto.Access_Stool_Size_Mm,
                Access_Stool_Size_Kg = dto.Access_Stool_Size_Kg,
                Roof_Door_Thickness = dto.Roof_Door_Thickness,
                Column_Height = dto.Column_Height,

                // computed values (you already had)
                Bag_Per_Row = dto.Bag_Per_Row,
                Number_Of_Rows = dto.Number_Of_Rows,

                // NEW: EnquiryId - used to exclude same enquiry matches
                EnquiryId = dto.EnquiryId,
                S3dModel = dto.S3dModel,
            };

            return e;
        }

        private static string BuildGroupKey(string? location, decimal? no, decimal? ground, decimal? bag, decimal? rows)
        {
            // normalize using InvariantCulture to avoid culture issues
            string Norm(decimal? d) => d?.ToString(CultureInfo.InvariantCulture) ?? "__NULL__";
            var loc = (location ?? "__NULL__").Trim().ToLowerInvariant();
            return $"{loc}|{Norm(no)}|{Norm(ground)}|{Norm(bag)}|{Norm(rows)}";
        }

        private static bool EqualsNullable(decimal? a, decimal? b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            return a.Value == b.Value;
        }

        private static int? dtoHasEnquiryId((BagfilterMaster Master, BagfilterInput Input) pair)
        {
            // Try to find EnquiryId from input first, then master, else null.
            if (pair.Input?.EnquiryId != null) return pair.Input.EnquiryId;
            if (pair.Master?.EnquiryId != null) return pair.Master.EnquiryId;
            return null;
        }


       
        // Register an add handler and optionally an update handler (both use masterId only)
        private void RegisterChildHandler(
            string key,
            Func<BagfilterInputMainDto, int, CancellationToken, Task> addHandler,
            Func<BagfilterInputMainDto, int, CancellationToken, Task>? updateHandler = null)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key required", nameof(key));
            if (addHandler == null) throw new ArgumentNullException(nameof(addHandler));

            _childHandlers[key] = addHandler;

            if (updateHandler != null)
                _childUpdateHandlers[key] = updateHandler;
        }



        // Section Helper methods
        private async Task HandleWeightSummaryAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.WeightSummary == null)
                return; // nothing to do for this DTO

            // Map DTO -> Entity (adjust property names to match your DTO/entity)
            var entity = new WeightSummary
            {

                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Casing_Weight = dto.WeightSummary.Casing_Weight,
                Capsule_Weight = dto.WeightSummary.Capsule_Weight,
                Tot_Weight_Per_Compartment = dto.WeightSummary.Tot_Weight_Per_Compartment,
                Hopper_Weight = dto.WeightSummary.Hopper_Weight,
                Weight_Of_Cage_Ladder = dto.WeightSummary.Weight_Of_Cage_Ladder,
                Railing_Weight = dto.WeightSummary.Railing_Weight,
                Tubesheet_Weight = dto.WeightSummary.Tubesheet_Weight,
                Air_Header_Blow_Pipe = dto.WeightSummary.Air_Header_Blow_Pipe,
                Hopper_Access_Stool_Weight = dto.WeightSummary.Hopper_Access_Stool_Weight,
                Weight_Of_Mid_Landing_Plt = dto.WeightSummary.Weight_Of_Mid_Landing_Plt,
                Weight_Of_Maintainence_Pltform = dto.WeightSummary.Weight_Of_Maintainence_Pltform,
                Cage_Weight = dto.WeightSummary.Cage_Weight,
                Structure_Weight = dto.WeightSummary.Structure_Weight,
                Weight_Total = dto.WeightSummary.Weight_Total,

                CreatedAt = DateTime.UtcNow
            };

            // Persist using the dedicated repository
            // Implement AddAsync on IWeightSummaryRepository if not present.
            try
            {
                await _weightSummaryRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                // decide how to handle child persistence failure: log and continue, or rethrow to fail the whole flow
                _logger.LogError(ex, "Failed to persist WeightSummary for BagfilterMasterId {Id}", bagfilterMasterId);
                throw; // rethrow if you want to fail the parent transaction/operation
                // OR remove the throw; to log and continue
            }
        }

        private async Task HandleWeightSummaryUpdateAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.WeightSummary == null) return;

            // Map DTO -> entity
            var entity = new WeightSummary
            {
                // keep EnquiryId if you want to persist it from incoming DTO
                EnquiryId = (int)dto.BagfilterInput?.EnquiryId, // int? on entity if applicable
                BagfilterMasterId = bagfilterMasterId,

                Casing_Weight = dto.WeightSummary.Casing_Weight,
                Capsule_Weight = dto.WeightSummary.Capsule_Weight,
                Tot_Weight_Per_Compartment = dto.WeightSummary.Tot_Weight_Per_Compartment,
                Hopper_Weight = dto.WeightSummary.Hopper_Weight,
                Weight_Of_Cage_Ladder = dto.WeightSummary.Weight_Of_Cage_Ladder,
                Railing_Weight = dto.WeightSummary.Railing_Weight,
                Tubesheet_Weight = dto.WeightSummary.Tubesheet_Weight,
                Air_Header_Blow_Pipe = dto.WeightSummary.Air_Header_Blow_Pipe,
                Hopper_Access_Stool_Weight = dto.WeightSummary.Hopper_Access_Stool_Weight,
                Weight_Of_Mid_Landing_Plt = dto.WeightSummary.Weight_Of_Mid_Landing_Plt,
                Weight_Of_Maintainence_Pltform = dto.WeightSummary.Weight_Of_Maintainence_Pltform,
                Cage_Weight = dto.WeightSummary.Cage_Weight,
                Structure_Weight = dto.WeightSummary.Structure_Weight,
                Weight_Total = dto.WeightSummary.Weight_Total,

                UpdatedAt = DateTime.UtcNow
            };

            try
            {
                var existingId = await _weightSummaryRepository.GetIdForMasterAsync(bagfilterMasterId);
                if (existingId.HasValue)
                {
                    entity.Id = existingId.Value;
                    await _weightSummaryRepository.UpdateAsync(entity);
                }
                else
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    await _weightSummaryRepository.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert WeightSummary for MasterId {MasterId}", bagfilterMasterId);
                throw;
            }
        }


        private async Task HandleProcessInfoAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.BagfilterInput == null)
                return; // nothing to process

            var input = dto.ProcessInfo;

            // Map DTO -> Entity
            var entity = new ProcessInfo
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Process_Volume_M3h = input.Process_Volume_M3h,
                Location = dto.ProcessInfo.Location,
                ProcessVolumeMin = input.Process_Vol_M3_Min,
                Process_Acrmax = input.Process_Acrmax, 
                ClothArea = input.ClothArea,
                Process_Dust = input.Process_Dust,
                Process_Dustload_gmspm3 = input.Process_Dustload_gmspm3,
                Process_Temp_C = input.Process_Temp_C,
                Dew_Point_C = input.Dew_Point_C,
                Outlet_Emission_mgpm3 = input.Outlet_Emission_mgpm3,
                Process_Cloth_Ratio = input.Process_Cloth_Ratio,
                Can_Correction = input.Can_Correction,
                Customer_Equipment_Tag_No = input.Customer_Equipment_Tag_No,
                Bagfilter_Cleaning_Type = input.Bagfilter_Cleaning_Type,
                Offline_Maintainence = input.Offline_Maintainence,
                Bag_Filter_Capacity_V = input.Bag_Filter_Capacity_V,
                Process_Vol_M3_Sec = input.Process_Vol_M3_Sec,    // if you have in DTO, map it
                Process_Vol_M3_Min = input.Process_Vol_M3_Min,
                Bag_Area = input.Bag_Area,
                Bag_Bottom_Area = input.Bag_Bottom_Area,
                Min_Cloth_Area_Req = input.Min_Cloth_Area_Req,
                Min_Bag_Req = input.Min_Bag_Req,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _processInfoRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist ProcessInfo for BagfilterMasterId {Id}", bagfilterMasterId);
                throw;
            }
        }

        private async Task HandleProcessInfoUpdateAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.ProcessInfo == null)
                return;

            var input = dto.ProcessInfo;

            // Map DTO -> Entity (same mapping as your Add handler)
            var entity = new ProcessInfo
            {
                EnquiryId = (int)dto.BagfilterInput?.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Process_Volume_M3h = input.Process_Volume_M3h,
                Location = input.Location,
                ProcessVolumeMin = input.Process_Vol_M3_Min,
                Process_Acrmax = input.Process_Acrmax,
                ClothArea = input.ClothArea,
                Process_Dust = input.Process_Dust,
                Process_Dustload_gmspm3 = input.Process_Dustload_gmspm3,
                Process_Temp_C = input.Process_Temp_C,
                Dew_Point_C = input.Dew_Point_C,
                Outlet_Emission_mgpm3 = input.Outlet_Emission_mgpm3,
                Process_Cloth_Ratio = input.Process_Cloth_Ratio,
                Can_Correction = input.Can_Correction,
                Customer_Equipment_Tag_No = input.Customer_Equipment_Tag_No,
                Bagfilter_Cleaning_Type = input.Bagfilter_Cleaning_Type,
                Offline_Maintainence = input.Offline_Maintainence,
                Bag_Filter_Capacity_V = input.Bag_Filter_Capacity_V,
                Process_Vol_M3_Sec = input.Process_Vol_M3_Sec,
                Process_Vol_M3_Min = input.Process_Vol_M3_Min,
                Bag_Area = input.Bag_Area,
                Bag_Bottom_Area = input.Bag_Bottom_Area,
                Min_Cloth_Area_Req = input.Min_Cloth_Area_Req,
                Min_Bag_Req = input.Min_Bag_Req,
                UpdatedAt = DateTime.UtcNow
            };

            try
            {
                var existingId = await _processInfoRepository.GetIdForMasterAsync(bagfilterMasterId, ct);
                if (existingId.HasValue)
                {
                    entity.Id = existingId.Value;
                    await _processInfoRepository.UpdateAsync(entity);
                }
                else
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    await _processInfoRepository.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert ProcessInfo for MasterId {MasterId}", bagfilterMasterId);
                throw;
            }
        }


        private async Task HandleCageInputsAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.BagfilterInput == null)
                return; // nothing to process

            var input = dto.CageInputs;

            // Map DTO -> Entity
            var entity = new CageInputs
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Cage_Type = input.Cage_Type,
                Cage_Sub_Type = input.Cage_Sub_Type,
                Cage_Wire_Dia = input.Cage_Wire_Dia,
                No_Of_Cage_Wires = input.No_Of_Cage_Wires,
                Ring_Spacing = input.Ring_Spacing,
                Cage_Diameter = input.Cage_Diameter,
                Cage_Length = input.Cage_Length,
                Cage_Configuration = input.Cage_Configuration,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _cageInputsRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist CageInputs for BagfilterMasterId {Id}", bagfilterMasterId);
                throw;
            }
        }

        private async Task HandleCageInputsUpdateAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.CageInputs == null) return;

            var src = dto.CageInputs;

            var entity = new CageInputs
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Cage_Type = src.Cage_Type,
                Cage_Sub_Type = src.Cage_Sub_Type,
                Cage_Wire_Dia = src.Cage_Wire_Dia,
                No_Of_Cage_Wires = src.No_Of_Cage_Wires,
                Ring_Spacing = src.Ring_Spacing,
                Cage_Diameter = src.Cage_Diameter,
                Cage_Length = src.Cage_Length,
                Cage_Configuration = src.Cage_Configuration,
                UpdatedAt = DateTime.UtcNow
            };

            try
            {
                var existingId = await _cageInputsRepository.GetIdForMasterAsync(bagfilterMasterId, ct);
                if (existingId.HasValue)
                {
                    entity.Id = existingId.Value;
                    await _cageInputsRepository.UpdateAsync(entity);
                }
                else
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    await _cageInputsRepository.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert CageInputs for MasterId {MasterId}", bagfilterMasterId);
                throw;
            }
        }


        private async Task HandleBagSelectionAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.BagSelection == null)
                return; // nothing to process

            var input = dto.BagSelection;

            // Map DTO -> Entity
            var entity = new BagSelection
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Filter_Bag_Dia = input.Filter_Bag_Dia,
                Fil_Bag_Length = input.Fil_Bag_Length,
                ClothAreaPerBag = input.ClothAreaPerBag,
                noOfBags = input.noOfBags,
                Fil_Bag_Recommendation = input.Fil_Bag_Recommendation,
                Bag_Per_Row = input.Bag_Per_Row,
                Number_Of_Rows = input.Number_Of_Rows,
                Actual_Bag_Req = input.Actual_Bag_Req,
                Wire_Cross_Sec_Area = input.Wire_Cross_Sec_Area,
                No_Of_Rings = input.No_Of_Rings,
                Tot_Wire_Length = input.Tot_Wire_Length,
                Cage_Weight = input.Cage_Weight,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _bagSelectionRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist BagSelection for BagfilterMasterId {Id}", bagfilterMasterId);
                throw;
            }
        }

        private async Task HandleBagSelectionUpdateAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.BagSelection == null) return;

            var src = dto.BagSelection;
            var entity = new BagSelection
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Filter_Bag_Dia = src.Filter_Bag_Dia,
                Fil_Bag_Length = src.Fil_Bag_Length,
                ClothAreaPerBag = src.ClothAreaPerBag,
                noOfBags = src.noOfBags,
                Fil_Bag_Recommendation = src.Fil_Bag_Recommendation,
                Bag_Per_Row = src.Bag_Per_Row,
                Number_Of_Rows = src.Number_Of_Rows,
                Actual_Bag_Req = src.Actual_Bag_Req,
                Wire_Cross_Sec_Area = src.Wire_Cross_Sec_Area,
                No_Of_Rings = src.No_Of_Rings,
                Tot_Wire_Length = src.Tot_Wire_Length,
                Cage_Weight = src.Cage_Weight,
                UpdatedAt = DateTime.UtcNow
            };

            try
            {
                var existingId = await _bagSelectionRepository.GetIdForMasterAsync(bagfilterMasterId, ct);
                if (existingId.HasValue)
                {
                    entity.Id = existingId.Value;
                    await _bagSelectionRepository.UpdateAsync(entity);
                }
                else
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    await _bagSelectionRepository.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert BagSelection for MasterId {MasterId}", bagfilterMasterId);
                throw;
            }
        }


        private async Task HandleStructureInputsAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.StructureInputs == null)
                return; // nothing to process

            var input = dto.StructureInputs;

            // Map DTO -> Entity
            var entity = new StructureInputs
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Gas_Entry = input.Gas_Entry,
                Support_Structure_Type = input.Support_Structure_Type,
                
                Nominal_Width = input.Nominal_Width,
                Max_Bags_And_Pitch = input.Max_Bags_And_Pitch,
                Nominal_Width_Meters = input.Nominal_Width_Meters,
                Nominal_Length = input.Nominal_Length,
                Nominal_Length_Meters = input.Nominal_Length_Meters,
                Area_Adjust_Can_Vel = input.Area_Adjust_Can_Vel,
                Can_Area_Req = input.Can_Area_Req,
                Total_Avl_Area = input.Total_Avl_Area,
                Length_Correction = input.Length_Correction,
                Length_Correction_Derived = input.Length_Correction_Derived,
                Actual_Length = input.Actual_Length,
                Actual_Length_Meters = input.Actual_Length_Meters,
                Ol_Flange_Length = input.Ol_Flange_Length,
                Ol_Flange_Length_Mm = input.Ol_Flange_Length_Mm,

                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _structureInputsRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist StructureInputs for BagfilterMasterId {Id}", bagfilterMasterId);
                throw;
            }
        }

        private async Task HandleStructureInputsUpdateAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.StructureInputs == null) return;

            var src = dto.StructureInputs;
            var entity = new StructureInputs
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Gas_Entry = src.Gas_Entry,
                Support_Structure_Type = src.Support_Structure_Type,

                Nominal_Width = src.Nominal_Width,
                Max_Bags_And_Pitch = src.Max_Bags_And_Pitch,
                Nominal_Width_Meters = src.Nominal_Width_Meters,
                Nominal_Length = src.Nominal_Length,
                Nominal_Length_Meters = src.Nominal_Length_Meters,
                Area_Adjust_Can_Vel = src.Area_Adjust_Can_Vel,
                Can_Area_Req = src.Can_Area_Req,
                Total_Avl_Area = src.Total_Avl_Area,
                Length_Correction = src.Length_Correction,
                Length_Correction_Derived = src.Length_Correction_Derived,
                Actual_Length = src.Actual_Length,
                Actual_Length_Meters = src.Actual_Length_Meters,
                Ol_Flange_Length = src.Ol_Flange_Length,
                Ol_Flange_Length_Mm = src.Ol_Flange_Length_Mm,
                UpdatedAt = DateTime.UtcNow
            };

            try
            {
                var existingId = await _structureInputsRepository.GetIdForMasterAsync(bagfilterMasterId, ct);
                if (existingId.HasValue)
                {
                    entity.Id = existingId.Value;
                    await _structureInputsRepository.UpdateAsync(entity);
                }
                else
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    await _structureInputsRepository.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert StructureInputs for MasterId {MasterId}", bagfilterMasterId);
                throw;
            }
        }


        private async Task HandleCapsuleInputsAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.CapsuleInputs == null)
                return; // nothing to process

            var input = dto.CapsuleInputs;

            // Map DTO -> Entity
            var entity = new CapsuleInputs
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Valve_Size = input.Valve_Size,
                Voltage_Rating = input.Voltage_Rating,
                Capsule_Height = input.Capsule_Height,
                Tube_Sheet_Thickness = input.Tube_Sheet_Thickness,
                Capsule_Wall_Thickness = input.Capsule_Wall_Thickness,
                Canopy = input.Canopy,
                Solenoid_Valve_Maintainence = input.Solenoid_Valve_Maintainence,
                Capsule_Area = input.Capsule_Area,
                Capsule_Weight = input.Capsule_Weight,
                Tubesheet_Area = input.Tubesheet_Area,
                Tubesheet_Weight = input.Tubesheet_Weight,

                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _capsuleInputsRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist CapsuleInputs for BagfilterMasterId {Id}", bagfilterMasterId);
                throw;
            }
        }

        private async Task HandleCapsuleInputsUpdateAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.CapsuleInputs == null) return;

            var src = dto.CapsuleInputs;
            var entity = new CapsuleInputs
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Valve_Size = src.Valve_Size,
                Voltage_Rating = src.Voltage_Rating,
                Capsule_Height = src.Capsule_Height,
                Tube_Sheet_Thickness = src.Tube_Sheet_Thickness,
                Capsule_Wall_Thickness = src.Capsule_Wall_Thickness,
                Canopy = src.Canopy,
                Solenoid_Valve_Maintainence = src.Solenoid_Valve_Maintainence,
                Capsule_Area = src.Capsule_Area,
                Capsule_Weight = src.Capsule_Weight,
                Tubesheet_Area = src.Tubesheet_Area,
                Tubesheet_Weight = src.Tubesheet_Weight,
                UpdatedAt = DateTime.UtcNow
            };

            try
            {
                var existingId = await _capsuleInputsRepository.GetIdForMasterAsync(bagfilterMasterId, ct);
                if (existingId.HasValue)
                {
                    entity.Id = existingId.Value;
                    await _capsuleInputsRepository.UpdateAsync(entity);
                }
                else
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    await _capsuleInputsRepository.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert CapsuleInputs for MasterId {MasterId}", bagfilterMasterId);
                throw;
            }
        }


        private async Task HandleCasingInputsAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.CasingInputs == null)
                return; // nothing to process

            var input = dto.CasingInputs;

            // Map DTO -> Entity
            var entity = new CasingInputs
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Casing_Wall_Thickness = input.Casing_Wall_Thickness,
                Casing_Height = input.Casing_Height,
                Casing_Area = input.Casing_Area,
                Casing_Weight = input.Casing_Weight,

                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _casingInputsRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist CasingInputs for bagfilterMasterId {Id}", bagfilterMasterId);
                throw;
            }
        }

        private async Task HandleCasingInputsUpdateAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.CasingInputs == null) return;

            var src = dto.CasingInputs;
            var entity = new CasingInputs
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Casing_Wall_Thickness = src.Casing_Wall_Thickness,
                Casing_Height = src.Casing_Height,
                Casing_Area = src.Casing_Area,
                Casing_Weight = src.Casing_Weight,
                UpdatedAt = DateTime.UtcNow
            };

            try
            {
                var existingId = await _casingInputsRepository.GetIdForMasterAsync(bagfilterMasterId, ct);
                if (existingId.HasValue)
                {
                    entity.Id = existingId.Value;
                    await _casingInputsRepository.UpdateAsync(entity);
                }
                else
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    await _casingInputsRepository.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert CasingInputs for MasterId {MasterId}", bagfilterMasterId);
                throw;
            }
        }


        private async Task HandleHopperInputsAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.HopperInputs == null)
                return; // nothing to process

            var input = dto.HopperInputs;

            // Map DTO -> Entity
            var entity = new HopperInputs
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,

                Hopper_Type = input.Hopper_Type,
                Process_Compartments = input.Process_Compartments,
                Tot_No_Of_Hoppers = input.Tot_No_Of_Hoppers,
                Tot_No_Of_Trough = input.Tot_No_Of_Trough,
                Plenum_Width = input.Plenum_Width,
                Inlet_Height = input.Inlet_Height,
                Hopper_Thickness = input.Hopper_Thickness,
                Hopper_Valley_Angle = input.Hopper_Valley_Angle,
                Access_Door_Type = input.Access_Door_Type,
                Access_Door_Qty = input.Access_Door_Qty,
                Rav_Maintainence_Pltform = input.Rav_Maintainence_Pltform,
                Hopper_Access_Stool = input.Hopper_Access_Stool,
                Is_Distance_Piece = input.Is_Distance_Piece,
                Distance_Piece_Height = input.Distance_Piece_Height,
                Stiffening_Factor = input.Stiffening_Factor,
                Hopper = input.Hopper,
                Discharge_Opening_Sqr = input.Discharge_Opening_Sqr,
                Rav_Height = input.Rav_Height,
                Material_Handling = input.Material_Handling,
                Material_Handling_Qty = input.Material_Handling_Qty,
                Trough_Outlet_Length = input.Trough_Outlet_Length,
                Trough_Outlet_Width = input.Trough_Outlet_Width,
                Material_Handling_Xxx = input.Material_Handling_Xxx,
                Hor_Diff_Length = input.Hor_Diff_Length,
                Hor_Diff_Width = input.Hor_Diff_Width,
                Slant_Offset_Dist = input.Slant_Offset_Dist,
                Hopper_Height = input.Hopper_Height,
                Hopper_Height_Mm = input.Hopper_Height_Mm,
                Slanting_Hopper_Height = input.Slanting_Hopper_Height,
                Hopper_Area_Length = input.Hopper_Area_Length,
                Hopper_Area_Width = input.Hopper_Area_Width,
                Hopper_Tot_Area = input.Hopper_Tot_Area,
                Hopper_Weight = input.Hopper_Weight,

                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _hopperInputsRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist HopperInputs for bagfilterMasterId {Id}", bagfilterMasterId);
                throw;
            }
        }

        private async Task HandleHopperInputsUpdateAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.HopperInputs == null) return;

            var src = dto.HopperInputs;
            var entity = new HopperInputs
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,

                Hopper_Type = src.Hopper_Type,
                Process_Compartments = src.Process_Compartments,
                Tot_No_Of_Hoppers = src.Tot_No_Of_Hoppers,
                Tot_No_Of_Trough = src.Tot_No_Of_Trough,
                Plenum_Width = src.Plenum_Width,
                Inlet_Height = src.Inlet_Height,
                Hopper_Thickness = src.Hopper_Thickness,
                Hopper_Valley_Angle = src.Hopper_Valley_Angle,
                Access_Door_Type = src.Access_Door_Type,
                Access_Door_Qty = src.Access_Door_Qty,
                Rav_Maintainence_Pltform = src.Rav_Maintainence_Pltform,
                Hopper_Access_Stool = src.Hopper_Access_Stool,
                Is_Distance_Piece = src.Is_Distance_Piece,
                Distance_Piece_Height = src.Distance_Piece_Height,
                Stiffening_Factor = src.Stiffening_Factor,
                Hopper = src.Hopper,
                Discharge_Opening_Sqr = src.Discharge_Opening_Sqr,
                Rav_Height = src.Rav_Height,
                Material_Handling = src.Material_Handling,
                Material_Handling_Qty = src.Material_Handling_Qty,
                Trough_Outlet_Length = src.Trough_Outlet_Length,
                Trough_Outlet_Width = src.Trough_Outlet_Width,
                Material_Handling_Xxx = src.Material_Handling_Xxx,
                Hor_Diff_Length = src.Hor_Diff_Length,
                Hor_Diff_Width = src.Hor_Diff_Width,
                Slant_Offset_Dist = src.Slant_Offset_Dist,
                Hopper_Height = src.Hopper_Height,
                Hopper_Height_Mm = src.Hopper_Height_Mm,
                Slanting_Hopper_Height = src.Slanting_Hopper_Height,
                Hopper_Area_Length = src.Hopper_Area_Length,
                Hopper_Area_Width = src.Hopper_Area_Width,
                Hopper_Tot_Area = src.Hopper_Tot_Area,
                Hopper_Weight = src.Hopper_Weight,
                UpdatedAt = DateTime.UtcNow
            };

            try
            {
                var existingId = await _hopperInputsRepository.GetIdForMasterAsync(bagfilterMasterId, ct);
                if (existingId.HasValue)
                {
                    entity.Id = existingId.Value;
                    await _hopperInputsRepository.UpdateAsync(entity);
                }
                else
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    await _hopperInputsRepository.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert HopperInputs for MasterId {MasterId}", bagfilterMasterId);
                throw;
            }
        }


        private async Task HandleSupportStructureAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.SupportStructure == null)
                return; // nothing to process

            var input = dto.SupportStructure;

            // Map DTO -> Entity
            var entity = new SupportStructure
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Support_Struct_Type = input.Support_Struct_Type,
                NoOfColumn = input.NoOfColumn,
                Column_Height = input.Column_Height,
                Ground_Clearance = input.Ground_Clearance,
                Slide_Gate_Height = input.Slide_Gate_Height,
                Dist_Btw_Column_In_X = input.Dist_Btw_Column_In_X,
                Dist_Btw_Column_In_Z = input.Dist_Btw_Column_In_Z,
                No_Of_Bays_In_X = input.No_Of_Bays_In_X,
                No_Of_Bays_In_Z = input.No_Of_Bays_In_Z,

                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _supportStructureRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist SupportStructure for BagfilterMasterId {Id}", bagfilterMasterId);
                throw;
            }
        }

        private async Task HandleSupportStructureUpdateAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.SupportStructure == null) return;

            var src = dto.SupportStructure;
            var entity = new SupportStructure
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Support_Struct_Type = src.Support_Struct_Type,
                NoOfColumn = src.NoOfColumn,
                Column_Height = src.Column_Height,
                Ground_Clearance = src.Ground_Clearance,
                Slide_Gate_Height = src.Slide_Gate_Height,
                Dist_Btw_Column_In_X = src.Dist_Btw_Column_In_X,
                Dist_Btw_Column_In_Z = src.Dist_Btw_Column_In_Z,
                No_Of_Bays_In_X = src.No_Of_Bays_In_X,
                No_Of_Bays_In_Z = src.No_Of_Bays_In_Z,
                UpdatedAt = DateTime.UtcNow
            };

            try
            {
                var existingId = await _supportStructureRepository.GetIdForMasterAsync(bagfilterMasterId, ct);
                if (existingId.HasValue)
                {
                    entity.Id = existingId.Value;
                    await _supportStructureRepository.UpdateAsync(entity);
                }
                else
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    await _supportStructureRepository.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert SupportStructure for MasterId {MasterId}", bagfilterMasterId);
                throw;
            }
        }


        private async Task HandleAccessGroupAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.AccessGroup == null)
                return; // nothing to process

            var input = dto.AccessGroup;

            var entity = new AccessGroup
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,

                Access_Type = input.Access_Type,
                Cage_Weight_Ladder = input.Cage_Weight_Ladder,
                Mid_Landing_Pltform = input.Mid_Landing_Pltform,
                Platform_Weight = input.Platform_Weight,
                Staircase_Height = input.Staircase_Height,
                Staircase_Weight = input.Staircase_Weight,
                Railing_Weight = input.Railing_Weight,
                Maintainence_Pltform = input.Maintainence_Pltform,
                Maintainence_Pltform_Weight = input.Maintainence_Pltform_Weight,
                BlowPipe = input.BlowPipe,
                PressureHeader = input.PressureHeader,
                DistancePiece = input.DistancePiece,
                Access_Stool_Size_Mm = input.Access_Stool_Size_Mm,
                Access_Stool_Size_Kg = input.Access_Stool_Size_Kg,

                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _accessGroupRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist AccessGroup for BagfilterMasterId {Id}", bagfilterMasterId);
                throw;
            }
        }

        private async Task HandleAccessGroupUpdateAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.AccessGroup == null) return;

            var src = dto.AccessGroup;
            var entity = new AccessGroup
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,

                Access_Type = src.Access_Type,
                Cage_Weight_Ladder = src.Cage_Weight_Ladder,
                Mid_Landing_Pltform = src.Mid_Landing_Pltform,
                Platform_Weight = src.Platform_Weight,
                Staircase_Height = src.Staircase_Height,
                Staircase_Weight = src.Staircase_Weight,
                Railing_Weight = src.Railing_Weight,
                Maintainence_Pltform = src.Maintainence_Pltform,
                Maintainence_Pltform_Weight = src.Maintainence_Pltform_Weight,
                BlowPipe = src.BlowPipe,
                PressureHeader = src.PressureHeader,
                DistancePiece = src.DistancePiece,
                Access_Stool_Size_Mm = src.Access_Stool_Size_Mm,
                Access_Stool_Size_Kg = src.Access_Stool_Size_Kg,

                UpdatedAt = DateTime.UtcNow
            };

            try
            {
                var existingId = await _accessGroupRepository.GetIdForMasterAsync(bagfilterMasterId, ct);
                if (existingId.HasValue)
                {
                    entity.Id = existingId.Value;
                    await _accessGroupRepository.UpdateAsync(entity);
                }
                else
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    await _accessGroupRepository.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert AccessGroup for MasterId {MasterId}", bagfilterMasterId);
                throw;
            }
        }


        private async Task HandleRoofDoorAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.RoofDoor == null)
                return; // nothing to process

            var input = dto.RoofDoor;

            // Map DTO -> Entity
            var entity = new RoofDoor
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,

                Roof_Door_Thickness = input.Roof_Door_Thickness,
                T2d = input.T2d,
                T3d = input.T3d,
                N_Doors = input.N_Doors,
                Compartment_No = input.Compartment_No,
                Stiffness_Factor_For_Roof_Door = input.Stiffness_Factor_For_Roof_Door,
                Weight_Per_Door = input.Weight_Per_Door,
                Tot_Weight_Per_Compartment = input.Tot_Weight_Per_Compartment,

                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _roofDoorRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist RoofDoor for BagfilterMasterId {Id}", bagfilterMasterId);
                throw;
            }
        }

        private async Task HandleRoofDoorUpdateAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.RoofDoor == null) return;

            var src = dto.RoofDoor;
            var entity = new RoofDoor
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,

                Roof_Door_Thickness = src.Roof_Door_Thickness,
                T2d = src.T2d,
                T3d = src.T3d,
                N_Doors = src.N_Doors,
                Compartment_No = src.Compartment_No,
                Stiffness_Factor_For_Roof_Door = src.Stiffness_Factor_For_Roof_Door,
                Weight_Per_Door = src.Weight_Per_Door,
                Tot_Weight_Per_Compartment = src.Tot_Weight_Per_Compartment,
                UpdatedAt = DateTime.UtcNow
            };

            try
            {
                var existingId = await _roofDoorRepository.GetIdForMasterAsync(bagfilterMasterId, ct);
                if (existingId.HasValue)
                {
                    entity.Id = existingId.Value;
                    await _roofDoorRepository.UpdateAsync(entity);
                }
                else
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    await _roofDoorRepository.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert RoofDoor for MasterId {MasterId}", bagfilterMasterId);
                throw;
            }
        }


        private async Task HandlePaintingAreaAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.PaintingArea == null)
                return; // nothing to process

            var input = dto.PaintingArea;

            // Map DTO -> Entity
            var entity = new PaintingArea
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,

                Inside_Area_Casing_Area_Mm2 = input.Inside_Area_Casing_Area_Mm2,
                Inside_Area_Casing_Area_M2 = input.Inside_Area_Casing_Area_M2,
                Inside_Area_Hopper_Area_Mm2 = input.Inside_Area_Hopper_Area_Mm2,
                Inside_Area_Hopper_Area_M2 = input.Inside_Area_Hopper_Area_M2,
                Inside_Area_Air_Header_Mm2 = input.Inside_Area_Air_Header_Mm2,
                Inside_Area_Air_Header_M2 = input.Inside_Area_Air_Header_M2,
                Inside_Area_Purge_Pipe_Mm2 = input.Inside_Area_Purge_Pipe_Mm2,
                Inside_Area_Purge_Pipe_M2 = input.Inside_Area_Purge_Pipe_M2,
                Inside_Area_Roof_Door_Mm2 = input.Inside_Area_Roof_Door_Mm2,
                Inside_Area_Roof_Door_M2 = input.Inside_Area_Roof_Door_M2,
                Inside_Area_Tube_Sheet_Mm2 = input.Inside_Area_Tube_Sheet_Mm2,
                Inside_Area_Tube_Sheet_M2 = input.Inside_Area_Tube_Sheet_M2,
                Inside_Area_Total_M2 = input.Inside_Area_Total_M2,
                Outside_Area_Casing_Area_Mm2 = input.Outside_Area_Casing_Area_Mm2,
                Outside_Area_Casing_Area_M2 = input.Outside_Area_Casing_Area_M2,
                Outside_Area_Hopper_Area_Mm2 = input.Outside_Area_Hopper_Area_Mm2,
                Outside_Area_Hopper_Area_M2 = input.Outside_Area_Hopper_Area_M2,
                Outside_Area_Air_Header_Mm2 = input.Outside_Area_Air_Header_Mm2,
                Outside_Area_Air_Header_M2 = input.Outside_Area_Air_Header_M2,
                Outside_Area_Purge_Pipe_Mm2 = input.Outside_Area_Purge_Pipe_Mm2,
                Outside_Area_Purge_Pipe_M2 = input.Outside_Area_Purge_Pipe_M2,
                Outside_Area_Roof_Door_Mm2 = input.Outside_Area_Roof_Door_Mm2,
                Outside_Area_Roof_Door_M2 = input.Outside_Area_Roof_Door_M2,
                Outside_Area_Tube_Sheet_Mm2 = input.Outside_Area_Tube_Sheet_Mm2,
                Outside_Area_Tube_Sheet_M2 = input.Outside_Area_Tube_Sheet_M2,
                Outside_Area_Total_M2 = input.Outside_Area_Total_M2,

                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _paintingAreaRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist PaintingArea for BagfilterMasterId {Id}", bagfilterMasterId);
                throw;
            }
        }

        private async Task HandlePaintingAreaUpdateAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.PaintingArea == null) return;

            var src = dto.PaintingArea;
            var entity = new PaintingArea
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,

                Inside_Area_Casing_Area_Mm2 = src.Inside_Area_Casing_Area_Mm2,
                Inside_Area_Casing_Area_M2 = src.Inside_Area_Casing_Area_M2,
                Inside_Area_Hopper_Area_Mm2 = src.Inside_Area_Hopper_Area_Mm2,
                Inside_Area_Hopper_Area_M2 = src.Inside_Area_Hopper_Area_M2,
                Inside_Area_Air_Header_Mm2 = src.Inside_Area_Air_Header_Mm2,
                Inside_Area_Air_Header_M2 = src.Inside_Area_Air_Header_M2,
                Inside_Area_Purge_Pipe_Mm2 = src.Inside_Area_Purge_Pipe_Mm2,
                Inside_Area_Purge_Pipe_M2 = src.Inside_Area_Purge_Pipe_M2,
                Inside_Area_Roof_Door_Mm2 = src.Inside_Area_Roof_Door_Mm2,
                Inside_Area_Roof_Door_M2 = src.Inside_Area_Roof_Door_M2,
                Inside_Area_Tube_Sheet_Mm2 = src.Inside_Area_Tube_Sheet_Mm2,
                Inside_Area_Tube_Sheet_M2 = src.Inside_Area_Tube_Sheet_M2,
                Inside_Area_Total_M2 = src.Inside_Area_Total_M2,
                Outside_Area_Casing_Area_Mm2 = src.Outside_Area_Casing_Area_Mm2,
                Outside_Area_Casing_Area_M2 = src.Outside_Area_Casing_Area_M2,
                Outside_Area_Hopper_Area_Mm2 = src.Outside_Area_Hopper_Area_Mm2,
                Outside_Area_Hopper_Area_M2 = src.Outside_Area_Hopper_Area_M2,
                Outside_Area_Air_Header_Mm2 = src.Outside_Area_Air_Header_Mm2,
                Outside_Area_Air_Header_M2 = src.Outside_Area_Air_Header_M2,
                Outside_Area_Purge_Pipe_Mm2 = src.Outside_Area_Purge_Pipe_Mm2,
                Outside_Area_Purge_Pipe_M2 = src.Outside_Area_Purge_Pipe_M2,
                Outside_Area_Roof_Door_Mm2 = src.Outside_Area_Roof_Door_Mm2,
                Outside_Area_Roof_Door_M2 = src.Outside_Area_Roof_Door_M2,
                Outside_Area_Tube_Sheet_Mm2 = src.Outside_Area_Tube_Sheet_Mm2,
                Outside_Area_Tube_Sheet_M2 = src.Outside_Area_Tube_Sheet_M2,
                Outside_Area_Total_M2 = src.Outside_Area_Total_M2,
                // TODO: map painting area fields
                UpdatedAt = DateTime.UtcNow
            };

            try
            {
                var existingId = await _paintingAreaRepository.GetIdForMasterAsync(bagfilterMasterId, ct);
                if (existingId.HasValue)
                {
                    entity.Id = existingId.Value;
                    await _paintingAreaRepository.UpdateAsync(entity);
                }
                else
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    await _paintingAreaRepository.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert PaintingArea for MasterId {MasterId}", bagfilterMasterId);
                throw;
            }
        }

        private async Task HandleBillOfMaterialAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.BillOfMaterial == null || !dto.BillOfMaterial.Any())
                return;

            var enquiryId = (int)dto.BagfilterInput.EnquiryId;

            // Convert all DTO → Entities in one shot
            var entities = dto.BillOfMaterial.Select(line => new BillOfMaterial
            {
                EnquiryId = enquiryId,
                BagfilterMasterId = bagfilterMasterId,

                Item = line.Item,
                Material = line.Material,
                Weight = line.Weight,
                Units = line.Units,
                Rate = line.Rate,
                Cost = line.Cost,

                CreatedAt = DateTime.UtcNow
            }).ToList();

            try
            {
                await _billOfMaterialRepository.AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to persist BillOfMaterial (Batch) for BagfilterMasterId {BfMasterId}",
                    bagfilterMasterId);
                throw;
            }
        }

        private async Task HandleBillOfMaterialUpdateAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.BillOfMaterial == null || !dto.BillOfMaterial.Any())
            {
               
                return;
            }

            var enquiryId = dto.BagfilterInput?.EnquiryId ?? 0;

            var entities = dto.BillOfMaterial.Select((line, idx) => new BillOfMaterial
            {
                EnquiryId = enquiryId,
                BagfilterMasterId = bagfilterMasterId,
                Item = line.Item,
                Material = line.Material,
                Weight = line.Weight,
                Units = line.Units,
                Rate = line.Rate,
                Cost = line.Cost,
                SortOrder = line.SortOrder ?? (idx + 1),
                CreatedAt = DateTime.UtcNow
            }).ToList();

            try
            {
                // Do delete + insert in repository to keep a single transaction if you prefer:
                await _billOfMaterialRepository.ReplaceForMasterAsync(bagfilterMasterId, entities, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist BillOfMaterial (Batch) for BagfilterMasterId {BfMasterId}", bagfilterMasterId);
                throw;
            }
        }



        private async Task HandlePaintingCostAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.PaintingCost == null)
                return; // nothing to process

            var input = dto.PaintingCost;

            // Map DTO -> Entity
            var entity = new PaintingCost
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,

                PaintingTableJson = input.PaintingTableJson,

                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _paintingCostRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist Painting Cost for BagfilterMasterId {Id}", bagfilterMasterId);
                throw;
            }
        }
        private async Task HandlePaintingCostUpdateAsync(BagfilterInputMainDto dto, int bagfilterMasterId, CancellationToken ct)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.PaintingCost == null) return;

            var src = dto.PaintingCost;
            var entity = new PaintingCost
            {
                EnquiryId = (int)dto.BagfilterInput.EnquiryId,
                BagfilterMasterId = bagfilterMasterId,

                PaintingTableJson = src.PaintingTableJson,

                UpdatedAt = DateTime.UtcNow
            };

            try
            {
                var existingId = await _paintingCostRepository.GetIdForMasterAsync(bagfilterMasterId, ct);
                if (existingId.HasValue)
                {
                    entity.Id = existingId.Value;
                    await _paintingCostRepository.UpdateAsync(entity);
                }
                else
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    await _paintingCostRepository.AddAsync(entity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert PaintingCost for MasterId {MasterId}", bagfilterMasterId);
                throw;
            }
        }





        private WeightSummary MapWeightSummary(
        BagfilterInputMainDto dto,
        int bagfilterMasterId,
        WeightSummary existing = null)
        {
            if (dto == null || dto.WeightSummary == null)
                return null;

            var src = dto.WeightSummary;

            // Choose how to determine EnquiryId.
            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                existing?.EnquiryId ??
                0;

            var entity = new WeightSummary
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                Casing_Weight = src.Casing_Weight,
                Capsule_Weight = src.Capsule_Weight,
                Tot_Weight_Per_Compartment = src.Tot_Weight_Per_Compartment,
                Hopper_Weight = src.Hopper_Weight,
                Weight_Of_Cage_Ladder = src.Weight_Of_Cage_Ladder,
                Railing_Weight = src.Railing_Weight,
                Tubesheet_Weight = src.Tubesheet_Weight,
                Air_Header_Blow_Pipe = src.Air_Header_Blow_Pipe,
                Hopper_Access_Stool_Weight = src.Hopper_Access_Stool_Weight,
                Weight_Of_Mid_Landing_Plt = src.Weight_Of_Mid_Landing_Plt,
                Weight_Of_Maintainence_Pltform = src.Weight_Of_Maintainence_Pltform,
                Cage_Weight = src.Cage_Weight,
                Structure_Weight = src.Structure_Weight,
                Weight_Total = src.Weight_Total,
                // CreatedAt / UpdatedAt handled in repo
            };

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.CreatedAt = existing.CreatedAt;
            }

            return entity;
        }


        private ProcessInfo MapProcessInfo(
    BagfilterInputMainDto dto,
    int bagfilterMasterId,
    ProcessInfo existing = null)
        {
            if (dto == null || dto.ProcessInfo == null)
                return null;

            var src = dto.ProcessInfo;

            // Choose how to determine EnquiryId.
            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                existing?.EnquiryId ??
                0;

            var entity = new ProcessInfo
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                Process_Volume_M3h = src.Process_Volume_M3h,
                Location = src.Location,
                ProcessVolumeMin = src.ProcessVolumeMin,
                Process_Acrmax = src.Process_Acrmax,
                ClothArea = src.ClothArea,
                Process_Dust = src.Process_Dust,
                Process_Dustload_gmspm3 = src.Process_Dustload_gmspm3,
                Process_Temp_C = src.Process_Temp_C,
                Dew_Point_C = src.Dew_Point_C,
                Outlet_Emission_mgpm3 = src.Outlet_Emission_mgpm3,
                Process_Cloth_Ratio = src.Process_Cloth_Ratio,
                Can_Correction = src.Can_Correction,
                Customer_Equipment_Tag_No = src.Customer_Equipment_Tag_No,
                Bagfilter_Cleaning_Type = src.Bagfilter_Cleaning_Type,
                Offline_Maintainence = src.Offline_Maintainence,
                Bag_Filter_Capacity_V = src.Bag_Filter_Capacity_V,
                Process_Vol_M3_Sec = src.Process_Vol_M3_Sec,
                Process_Vol_M3_Min = src.Process_Vol_M3_Min,
                Bag_Area = src.Bag_Area,
                Bag_Bottom_Area = src.Bag_Bottom_Area,
                Min_Cloth_Area_Req = src.Min_Cloth_Area_Req,
                Min_Bag_Req = src.Min_Bag_Req,

                // CreatedAt / UpdatedAt handled in repo
            };

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.CreatedAt = existing.CreatedAt;
            }

            return entity;
        }

        private CageInputs MapCageInputs(
    BagfilterInputMainDto dto,
    int bagfilterMasterId,
    CageInputs existing = null)
        {
            if (dto == null || dto.CageInputs == null)
                return null;

            var src = dto.CageInputs;

            // Choose how to determine EnquiryId.
            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                existing?.EnquiryId ??
                0;

            var entity = new CageInputs
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                Cage_Type = src.Cage_Type,
                Cage_Sub_Type = src.Cage_Sub_Type,
                Cage_Wire_Dia = src.Cage_Wire_Dia,
                No_Of_Cage_Wires = src.No_Of_Cage_Wires,
                Ring_Spacing = src.Ring_Spacing,
                Cage_Diameter = src.Cage_Diameter,
                Cage_Length = src.Cage_Length,
                Cage_Configuration = src.Cage_Configuration,

                // CreatedAt / UpdatedAt handled in repo
            };

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.CreatedAt = existing.CreatedAt;
            }

            return entity;
        }

        private BagSelection MapBagSelection(
    BagfilterInputMainDto dto,
    int bagfilterMasterId,
    BagSelection existing = null)
        {
            if (dto == null || dto.BagSelection == null)
                return null;

            var src = dto.BagSelection;

            // Choose how to determine EnquiryId.
            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                existing?.EnquiryId ??
                0;

            var entity = new BagSelection
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                Filter_Bag_Dia = src.Filter_Bag_Dia,
                Fil_Bag_Length = src.Fil_Bag_Length,
                ClothAreaPerBag = src.ClothAreaPerBag,
                noOfBags = src.noOfBags,
                Fil_Bag_Recommendation = src.Fil_Bag_Recommendation,
                Bag_Per_Row = src.Bag_Per_Row,
                Number_Of_Rows = src.Number_Of_Rows,
                Actual_Bag_Req = src.Actual_Bag_Req,
                Wire_Cross_Sec_Area = src.Wire_Cross_Sec_Area,
                No_Of_Rings = src.No_Of_Rings,
                Tot_Wire_Length = src.Tot_Wire_Length,
                Cage_Weight = src.Cage_Weight,

                // CreatedAt / UpdatedAt handled in repo
            };

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.CreatedAt = existing.CreatedAt;
            }

            return entity;
        }

        private StructureInputs MapStructureInputs(
    BagfilterInputMainDto dto,
    int bagfilterMasterId,
    StructureInputs existing = null)
        {
            if (dto == null || dto.StructureInputs == null)
                return null;

            var src = dto.StructureInputs;

            // Choose how to determine EnquiryId.
            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                existing?.EnquiryId ??
                0;

            var entity = new StructureInputs
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                Gas_Entry = src.Gas_Entry,
                Support_Structure_Type = src.Support_Structure_Type,

                Nominal_Width = src.Nominal_Width,
                Max_Bags_And_Pitch = src.Max_Bags_And_Pitch,
                Nominal_Width_Meters = src.Nominal_Width_Meters,
                Nominal_Length = src.Nominal_Length,
                Nominal_Length_Meters = src.Nominal_Length_Meters,
                Area_Adjust_Can_Vel = src.Area_Adjust_Can_Vel,
                Can_Area_Req = src.Can_Area_Req,
                Total_Avl_Area = src.Total_Avl_Area,
                Length_Correction = src.Length_Correction,
                Length_Correction_Derived = src.Length_Correction_Derived,
                Actual_Length = src.Actual_Length,
                Actual_Length_Meters = src.Actual_Length_Meters,
                Ol_Flange_Length = src.Ol_Flange_Length,
                Ol_Flange_Length_Mm = src.Ol_Flange_Length_Mm,

                // CreatedAt / UpdatedAt handled in repo
            };

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.CreatedAt = existing.CreatedAt;
            }

            return entity;
        }

        private CapsuleInputs MapCapsuleInputs(
    BagfilterInputMainDto dto,
    int bagfilterMasterId,
    CapsuleInputs existing = null)
        {
            if (dto == null || dto.CapsuleInputs == null)
                return null;

            var src = dto.CapsuleInputs;

            // Choose how to determine EnquiryId.
            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                existing?.EnquiryId ??
                0;

            var entity = new CapsuleInputs
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                Valve_Size = src.Valve_Size,
                Voltage_Rating = src.Voltage_Rating,
                Capsule_Height = src.Capsule_Height,
                Tube_Sheet_Thickness = src.Tube_Sheet_Thickness,
                Capsule_Wall_Thickness = src.Capsule_Wall_Thickness,
                Canopy = src.Canopy,
                Solenoid_Valve_Maintainence = src.Solenoid_Valve_Maintainence,
                Capsule_Area = src.Capsule_Area,
                Capsule_Weight = src.Capsule_Weight,
                Tubesheet_Area = src.Tubesheet_Area,
                Tubesheet_Weight = src.Tubesheet_Weight,

                // CreatedAt / UpdatedAt handled in repo
            };

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.CreatedAt = existing.CreatedAt;
            }

            return entity;
        }

        private CasingInputs MapCasingInputs(
    BagfilterInputMainDto dto,
    int bagfilterMasterId,
    CasingInputs existing = null)
        {
            if (dto == null || dto.CasingInputs == null)
                return null;

            var src = dto.CasingInputs;

            // Choose how to determine EnquiryId.
            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                existing?.EnquiryId ??
                0;

            var entity = new CasingInputs
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                Casing_Wall_Thickness = src.Casing_Wall_Thickness,
                Casing_Height = src.Casing_Height,
                Casing_Area = src.Casing_Area,
                Casing_Weight = src.Casing_Weight,

                // CreatedAt / UpdatedAt handled in repo
            };

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.CreatedAt = existing.CreatedAt;
            }

            return entity;
        }

        private HopperInputs MapHopperInputs(
    BagfilterInputMainDto dto,
    int bagfilterMasterId,
    HopperInputs existing = null)
        {
            if (dto == null || dto.HopperInputs == null)
                return null;

            var src = dto.HopperInputs;

            // Choose how to determine EnquiryId.
            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                existing?.EnquiryId ??
                0;

            var entity = new HopperInputs
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                Hopper_Type = src.Hopper_Type,
                Process_Compartments = src.Process_Compartments,
                Tot_No_Of_Hoppers = src.Tot_No_Of_Hoppers,
                Tot_No_Of_Trough = src.Tot_No_Of_Trough,
                Plenum_Width = src.Plenum_Width,
                Inlet_Height = src.Inlet_Height,
                Hopper_Thickness = src.Hopper_Thickness,
                Hopper_Valley_Angle = src.Hopper_Valley_Angle,
                Access_Door_Type = src.Access_Door_Type,
                Access_Door_Qty = src.Access_Door_Qty,
                Rav_Maintainence_Pltform = src.Rav_Maintainence_Pltform,
                Hopper_Access_Stool = src.Hopper_Access_Stool,
                Is_Distance_Piece = src.Is_Distance_Piece,
                Distance_Piece_Height = src.Distance_Piece_Height,
                Stiffening_Factor = src.Stiffening_Factor,
                Hopper = src.Hopper,
                Discharge_Opening_Sqr = src.Discharge_Opening_Sqr,
                Rav_Height = src.Rav_Height,
                Material_Handling = src.Material_Handling,
                Material_Handling_Qty = src.Material_Handling_Qty,
                Trough_Outlet_Length = src.Trough_Outlet_Length,
                Trough_Outlet_Width = src.Trough_Outlet_Width,
                Material_Handling_Xxx = src.Material_Handling_Xxx,
                Hor_Diff_Length = src.Hor_Diff_Length,
                Hor_Diff_Width = src.Hor_Diff_Width,
                Slant_Offset_Dist = src.Slant_Offset_Dist,
                Hopper_Height = src.Hopper_Height,
                Hopper_Height_Mm = src.Hopper_Height_Mm,
                Slanting_Hopper_Height = src.Slanting_Hopper_Height,
                Hopper_Area_Length = src.Hopper_Area_Length,
                Hopper_Area_Width = src.Hopper_Area_Width,
                Hopper_Tot_Area = src.Hopper_Tot_Area,
                Hopper_Weight = src.Hopper_Weight,

                // CreatedAt / UpdatedAt handled in repo
            };

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.CreatedAt = existing.CreatedAt;
            }

            return entity;
        }

        private SupportStructure MapSupportStructure(
    BagfilterInputMainDto dto,
    int bagfilterMasterId,
    SupportStructure existing = null)
        {
            if (dto == null || dto.SupportStructure == null)
                return null;

            var src = dto.SupportStructure;

            // Choose how to determine EnquiryId.
            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                existing?.EnquiryId ??
                0;

            var entity = new SupportStructure
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                Support_Struct_Type = src.Support_Struct_Type,
                NoOfColumn = src.NoOfColumn,
                Column_Height = src.Column_Height,
                Ground_Clearance = src.Ground_Clearance,
                Slide_Gate_Height = src.Slide_Gate_Height,
                Dist_Btw_Column_In_X = src.Dist_Btw_Column_In_X,
                Dist_Btw_Column_In_Z = src.Dist_Btw_Column_In_Z,
                No_Of_Bays_In_X = src.No_Of_Bays_In_X,
                No_Of_Bays_In_Z = src.No_Of_Bays_In_Z,

                // CreatedAt / UpdatedAt handled in repo
            };

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.CreatedAt = existing.CreatedAt;
            }

            return entity;
        }

        private AccessGroup MapAccessGroup(
    BagfilterInputMainDto dto,
    int bagfilterMasterId,
    AccessGroup existing = null)
        {
            if (dto == null || dto.AccessGroup == null)
                return null;

            var src = dto.AccessGroup;

            // Choose how to determine EnquiryId.
            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                existing?.EnquiryId ??
                0;

            var entity = new AccessGroup
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                Access_Type = src.Access_Type,
                Cage_Weight_Ladder = src.Cage_Weight_Ladder,
                Mid_Landing_Pltform = src.Mid_Landing_Pltform,
                Platform_Weight = src.Platform_Weight,
                Staircase_Height = src.Staircase_Height,
                Staircase_Weight = src.Staircase_Weight,
                Railing_Weight = src.Railing_Weight,
                Maintainence_Pltform = src.Maintainence_Pltform,
                Maintainence_Pltform_Weight = src.Maintainence_Pltform_Weight,
                BlowPipe = src.BlowPipe,
                PressureHeader = src.PressureHeader,
                DistancePiece = src.DistancePiece,
                Access_Stool_Size_Mm = src.Access_Stool_Size_Mm,
                Access_Stool_Size_Kg = src.Access_Stool_Size_Kg,

                // CreatedAt / UpdatedAt handled in repo
            };

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.CreatedAt = existing.CreatedAt;
            }

            return entity;
        }

        private RoofDoor MapRoofDoor(
    BagfilterInputMainDto dto,
    int bagfilterMasterId,
    RoofDoor existing = null)
        {
            if (dto == null || dto.RoofDoor == null)
                return null;

            var src = dto.RoofDoor;

            // Choose how to determine EnquiryId.
            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                existing?.EnquiryId ??
                0;

            var entity = new RoofDoor
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                Roof_Door_Thickness = src.Roof_Door_Thickness,
                T2d = src.T2d,
                T3d = src.T3d,
                N_Doors = src.N_Doors,
                Compartment_No = src.Compartment_No,
                Stiffness_Factor_For_Roof_Door = src.Stiffness_Factor_For_Roof_Door,
                Weight_Per_Door = src.Weight_Per_Door,
                Tot_Weight_Per_Compartment = src.Tot_Weight_Per_Compartment,

                // CreatedAt / UpdatedAt handled in repo
            };

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.CreatedAt = existing.CreatedAt;
            }

            return entity;
        }

        private PaintingArea MapPaintingArea(
    BagfilterInputMainDto dto,
    int bagfilterMasterId,
    PaintingArea existing = null)
        {
            if (dto == null || dto.PaintingArea == null)
                return null;

            var src = dto.PaintingArea;

            // Choose how to determine EnquiryId.
            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                existing?.EnquiryId ??
                0;

            var entity = new PaintingArea
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                Inside_Area_Casing_Area_Mm2 = src.Inside_Area_Casing_Area_Mm2,
                Inside_Area_Casing_Area_M2 = src.Inside_Area_Casing_Area_M2,
                Inside_Area_Hopper_Area_Mm2 = src.Inside_Area_Hopper_Area_Mm2,
                Inside_Area_Hopper_Area_M2 = src.Inside_Area_Hopper_Area_M2,
                Inside_Area_Air_Header_Mm2 = src.Inside_Area_Air_Header_Mm2,
                Inside_Area_Air_Header_M2 = src.Inside_Area_Air_Header_M2,
                Inside_Area_Purge_Pipe_Mm2 = src.Inside_Area_Purge_Pipe_Mm2,
                Inside_Area_Purge_Pipe_M2 = src.Inside_Area_Purge_Pipe_M2,
                Inside_Area_Roof_Door_Mm2 = src.Inside_Area_Roof_Door_Mm2,
                Inside_Area_Roof_Door_M2 = src.Inside_Area_Roof_Door_M2,
                Inside_Area_Tube_Sheet_Mm2 = src.Inside_Area_Tube_Sheet_Mm2,
                Inside_Area_Tube_Sheet_M2 = src.Inside_Area_Tube_Sheet_M2,
                Inside_Area_Total_M2 = src.Inside_Area_Total_M2,

                Outside_Area_Casing_Area_Mm2 = src.Outside_Area_Casing_Area_Mm2,
                Outside_Area_Casing_Area_M2 = src.Outside_Area_Casing_Area_M2,
                Outside_Area_Hopper_Area_Mm2 = src.Outside_Area_Hopper_Area_Mm2,
                Outside_Area_Hopper_Area_M2 = src.Outside_Area_Hopper_Area_M2,
                Outside_Area_Air_Header_Mm2 = src.Outside_Area_Air_Header_Mm2,
                Outside_Area_Air_Header_M2 = src.Outside_Area_Air_Header_M2,
                Outside_Area_Purge_Pipe_Mm2 = src.Outside_Area_Purge_Pipe_Mm2,
                Outside_Area_Purge_Pipe_M2 = src.Outside_Area_Purge_Pipe_M2,
                Outside_Area_Roof_Door_Mm2 = src.Outside_Area_Roof_Door_Mm2,
                Outside_Area_Roof_Door_M2 = src.Outside_Area_Roof_Door_M2,
                Outside_Area_Tube_Sheet_Mm2 = src.Outside_Area_Tube_Sheet_Mm2,
                Outside_Area_Tube_Sheet_M2 = src.Outside_Area_Tube_Sheet_M2,
                Outside_Area_Total_M2 = src.Outside_Area_Total_M2,

                // CreatedAt / UpdatedAt handled in repo
            };

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.CreatedAt = existing.CreatedAt;
            }

            return entity;
        }

        private PaintingCost MapPaintingCost(
         BagfilterInputMainDto dto,
         int bagfilterMasterId,
         PaintingCost existing = null)
        {
            if (dto == null || dto.PaintingCost == null)
                return null;

            var src = dto.PaintingCost;

            // Choose how to determine EnquiryId.
            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                existing?.EnquiryId ??
                0;

            var entity = new PaintingCost
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                PaintingTableJson = src.PaintingTableJson,

                // CreatedAt / UpdatedAt handled in repo
            };

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.CreatedAt = existing.CreatedAt;
            }

            return entity;
        }



        private BillOfMaterial MapBillOfMaterialItem(
        BillOfMaterialDto dto,
        int bagfilterMasterId,
        int enquiryId,
        int sortOrder)
        {
            if (dto == null) return null;

            return new BillOfMaterial
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                Item = dto.Item,
                Material = dto.Material,
                Weight = dto.Weight,
                Units = dto.Units,
                Rate = dto.Rate,
                Cost = dto.Cost,
                SortOrder = dto.SortOrder ?? sortOrder,

                // CreatedAt / UpdatedAt handled in repo
            };
        }

        private List<BillOfMaterial> MapBillOfMaterialCollection(
    BagfilterInputMainDto dto,
    int bagfilterMasterId)
        {
            // interpret: if list is null or empty, caller decides semantics (ignore / clear)
            if (dto == null || dto.BillOfMaterial == null || dto.BillOfMaterial.Count == 0)
                return new List<BillOfMaterial>();

            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                0;

            var result = new List<BillOfMaterial>(dto.BillOfMaterial.Count);

            for (int index = 0; index < dto.BillOfMaterial.Count; index++)
            {
                var src = dto.BillOfMaterial[index];
                var sortOrder = src.SortOrder ?? (index + 1);

                var entity = MapBillOfMaterialItem(
                    src,
                    bagfilterMasterId,
                    enquiryId,
                    sortOrder);

                if (entity != null)
                    result.Add(entity);
            }

            return result;
        }


        /// <summary>
        /// Generic batched upsert for child entities keyed by BagfilterMasterId.
        /// </summary>
        private async Task BatchUpsertChildAsync<TEntity>(
            List<BagfilterInputMainDto> dtos,
            int[] masterIdByIndex,
            Func<BagfilterInputMainDto, bool> hasChildDto,
            Func<IEnumerable<int>, CancellationToken, Task<Dictionary<int, TEntity>>> getExistingByMasterIds,
            Func<BagfilterInputMainDto, int, TEntity?, TEntity?> mapEntity,
            Func<IEnumerable<TEntity>, CancellationToken, Task> upsertRange,
            CancellationToken ct)
        {
            if (dtos == null || dtos.Count == 0) return;

            // 1) Collect masterIds that actually have this child DTO
            var masterIds = new HashSet<int>();

            for (int i = 0; i < dtos.Count; i++)
            {
                if (!hasChildDto(dtos[i])) continue;

                var masterId = masterIdByIndex[i];
                if (masterId > 0)
                    masterIds.Add(masterId);
            }

            if (masterIds.Count == 0)
                return;

            // 2) Load existing entities for those masterIds in one shot
            var existingByMasterId = await getExistingByMasterIds(masterIds, ct);
            existingByMasterId ??= new Dictionary<int, TEntity>();

            // 3) Map all DTOs -> entities
            var toUpsert = new List<TEntity>();

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];
                if (!hasChildDto(dto)) continue;

                var masterId = masterIdByIndex[i];
                if (masterId <= 0) continue;

                existingByMasterId.TryGetValue(masterId, out var existing);

                var entity = mapEntity(dto, masterId, existing);
                if (entity != null)
                    toUpsert.Add(entity);
            }

            if (toUpsert.Count == 0)
                return;

            // 4) One batched upsert call for this child type
            await upsertRange(toUpsert, ct);
        }



    }
}

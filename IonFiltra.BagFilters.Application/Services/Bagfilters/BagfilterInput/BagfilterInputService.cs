using System.Globalization;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterInputs;
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



        public BagfilterInputService(
            IBagfilterInputRepository repository,
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

            // initialize handler registry
            _childHandlers = new Dictionary<string, Func<BagfilterInputMainDto, int, CancellationToken, Task>>(StringComparer.OrdinalIgnoreCase)
            {
                // key is the DTO property name (e.g. "WeightSummary" or "weightSummary" - registry is case-insensitive)
                ["weightSummary"] = HandleWeightSummaryAsync,
                ["processInfo"] = HandleProcessInfoAsync,
                ["cageInputs"] = HandleCageInputsAsync,
                ["bagSelection"] = HandleBagSelectionAsync,
                ["structureInputs"] = HandleStructureInputsAsync,
                ["capsuleInputs"] = HandleCapsuleInputsAsync,
                ["casingInputs"] = HandleCasingInputsAsync,
                ["hopperInputs"] = HandleHopperInputsAsync,
                ["supportStructure"] = HandleSupportStructureAsync,
                ["accessGroup"] = HandleAccessGroupAsync,
                ["roofDoor"] = HandleRoofDoorAsync,
                ["paintingArea"] = HandlePaintingAreaAsync,
                ["billOfMaterial"] = HandleBillOfMaterialAsync,
                ["paintingCost"] = HandlePaintingCostAsync,
            };

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

            // 7)
            // AFTER you have createdInputIds, invoke handlers for each dto:
            var childTasks = new List<Task>();
            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];
                var createdInputId = createdInputIds.ElementAtOrDefault(i); // safe access
                var masterId = masterMap.TryGetValue(createdInputId, out var mid) ? mid : 0;
                foreach (var handlerKv in _childHandlers)
                {
                    // handler function itself checks dto.* == null and returns early if not present
                    // so we can call it safely; you can also check dto props before calling to save calls
                    childTasks.Add(handlerKv.Value(dto, masterId, ct));
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
            if (unmatchedPairIndices.Any())
            {
                // limiter to avoid flooding SkyCiv — adjust concurrency as needed
                var maxConcurrency = 2; // or 1 if you want strictly sequential
                using var sem = new SemaphoreSlim(maxConcurrency);
                var tasks = new List<Task>();

                foreach (var pairIndex in unmatchedPairIndices)
                {
                    await sem.WaitAsync(); // respect cancellation
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            // get model to send
                            var inputEntity = pairs[pairIndex].Input;
                            // assume the DTO or entity stored s3dModel as JObject (if not, adapt to convert string->JObject)
                            var s3dModel = inputEntity.S3dModel != null
                                ? JObject.Parse(inputEntity.S3dModel) // if stored as string
                                : null;

                            if (s3dModel == null)
                            {
                                _logger?.LogWarning("No S3D model present for pairIndex {idx}", pairIndex);
                                return;
                            }

                            // Call analysis service
                            AnalysisResponseDto analysisResponse;
                            try
                            {
                                analysisResponse = await _skyCivService.RunAnalysisAsync(s3dModel, ct);
                            }
                            catch (Exception ex)
                            {
                                _logger?.LogError(ex, "SkyCiv analysis failed for pairIndex {idx}", pairIndex);
                                return;
                            }

                            if (analysisResponse != null && analysisResponse.Status == "Succeeded")
                            {
                                // Persist model data and session id to the DB row that was created earlier
                                var createdId = createdInputIds[pairIndex]; // createdInputIds indexes map to pairs
                                var modelJson = analysisResponse.ModelData?.ToString(Formatting.None) ?? string.Empty;
                                var sessionId = analysisResponse.SessionId;

                                // Call repository to update the row (you need to implement this)
                                await _repository.UpdateS3dModelAsync(createdId, modelJson, sessionId);

                                // Optionally update the groupMatchMap or matchesList for return DTOs
                                var groupKey = pairGroupKeys[pairIndex];
                                if (groupMatchMap.TryGetValue(groupKey, out var gm2))
                                {
                                    // annotate placeholder to indicate analysis was run
                                    gm2.IsMatched = false; // still unmatched, but tried analysis

                                }
                            }
                            else
                            {
                                _logger?.LogWarning("Analysis didn't succeed for pairIndex {idx}. Status: {status}", pairIndex, analysisResponse?.Status);
                            }
                        }
                        finally
                        {
                            sem.Release();
                        }
                    }));
                }

                // Wait for all tasks to complete
                await Task.WhenAll(tasks);
            }

            return result;
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
                Specific_Gravity = dto.Specific_Gravity,
                Customer_Equipment_Tag_No = dto.Customer_Equipment_Tag_No,
                Bagfilter_Cleaning_Type = dto.Bagfilter_Cleaning_Type,
                Offline_Maintainence = dto.Offline_Maintainence,
                Cage_Wire_Dia = dto.Cage_Wire_Dia,
                No_Of_Cage_Wires = dto.No_Of_Cage_Wires,
                Ring_Spacing = dto.Ring_Spacing,
                Filter_Bag_Dia = dto.Filter_Bag_Dia,
                Fil_Bag_Length = dto.Fil_Bag_Length,
                Fil_Bag_Recommendation = dto.Fil_Bag_Recommendation,
                Gas_Entry = dto.Gas_Entry,
                Support_Structure_Type = dto.Support_Structure_Type,
                Can_Correction = dto.Can_Correction,
                Valve_Size = dto.Valve_Size,
                Voltage_Rating = dto.Voltage_Rating,
                Cage_Type = dto.Cage_Type,
                Cage_Length = dto.Cage_Length,
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
                Material_Handling = dto.Material_Handling,
                Material_Handling_Qty = dto.Material_Handling_Qty,
                Trough_Outlet_Length = dto.Trough_Outlet_Length,
                Trough_Outlet_Width = dto.Trough_Outlet_Width,
                Material_Handling_XXX = dto.Material_Handling_XXX,
                Support_Struct_Type = dto.Support_Struct_Type,
                No_Of_Column = dto.No_Of_Column,
                Ground_Clearance = dto.Ground_Clearance,
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
                Specific_Gravity = input.Specific_Gravity,
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
                Cage_Wire_Dia = input.Cage_Wire_Dia,
                No_Of_Cage_Wires = input.No_Of_Cage_Wires,
                Ring_Spacing = input.Ring_Spacing,

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
                Can_Correction = input.Can_Correction,
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
                Cage_Type = input.Cage_Type,
                Cage_Length = input.Cage_Length,
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


    }
}

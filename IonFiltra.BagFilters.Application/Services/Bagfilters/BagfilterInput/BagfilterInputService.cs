using System.Globalization;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Application.DTOs.SkyCiv;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterInputs;

using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterMasterEntity;
using IonFiltra.BagFilters.Core.Entities.EnquiryEntity;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.BagfilterInputs;
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

        public BagfilterInputService(
            IBagfilterInputRepository repository,
            ILogger<BagfilterInputService> logger,
            ISkyCivAnalysisService skyCivService)
        {
            _repository = repository;
            _logger = logger;
            _skyCivService = skyCivService;
        }

        public async Task<BagfilterInputMainDto> GetByProjectId(int id)
        {
            _logger.LogInformation("Fetching BagfilterInput for Id {id}", id);
            var entity = await _repository.GetByProjectId(id);
            return BagfilterInputMapper.ToMainDto(entity);
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


        //old method
        //public async Task<List<int>> AddRangeAsync(List<BagfilterInputMainDto> dtos)
        //{
        //    if (dtos == null || dtos.Count == 0) return new List<int>();

        //    _logger.LogInformation("Adding batch of BagfilterInputs. Count={Count}", dtos.Count);

        //    // Map DTOs to entity pairs (master + input)
        //    var pairs = new List<(BagfilterMaster Master, BagfilterInput Input)>(dtos.Count);

        //    foreach (var dto in dtos)
        //    {
        //        // validation: ensure master info present OR masterId provided
        //        if (dto.BagfilterMaster == null && dto.BagfilterMasterId <= 0)
        //        {
        //            // either throw or skip; here we throw for clarity
        //            throw new ArgumentException("Each item must include BagfilterMaster data or an existing BagfilterMasterId.");
        //        }

        //        // create master entity (if BagfilterMaster provided)
        //        var masterEntity = dto.BagfilterMaster != null
        //            ? new BagfilterMaster
        //            {
        //                BagFilterName = dto.BagfilterMaster.BagfilterMaster.BagFilterName,
        //                Status = dto.BagfilterMaster.BagfilterMaster.Status,
        //                Revision = dto.BagfilterMaster.BagfilterMaster.Revision,
        //                CreatedAt = DateTime.Now
        //            }
        //            : new BagfilterMaster
        //            {
        //                // if frontend provided only existing BagfilterMasterId, set that and don't create a new master.
        //                BagfilterMasterId = dto.BagfilterMasterId,
        //                CreatedAt = DateTime.Now
        //            };

        //        // map input DTO -> input entity (explicit map to avoid reflection overhead):
        //        var inputDto = dto.BagfilterInput ?? throw new ArgumentException("BagfilterInput is required.");
        //        var inputEntity = MapBagfilterInputDtoToEntity(inputDto);

        //        // keep CreatedAt to be set in repository too (optional)
        //        inputEntity.CreatedAt = DateTime.Now;

        //        // If masterEntity has Id prefilled (existing master), set foreign key now; otherwise repository will wire navigation
        //        if (masterEntity.BagfilterMasterId > 0)
        //        {
        //            inputEntity.BagfilterMasterId = masterEntity.BagfilterMasterId;
        //        }
        //        else
        //        {
        //            // set navigation so EF will wire FK for new masters
        //            inputEntity.BagfilterMaster = masterEntity;
        //        }

        //        pairs.Add((masterEntity, inputEntity));
        //    }

        //    // Call repository which will insert all masters + inputs within a single transaction and SaveChanges once
        //    var createdInputIds = await _repository.AddRangeAsync(pairs);

        //    _logger.LogInformation("Batch insert completed. Created {Count} inputs.", createdInputIds);
        //    return createdInputIds;
        //}

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

            // after createdInputIds = await _repository.AddRangeAsync(...)
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

    }
}
    
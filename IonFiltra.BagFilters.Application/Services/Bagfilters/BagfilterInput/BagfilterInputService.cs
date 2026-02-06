using IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.DamperSize;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.EV;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Application.DTOs.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Application.DTOs.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Application.DTOs.SkyCiv;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Core.Entities.BagfilterDatabase.WithCanopy;
using IonFiltra.BagFilters.Core.Entities.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterMasterEntity;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.DamperSize;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.EV;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Core.Entities.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Core.Entities.BOM.Cage_Cost;
using IonFiltra.BagFilters.Core.Entities.BOM.Damper_Cost;
using IonFiltra.BagFilters.Core.Entities.BOM.Painting_Cost;
using IonFiltra.BagFilters.Core.Entities.BOM.Transp_Cost;
using IonFiltra.BagFilters.Core.Entities.EnquiryEntity;
using IonFiltra.BagFilters.Core.Entities.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Core.Interfaces.Bagfilters.BagfilterMasters;
using IonFiltra.BagFilters.Core.Interfaces.GenericView;
using IonFiltra.BagFilters.Core.Interfaces.MasterData.Master_Definition;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BagfilterDatabase.WithCanopy;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.DamperSize;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.EV;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Cage_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Damper_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Painting_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Transp_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Core.Interfaces.SkyCiv;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Nodes;

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

        private readonly IBoughtOutItemSelectionRepository _boughtOutRepo;
        private readonly IMasterDefinitionsRepository _masterDefinitionsRepository;

        private readonly IGenericViewRepository _genericViewRepository;

        private readonly ITransportationCostEntityRepository _transportationCostRepository;

        private readonly IDamperCostEntityRepository _damperCostRepository;

        private readonly ICageCostEntityRepository _cageCostRepository;
        private readonly IDamperSizeInputsRepository _damperSizeInputsRepository;
        private readonly IExplosionVentEntityRepository _explosionVentEntityRepository;
        private readonly IIFI_Bagfilter_Database_Without_CanopyRepository _withoutCanopyRepo;
        private readonly IIFI_Bagfilter_Database_With_CanopyRepository _withCanopyRepo;

        private readonly ISecondaryBoughtOutItemRepository _secondaryBoughtOutRepo;

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
            IPaintingCostRepository paintingCostRepository,
            IBoughtOutItemSelectionRepository boughtOutRepo,
            IMasterDefinitionsRepository masterDefinitionsRepository,
            IGenericViewRepository genericViewRepository,
            ITransportationCostEntityRepository transportationCostEntityRepository,
            IDamperCostEntityRepository damperCostRepository,
            ICageCostEntityRepository cageCostEntityRepository,
            IDamperSizeInputsRepository damperSizeInputsRepository,
            IExplosionVentEntityRepository explosionVentEntityRepository,
            IIFI_Bagfilter_Database_Without_CanopyRepository withoutCanopyRepo,
            IIFI_Bagfilter_Database_With_CanopyRepository withCanopyRepo,
            ISecondaryBoughtOutItemRepository secondaryBoughtOutItemRepository
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
            _boughtOutRepo = boughtOutRepo;
            _masterDefinitionsRepository = masterDefinitionsRepository;
            _genericViewRepository = genericViewRepository;
            _transportationCostRepository = transportationCostEntityRepository;
            _damperCostRepository = damperCostRepository;
            _cageCostRepository = cageCostEntityRepository;
            _damperSizeInputsRepository = damperSizeInputsRepository;
            _explosionVentEntityRepository = explosionVentEntityRepository;
            _withoutCanopyRepo = withoutCanopyRepo;
            _withCanopyRepo = withCanopyRepo;
            _secondaryBoughtOutRepo = secondaryBoughtOutItemRepository;

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
            var pairs = new List<(BagfilterMaster Master, BagfilterInput Input, SupportStructureDto Support)>(dtos.Count);

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


                // SupportStructure is REQUIRED for matching
                var support = dto.SupportStructure
                    ?? throw new ArgumentException("SupportStructure is required for bagfilter matching.");

                pairs.Add((masterEntity, inputEntity, support));


                var effectiveColumns = ResolveNumberOfColumns(
                    inputEntity.Support_Struct_Type,
                    inputEntity.No_Of_Column,
                    support?.No_Of_Bays_In_X,
                    support?.No_Of_Bays_In_Z
                );

                // If columns cannot be resolved → force unmatched later
                // but still group deterministically
                var groupKey = BuildGroupKeyV2(
                    inputEntity.Process_Volume_M3h,
                    inputEntity.Hopper_Type,
                    inputEntity.Canopy,
                    effectiveColumns,
                    support?.No_Of_Bays_In_X,
                    support?.No_Of_Bays_In_Z
                );


                pairGroupKeys.Add(groupKey);

                if (!groups.TryGetValue(groupKey, out var list)) { list = new List<int>(); groups[groupKey] = list; }
                list.Add(idx);
            }

            

            // --- Build Group labels (Group 1, Group 2, ...) ---
            var groupKeysList = groups.Keys.ToList();

            var groupKeyObjectMap = new Dictionary<string, GroupKey>();

            foreach (var groupKeyString in groupKeysList)
            {
                var firstIdx = groups[groupKeyString].First();
                var repInput = pairs[firstIdx].Input;
                var repDto = dtos[firstIdx];
                var support = repDto.SupportStructure!;

                var effectiveColumns = ResolveNumberOfColumns(
                    repInput.Support_Struct_Type,
                    repInput.No_Of_Column,
                    support.No_Of_Bays_In_X,
                    support.No_Of_Bays_In_Z
                );

                groupKeyObjectMap[groupKeyString] = new GroupKey
                {
                    ProcessVolume = repInput.Process_Volume_M3h,
                    HopperType = repInput.Hopper_Type,
                    Canopy = repInput.Canopy,
                    EffectiveNoOfColumns = effectiveColumns,
                    BaysInX = support.No_Of_Bays_In_X,
                    BaysInZ = support.No_Of_Bays_In_Z
                };
            }

            var masterMatchesByGroupKey = await FindMasterMatchesAsync(groupKeyObjectMap);

            var groupIdByKey = new Dictionary<string, string>(groupKeysList.Count);
            for (int i = 0; i < groupKeysList.Count; i++)
            {
                groupIdByKey[groupKeysList[i]] = $"Group {i + 1}";
            }

            // Prepare container for results and Enquiry ids to fetch
            var groupMatchMap = new Dictionary<string, BagfilterMatchDto>(groups.Count);
            var matchedEnquiryIds = new HashSet<int>();


            foreach (var groupKey in groupKeysList)
            {
                var representativePairIndex = groups[groupKey].First();
                var repInput = pairs[representativePairIndex].Input;

                var placeholder = new BagfilterMatchDto
                {
                    GroupId = groupIdByKey[groupKey],
                    IsMatched = false
                };

                if (masterMatchesByGroupKey.TryGetValue(groupKey, out var masterId)
                    && masterId.HasValue)
                {
                    placeholder.IsMatched = true;
                    placeholder.BagfilterMasterId = masterId.Value;
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
                    matchMappingByPairIndex[pairIndex] = (0, matchDto.BagfilterMasterId);
                }
            }

            //remove support structure from pairs
            var dbPairs = pairs
            .Select(p => (p.Master, p.Input))
            .ToList();


            // 6) Insert all (masters + inputs) and apply match mapping inside repository in same transaction
            var createdInputIds = await _repository.AddRangeAsync(dbPairs, matchMappingByPairIndex);

            // Batch fetch all inputs (one DB call)
            var inputs = await _repository.GetByIdsAsync(createdInputIds);

            // Build a dictionary for quick lookup
            var masterMap = inputs.ToDictionary(x => x.BagfilterInputId, x => x.BagfilterMasterId);


           
            // Build an array mapping dto index -> BagfilterMasterId
            var masterIdByIndex = new int[dtos.Count];

            for (int i = 0; i < dtos.Count; i++)
            {
                var createdInputId = createdInputIds.ElementAtOrDefault(i);
                if (createdInputId > 0 && masterMap.TryGetValue(createdInputId, out var mid))
                    masterIdByIndex[i] = mid;
                else
                    masterIdByIndex[i] = 0;
            }

            // === Batched children (same pattern as UpdateRangeAsync) ===

            // WeightSummary
            await BatchUpsertChildAsync<WeightSummary>(
                dtos,
                masterIdByIndex,
                hasChildDto: dto => dto.WeightSummary != null,
                getExistingByMasterIds: (masterIds, token) => _weightSummaryRepository.GetByMasterIdsAsync(masterIds, token),
                mapEntity: (dto, masterId, existing) => MapWeightSummary(dto, masterId, existing),
                upsertRange: (entities, token) => _weightSummaryRepository.UpsertRangeAsync(entities, token),
                ct);

            // ProcessInfo
            await BatchUpsertChildAsync<ProcessInfo>(
                dtos,
                masterIdByIndex,
                hasChildDto: dto => dto.ProcessInfo != null,
                getExistingByMasterIds: (masterIds, token) => _processInfoRepository.GetByMasterIdsAsync(masterIds, token),
                mapEntity: (dto, masterId, existing) => MapProcessInfo(dto, masterId, existing),
                upsertRange: (entities, token) => _processInfoRepository.UpsertRangeAsync(entities, token),
                ct);

            // CageInputs
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

            await BatchUpsertChildAsync<DamperSizeInputs>(
            dtos,
            masterIdByIndex,
            hasChildDto: dto => dto.DamperSizeInputs != null,
            getExistingByMasterIds: (masterIds, token) =>
                _damperSizeInputsRepository.GetByMasterIdsAsync(masterIds, token),
            mapEntity: (dto, masterId, existing) =>
                MapDamperSizeInputs(dto, masterId, existing),
            upsertRange: (entities, token) =>
                _damperSizeInputsRepository.UpsertRangeAsync(entities, token),
            ct);

            await BatchUpsertChildAsync<ExplosionVentEntity>(
            dtos,
            masterIdByIndex,
            hasChildDto: dto => dto.ExplosionVent != null,
            getExistingByMasterIds: (masterIds, token) =>
                _explosionVentEntityRepository.GetByMasterIdsAsync(masterIds, token),
            mapEntity: (dto, masterId, existing) =>
                MapExplosionVentEntity(dto, masterId, existing),
            upsertRange: (entities, token) =>
                _explosionVentEntityRepository.UpsertRangeAsync(entities, token),
            ct);



            // === Batched BillOfMaterial

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


            // ===============================
            // === Batched BoughtOutItems ===
            // ===============================

            var boughtOutMasterIds = masterIdByIndex.Where(x => x > 0).Distinct().ToList();

            if (boughtOutMasterIds.Count > 0)
            {
                var existingBoughtOut =
                    await _boughtOutRepo.GetByMasterIdsAsync(boughtOutMasterIds, ct);

                var boughtOutEntities =
                    await MapBoughtOutItemsBatchAsync(
                        dtos,
                        masterIdByIndex,
                        existingBoughtOut,
                        ct);

                if (boughtOutEntities.Count > 0)
                {
                    await _boughtOutRepo.UpsertRangeAsync(boughtOutEntities);
                }
            }

            // ===============================
            // === Batched SecondaryBoughtOutItems ===
            // ===============================

            var secondaryEntities = new List<SecondaryBoughtOutItem>();

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];

                if (dto.SecondaryBoughtOutItems == null || dto.SecondaryBoughtOutItems.Count == 0)
                    continue;

                var masterId = masterIdByIndex[i];
                if (masterId <= 0)
                    continue;

                var enquiryId = dto.BagfilterInput.EnquiryId;

                foreach (var sec in dto.SecondaryBoughtOutItems)
                {
                    // Ignore empty rows (important)
                    if (sec.Make == null && sec.Cost == null)
                        continue;

                    secondaryEntities.Add(new SecondaryBoughtOutItem
                    {
                        EnquiryId = (int)enquiryId,
                        BagfilterMasterId = masterId,
                        MasterKey = sec.MasterKey,
                        Make = sec.Make,
                        Cost = sec.Cost,
                        Qty = sec.Qty,
                        Rate = sec.Rate,
                        Unit = sec.Unit
                    });
                }
            }
            if (secondaryEntities.Count > 0)
            {
                await _secondaryBoughtOutRepo.UpsertRangeAsync(secondaryEntities);
            }



            // === Batched TransportationCost ===

            var transportDataByMaster = new Dictionary<int, List<TransportationCostEntity>>();

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];

                if (dto.TransportationCost == null || dto.TransportationCost.Count == 0)
                    continue;

                var masterId = masterIdByIndex[i];
                if (masterId <= 0)
                    continue;

                var mappedRows = MapTransportationCostCollection(dto, masterId);
                if (mappedRows.Count == 0)
                    continue;

                if (!transportDataByMaster.TryGetValue(masterId, out var list))
                {
                    list = new List<TransportationCostEntity>();
                    transportDataByMaster[masterId] = list;
                }

                list.AddRange(mappedRows);
            }

            if (transportDataByMaster.Count > 0)
            {
                await _transportationCostRepository
                    .ReplaceForMastersAsync(transportDataByMaster, ct);
            }

            // === Batched DamperCost ===

            var damperDataByMaster = new Dictionary<int, List<DamperCostEntity>>();

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];

                if (dto.DamperCost == null || dto.DamperCost.Count == 0)
                    continue;

                var masterId = masterIdByIndex[i];
                if (masterId <= 0)
                    continue;

                var mappedRows = MapDamperCostCollection(dto, masterId);
                if (mappedRows.Count == 0)
                    continue;

                if (!damperDataByMaster.TryGetValue(masterId, out var list))
                {
                    list = new List<DamperCostEntity>();
                    damperDataByMaster[masterId] = list;
                }

                list.AddRange(mappedRows);
            }

            if (damperDataByMaster.Count > 0)
            {
                await _damperCostRepository
                    .ReplaceForMastersAsync(damperDataByMaster, ct);
            }


            // === Batched DamperCost ===

            var cageDataByMaster = new Dictionary<int, List<CageCostEntity>>();

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];

                if (dto.CageCost == null || dto.CageCost.Count == 0)
                    continue;

                var masterId = masterIdByIndex[i];
                if (masterId <= 0)
                    continue;

                var mappedRows = MapCageCostCollection(dto, masterId);
                if (mappedRows.Count == 0)
                    continue;

                if (!cageDataByMaster.TryGetValue(masterId, out var list))
                {
                    list = new List<CageCostEntity>();
                    cageDataByMaster[masterId] = list;
                }

                list.AddRange(mappedRows);
            }

            if (cageDataByMaster.Count > 0)
            {
                await _cageCostRepository
                    .ReplaceForMastersAsync(cageDataByMaster, ct);
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

            var optimizationBag = new ConcurrentBag<SkyCivOptimizationDto>();
            //call for sky civ for unmatched items only
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
                    tasks.Add(ProcessPairAsync(pairIndex, pairs, pairGroupKeys, createdInputIds, groupMatchMap, optimizationBag, sem, ct));
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


            result.Optimizations = optimizationBag.Any()
                   ? optimizationBag.ToList()
                   : new List<SkyCivOptimizationDto>();

            return result;
        }




        public async Task<UpdateRangeResultDto> UpdateRangeAsync(List<BagfilterInputMainDto> dtos, CancellationToken ct)
        {
            if (dtos == null || dtos.Count == 0) return new UpdateRangeResultDto();

            // Step A: Map DTOs -> desired pair state, build grouping keys & groups (same as AddRange)
            var pairs = new List<(BagfilterMaster Master, BagfilterInput Input, SupportStructureDto support)>(dtos.Count);
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

                // SupportStructure is REQUIRED for matching
                var support = dto.SupportStructure
                    ?? throw new ArgumentException("SupportStructure is required for bagfilter matching.");

                pairs.Add((masterEntity, inputEntity, support));

              

                var effectiveColumns = ResolveNumberOfColumns(
                    inputEntity.Support_Struct_Type,
                    inputEntity.No_Of_Column,
                    support?.No_Of_Bays_In_X,
                    support?.No_Of_Bays_In_Z
                );

                // If columns cannot be resolved → force unmatched later
                // but still group deterministically
                var groupKey = BuildGroupKeyV2(
                    inputEntity.Process_Volume_M3h,
                    inputEntity.Hopper_Type,
                    inputEntity.Canopy,
                    effectiveColumns,
                    support?.No_Of_Bays_In_X,
                    support?.No_Of_Bays_In_Z
                );

                pairGroupKeys.Add(groupKey);

                if (!groups.TryGetValue(groupKey, out var list)) { list = new List<int>(); groups[groupKey] = list; }
                list.Add(idx);
            }

            

            // Step C: For each group, create placeholder and decide match/update vs insert
            var groupKeysList = groups.Keys.ToList();

            // === Build GroupKey → GroupKey object map (IDENTICAL to AddRange) ===
            var groupKeyObjectMap = new Dictionary<string, GroupKey>();

            foreach (var groupKeyString in groups.Keys)
            {
                var firstIdx = groups[groupKeyString].First();
                var repInput = pairs[firstIdx].Input;
                var repDto = dtos[firstIdx];
                var support = repDto.SupportStructure!;

                var effectiveColumns = ResolveNumberOfColumns(
                    repInput.Support_Struct_Type,
                    repInput.No_Of_Column,
                    support.No_Of_Bays_In_X,
                    support.No_Of_Bays_In_Z
                );

                groupKeyObjectMap[groupKeyString] = new GroupKey
                {
                    ProcessVolume = repInput.Process_Volume_M3h,
                    HopperType = repInput.Hopper_Type,
                    Canopy = repInput.Canopy,
                    EffectiveNoOfColumns = effectiveColumns,
                    BaysInX = support.No_Of_Bays_In_X,
                    BaysInZ = support.No_Of_Bays_In_Z
                };
            }

            // === MASTER MATCHING (single source of truth) ===
            var masterMatchesByGroupKey =
                await FindMasterMatchesAsync(groupKeyObjectMap);

            var groupIdByKey = new Dictionary<string, string>(groupKeysList.Count);
            for (int i = 0; i < groupKeysList.Count; i++) groupIdByKey[groupKeysList[i]] = $"Group {i + 1}";

            var matchedEnquiryIds = new HashSet<int>();
            // === Build groupMatchMap (NO input-level ids) ===
            var groupMatchMap = new Dictionary<string, BagfilterMatchDto>();

            foreach (var groupKey in groups.Keys)
            {
                var placeholder = new BagfilterMatchDto
                {
                    GroupId = groupKey,
                    IsMatched = false
                };

                if (masterMatchesByGroupKey.TryGetValue(groupKey, out var masterId)
                    && masterId.HasValue)
                {
                    placeholder.IsMatched = true;
                    placeholder.BagfilterMasterId = masterId.Value;
                }

                groupMatchMap[groupKey] = placeholder;
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

                // 2) NEW ROWS (no BagfilterInputId)
                // If group matched → insert new input under matched master
                if (matchDto != null && matchDto.IsMatched)
                {
                    pair.Input.BagfilterMasterId = matchDto.BagfilterMasterId;
                    pair.Input.BagfilterMaster = null;

                    inserts.Add((pair.Master, pair.Input, pairIndex));
                }
                else
                {
                    // fully unmatched → new master + input
                    inserts.Add((pair.Master, pair.Input, pairIndex));
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

            await BatchUpsertChildAsync<DamperSizeInputs>(
           dtos,
           masterIdByIndex,
           hasChildDto: dto => dto.DamperSizeInputs != null,
           getExistingByMasterIds: (masterIds, token) =>
               _damperSizeInputsRepository.GetByMasterIdsAsync(masterIds, token),
           mapEntity: (dto, masterId, existing) =>
               MapDamperSizeInputs(dto, masterId, existing),
           upsertRange: (entities, token) =>
               _damperSizeInputsRepository.UpsertRangeAsync(entities, token),
           ct);

            await BatchUpsertChildAsync<ExplosionVentEntity>(
            dtos,
            masterIdByIndex,
            hasChildDto: dto => dto.ExplosionVent != null,
            getExistingByMasterIds: (masterIds, token) =>
                _explosionVentEntityRepository.GetByMasterIdsAsync(masterIds, token),
            mapEntity: (dto, masterId, existing) =>
                MapExplosionVentEntity(dto, masterId, existing),
            upsertRange: (entities, token) =>
                _explosionVentEntityRepository.UpsertRangeAsync(entities, token),
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


            // ===============================
            // === Batched BoughtOutItems ===
            // ===============================

            var boughtOutMasterIds = masterIdByIndex.Where(x => x > 0).Distinct().ToList();

            if (boughtOutMasterIds.Count > 0)
            {
                var existingBoughtOut =
                    await _boughtOutRepo.GetByMasterIdsAsync(boughtOutMasterIds, ct);

                var boughtOutEntities =
                    await MapBoughtOutItemsBatchAsync(
                        dtos,
                        masterIdByIndex,
                        existingBoughtOut,
                        ct);

                if (boughtOutEntities.Count > 0)
                {
                    await _boughtOutRepo.UpsertRangeAsync(boughtOutEntities);
                }
            }

            // ===============================
            // === Batched SecondaryBoughtOutItems ===
            // ===============================

            var secondaryEntities = new List<SecondaryBoughtOutItem>();

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];

                if (dto.SecondaryBoughtOutItems == null || dto.SecondaryBoughtOutItems.Count == 0)
                    continue;

                var masterId = masterIdByIndex[i];
                if (masterId <= 0)
                    continue;

                var enquiryId = dto.BagfilterInput.EnquiryId;

                foreach (var sec in dto.SecondaryBoughtOutItems)
                {
                    // Skip empty rows
                    if (sec.Make == null && sec.Cost == null)
                        continue;

                    secondaryEntities.Add(new SecondaryBoughtOutItem
                    {
                        EnquiryId = (int)enquiryId,
                        BagfilterMasterId = masterId,
                        MasterKey = sec.MasterKey,
                        Make = sec.Make,
                        Cost = sec.Cost,
                        Qty = sec.Qty,
                        Rate = sec.Rate,
                        Unit = sec.Unit
                    });
                }
            }

            if (secondaryEntities.Count > 0)
            {
                await _secondaryBoughtOutRepo.UpsertRangeAsync(secondaryEntities);
            }



            // === Batched TransportationCost ===

            var transportDataByMaster = new Dictionary<int, List<TransportationCostEntity>>();

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];

                if (dto.TransportationCost == null || dto.TransportationCost.Count == 0)
                    continue;

                var masterId = masterIdByIndex[i];
                if (masterId <= 0)
                    continue;

                var mappedRows = MapTransportationCostCollection(dto, masterId);
                if (mappedRows.Count == 0)
                    continue;

                if (!transportDataByMaster.TryGetValue(masterId, out var list))
                {
                    list = new List<TransportationCostEntity>();
                    transportDataByMaster[masterId] = list;
                }

                list.AddRange(mappedRows);
            }

            if (transportDataByMaster.Count > 0)
            {
                await _transportationCostRepository
                    .ReplaceForMastersAsync(transportDataByMaster, ct);
            }

            // === Batched DamperCost ===

            var damperDataByMaster = new Dictionary<int, List<DamperCostEntity>>();

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];

                if (dto.DamperCost == null || dto.DamperCost.Count == 0)
                    continue;

                var masterId = masterIdByIndex[i];
                if (masterId <= 0)
                    continue;

                var mappedRows = MapDamperCostCollection(dto, masterId);
                if (mappedRows.Count == 0)
                    continue;

                if (!damperDataByMaster.TryGetValue(masterId, out var list))
                {
                    list = new List<DamperCostEntity>();
                    damperDataByMaster[masterId] = list;
                }

                list.AddRange(mappedRows);
            }

            if (damperDataByMaster.Count > 0)
            {
                await _damperCostRepository
                    .ReplaceForMastersAsync(damperDataByMaster, ct);
            }


            // === Batched DamperCost ===

            var cageDataByMaster = new Dictionary<int, List<CageCostEntity>>();

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];

                if (dto.CageCost == null || dto.CageCost.Count == 0)
                    continue;

                var masterId = masterIdByIndex[i];
                if (masterId <= 0)
                    continue;

                var mappedRows = MapCageCostCollection(dto, masterId);
                if (mappedRows.Count == 0)
                    continue;

                if (!cageDataByMaster.TryGetValue(masterId, out var list))
                {
                    list = new List<CageCostEntity>();
                    cageDataByMaster[masterId] = list;
                }

                list.AddRange(mappedRows);
            }

            if (cageDataByMaster.Count > 0)
            {
                await _cageCostRepository
                    .ReplaceForMastersAsync(cageDataByMaster, ct);
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
            var existingIds = updates.Values.Select(u => u.ExistingInputId).Distinct().ToList();

            var beforeUpdateInputs = await _repository.GetByIdsAsync(existingIds);

            var candidateModelMap = beforeUpdateInputs
                .ToDictionary(x => x.BagfilterInputId, x => x.S3dModel ?? string.Empty);


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

            var optimizationBag = new ConcurrentBag<SkyCivOptimizationDto>();
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

                    tasks.Add(ProcessPairAsync(pairIndex, pairs, pairGroupKeys, allIdsToFetch.ToList(), groupMatchMap, optimizationBag, sem, ct));
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

            result.Optimizations = optimizationBag.Any()
            ? optimizationBag.ToList()
            : new List<SkyCivOptimizationDto>();

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

        private (string? col, string? beam, string? rav, string? bracing)
        ExtractOptimizedSections(JObject skycivResult)
        {
            var arr = skycivResult["response"]?["data"] as JArray;
            if (arr == null || arr.Count < 4) return default;

            return (
                arr[0]?["sections"]?.ToString(),
                arr[1]?["sections"]?.ToString(),
                arr[2]?["sections"]?.ToString(),
                arr[3]?["sections"]?.ToString()
            );
        }


        private async Task ProcessPairAsync(
                int pairIndex,
                List<(BagfilterMaster Master, BagfilterInput Input, SupportStructureDto Support)> pairs,
                List<string> pairGroupKeys,
                IList<int> createdInputIds,
                Dictionary<string, BagfilterMatchDto> groupMatchMap,
                ConcurrentBag<SkyCivOptimizationDto> optimizations,
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
                var support = pairs[pairIndex].Support;
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
                if (groupKey == null || !groupMatchMap.TryGetValue(groupKey, out var gm))
                {
                    _logger?.LogWarning("GroupKey missing for pairIndex {pairIndex}", pairIndex);
                    return;
                }

                var (col, beam, rav, bracing) =
                        ExtractOptimizedSections(analysisResponse.RawResponse);


                //new code to reapply optimized sections and get the structure weight.

                var optimizedMap = new Dictionary<int, string>
                {
                    { 1, col },
                    { 2, beam },
                    { 3, rav },
                    { 4, bracing }
                };

                var sections = (JObject)s3dModel["sections"];

                foreach (var (id, newProfile) in optimizedMap)
                {
                    if (string.IsNullOrWhiteSpace(newProfile))
                        throw new InvalidOperationException($"Optimized profile missing for section {id}");

                    var sectionId = id.ToString();

                    if (!sections.TryGetValue(sectionId, out var sectionToken))
                        throw new InvalidOperationException($"Missing section {sectionId} in S3D model");

                    var loadSection = (JArray)sectionToken["load_section"];

                    var oldProfile = loadSection[loadSection.Count - 1]?.ToString();

                    if (!string.Equals(oldProfile, newProfile, StringComparison.OrdinalIgnoreCase))
                    {
                        _logger?.LogInformation(
                            "Section {id} changed from {old} to {new}",
                            id, oldProfile, newProfile
                        );

                        loadSection[loadSection.Count - 1] = newProfile;
                    }
                }

                double totalStructureWeight = 0;
                // calling the bom calculate with the optimized model 
                try
                {
                    var bomResp = await _skyCivService.RunBOMAsync(s3dModel, ct).ConfigureAwait(false);

                    bool isCanopy = string.Equals(inputEntity.Canopy, "Yes", StringComparison.OrdinalIgnoreCase);

                    //fetch the data from response

                    var bomDataToken = bomResp.ModelData?["data"] as JObject;

                    if (bomDataToken == null)
                        throw new InvalidOperationException("Invalid BOM response shape");

                    var memberSections = new List<JObject>();

                    foreach (var prop in bomDataToken.Properties())
                    {
                        // Skip plate row (string key)
                        if (!int.TryParse(prop.Name, out _))
                            continue;

                        var row = (JObject)prop.Value;

                        memberSections.Add(row);
                    }

                   //summation of total_weight of all sections.

                    foreach (var row in memberSections)
                    {
                        var weightToken = row["total_weight"];

                        if (weightToken == null)
                            continue;

                        if (double.TryParse(weightToken.ToString(), out var weight))
                        {
                            totalStructureWeight += weight;
                        }
                    }

                    var effectiveColumns = ResolveNumberOfColumns(
                        inputEntity.Support_Struct_Type,
                        inputEntity.No_Of_Column,
                        support?.No_Of_Bays_In_X,
                        support?.No_Of_Bays_In_Z
                    );

                    if (!effectiveColumns.HasValue)
                    {
                        _logger?.LogWarning(
                            "Unable to resolve column count for pairIndex {pairIndex} (SupportType={type})",
                            pairIndex,
                            inputEntity.Support_Struct_Type
                        );
                    }


                    //craeting payload based in isCanopy
                    int structureRowId;

                    if (isCanopy)
                    {
                        var payload = new IFI_Bagfilter_Database_With_Canopy
                        {
                            Process_Volume_m3hr = inputEntity.Process_Volume_M3h?.ToString(),
                            Hopper_type = inputEntity.Hopper_Type,

                            Number_of_columns = effectiveColumns,
                            Number_of_bays_in_X_direction = support.No_Of_Bays_In_X,
                            Number_of_bays_in_Y_direction = support.No_Of_Bays_In_Z,

                            Member_Sizes_Column = col,
                            Member_Sizes_Beam = beam,
                            Member_Sizes_RAV = rav,
                            Member_Sizes_Bracing_Ties = bracing,

                            Total_Weight_of_Structure_kg = (decimal?)totalStructureWeight,
                            CreatedAt = DateTime.UtcNow
                        };

                        structureRowId = await _withCanopyRepo.AddAsync(payload);
                    }
                    else
                    {
                        var payload = new IFI_Bagfilter_Database_Without_Canopy
                        {
                            Process_Volume_m3hr = inputEntity.Process_Volume_M3h?.ToString(),
                            Hopper_type = inputEntity.Hopper_Type,

                            Number_of_columns = effectiveColumns ?? 0,
                            Number_of_bays_in_X_direction = support.No_Of_Bays_In_X ?? 0,
                            Number_of_bays_in_Y_direction = support.No_Of_Bays_In_Z ?? 0,

                            Member_Sizes_Column = col,
                            Member_Sizes_Beam = beam,
                            Member_Sizes_RAV = rav,
                            Member_Sizes_Bracing_and_Ties = bracing,

                            Total_Weight_of_Structure_kg = (decimal)totalStructureWeight,
                            CreatedAt = DateTime.UtcNow
                        };

                        structureRowId = await _withoutCanopyRepo.AddAsync(payload);
                    }

                }
                catch (Exception) { throw; }





                if (col == null || beam == null || rav == null || bracing == null)
                {
                    _logger?.LogWarning("Incomplete optimization data for pairIndex {pairIndex}", pairIndex);
                    return;
                }

                gm.AnalysisAttempted = true;

                if (optimizations.Any(o => o.GroupId == gm.GroupId))
                    return;

                optimizations.Add(new SkyCivOptimizationDto
                {
                    PairIndex = pairIndex,
                    GroupId = groupMatchMap[groupKey].GroupId,
                    BagfilterInputId = createdId,

                    OriginalColumnSection = inputEntity.Column_Section,
                    OriginalBeamTieSection = inputEntity.Beam_Tie_Section,
                    OriginalRavSection = inputEntity.Rav_Section,
                    OriginalBracingSection = inputEntity.Bracing_Section,

                    OptimizedColumnSection = col,
                    OptimizedBeamTieSection = beam,
                    OptimizedRavSection = rav,
                    OptimizedBracingSection = bracing
                });

               

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
                Design_Pressure_Mmwc = dto.Design_Pressure_Mmwc,
                Mfg_Plant = string.IsNullOrWhiteSpace(dto.Mfg_Plant) ? null : dto.Mfg_Plant.Trim(),
                Destination_State = string.IsNullOrWhiteSpace(dto.Destination_State) ? null : dto.Destination_State.Trim(),
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
                Cage_Material = dto.Cage_Material,
                Cage_Wire_Dia = dto.Cage_Wire_Dia,
                No_Of_Cage_Wires = dto.No_Of_Cage_Wires,
                Ring_Spacing = dto.Ring_Spacing,
                Cage_Diameter = dto.Cage_Diameter,
                Cage_Length = dto.Cage_Length,
                Spare_Cages = dto.Spare_Cages,
                Cage_Configuration = dto.Cage_Configuration,
                Filter_Bag_Dia = dto.Filter_Bag_Dia,
                Fil_Bag_Length = dto.Fil_Bag_Length,
                Fil_Bag_Recommendation = dto.Fil_Bag_Recommendation,
                Gas_Entry = dto.Gas_Entry,
                Support_Structure_Type = dto.Support_Structure_Type,
                
                Valve_Size = dto.Valve_Size,
                Voltage_Rating = dto.Voltage_Rating,
                Capsule_Height = dto.Capsule_Height,
                Total_Capsule_Height = dto.Total_Capsule_Height,
                Tube_Sheet_Thickness = dto.Tube_Sheet_Thickness,
                Capsule_Wall_Thickness = dto.Capsule_Wall_Thickness,
                Canopy = dto.Canopy,
                Solenoid_Valve_Maintainence = dto.Solenoid_Valve_Maintainence,
                Casing_Wall_Thickness = dto.Casing_Wall_Thickness,
                Stiffening_Factor_Casing = dto.Stiffening_Factor_Casing,
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
                Stiffening_Factor_Hopper = dto.Stiffening_Factor_Hopper,
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
                //Maintainence_Pltform_Weight = dto.Maintainence_Pltform_Weight,
                Blow_Pipe = dto.Blow_Pipe,
                Pressure_Header = dto.Pressure_Header,
                Distance_Piece = dto.Distance_Piece,
                Access_Stool_Size_Mm = dto.Access_Stool_Size_Mm,
                Access_Stool_Size_Kg = dto.Access_Stool_Size_Kg,
                Roof_Door_Thickness = dto.Roof_Door_Thickness,
                Stiffening_Factor_Roof_Door = dto.Stiffening_Factor_Roof_Door,
                Column_Height = dto.Column_Height,

                // computed values (you already had)
                Bag_Per_Row = dto.Bag_Per_Row,
                Number_Of_Rows = dto.Number_Of_Rows,
                Is_Damper_Required = dto.Is_Damper_Required,
                Damper_Series = dto.Damper_Series,
                Damper_Diameter = dto.Damper_Diameter,
                Damper_Qty = dto.Damper_Qty,
                Column_Section = dto.Column_Section,
                Beam_Tie_Section = dto.Beam_Tie_Section,
                Rav_Section = dto.Rav_Section,
                Bracing_Section = dto.Bracing_Section,

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

        private static string BuildGroupKeyV2(
    decimal? processVolume,
    string? hopperType,
    string? canopy,
    decimal? effectiveColumns,
    decimal? baysInX,
    decimal? baysInZ)
        {
            string NormDecimal(decimal? d) =>
                d.HasValue
                    ? d.Value.ToString(CultureInfo.InvariantCulture)
                    : "__NULL__";

            string NormString(string? s) =>
                string.IsNullOrWhiteSpace(s)
                    ? "__NULL__"
                    : s.Trim().ToLowerInvariant();

            return string.Join("|",
                NormDecimal(processVolume),
                NormString(hopperType),
                NormString(canopy),
                NormDecimal(effectiveColumns),
                NormDecimal(baysInX),
                NormDecimal(baysInZ)
            );
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
                Scrap_Holes_Weight = src.Scrap_Holes_Weight,
                Total_Weight_Of_Pressure_Header = src.Total_Weight_Of_Pressure_Header,
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
                Design_Pressure_Mmwc = src.Design_Pressure_Mmwc,
                Mfg_Plant = src.Mfg_Plant,
                Destination_State = src.Destination_State,
                Location = src.Location,
                ProcessVolumeMin = src.ProcessVolumeMin,
                //Process_Acrmax = src.Process_Acrmax,
                //ClothArea = src.ClothArea,
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
                Cage_Material = src.Cage_Material,
                Cage_Wire_Dia = src.Cage_Wire_Dia,
                No_Of_Cage_Wires = src.No_Of_Cage_Wires,
                Ring_Spacing = src.Ring_Spacing,
                Cage_Diameter = src.Cage_Diameter,
                Cage_Length = src.Cage_Length,
                Spare_Cages = src.Spare_Cages,
                Cage_Configuration = src.Cage_Configuration,
                No_Of_Rings = src.No_Of_Rings,
                Tot_Wire_Length = src.Tot_Wire_Length,
                Weight_Of_Cage_Wires = src.Weight_Of_Cage_Wires,
                Weight_Of_Cage_Rings = src.Weight_Of_Cage_Rings,
                Weight_Of_One_Cage = src.Weight_Of_One_Cage,
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
                //noOfBags = src.noOfBags,
                Fil_Bag_Recommendation = src.Fil_Bag_Recommendation,
                Bag_Per_Row = src.Bag_Per_Row,
                Number_Of_Rows = src.Number_Of_Rows,
                Actual_Bag_Req = src.Actual_Bag_Req,
                Wire_Cross_Sec_Area = src.Wire_Cross_Sec_Area,
                //No_Of_Rings = src.No_Of_Rings,
                //Tot_Wire_Length = src.Tot_Wire_Length,
                //Cage_Weight = src.Cage_Weight,

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
                Total_Capsule_Height = src.Total_Capsule_Height,
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
                Stiffening_Factor_Casing = src.Stiffening_Factor_Casing,
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
                Stiffening_Factor_Hopper = src.Stiffening_Factor_Hopper,
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
                Total_Weight_Of_Cage_Ladder = src.Total_Weight_Of_Cage_Ladder,
                Mid_Landing_Pltform = src.Mid_Landing_Pltform,
                Platform_Weight = src.Platform_Weight,
                Staircase_Height = src.Staircase_Height,
                Staircase_Weight = src.Staircase_Weight,
                Railing_Weight = src.Railing_Weight,
                Total_Weight_Of_Railing = src.Total_Weight_Of_Railing,
                Maintainence_Pltform = src.Maintainence_Pltform,
                //Maintainence_Pltform_Weight = src.Maintainence_Pltform_Weight,
                Total_Weight_Of_Maintainence_Pltform = src.Total_Weight_Of_Maintainence_Pltform,
                BlowPipe = src.BlowPipe,
                Total_Weight_Of_Blow_Pipe = src.Total_Weight_Of_Blow_Pipe,
                Total_Weight_Of_Pressure_Header = src.Total_Weight_Of_Pressure_Header,
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
                Stiffening_Factor_Roof_Door = src.Stiffening_Factor_Roof_Door,
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



        private async Task<List<BoughtOutItemSelection>>
            MapBoughtOutItemsBatchAsync(
            List<BagfilterInputMainDto> dtos,
            int[] masterIdByIndex,
            Dictionary<(int, int), BoughtOutItemSelection> existingMap,
            CancellationToken ct)
        {
            var masterDefs = await _masterDefinitionsRepository.GetAllActiveAsync();
            var masterDefByKey = masterDefs
                .Where(x => !string.IsNullOrWhiteSpace(x.MasterKey))
                .ToDictionary(x => x.MasterKey, x => x, StringComparer.OrdinalIgnoreCase);

            var result = new List<BoughtOutItemSelection>();


            // We’ll precompute default SelectedRowId per masterDef (once)
            var defaultRowIdByMasterDefId = new Dictionary<int, int?>();

            // Only bother if there exists at least one dto with empty BoughtOutItems
            //var anyNeedsDefaults = dtos.Any(d =>
            //    d.BoughtOutItems == null || d.BoughtOutItems.Count == 0);

            var anyNeedsDefaults = dtos.Any(d =>
                    (d.BagfilterInputId == 0 || d.BagfilterInputId == null) &&
                    (d.BoughtOutItems == null || d.BoughtOutItems.Count == 0));

            if (anyNeedsDefaults)
            {
                foreach (var def in masterDefs)
                {
                    var defaultId = await GetDefaultRowIdForMasterAsync(def.ApiRoute, ct);
                    defaultRowIdByMasterDefId[def.Id] = defaultId;
                }
            }

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];
                var masterId = masterIdByIndex[i];
                if (masterId <= 0) continue;

                var enquiryId = dto.BagfilterInput?.EnquiryId;
                if (enquiryId == null || enquiryId <= 0) continue;

                var explicitItems = dto.BoughtOutItems;

                // Build effective list: explicit selections if any, otherwise defaults
                List<BoughtOutItemSelectionDto> effectiveItems;

                if (explicitItems != null && explicitItems.Count > 0)
                {
                    effectiveItems = explicitItems;
                }
                else
                {
                    // No explicit selections -> build defaults for this BF
                    effectiveItems = new List<BoughtOutItemSelectionDto>();

                    foreach (var def in masterDefs)
                    {
                        if (!defaultRowIdByMasterDefId.TryGetValue(def.Id, out var defaultId) ||
                            defaultId == null)
                            continue; // no default row for this master

                        if (string.IsNullOrWhiteSpace(def.MasterKey))
                            continue;

                        //effectiveItems.Add(new BoughtOutItemSelectionDto
                        //{
                        //    MasterKey = def.MasterKey,
                        //    SelectedRowId = defaultId
                        //});

                        effectiveItems.Add(new BoughtOutItemSelectionDto
                        {
                            MasterKey = def.MasterKey,
                            SelectedRowId = defaultId,
                            Qty = 1,
                            Unit = "No's",
                            Rate = null,   // let frontend derive once
                            Cost = null
                        });
                    }

                    // still nothing? then skip this BF
                    if (effectiveItems.Count == 0)
                        continue;
                }

                foreach (var item in effectiveItems)
                {
                    if (string.IsNullOrWhiteSpace(item.MasterKey) || item.SelectedRowId == null)
                        continue;

                    if (!masterDefByKey.TryGetValue(item.MasterKey, out var def))
                    {
                        _logger.LogWarning("Invalid masterKey {Key}", item.MasterKey);
                        continue;
                    }

                    var key = (masterId, def.Id);
                    existingMap.TryGetValue(key, out var existing);

                    var entity = new BoughtOutItemSelection
                    {
                        Id = existing?.Id ?? 0,
                        EnquiryId = enquiryId.Value,
                        BagfilterMasterId = masterId,
                        MasterDefinitionId = def.Id,
                        MasterKey = item.MasterKey,
                        SelectedRowId = item.SelectedRowId,

                        //new 
                        Qty = item.Qty ?? existing?.Qty,
                        Unit = item.Unit ?? existing?.Unit,
                        Weight = item.Weight ?? existing?.Weight,
                        Rate = item.Rate ?? existing?.Rate,
                        Cost = item.Cost ?? existing?.Cost,


                        CreatedAt = existing?.CreatedAt ?? DateTime.UtcNow,
                        UpdatedAt = existing != null ? DateTime.UtcNow : null
                    };

                    result.Add(entity);
                }
            }

            return result;
        }

        private List<TransportationCostEntity> MapTransportationCostCollection(
    BagfilterInputMainDto dto,
    int masterId)
        {
            var list = new List<TransportationCostEntity>();

            if (dto.TransportationCost == null)
                return list;

            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                0;

            foreach (var row in dto.TransportationCost)
            {
                if (string.IsNullOrWhiteSpace(row.Parameter))
                    continue;

                list.Add(new TransportationCostEntity
                {
                    EnquiryId = enquiryId,
                    BagfilterMasterId = masterId,
                    Parameter = row.Parameter.Trim(),
                    Value = row.Value,
                    Unit = row.Unit,
                    CreatedAt = DateTime.UtcNow
                });
            }

            return list;
        }

        private List<DamperCostEntity> MapDamperCostCollection(
 BagfilterInputMainDto dto,
 int masterId)
        {
            var list = new List<DamperCostEntity>();

            if (dto.DamperCost == null)
                return list;

            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                0;

            foreach (var row in dto.DamperCost)
            {
                if (string.IsNullOrWhiteSpace(row.Parameter))
                    continue;

                list.Add(new DamperCostEntity
                {
                    EnquiryId = enquiryId,
                    BagfilterMasterId = masterId,
                    Parameter = row.Parameter.Trim(),
                    Value = row.Value,
                    Unit = row.Unit,
                    CreatedAt = DateTime.UtcNow
                });
            }

            return list;
        }


        private List<CageCostEntity> MapCageCostCollection(
        BagfilterInputMainDto dto,
        int masterId)
        {
            var list = new List<CageCostEntity>();

            if (dto.CageCost == null)
                return list;

            var enquiryId =
                (int?)dto.BagfilterInput?.EnquiryId ??
                dto.BagfilterMaster?.EnquiryId ??
                0;

            foreach (var row in dto.CageCost)
            {
                if (string.IsNullOrWhiteSpace(row.Parameter))
                    continue;

                list.Add(new CageCostEntity
                {
                    EnquiryId = enquiryId,
                    BagfilterMasterId = masterId,
                    Parameter = row.Parameter.Trim(),
                    Value = row.Value,
                    Unit = row.Unit,
                    CreatedAt = DateTime.UtcNow
                });
            }

            return list;
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


        private async Task<int?> GetDefaultRowIdForMasterAsync(string? apiRoute, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(apiRoute))
                return null;

            // Uses your generic view to read master table rows
            var rows = await _genericViewRepository.GetViewData(apiRoute);

            if (rows == null || rows.Count == 0)
                return null;

            var first = rows[0];

            // your GenericViewRepository lowercases column names: columnName = reader.GetName(i).ToLower()
            if (!first.TryGetValue("id", out var raw) || raw == null)
                return null;

            try
            {
                return Convert.ToInt32(raw);
            }
            catch
            {
                return null;
            }
        }

        private DamperSizeInputs MapDamperSizeInputs(
    BagfilterInputMainDto dto,
    int bagfilterMasterId,
    DamperSizeInputs existing = null)
        {
            if (dto == null || dto.DamperSizeInputs == null)
                return null;

            var src = dto.DamperSizeInputs;

            // Resolve EnquiryId (same precedence as BagSelection)
            var enquiryId =
                (int?)dto.BagfilterInput.EnquiryId ??
                existing?.EnquiryId ??
                0;

            var entity = new DamperSizeInputs
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                Is_Damper_Required = src.Is_Damper_Required,
                Damper_Series = src.Damper_Series,
                Damper_Diameter = src.Damper_Diameter,
                Damper_Qty = src.Damper_Qty,

                // CreatedAt / UpdatedAt handled in repo
            };

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.CreatedAt = existing.CreatedAt;
            }

            return entity;
        }


        private ExplosionVentEntity MapExplosionVentEntity(
    BagfilterInputMainDto dto,
    int bagfilterMasterId,
    ExplosionVentEntity existing = null)
        {
            if (dto == null || dto.ExplosionVent == null)
                return null;

            var src = dto.ExplosionVent;

            // Resolve EnquiryId (same pattern)
            var enquiryId =
                (int?)dto.BagfilterInput.EnquiryId ??
                existing?.EnquiryId ??
                0;

            var entity = new ExplosionVentEntity
            {
                BagfilterMasterId = bagfilterMasterId,
                EnquiryId = enquiryId,

                Explosion_Vent_Design_Pressure = src.Explosion_Vent_Design_Pressure,
                Explosion_Vent_Quantity = src.Explosion_Vent_Quantity,
                Explosion_Vent_Size = src.Explosion_Vent_Size,

                // CreatedAt / UpdatedAt handled in repo
            };

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.CreatedAt = existing.CreatedAt;
            }

            return entity;
        }

        private static decimal? ResolveNumberOfColumns(
        string? supportStructType,
        decimal? inputNoOfColumns,
        decimal? baysX,
        decimal? baysZ)
        {
            if (string.Equals(supportStructType, "Regular", StringComparison.OrdinalIgnoreCase))
                return inputNoOfColumns;

            if (!string.Equals(supportStructType, "Pedestal", StringComparison.OrdinalIgnoreCase))
                return null;

            if (!baysX.HasValue || !baysZ.HasValue)
                return null;

            //return (baysX.Value, baysZ.Value) switch
            //{
            //    (1, 1) => 4,
            //    (2, 1) => 6,
            //    (2, 2) => 9,
            //    (3, 2) => 12,
            //    _ => null
            //};
            return (baysX.Value + 1) * (baysZ.Value + 1);
        }


        public async Task<Dictionary<string, int?>> FindMasterMatchesAsync(
        IReadOnlyDictionary<string, GroupKey> groupKeyMap)
        {
            var result = new Dictionary<string, int?>();

            foreach (var kv in groupKeyMap)
            {
                var groupKeyString = kv.Key;
                var key = kv.Value;

                if (!key.EffectiveNoOfColumns.HasValue)
                {
                    result[groupKeyString] = null;
                    continue;
                }

                if (string.Equals(key.Canopy, "Yes", StringComparison.OrdinalIgnoreCase))
                {
                    var match = await _withCanopyRepo.GetByMatchAsync(
                        key.ProcessVolume?.ToString(),
                        key.HopperType,
                        key.EffectiveNoOfColumns,
                        key.BaysInX,
                        key.BaysInZ
                    );

                    result[groupKeyString] = match?.Id;
                }
                else
                {
                    var match = await _withoutCanopyRepo.GetByMatchAsync(
                        key.ProcessVolume?.ToString(),
                        key.HopperType,
                        key.EffectiveNoOfColumns,
                        key.BaysInX,
                        key.BaysInZ
                    );

                    result[groupKeyString] = match?.Id;
                }
            }

            return result;
        }


    }
}

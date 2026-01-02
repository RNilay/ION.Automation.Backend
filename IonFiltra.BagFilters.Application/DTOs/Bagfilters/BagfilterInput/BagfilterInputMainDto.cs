using IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterMaster;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Application.DTOs.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Application.DTOs.BOM.Cage_Cost;
using IonFiltra.BagFilters.Application.DTOs.BOM.Damper_Cost;
using IonFiltra.BagFilters.Application.DTOs.BOM.Painting_Cost;
using IonFiltra.BagFilters.Application.DTOs.BOM.Transp_Cost;
using IonFiltra.BagFilters.Application.DTOs.MasterData.BoughtOutItems;

namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterInputs
{
    public class BagfilterInputMainDto
    {
        public int BagfilterInputId { get; set; }
        public int BagfilterMasterId { get; set; }
        public string? BagFilterName { get; set; }
        public BagfilterMasterDto? BagfilterMaster { get; set; }
        public BagfilterInputDto BagfilterInput { get; set; } = null!;

        public WeightSummaryDto? WeightSummary { get; set; }
        public ProcessInfoDto? ProcessInfo { get; set; }
        public CageInputsDto? CageInputs { get; set; }
        public BagSelectionDto? BagSelection { get; set; }
        public StructureInputsDto? StructureInputs { get; set; }
        public CapsuleInputsDto? CapsuleInputs { get; set; }
        public CasingInputsDto? CasingInputs { get; set; }
        public HopperInputsDto? HopperInputs { get; set; }
        public SupportStructureDto? SupportStructure { get; set; }
        public AccessGroupDto? AccessGroup { get; set; }
        public RoofDoorDto? RoofDoor { get; set; }
        public PaintingAreaDto? PaintingArea { get; set; }

        public List<BillOfMaterialDto> BillOfMaterial { get; set; }
        = new List<BillOfMaterialDto>();

        public PaintingCostDto? PaintingCost { get; set; }

        public List<BoughtOutItemSelectionDto>? BoughtOutItems { get; set; }
         = new List<BoughtOutItemSelectionDto>();

        public List<TransportationCostEntityDto> TransportationCost { get; set; }
        = new List<TransportationCostEntityDto>();

        public List<DamperCostEntityDto> DamperCost { get; set; }
        = new List<DamperCostEntityDto>();

        public List<CageCostEntityDto> CageCost { get; set; }
        = new List<CageCostEntityDto>();
    }
}

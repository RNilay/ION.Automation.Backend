using IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterMaster;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Weight_Summary;

namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterInputs
{
    public class BagfilterInputMainDto
    {
        public int BagfilterInputId { get; set; }
        public int BagfilterMasterId { get; set; }
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

    }
}

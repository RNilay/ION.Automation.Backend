namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Structure_Inputs
{
    public class StructureInputsMainDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public StructureInputsDto StructureInputs { get; set; }

    }
}

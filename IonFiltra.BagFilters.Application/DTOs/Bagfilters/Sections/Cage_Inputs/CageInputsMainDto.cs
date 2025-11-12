namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Cage_Inputs
{
    public class CageInputsMainDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public CageInputsDto CageInputs { get; set; }

    }
}

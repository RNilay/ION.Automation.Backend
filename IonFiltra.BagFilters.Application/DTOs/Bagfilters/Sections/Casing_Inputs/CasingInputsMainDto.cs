namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Casing_Inputs
{
    public class CasingInputsMainDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public CasingInputsDto CasingInputs { get; set; }

    }
}

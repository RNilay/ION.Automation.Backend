namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Capsule_Inputs
{
    public class CapsuleInputsMainDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public CapsuleInputsDto CapsuleInputs { get; set; }

    }
}

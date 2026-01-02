namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.DamperSize
{
    public class DamperSizeInputsMainDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public DamperSizeInputsDto DamperSizeInputs { get; set; }

    }
}

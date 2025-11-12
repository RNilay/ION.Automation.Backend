namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Support_Structure
{
    public class SupportStructureMainDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public SupportStructureDto SupportStructure { get; set; }

    }
}

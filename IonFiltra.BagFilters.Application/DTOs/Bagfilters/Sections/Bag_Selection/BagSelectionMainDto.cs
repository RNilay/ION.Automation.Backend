namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Bag_Selection
{
    public class BagSelectionMainDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public BagSelectionDto BagSelection { get; set; }

    }
}

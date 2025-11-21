namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Painting
{
    public class PaintingAreaMainDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public PaintingAreaDto PaintingArea { get; set; }

    }
}

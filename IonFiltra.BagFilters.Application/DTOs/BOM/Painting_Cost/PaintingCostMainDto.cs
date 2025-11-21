namespace IonFiltra.BagFilters.Application.DTOs.BOM.Painting_Cost
{
    public class PaintingCostMainDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public PaintingCostDto PaintingCost { get; set; }

    }
}

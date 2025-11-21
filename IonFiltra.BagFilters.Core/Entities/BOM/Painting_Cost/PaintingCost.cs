namespace IonFiltra.BagFilters.Core.Entities.BOM.Painting_Cost
{
    public class PaintingCost
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        
        public string? PaintingTableJson { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

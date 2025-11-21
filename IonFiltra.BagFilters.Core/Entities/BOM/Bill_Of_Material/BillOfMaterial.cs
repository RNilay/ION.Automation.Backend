namespace IonFiltra.BagFilters.Core.Entities.BOM.Bill_Of_Material
{
    public class BillOfMaterial
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public string? Item { get; set; }
        public string? Material { get; set; }
        public decimal? Weight { get; set; }
        public string? Units { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Cost { get; set; }
        public int? SortOrder { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}

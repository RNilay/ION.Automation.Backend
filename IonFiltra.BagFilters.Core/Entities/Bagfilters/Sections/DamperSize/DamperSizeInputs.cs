namespace IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.DamperSize
{
    public class DamperSizeInputs
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
    
        public string? Is_Damper_Required { get; set; }
        public string? Damper_Series { get; set; }
        public decimal? Damper_Diameter { get; set; }
        public int? Damper_Qty { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}

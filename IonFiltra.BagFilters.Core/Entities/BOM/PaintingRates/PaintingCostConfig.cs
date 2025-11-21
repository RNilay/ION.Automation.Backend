namespace IonFiltra.BagFilters.Core.Entities.BOM.PaintingRates
{
    public class PaintingCostConfig
    {
        public int Id { get; set; }
    
        public string? Code { get; set; }
        public string? Section { get; set; }
        public string? Item { get; set; }
        public decimal? InrPerLtr { get; set; }
        public decimal? SqmPerLtr { get; set; }
        public decimal? Coats { get; set; }
        public decimal? LabourRate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}

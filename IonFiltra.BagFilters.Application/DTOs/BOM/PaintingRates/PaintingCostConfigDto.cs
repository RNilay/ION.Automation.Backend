namespace IonFiltra.BagFilters.Application.DTOs.BOM.PaintingRates
{
    public class PaintingCostConfigDto
    {
        public string? Code { get; set; } = string.Empty;
        public string? Section { get; set; } = string.Empty;
        public string? Item { get; set; } = string.Empty;
        public decimal? InrPerLtr { get; set; }
        public decimal? SqmPerLtr { get; set; }
        public decimal? Coats { get; set; }
        public decimal? LabourRate { get; set; }

    }
}

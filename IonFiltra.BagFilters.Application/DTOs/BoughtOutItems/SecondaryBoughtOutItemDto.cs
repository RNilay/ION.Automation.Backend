namespace IonFiltra.BagFilters.Application.DTOs.MasterData.BoughtOutItems
{
    public class SecondaryBoughtOutItemDto
    {
        public string MasterKey { get; set; } = null!;
        public string? Make { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Qty { get; set; } = 1;
        public string? Unit { get; set; } = "No's";
        public decimal? Rate { get; set; }   // rate = per-unit
    }
}

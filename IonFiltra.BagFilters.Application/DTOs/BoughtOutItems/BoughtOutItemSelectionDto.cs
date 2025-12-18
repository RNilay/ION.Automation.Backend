namespace IonFiltra.BagFilters.Application.DTOs.MasterData.BoughtOutItems
{
    public class BoughtOutItemSelectionDto
    {
 
        public string? MasterKey { get; set; }
        public int? SelectedRowId { get; set; }

        //new
        public decimal? Qty { get; set; }
        public string? Unit { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Cost { get; set; }

    }
}

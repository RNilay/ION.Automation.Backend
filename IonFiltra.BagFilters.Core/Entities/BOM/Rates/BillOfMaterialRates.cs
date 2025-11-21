namespace IonFiltra.BagFilters.Core.Entities.BOM.Rates
{
    public class BillOfMaterialRates
    {
        public int Id { get; set; }
        public string? ItemKey { get; set; }
        public decimal? Rate { get; set; }
        public string? Unit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}

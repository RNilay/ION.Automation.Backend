namespace IonFiltra.BagFilters.Application.DTOs.BOM.Bill_Of_Material
{
    public class BillOfMaterialDto
    {
        public string? Item { get; set; }
        public string? Material { get; set; }
        public decimal? Weight { get; set; }
        public string? Units { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Cost { get; set; }
        public int? SortOrder { get; set; }

    }
}

namespace IonFiltra.BagFilters.Core.Entities.MasterData.FilterBagData
{
    public class FilterBag
    {
        public int Id { get; set; }
       
       

        public string? Material { get; set; }
        public decimal? Gsm { get; set; }
        public string? Size { get; set; }
        public string? Make { get; set; }
        public decimal? Cost { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}

namespace IonFiltra.BagFilters.Core.Entities.MasterData.DPTData
{
    public class DPTEntity
    {
        public int Id { get; set; }


        public string? Make { get; set; }
        public string? Model { get; set; }
        public decimal? Cost { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}

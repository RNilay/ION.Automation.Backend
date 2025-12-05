namespace IonFiltra.BagFilters.Core.Entities.MasterData.TimerData
{
    public class TimerEntity
    {
        public int Id { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public decimal? Cost { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}

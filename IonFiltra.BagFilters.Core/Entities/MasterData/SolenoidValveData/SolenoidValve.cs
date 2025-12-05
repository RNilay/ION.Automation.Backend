namespace IonFiltra.BagFilters.Core.Entities.MasterData.SolenoidValveData
{
    public class SolenoidValve
    {
        public int Id { get; set; }
        public string? Make { get; set; }
        public string? Size { get; set; }
        public String? Model { get; set; }
        public decimal? Cost { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }   // NEW

    }
}

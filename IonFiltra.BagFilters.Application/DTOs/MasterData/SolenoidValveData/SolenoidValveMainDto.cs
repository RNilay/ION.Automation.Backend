namespace IonFiltra.BagFilters.Application.DTOs.MasterData.SolenoidValveData
{
    public class SolenoidValveMainDto
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }  // NEW
        public SolenoidValveDto SolenoidValve { get; set; }

    }
}

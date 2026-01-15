namespace IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Capsule_Inputs
{
    public class CapsuleInputs
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public decimal? Valve_Size { get; set; }
        public string? Voltage_Rating { get; set; }
        public decimal? Capsule_Height { get; set; }
        public decimal? Total_Capsule_Height { get; set; }
        public decimal? Tube_Sheet_Thickness { get; set; }
        public decimal? Capsule_Wall_Thickness { get; set; }
        public string? Canopy { get; set; }
        public string? Solenoid_Valve_Maintainence { get; set; }
        public decimal? Capsule_Area { get; set; }
        public decimal? Capsule_Weight { get; set; }
        public decimal? Tubesheet_Area { get; set; }
        public decimal? Tubesheet_Weight { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

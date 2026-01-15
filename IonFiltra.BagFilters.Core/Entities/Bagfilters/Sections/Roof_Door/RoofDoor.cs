namespace IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Roof_Door
{
    public class RoofDoor
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public decimal? Roof_Door_Thickness { get; set; }
        public decimal? T2d { get; set; }
        public decimal? T3d { get; set; }
        public decimal? N_Doors { get; set; }
        public decimal? Compartment_No { get; set; }
        public decimal? Stiffening_Factor_Roof_Door { get; set; }
        public decimal? Weight_Per_Door { get; set; }
        public decimal? Tot_Weight_Per_Compartment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

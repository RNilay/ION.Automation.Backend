namespace IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Casing_Inputs
{
    public class CasingInputs
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public decimal? Casing_Wall_Thickness { get; set; }
        public decimal? Stiffening_Factor_Casing { get; set; }
        public decimal? Casing_Height { get; set; }
        public decimal? Casing_Area { get; set; }
        public decimal? Casing_Weight { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}

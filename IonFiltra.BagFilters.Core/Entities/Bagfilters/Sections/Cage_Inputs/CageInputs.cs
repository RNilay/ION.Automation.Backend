namespace IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Cage_Inputs
{
    public class CageInputs
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public string? Cage_Type { get; set; }
        public string? Cage_Sub_Type { get; set; }
        public decimal? Cage_Wire_Dia { get; set; }
        public decimal? No_Of_Cage_Wires { get; set; }
        public decimal? Ring_Spacing { get; set; }
        public decimal? Cage_Diameter { get; set; }
        public decimal? Cage_Length { get; set; }
        public string? Cage_Configuration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}

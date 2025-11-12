namespace IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Support_Structure
{
    public class SupportStructure
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public string? Support_Struct_Type { get; set; }
        public decimal? NoOfColumn { get; set; }
        public decimal? Column_Height { get; set; }
        public decimal? Ground_Clearance { get; set; }
        public decimal? Dist_Btw_Column_In_X { get; set; }
        public decimal? Dist_Btw_Column_In_Z { get; set; }
        public decimal? No_Of_Bays_In_X { get; set; }
        public decimal? No_Of_Bays_In_Z { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}

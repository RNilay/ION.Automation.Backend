namespace IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Access_Group
{
    public class AccessGroup
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public string? Access_Type { get; set; }
        public decimal? Cage_Weight_Ladder { get; set; }
        public decimal? Total_Weight_Of_Cage_Ladder { get; set; }
        public string? Mid_Landing_Pltform { get; set; }
        public decimal? Platform_Weight { get; set; }
        public decimal? Staircase_Height { get; set; }
        public decimal? Staircase_Weight { get; set; }
        public decimal? Railing_Weight { get; set; }
        public decimal? Total_Weight_Of_Railing { get; set; }
        public string? Maintainence_Pltform { get; set; }
        public decimal? Maintainence_Pltform_Weight { get; set; }
        public decimal? Total_Weight_Of_Maintainence_Pltform { get; set; }
        public decimal? BlowPipe { get; set; }
        public decimal? Total_Weight_Of_Blow_Pipe { get; set; }
        public decimal? PressureHeader { get; set; }
        public decimal? Total_Weight_Of_Pressure_Header { get; set; }
        public decimal? DistancePiece { get; set; }
        public decimal? Access_Stool_Size_Mm { get; set; }
        public decimal? Access_Stool_Size_Kg { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}

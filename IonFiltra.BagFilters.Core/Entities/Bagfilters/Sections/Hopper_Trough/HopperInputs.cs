namespace IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Hopper_Trough
{
    public class HopperInputs
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public string? Hopper_Type { get; set; }
        public decimal? Process_Compartments { get; set; }
        public decimal? Tot_No_Of_Hoppers { get; set; }
        public decimal? Tot_No_Of_Trough { get; set; }
        public decimal? Plenum_Width { get; set; }
        public decimal? Inlet_Height { get; set; }
        public decimal? Hopper_Thickness { get; set; }
        public decimal? Hopper_Valley_Angle { get; set; }
        public string? Access_Door_Type { get; set; }
        public decimal? Access_Door_Qty { get; set; }
        public string? Rav_Maintainence_Pltform { get; set; }
        public string? Hopper_Access_Stool { get; set; }
        public string? Is_Distance_Piece { get; set; }
        public decimal? Distance_Piece_Height { get; set; }
        public decimal? Stiffening_Factor { get; set; }
        public decimal? Hopper { get; set; }
        public decimal? Discharge_Opening_Sqr { get; set; }
        public string? Material_Handling { get; set; }
        public decimal? Material_Handling_Qty { get; set; }
        public decimal? Trough_Outlet_Length { get; set; }
        public decimal? Trough_Outlet_Width { get; set; }
        public string? Material_Handling_Xxx { get; set; }
        public decimal? Hor_Diff_Length { get; set; }
        public decimal? Hor_Diff_Width { get; set; }
        public decimal? Slant_Offset_Dist { get; set; }
        public decimal? Hopper_Height { get; set; }
        public decimal? Hopper_Height_Mm { get; set; }
        public decimal? Slanting_Hopper_Height { get; set; }
        public decimal? Hopper_Area_Length { get; set; }
        public decimal? Hopper_Area_Width { get; set; }
        public decimal? Hopper_Tot_Area { get; set; }
        public decimal? Hopper_Weight { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}

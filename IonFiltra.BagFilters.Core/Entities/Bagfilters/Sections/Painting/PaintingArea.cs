namespace IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Painting
{
    public class PaintingArea
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }

        public decimal? Inside_Area_Casing_Area_Mm2 { get; set; }
        public decimal? Inside_Area_Casing_Area_M2 { get; set; }
        public decimal? Inside_Area_Hopper_Area_Mm2 { get; set; }
        public decimal? Inside_Area_Hopper_Area_M2 { get; set; }
        public decimal? Inside_Area_Air_Header_Mm2 { get; set; }
        public decimal? Inside_Area_Air_Header_M2 { get; set; }
        public decimal? Inside_Area_Purge_Pipe_Mm2 { get; set; }
        public decimal? Inside_Area_Purge_Pipe_M2 { get; set; }
        public decimal? Inside_Area_Roof_Door_Mm2 { get; set; }
        public decimal? Inside_Area_Roof_Door_M2 { get; set; }
        public decimal? Inside_Area_Tube_Sheet_Mm2 { get; set; }
        public decimal? Inside_Area_Tube_Sheet_M2 { get; set; }
        public decimal? Inside_Area_Total_M2 { get; set; }
        public decimal? Outside_Area_Casing_Area_Mm2 { get; set; }
        public decimal? Outside_Area_Casing_Area_M2 { get; set; }
        public decimal? Outside_Area_Hopper_Area_Mm2 { get; set; }
        public decimal? Outside_Area_Hopper_Area_M2 { get; set; }
        public decimal? Outside_Area_Air_Header_Mm2 { get; set; }
        public decimal? Outside_Area_Air_Header_M2 { get; set; }
        public decimal? Outside_Area_Purge_Pipe_Mm2 { get; set; }
        public decimal? Outside_Area_Purge_Pipe_M2 { get; set; }
        public decimal? Outside_Area_Roof_Door_Mm2 { get; set; }
        public decimal? Outside_Area_Roof_Door_M2 { get; set; }
        public decimal? Outside_Area_Tube_Sheet_Mm2 { get; set; }
        public decimal? Outside_Area_Tube_Sheet_M2 { get; set; }
        public decimal? Outside_Area_Total_M2 { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

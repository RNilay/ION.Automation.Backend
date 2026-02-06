namespace IonFiltra.BagFilters.Core.Entities.BagfilterDatabase.WithoutCanopy
{
    public class IFI_Bagfilter_Database_Without_Canopy
    {
        public int Id { get; set; }
        public string? Process_Volume_m3hr { get; set; }
        public string? Hopper_type { get; set; }
        public decimal? Number_of_columns { get; set; }
        public decimal? Number_of_bays_in_X_direction { get; set; }
        public decimal? Number_of_bays_in_Y_direction { get; set; }
        public decimal? Column_CC_distance_in_X_direction_mm { get; set; }
        public decimal? Column_CC_distance_in_Y_direction_mm { get; set; }
        public decimal? Clearance_Below_Hopper_Flange_mm { get; set; }
        public decimal? Height_upto_mm_Column { get; set; }
        public decimal? Height_upto_mm_Tube_Sheet { get; set; }
        public decimal? Height_upto_mm_Capsule_Top { get; set; }
        public string? Member_Sizes_Column { get; set; }
        public string? Member_Sizes_Beam { get; set; }
        public string? Member_Sizes_Bracing_and_Ties { get; set; }
        public string? Member_Sizes_RAV { get; set; }
        public string? Member_Sizes_Staging_Beam { get; set; }
        public string? Member_Sizes_Grid_Beam { get; set; }
        public decimal? Bolts_No_of_Bolt { get; set; }
        public decimal? Bolts_Dia_of_Bolt { get; set; }
        public decimal? Bolts_Grade_of_Bolt { get; set; }
        public string? Bolts_Sleeve_Size_mm { get; set; }
        public decimal? Bolts_Embedded_Length_mm { get; set; }
        public string? Bolt_CC_Distance_Confirguration_RCC { get; set; }
        public string? Bolt_CC_Distance_Confirguration_Steel { get; set; }
        public string? Base_Plate_Dimension_RCC { get; set; }
        public string? Base_Plate_Dimension_Steel { get; set; }
        public decimal? Weight_of_Base_Plate_kg { get; set; }
        public decimal? Total_Weight_of_Structure_kg { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}

using Newtonsoft.Json.Linq;

namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterInputs
{
    public class BagfilterInputDto
    {
        public int? EnquiryId { get; set; }
        public decimal? Process_Volume_M3h { get; set; }
        public string? Location { get; set; }
        public string? Process_Dust { get; set; }
        public decimal? Process_Dustload_Gmspm3 { get; set; }
        public decimal? Process_Temp_C { get; set; }
        public decimal? Dew_Point_C { get; set; }
        public string? Outlet_Emission_Mgpm3 { get; set; }
        public decimal? Process_Cloth_Ratio { get; set; }
        public decimal? Specific_Gravity { get; set; }
        public string? Customer_Equipment_Tag_No { get; set; }
        public string? Bagfilter_Cleaning_Type { get; set; }
        public string? Offline_Maintainence { get; set; }
        public decimal? Cage_Wire_Dia { get; set; }
        public decimal? No_Of_Cage_Wires { get; set; }
        public decimal? Ring_Spacing { get; set; }
        public decimal? Filter_Bag_Dia { get; set; }
        public decimal? Fil_Bag_Length { get; set; }
        public string? Fil_Bag_Recommendation { get; set; }
        public string? Gas_Entry { get; set; }
        public string? Support_Structure_Type { get; set; }
        public decimal? Can_Correction { get; set; }
        public decimal? Valve_Size { get; set; }
        public string? Voltage_Rating { get; set; }
        public string? Cage_Type { get; set; }
        public string? Cage_Length { get; set; }
        public decimal? Capsule_Height { get; set; }
        public decimal? Tube_Sheet_Thickness { get; set; }
        public decimal? Capsule_Wall_Thickness { get; set; }
        public string? Canopy { get; set; }
        public string? Solenoid_Valve_Maintainence { get; set; }
        public decimal? Casing_Wall_Thickness { get; set; }
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
        public string? Material_Handling_XXX { get; set; }
        public string? Support_Struct_Type { get; set; }
        public decimal? No_Of_Column { get; set; }
        public decimal? Ground_Clearance { get; set; }
        public string? Access_Type { get; set; }
        public decimal? Cage_Weight_Ladder { get; set; }
        public string? Mid_Landing_Pltform { get; set; }
        public decimal? Platform_Weight { get; set; }
        public decimal? Staircase_Height { get; set; }
        public decimal? Staircase_Weight { get; set; }
        public decimal? Railing_Weight { get; set; }
        public string? Maintainence_Pltform { get; set; }
        public decimal? Maintainence_Pltform_Weight { get; set; }
        public decimal? Blow_Pipe { get; set; }
        public decimal? Pressure_Header { get; set; }
        public decimal? Distance_Piece { get; set; }
        public decimal? Access_Stool_Size_Mm { get; set; }
        public decimal? Access_Stool_Size_Kg { get; set; }
        public decimal? Roof_Door_Thickness { get; set; }
        public decimal? Column_Height { get; set; }
        public decimal? Bag_Per_Row { get; set; } //calculated values
        public decimal? Number_Of_Rows { get; set; } //calculated values

        public string? S3dModel { get; set; }

    }

    //public class BagfilterMatchDto
    //{
    //    public int BagfilterInputId { get; set; }
    //    public int BagfilterMasterId { get; set; }
    //    public int? AssignmentId { get; set; }    // will be available if BagfilterMaster has it
    //    public int? EnquiryId { get; set; }       // the matched row's EnquiryId
    //    public string? BagFilterName { get; set; }
    //    public string? CustomerName { get; set; }

    //    // echo keys
    //    public decimal? No_Of_Column { get; set; }
    //    public decimal? Ground_Clearance { get; set; }
    //    public decimal? Bag_Per_Row { get; set; }
    //    public decimal? Number_Of_Rows { get; set; }
    //}

    public class BagfilterMatchDto
    {
        public string? GroupId { get; set; }          // e.g. "Group 1"
        public string? Location { get; set; }         // group's Location
        public int BagfilterInputId { get; set; }
        public int BagfilterMasterId { get; set; }
        public int? AssignmentId { get; set; }
        public int? EnquiryId { get; set; }
        public string? BagFilterName { get; set; }
        public string? CustomerName { get; set; }    // from Enquiry.Customer
        public DateTime? CreatedAt { get; set; }     // from Enquiry.CreatedAt

        // echo keys
        public decimal? No_Of_Column { get; set; }
        public decimal? Ground_Clearance { get; set; }
        public decimal? Bag_Per_Row { get; set; }
        public decimal? Number_Of_Rows { get; set; }
        public bool IsMatched { get; set; } = false;

    }


    public class AddRangeResultDto
    {
        public List<int> CreatedBagfilterInputIds { get; set; } = new();
        public List<BagfilterMatchDto>? Matches { get; set; } = null;
        public string? Message { get; set; } = null;

        // optional: total matched items count (sum of sizes of matched groups)
        public int MatchedItemsCount { get; set; } = 0;
        // optional:
        public int TotalGroupsCount { get; set; } = 0;
        public int MatchedGroupsCount { get; set; } = 0;
    }
}

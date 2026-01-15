namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Cage_Inputs
{
    public class CageInputsDto
    {
        public string? Cage_Type { get; set; }
        public string? Cage_Sub_Type { get; set; }
        public string? Cage_Material { get; set; }
        public decimal? Cage_Wire_Dia { get; set; }
        public decimal? No_Of_Cage_Wires { get; set; }
        public decimal? Ring_Spacing { get; set; }
        public decimal? Cage_Diameter { get; set; }
        public decimal? Cage_Length { get; set; }
        public decimal? Spare_Cages { get; set; }
        public string? Cage_Configuration { get; set; }

        //newly added
        public decimal? No_Of_Rings { get; set; }
        public decimal? Tot_Wire_Length { get; set; }

        public decimal? Weight_Of_Cage_Wires { get; set; }
        public decimal? Weight_Of_Cage_Rings { get; set; }
        public decimal? Weight_Of_One_Cage { get; set; }
        public decimal? Cage_Weight { get; set; }




    }
}

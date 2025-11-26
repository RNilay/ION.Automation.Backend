using IonFiltra.BagFilters.Application.DTOs.BagfilterDatabase.WithCanopy;
using IonFiltra.BagFilters.Core.Entities.BagfilterDatabase.WithCanopy;

namespace IonFiltra.BagFilters.Application.Mappers.BagfilterDatabase.WithCanopy
{
    public static class IFI_Bagfilter_Database_With_CanopyMapper
    {
        public static IFI_Bagfilter_Database_With_Canopy_MainDto ToMainDto(IFI_Bagfilter_Database_With_Canopy entity)
        {
            if (entity == null) return null;
            return new IFI_Bagfilter_Database_With_Canopy_MainDto
            {
                Id = entity.Id,
              
                IFI_Bagfilter_Database_With_Canopy = new IFI_Bagfilter_Database_With_CanopyDto
                {
                    Process_Volume_m3hr = entity.Process_Volume_m3hr,
                    Hopper_type = entity.Hopper_type,
                    Number_of_columns = entity.Number_of_columns,
                    Number_of_bays_in_X_direction = entity.Number_of_bays_in_X_direction,
                    Number_of_bays_in_Y_direction = entity.Number_of_bays_in_Y_direction,

                    Foot_Print_Column_CC_Header_Side_mm_x = entity.Foot_Print_Column_CC_Header_Side_mm_x,
                    Foot_Print_Column_CC_Other_Side_mm_y = entity.Foot_Print_Column_CC_Other_Side_mm_y,
                    Ht_of_Supp_Structure_mm_Hopper_Bottom = entity.Ht_of_Supp_Structure_mm_Hopper_Bottom,
                    Ht_of_Supp_Structure_mm_Column = entity.Ht_of_Supp_Structure_mm_Column,
                    Ht_of_Supp_Structure_mm_Tube_Sheet = entity.Ht_of_Supp_Structure_mm_Tube_Sheet,
                    Ht_of_Supp_Structure_mm_Capsule_Top = entity.Ht_of_Supp_Structure_mm_Capsule_Top,
                    Ht_of_Supp_Structure_mm_Shed_Height = entity.Ht_of_Supp_Structure_mm_Shed_Height,
                    Member_Sizes_Column = entity.Member_Sizes_Column,
                    Member_Sizes_Beam = entity.Member_Sizes_Beam,
                    Member_Sizes_Bracing_Ties = entity.Member_Sizes_Bracing_Ties,
                    Member_Sizes_RAV = entity.Member_Sizes_RAV,
                    Member_Sizes_Staging_Beam = entity.Member_Sizes_Staging_Beam,
                    Member_Sizes_Grid_Beam = entity.Member_Sizes_Grid_Beam,
                    Member_Sizes_mm_Shed_Column_Rafter = entity.Member_Sizes_mm_Shed_Column_Rafter,
                    Member_Sizes_mm_Shed_Girt_and_Purlin_along_X_axis = entity.Member_Sizes_mm_Shed_Girt_and_Purlin_along_X_axis,
                    Member_Sizes_mm_Shed_Girt_Purlin_and_Ridge_along_Y_axis = entity.Member_Sizes_mm_Shed_Girt_Purlin_and_Ridge_along_Y_axis,
                    Member_Sizes_mm_Shed_Shed_Bracings = entity.Member_Sizes_mm_Shed_Shed_Bracings,
                    Member_Sizes_mm_Shed_Roof_Truss = entity.Member_Sizes_mm_Shed_Roof_Truss,
                    Bolts_No_of_Bolt = entity.Bolts_No_of_Bolt,
                    Bolts_Dia_of_Bolt = entity.Bolts_Dia_of_Bolt,
                    Bolts_Grade_of_Bolt = entity.Bolts_Grade_of_Bolt,
                    Bolts_Sleeve_Size_mm = entity.Bolts_Sleeve_Size_mm,
                    Bolts_Embedded_Length_mm = entity.Bolts_Embedded_Length_mm,
                    Bolt_CC_Distance_Confirguration_RCC = entity.Bolt_CC_Distance_Confirguration_RCC,
                    Bolt_CC_Distance_Confirguration_Steel = entity.Bolt_CC_Distance_Confirguration_Steel,
                    Base_Plate_Dimension_RCC = entity.Base_Plate_Dimension_RCC,
                    Base_Plate_Dimension_Steel = entity.Base_Plate_Dimension_Steel,
                    Weight_of_Base_Plate_kg = entity.Weight_of_Base_Plate_kg,
                    Total_Weight_of_Structure_kg = entity.Total_Weight_of_Structure_kg,
                    Weight_of_Plates_kg = entity.Weight_of_Plates_kg,
                },

            };
        }

        public static IFI_Bagfilter_Database_With_Canopy ToEntity(IFI_Bagfilter_Database_With_Canopy_MainDto dto)
        {
            if (dto == null) return null;
            return new IFI_Bagfilter_Database_With_Canopy
            {
                Id = dto.Id,

                Process_Volume_m3hr = dto.IFI_Bagfilter_Database_With_Canopy.Process_Volume_m3hr,
                Hopper_type = dto.IFI_Bagfilter_Database_With_Canopy.Hopper_type,
                Number_of_columns = dto.IFI_Bagfilter_Database_With_Canopy.Number_of_columns,
                Number_of_bays_in_X_direction = dto.IFI_Bagfilter_Database_With_Canopy.Number_of_bays_in_X_direction,
                Number_of_bays_in_Y_direction = dto.IFI_Bagfilter_Database_With_Canopy.Number_of_bays_in_Y_direction,

                Foot_Print_Column_CC_Header_Side_mm_x = dto.IFI_Bagfilter_Database_With_Canopy.Foot_Print_Column_CC_Header_Side_mm_x,
                Foot_Print_Column_CC_Other_Side_mm_y = dto.IFI_Bagfilter_Database_With_Canopy.Foot_Print_Column_CC_Other_Side_mm_y,
                Ht_of_Supp_Structure_mm_Hopper_Bottom = dto.IFI_Bagfilter_Database_With_Canopy.Ht_of_Supp_Structure_mm_Hopper_Bottom,
                Ht_of_Supp_Structure_mm_Column = dto.IFI_Bagfilter_Database_With_Canopy.Ht_of_Supp_Structure_mm_Column,
                Ht_of_Supp_Structure_mm_Tube_Sheet = dto.IFI_Bagfilter_Database_With_Canopy.Ht_of_Supp_Structure_mm_Tube_Sheet,
                Ht_of_Supp_Structure_mm_Capsule_Top = dto.IFI_Bagfilter_Database_With_Canopy.Ht_of_Supp_Structure_mm_Capsule_Top,
                Ht_of_Supp_Structure_mm_Shed_Height = dto.IFI_Bagfilter_Database_With_Canopy.Ht_of_Supp_Structure_mm_Shed_Height,
                Member_Sizes_Column = dto.IFI_Bagfilter_Database_With_Canopy.Member_Sizes_Column,
                Member_Sizes_Beam = dto.IFI_Bagfilter_Database_With_Canopy.Member_Sizes_Beam,
                Member_Sizes_Bracing_Ties = dto.IFI_Bagfilter_Database_With_Canopy.Member_Sizes_Bracing_Ties,
                Member_Sizes_RAV = dto.IFI_Bagfilter_Database_With_Canopy.Member_Sizes_RAV,
                Member_Sizes_Staging_Beam = dto.IFI_Bagfilter_Database_With_Canopy.Member_Sizes_Staging_Beam,
                Member_Sizes_Grid_Beam = dto.IFI_Bagfilter_Database_With_Canopy.Member_Sizes_Grid_Beam,
                Member_Sizes_mm_Shed_Column_Rafter = dto.IFI_Bagfilter_Database_With_Canopy.Member_Sizes_mm_Shed_Column_Rafter,
                Member_Sizes_mm_Shed_Girt_and_Purlin_along_X_axis = dto.IFI_Bagfilter_Database_With_Canopy.Member_Sizes_mm_Shed_Girt_and_Purlin_along_X_axis,
                Member_Sizes_mm_Shed_Girt_Purlin_and_Ridge_along_Y_axis = dto.IFI_Bagfilter_Database_With_Canopy.Member_Sizes_mm_Shed_Girt_Purlin_and_Ridge_along_Y_axis,
                Member_Sizes_mm_Shed_Shed_Bracings = dto.IFI_Bagfilter_Database_With_Canopy.Member_Sizes_mm_Shed_Shed_Bracings,
                Member_Sizes_mm_Shed_Roof_Truss = dto.IFI_Bagfilter_Database_With_Canopy.Member_Sizes_mm_Shed_Roof_Truss,
                Bolts_No_of_Bolt = dto.IFI_Bagfilter_Database_With_Canopy.Bolts_No_of_Bolt,
                Bolts_Dia_of_Bolt = dto.IFI_Bagfilter_Database_With_Canopy.Bolts_Dia_of_Bolt,
                Bolts_Grade_of_Bolt = dto.IFI_Bagfilter_Database_With_Canopy.Bolts_Grade_of_Bolt,
                Bolts_Sleeve_Size_mm = dto.IFI_Bagfilter_Database_With_Canopy.Bolts_Sleeve_Size_mm,
                Bolts_Embedded_Length_mm = dto.IFI_Bagfilter_Database_With_Canopy.Bolts_Embedded_Length_mm,
                Bolt_CC_Distance_Confirguration_RCC = dto.IFI_Bagfilter_Database_With_Canopy.Bolt_CC_Distance_Confirguration_RCC,
                Bolt_CC_Distance_Confirguration_Steel = dto.IFI_Bagfilter_Database_With_Canopy.Bolt_CC_Distance_Confirguration_Steel,
                Base_Plate_Dimension_RCC = dto.IFI_Bagfilter_Database_With_Canopy.Base_Plate_Dimension_RCC,
                Base_Plate_Dimension_Steel = dto.IFI_Bagfilter_Database_With_Canopy.Base_Plate_Dimension_Steel,
                Weight_of_Base_Plate_kg = dto.IFI_Bagfilter_Database_With_Canopy.Weight_of_Base_Plate_kg,
                Total_Weight_of_Structure_kg = dto.IFI_Bagfilter_Database_With_Canopy.Total_Weight_of_Structure_kg,
                Weight_of_Plates_kg = dto.IFI_Bagfilter_Database_With_Canopy.Weight_of_Plates_kg,

            };
        }
    }
}

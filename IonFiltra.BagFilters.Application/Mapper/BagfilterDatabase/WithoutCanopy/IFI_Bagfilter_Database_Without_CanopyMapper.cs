using IonFiltra.BagFilters.Application.DTOs.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Core.Entities.BagfilterDatabase.WithoutCanopy;

namespace IonFiltra.BagFilters.Application.Mappers.BagfilterDatabase.WithoutCanopy
{
    public static class IFI_Bagfilter_Database_Without_CanopyMapper
    {
        public static IFI_Bagfilter_Database_Without_Canopy_Main_Dto ToMainDto(IFI_Bagfilter_Database_Without_Canopy entity)
        {
            if (entity == null) return null;
            return new IFI_Bagfilter_Database_Without_Canopy_Main_Dto
            {
                Id = entity.Id,
             
                IFI_Bagfilter_Database_Without_Canopy = new IFI_Bagfilter_Database_Without_CanopyDto
                {
                    Process_Volume_m3hr = entity.Process_Volume_m3hr,
                    Hopper_type = entity.Hopper_type,
                    Number_of_columns = entity.Number_of_columns,
                    Number_of_bays_in_X_direction = entity.Number_of_bays_in_X_direction,
                    Number_of_bays_in_Y_direction = entity.Number_of_bays_in_Y_direction,
                    Column_CC_distance_in_X_direction_mm = entity.Column_CC_distance_in_X_direction_mm,
                    Column_CC_distance_in_Y_direction_mm = entity.Column_CC_distance_in_Y_direction_mm,
                    Clearance_Below_Hopper_Flange_mm = entity.Clearance_Below_Hopper_Flange_mm,
                    Height_upto_mm_Column = entity.Height_upto_mm_Column,
                    Height_upto_mm_Tube_Sheet = entity.Height_upto_mm_Tube_Sheet,
                    Height_upto_mm_Capsule_Top = entity.Height_upto_mm_Capsule_Top,
                    Member_Sizes_Column = entity.Member_Sizes_Column,
                    Member_Sizes_Beam = entity.Member_Sizes_Beam,
                    Member_Sizes_Bracing_and_Ties = entity.Member_Sizes_Bracing_and_Ties,
                    Member_Sizes_RAV = entity.Member_Sizes_RAV,
                    Member_Sizes_Staging_Beam = entity.Member_Sizes_Staging_Beam,
                    Member_Sizes_Grid_Beam = entity.Member_Sizes_Grid_Beam,
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
                },

            };
        }

        public static IFI_Bagfilter_Database_Without_Canopy ToEntity(IFI_Bagfilter_Database_Without_Canopy_Main_Dto dto)
        {
            if (dto == null) return null;
            return new IFI_Bagfilter_Database_Without_Canopy
            {
                Id = dto.Id,
              
                Process_Volume_m3hr = dto.IFI_Bagfilter_Database_Without_Canopy.Process_Volume_m3hr,
                Hopper_type = dto.IFI_Bagfilter_Database_Without_Canopy.Hopper_type,
                Number_of_columns = dto.IFI_Bagfilter_Database_Without_Canopy.Number_of_columns,
                Number_of_bays_in_X_direction = dto.IFI_Bagfilter_Database_Without_Canopy.Number_of_bays_in_X_direction,
                Number_of_bays_in_Y_direction = dto.IFI_Bagfilter_Database_Without_Canopy.Number_of_bays_in_Y_direction,
                Column_CC_distance_in_X_direction_mm = dto.IFI_Bagfilter_Database_Without_Canopy.Column_CC_distance_in_X_direction_mm,
                Column_CC_distance_in_Y_direction_mm = dto.IFI_Bagfilter_Database_Without_Canopy.Column_CC_distance_in_Y_direction_mm,
                Clearance_Below_Hopper_Flange_mm = dto.IFI_Bagfilter_Database_Without_Canopy.Clearance_Below_Hopper_Flange_mm,
                Height_upto_mm_Column = dto.IFI_Bagfilter_Database_Without_Canopy.Height_upto_mm_Column,
                Height_upto_mm_Tube_Sheet = dto.IFI_Bagfilter_Database_Without_Canopy.Height_upto_mm_Tube_Sheet,
                Height_upto_mm_Capsule_Top = dto.IFI_Bagfilter_Database_Without_Canopy.Height_upto_mm_Capsule_Top,
                Member_Sizes_Column = dto.IFI_Bagfilter_Database_Without_Canopy.Member_Sizes_Column,
                Member_Sizes_Beam = dto.IFI_Bagfilter_Database_Without_Canopy.Member_Sizes_Beam,
                Member_Sizes_Bracing_and_Ties = dto.IFI_Bagfilter_Database_Without_Canopy.Member_Sizes_Bracing_and_Ties,
                Member_Sizes_RAV = dto.IFI_Bagfilter_Database_Without_Canopy.Member_Sizes_RAV,
                Member_Sizes_Staging_Beam = dto.IFI_Bagfilter_Database_Without_Canopy.Member_Sizes_Staging_Beam,
                Member_Sizes_Grid_Beam = dto.IFI_Bagfilter_Database_Without_Canopy.Member_Sizes_Grid_Beam,
                Bolts_No_of_Bolt = dto.IFI_Bagfilter_Database_Without_Canopy.Bolts_No_of_Bolt,
                Bolts_Dia_of_Bolt = dto.IFI_Bagfilter_Database_Without_Canopy.Bolts_Dia_of_Bolt,
                Bolts_Grade_of_Bolt = dto.IFI_Bagfilter_Database_Without_Canopy.Bolts_Grade_of_Bolt,
                Bolts_Sleeve_Size_mm = dto.IFI_Bagfilter_Database_Without_Canopy.Bolts_Sleeve_Size_mm,
                Bolts_Embedded_Length_mm = dto.IFI_Bagfilter_Database_Without_Canopy.Bolts_Embedded_Length_mm,
                Bolt_CC_Distance_Confirguration_RCC = dto.IFI_Bagfilter_Database_Without_Canopy.Bolt_CC_Distance_Confirguration_RCC,
                Bolt_CC_Distance_Confirguration_Steel = dto.IFI_Bagfilter_Database_Without_Canopy.Bolt_CC_Distance_Confirguration_Steel,
                Base_Plate_Dimension_RCC = dto.IFI_Bagfilter_Database_Without_Canopy.Base_Plate_Dimension_RCC,
                Base_Plate_Dimension_Steel = dto.IFI_Bagfilter_Database_Without_Canopy.Base_Plate_Dimension_Steel,
                Weight_of_Base_Plate_kg = dto.IFI_Bagfilter_Database_Without_Canopy.Weight_of_Base_Plate_kg,
                Total_Weight_of_Structure_kg = dto.IFI_Bagfilter_Database_Without_Canopy.Total_Weight_of_Structure_kg,
            };
        }
    }
}

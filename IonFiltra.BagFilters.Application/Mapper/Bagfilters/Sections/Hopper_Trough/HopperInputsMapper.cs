using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Hopper_Trough;

namespace IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Hopper_Trough
{
    public static class HopperInputsMapper
    {
        public static HopperInputsMainDto ToMainDto(HopperInputs entity)
        {
            if (entity == null) return null;
            return new HopperInputsMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                HopperInputs = new HopperInputsDto
                {
                    Hopper_Type = entity.Hopper_Type,
                    Process_Compartments = entity.Process_Compartments,
                    Tot_No_Of_Hoppers = entity.Tot_No_Of_Hoppers,
                    Tot_No_Of_Trough = entity.Tot_No_Of_Trough,
                    Plenum_Width = entity.Plenum_Width,
                    Inlet_Height = entity.Inlet_Height,
                    Hopper_Thickness = entity.Hopper_Thickness,
                    Hopper_Valley_Angle = entity.Hopper_Valley_Angle,
                    Access_Door_Type = entity.Access_Door_Type,
                    Access_Door_Qty = entity.Access_Door_Qty,
                    Rav_Maintainence_Pltform = entity.Rav_Maintainence_Pltform,
                    Hopper_Access_Stool = entity.Hopper_Access_Stool,
                    Is_Distance_Piece = entity.Is_Distance_Piece,
                    Distance_Piece_Height = entity.Distance_Piece_Height,
                    Stiffening_Factor = entity.Stiffening_Factor,
                    Hopper = entity.Hopper,
                    Discharge_Opening_Sqr = entity.Discharge_Opening_Sqr,
                    Material_Handling = entity.Material_Handling,
                    Material_Handling_Qty = entity.Material_Handling_Qty,
                    Trough_Outlet_Length = entity.Trough_Outlet_Length,
                    Trough_Outlet_Width = entity.Trough_Outlet_Width,
                    Material_Handling_Xxx = entity.Material_Handling_Xxx,
                    Hor_Diff_Length = entity.Hor_Diff_Length,
                    Hor_Diff_Width = entity.Hor_Diff_Width,
                    Slant_Offset_Dist = entity.Slant_Offset_Dist,
                    Hopper_Height = entity.Hopper_Height,
                    Hopper_Height_Mm = entity.Hopper_Height_Mm,
                    Slanting_Hopper_Height = entity.Slanting_Hopper_Height,
                    Hopper_Area_Length = entity.Hopper_Area_Length,
                    Hopper_Area_Width = entity.Hopper_Area_Width,
                    Hopper_Tot_Area = entity.Hopper_Tot_Area,
                    Hopper_Weight = entity.Hopper_Weight,
                },

            };
        }

        public static HopperInputs ToEntity(HopperInputsMainDto dto)
        {
            if (dto == null) return null;
            return new HopperInputs
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Hopper_Type = dto.HopperInputs.Hopper_Type,
                Process_Compartments = dto.HopperInputs.Process_Compartments,
                Tot_No_Of_Hoppers = dto.HopperInputs.Tot_No_Of_Hoppers,
                Tot_No_Of_Trough = dto.HopperInputs.Tot_No_Of_Trough,
                Plenum_Width = dto.HopperInputs.Plenum_Width,
                Inlet_Height = dto.HopperInputs.Inlet_Height,
                Hopper_Thickness = dto.HopperInputs.Hopper_Thickness,
                Hopper_Valley_Angle = dto.HopperInputs.Hopper_Valley_Angle,
                Access_Door_Type = dto.HopperInputs.Access_Door_Type,
                Access_Door_Qty = dto.HopperInputs.Access_Door_Qty,
                Rav_Maintainence_Pltform = dto.HopperInputs.Rav_Maintainence_Pltform,
                Hopper_Access_Stool = dto.HopperInputs.Hopper_Access_Stool,
                Is_Distance_Piece = dto.HopperInputs.Is_Distance_Piece,
                Distance_Piece_Height = dto.HopperInputs.Distance_Piece_Height,
                Stiffening_Factor = dto.HopperInputs.Stiffening_Factor,
                Hopper = dto.HopperInputs.Hopper,
                Discharge_Opening_Sqr = dto.HopperInputs.Discharge_Opening_Sqr,
                Material_Handling = dto.HopperInputs.Material_Handling,
                Material_Handling_Qty = dto.HopperInputs.Material_Handling_Qty,
                Trough_Outlet_Length = dto.HopperInputs.Trough_Outlet_Length,
                Trough_Outlet_Width = dto.HopperInputs.Trough_Outlet_Width,
                Material_Handling_Xxx = dto.HopperInputs.Material_Handling_Xxx,
                Hor_Diff_Length = dto.HopperInputs.Hor_Diff_Length,
                Hor_Diff_Width = dto.HopperInputs.Hor_Diff_Width,
                Slant_Offset_Dist = dto.HopperInputs.Slant_Offset_Dist,
                Hopper_Height = dto.HopperInputs.Hopper_Height,
                Hopper_Height_Mm = dto.HopperInputs.Hopper_Height_Mm,
                Slanting_Hopper_Height = dto.HopperInputs.Slanting_Hopper_Height,
                Hopper_Area_Length = dto.HopperInputs.Hopper_Area_Length,
                Hopper_Area_Width = dto.HopperInputs.Hopper_Area_Width,
                Hopper_Tot_Area = dto.HopperInputs.Hopper_Tot_Area,
                Hopper_Weight = dto.HopperInputs.Hopper_Weight,

            };
        }
    }
}

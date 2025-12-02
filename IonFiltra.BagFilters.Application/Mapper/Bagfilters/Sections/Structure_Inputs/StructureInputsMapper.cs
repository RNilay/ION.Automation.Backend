using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Structure_Inputs;

namespace IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Structure_Inputs
{
    public static class StructureInputsMapper
    {
        public static StructureInputsMainDto ToMainDto(StructureInputs entity)
        {
            if (entity == null) return null;
            return new StructureInputsMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                StructureInputs = new StructureInputsDto
                {
                    Gas_Entry = entity.Gas_Entry,
                    Support_Structure_Type = entity.Support_Structure_Type,
                   
                    Nominal_Width = entity.Nominal_Width,
                    Max_Bags_And_Pitch = entity.Max_Bags_And_Pitch,
                    Nominal_Width_Meters = entity.Nominal_Width_Meters,
                    Nominal_Length = entity.Nominal_Length,
                    Nominal_Length_Meters = entity.Nominal_Length_Meters,
                    Area_Adjust_Can_Vel = entity.Area_Adjust_Can_Vel,
                    Can_Area_Req = entity.Can_Area_Req,
                    Total_Avl_Area = entity.Total_Avl_Area,
                    Length_Correction = entity.Length_Correction,
                    Length_Correction_Derived = entity.Length_Correction_Derived,
                    Actual_Length = entity.Actual_Length,
                    Actual_Length_Meters = entity.Actual_Length_Meters,
                    Ol_Flange_Length = entity.Ol_Flange_Length,
                    Ol_Flange_Length_Mm = entity.Ol_Flange_Length_Mm,
                },

            };
        }

        public static StructureInputs ToEntity(StructureInputsMainDto dto)
        {
            if (dto == null) return null;
            return new StructureInputs
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Gas_Entry = dto.StructureInputs.Gas_Entry,
                Support_Structure_Type = dto.StructureInputs.Support_Structure_Type,
               
                Nominal_Width = dto.StructureInputs.Nominal_Width,
                Max_Bags_And_Pitch = dto.StructureInputs.Max_Bags_And_Pitch,
                Nominal_Width_Meters = dto.StructureInputs.Nominal_Width_Meters,
                Nominal_Length = dto.StructureInputs.Nominal_Length,
                Nominal_Length_Meters = dto.StructureInputs.Nominal_Length_Meters,
                Area_Adjust_Can_Vel = dto.StructureInputs.Area_Adjust_Can_Vel,
                Can_Area_Req = dto.StructureInputs.Can_Area_Req,
                Total_Avl_Area = dto.StructureInputs.Total_Avl_Area,
                Length_Correction = dto.StructureInputs.Length_Correction,
                Length_Correction_Derived = dto.StructureInputs.Length_Correction_Derived,
                Actual_Length = dto.StructureInputs.Actual_Length,
                Actual_Length_Meters = dto.StructureInputs.Actual_Length_Meters,
                Ol_Flange_Length = dto.StructureInputs.Ol_Flange_Length,
                Ol_Flange_Length_Mm = dto.StructureInputs.Ol_Flange_Length_Mm,

            };
        }
    }
}

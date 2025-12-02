using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Capsule_Inputs;

namespace IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Capsule_Inputs
{
    public static class CapsuleInputsMapper
    {
        public static CapsuleInputsMainDto ToMainDto(CapsuleInputs entity)
        {
            if (entity == null) return null;
            return new CapsuleInputsMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                CapsuleInputs = new CapsuleInputsDto
                {
                    Valve_Size = entity.Valve_Size,
                    Voltage_Rating = entity.Voltage_Rating,
                  
                    Capsule_Height = entity.Capsule_Height,
                    Tube_Sheet_Thickness = entity.Tube_Sheet_Thickness,
                    Capsule_Wall_Thickness = entity.Capsule_Wall_Thickness,
                    Canopy = entity.Canopy,
                    Solenoid_Valve_Maintainence = entity.Solenoid_Valve_Maintainence,
                    Capsule_Area = entity.Capsule_Area,
                    Capsule_Weight = entity.Capsule_Weight,
                    Tubesheet_Area = entity.Tubesheet_Area,
                    Tubesheet_Weight = entity.Tubesheet_Weight,
                },

            };
        }

        public static CapsuleInputs ToEntity(CapsuleInputsMainDto dto)
        {
            if (dto == null) return null;
            return new CapsuleInputs
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Valve_Size = dto.CapsuleInputs.Valve_Size,
                Voltage_Rating = dto.CapsuleInputs.Voltage_Rating,
              
                Capsule_Height = dto.CapsuleInputs.Capsule_Height,
                Tube_Sheet_Thickness = dto.CapsuleInputs.Tube_Sheet_Thickness,
                Capsule_Wall_Thickness = dto.CapsuleInputs.Capsule_Wall_Thickness,
                Canopy = dto.CapsuleInputs.Canopy,
                Solenoid_Valve_Maintainence = dto.CapsuleInputs.Solenoid_Valve_Maintainence,
                Capsule_Area = dto.CapsuleInputs.Capsule_Area,
                Capsule_Weight = dto.CapsuleInputs.Capsule_Weight,
                Tubesheet_Area = dto.CapsuleInputs.Tubesheet_Area,
                Tubesheet_Weight = dto.CapsuleInputs.Tubesheet_Weight,

            };
        }
    }
}

using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Roof_Door;

namespace IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Roof_Door
{
    public static class RoofDoorMapper
    {
        public static RoofDoorMainDto ToMainDto(RoofDoor entity)
        {
            if (entity == null) return null;
            return new RoofDoorMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                RoofDoor = new RoofDoorDto
                {
                    Roof_Door_Thickness = entity.Roof_Door_Thickness,
                    T2d = entity.T2d,
                    T3d = entity.T3d,
                    N_Doors = entity.N_Doors,
                    Compartment_No = entity.Compartment_No,
                    Stiffness_Factor_For_Roof_Door = entity.Stiffness_Factor_For_Roof_Door,
                    Weight_Per_Door = entity.Weight_Per_Door,
                    Tot_Weight_Per_Compartment = entity.Tot_Weight_Per_Compartment,
                },

            };
        }

        public static RoofDoor ToEntity(RoofDoorMainDto dto)
        {
            if (dto == null) return null;
            return new RoofDoor
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Roof_Door_Thickness = dto.RoofDoor.Roof_Door_Thickness,
                T2d = dto.RoofDoor.T2d,
                T3d = dto.RoofDoor.T3d,
                N_Doors = dto.RoofDoor.N_Doors,
                Compartment_No = dto.RoofDoor.Compartment_No,
                Stiffness_Factor_For_Roof_Door = dto.RoofDoor.Stiffness_Factor_For_Roof_Door,
                Weight_Per_Door = dto.RoofDoor.Weight_Per_Door,
                Tot_Weight_Per_Compartment = dto.RoofDoor.Tot_Weight_Per_Compartment,

            };
        }
    }
}

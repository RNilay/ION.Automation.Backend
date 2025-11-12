using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Access_Group;

namespace IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Access_Group
{
    public static class AccessGroupMapper
    {
        public static AccessGroupMainDto ToMainDto(AccessGroup entity)
        {
            if (entity == null) return null;
            return new AccessGroupMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                AccessGroup = new AccessGroupDto
                {
                    Access_Type = entity.Access_Type,
                    Cage_Weight_Ladder = entity.Cage_Weight_Ladder,
                    Mid_Landing_Pltform = entity.Mid_Landing_Pltform,
                    Platform_Weight = entity.Platform_Weight,
                    Staircase_Height = entity.Staircase_Height,
                    Staircase_Weight = entity.Staircase_Weight,
                    Railing_Weight = entity.Railing_Weight,
                    Maintainence_Pltform = entity.Maintainence_Pltform,
                    Maintainence_Pltform_Weight = entity.Maintainence_Pltform_Weight,
                    BlowPipe = entity.BlowPipe,
                    PressureHeader = entity.PressureHeader,
                    DistancePiece = entity.DistancePiece,
                    Access_Stool_Size_Mm = entity.Access_Stool_Size_Mm,
                    Access_Stool_Size_Kg = entity.Access_Stool_Size_Kg,
                },

            };
        }

        public static AccessGroup ToEntity(AccessGroupMainDto dto)
        {
            if (dto == null) return null;
            return new AccessGroup
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Access_Type = dto.AccessGroup.Access_Type,
                Cage_Weight_Ladder = dto.AccessGroup.Cage_Weight_Ladder,
                Mid_Landing_Pltform = dto.AccessGroup.Mid_Landing_Pltform,
                Platform_Weight = dto.AccessGroup.Platform_Weight,
                Staircase_Height = dto.AccessGroup.Staircase_Height,
                Staircase_Weight = dto.AccessGroup.Staircase_Weight,
                Railing_Weight = dto.AccessGroup.Railing_Weight,
                Maintainence_Pltform = dto.AccessGroup.Maintainence_Pltform,
                Maintainence_Pltform_Weight = dto.AccessGroup.Maintainence_Pltform_Weight,
                BlowPipe = dto.AccessGroup.BlowPipe,
                PressureHeader = dto.AccessGroup.PressureHeader,
                DistancePiece = dto.AccessGroup.DistancePiece,
                Access_Stool_Size_Mm = dto.AccessGroup.Access_Stool_Size_Mm,
                Access_Stool_Size_Kg = dto.AccessGroup.Access_Stool_Size_Kg,

            };
        }
    }
}

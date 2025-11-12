using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Support_Structure;

namespace IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Support_Structure
{
    public static class SupportStructureMapper
    {
        public static SupportStructureMainDto ToMainDto(SupportStructure entity)
        {
            if (entity == null) return null;
            return new SupportStructureMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                SupportStructure = new SupportStructureDto
                {
                    Support_Struct_Type = entity.Support_Struct_Type,
                    NoOfColumn = entity.NoOfColumn,
                    Column_Height = entity.Column_Height,
                    Ground_Clearance = entity.Ground_Clearance,
                    Dist_Btw_Column_In_X = entity.Dist_Btw_Column_In_X,
                    Dist_Btw_Column_In_Z = entity.Dist_Btw_Column_In_Z,
                    No_Of_Bays_In_X = entity.No_Of_Bays_In_X,
                    No_Of_Bays_In_Z = entity.No_Of_Bays_In_Z,
                },

            };
        }

        public static SupportStructure ToEntity(SupportStructureMainDto dto)
        {
            if (dto == null) return null;
            return new SupportStructure
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Support_Struct_Type = dto.SupportStructure.Support_Struct_Type,
                NoOfColumn = dto.SupportStructure.NoOfColumn,
                Column_Height = dto.SupportStructure.Column_Height,
                Ground_Clearance = dto.SupportStructure.Ground_Clearance,
                Dist_Btw_Column_In_X = dto.SupportStructure.Dist_Btw_Column_In_X,
                Dist_Btw_Column_In_Z = dto.SupportStructure.Dist_Btw_Column_In_Z,
                No_Of_Bays_In_X = dto.SupportStructure.No_Of_Bays_In_X,
                No_Of_Bays_In_Z = dto.SupportStructure.No_Of_Bays_In_Z,
            };
        }
    }
}

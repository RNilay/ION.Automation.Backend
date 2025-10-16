using IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterMaster;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterMasterEntity;

namespace IonFiltra.BagFilters.Application.Mapper.Bagfilters.BagfilterMasters
{
    public static class BagfilterMasterMapper
    {
        public static BagfilterMasterMainDto ToMainDto(BagfilterMaster entity)
        {
            if (entity == null) return null;
            return new BagfilterMasterMainDto
            {
                BagfilterMasterId = entity.BagfilterMasterId,
                BagfilterMaster = new BagfilterMasterDto
                {
                    AssignmentId = entity.AssignmentId,
                    EnquiryId = entity.EnquiryId,
                    BagFilterName = entity.BagFilterName,
                    Status = entity.Status,
                    Revision = entity.Revision,
                },

            };
        }

        public static BagfilterMaster ToEntity(BagfilterMasterMainDto dto)
        {
            if (dto == null) return null;
            return new BagfilterMaster
            {
                BagfilterMasterId = dto.BagfilterMasterId,
                AssignmentId = dto.BagfilterMaster.AssignmentId,
                EnquiryId = dto.BagfilterMaster.EnquiryId,
                BagFilterName = dto.BagfilterMaster.BagFilterName,
                Status = dto.BagfilterMaster.Status,
                Revision = dto.BagfilterMaster.Revision,

            };
        }
    }
}

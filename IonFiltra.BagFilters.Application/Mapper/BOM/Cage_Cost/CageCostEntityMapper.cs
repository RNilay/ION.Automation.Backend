using IonFiltra.BagFilters.Application.DTOs.BOM.Cage_Cost;
using IonFiltra.BagFilters.Core.Entities.BOM.Cage_Cost;

namespace IonFiltra.BagFilters.Application.Mappers.BOM.Cage_Cost
{
    public static class CageCostEntityMapper
    {
        public static CageCostMainDto ToMainDto(CageCostEntity entity)
        {
            if (entity == null) return null;
            return new CageCostMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                            CageCostEntity = new CageCostEntityDto {
                    Parameter = entity.Parameter,
                    Value = entity.Value,
                    Unit = entity.Unit,
                },

            };
        }

        public static CageCostEntity ToEntity(CageCostMainDto dto)
        {
            if (dto == null) return null;
            return new CageCostEntity
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Parameter = dto.CageCostEntity.Parameter,
                Value = dto.CageCostEntity.Value,
                Unit = dto.CageCostEntity.Unit,

             };
        }
    }
}
    
using IonFiltra.BagFilters.Application.DTOs.BOM.Damper_Cost;
using IonFiltra.BagFilters.Core.Entities.BOM.Damper_Cost;

namespace IonFiltra.BagFilters.Application.Mappers.BOM.Damper_Cost
{
    public static class DamperCostEntityMapper
    {
        public static DamperCostMainDto ToMainDto(DamperCostEntity entity)
        {
            if (entity == null) return null;
            return new DamperCostMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                            DamperCostEntity = new DamperCostEntityDto {
                    Parameter = entity.Parameter,
                    Value = entity.Value,
                    Unit = entity.Unit,
                },

            };
        }

        public static DamperCostEntity ToEntity(DamperCostMainDto dto)
        {
            if (dto == null) return null;
            return new DamperCostEntity
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Parameter = dto.DamperCostEntity.Parameter,
                Value = dto.DamperCostEntity.Value,
                Unit = dto.DamperCostEntity.Unit,

             };
        }
    }
}
    
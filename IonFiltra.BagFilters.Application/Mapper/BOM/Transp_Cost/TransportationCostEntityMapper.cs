using IonFiltra.BagFilters.Application.DTOs.BOM.Transp_Cost;
using IonFiltra.BagFilters.Core.Entities.BOM.Transp_Cost;

namespace IonFiltra.BagFilters.Application.Mappers.BOM.Transp_Cost
{
    public static class TransportationCostEntityMapper
    {
        public static TransportationCostMainDto ToMainDto(TransportationCostEntity entity)
        {
            if (entity == null) return null;
            return new TransportationCostMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                            TransportationCostEntity = new TransportationCostEntityDto {
                    Parameter = entity.Parameter,
                    Value = entity.Value,
                    Unit = entity.Unit,
                },

            };
        }

        public static TransportationCostEntity ToEntity(TransportationCostMainDto dto)
        {
            if (dto == null) return null;
            return new TransportationCostEntity
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Parameter = dto.TransportationCostEntity.Parameter,
                Value = dto.TransportationCostEntity.Value,
                Unit = dto.TransportationCostEntity.Unit,

             };
        }
    }
}
    
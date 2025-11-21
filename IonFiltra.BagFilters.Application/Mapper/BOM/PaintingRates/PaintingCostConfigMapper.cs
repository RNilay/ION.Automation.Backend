using IonFiltra.BagFilters.Application.DTOs.BOM.PaintingRates;
using IonFiltra.BagFilters.Core.Entities.BOM.PaintingRates;

namespace IonFiltra.BagFilters.Application.Mappers.BOM.PaintingRates
{
    public static class PaintingCostConfigMapper
    {
        public static PaintingCostConfigMainDto ToMainDto(PaintingCostConfig entity)
        {
            if (entity == null) return null;
            return new PaintingCostConfigMainDto
            {
                Id = entity.Id,
                
                PaintingCostConfig = new PaintingCostConfigDto
                {
                    Code = entity.Code,
                    Section = entity.Section,
                    Item = entity.Item,
                    InrPerLtr = entity.InrPerLtr,
                    SqmPerLtr = entity.SqmPerLtr,
                    Coats = entity.Coats,
                    LabourRate = entity.LabourRate,
                },

            };
        }

        public static PaintingCostConfig ToEntity(PaintingCostConfigMainDto dto)
        {
            if (dto == null) return null;
            return new PaintingCostConfig
            {
                Id = dto.Id,
                Code = dto.PaintingCostConfig.Code,
                Section = dto.PaintingCostConfig.Section,
                Item = dto.PaintingCostConfig.Item,
                InrPerLtr = dto.PaintingCostConfig.InrPerLtr,
                SqmPerLtr = dto.PaintingCostConfig.SqmPerLtr,
                Coats = dto.PaintingCostConfig.Coats,
                LabourRate = dto.PaintingCostConfig.LabourRate,

            };
        }
    }
}

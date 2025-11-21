using IonFiltra.BagFilters.Application.DTOs.BOM.Painting_Cost;
using IonFiltra.BagFilters.Core.Entities.BOM.Painting_Cost;

namespace IonFiltra.BagFilters.Application.Mappers.BOM.Painting_Cost
{
    public static class PaintingCostMapper
    {
        public static PaintingCostMainDto ToMainDto(PaintingCost entity)
        {
            if (entity == null) return null;
            return new PaintingCostMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                PaintingCost = new PaintingCostDto
                {
                    PaintingTableJson = entity.PaintingTableJson,
                },

            };
        }

        public static PaintingCost ToEntity(PaintingCostMainDto dto)
        {
            if (dto == null) return null;
            return new PaintingCost
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                PaintingTableJson = dto.PaintingCost.PaintingTableJson,

            };
        }
    }
}

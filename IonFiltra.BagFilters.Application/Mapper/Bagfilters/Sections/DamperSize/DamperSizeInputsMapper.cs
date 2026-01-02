using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.DamperSize;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.DamperSize;

namespace IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.DamperSize
{
    public static class DamperSizeInputsMapper
    {
        public static DamperSizeInputsMainDto ToMainDto(DamperSizeInputs entity)
        {
            if (entity == null) return null;
            return new DamperSizeInputsMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                DamperSizeInputs = new DamperSizeInputsDto
                {
                    Is_Damper_Required = entity.Is_Damper_Required,
                    Damper_Series = entity.Damper_Series,
                    Damper_Diameter = entity.Damper_Diameter,
                    Damper_Qty = entity.Damper_Qty,
                },

            };
        }

        public static DamperSizeInputs ToEntity(DamperSizeInputsMainDto dto)
        {
            if (dto == null) return null;
            return new DamperSizeInputs
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Is_Damper_Required = dto.DamperSizeInputs.Is_Damper_Required,
                Damper_Series = dto.DamperSizeInputs.Damper_Series,
                Damper_Diameter = dto.DamperSizeInputs.Damper_Diameter,
                Damper_Qty = dto.DamperSizeInputs.Damper_Qty,

            };
        }
    }
}

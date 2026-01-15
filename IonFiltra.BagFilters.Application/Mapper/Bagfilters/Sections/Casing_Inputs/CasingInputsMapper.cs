using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Casing_Inputs;

namespace IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Casing_Inputs
{
    public static class CasingInputsMapper
    {
        public static CasingInputsMainDto ToMainDto(CasingInputs entity)
        {
            if (entity == null) return null;
            return new CasingInputsMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                CasingInputs = new CasingInputsDto
                {
                    Casing_Wall_Thickness = entity.Casing_Wall_Thickness,
                    Stiffening_Factor_Casing = entity.Stiffening_Factor_Casing,
                    Casing_Height = entity.Casing_Height,
                    Casing_Area = entity.Casing_Area,
                    Casing_Weight = entity.Casing_Weight,
                },

            };
        }

        public static CasingInputs ToEntity(CasingInputsMainDto dto)
        {
            if (dto == null) return null;
            return new CasingInputs
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Casing_Wall_Thickness = dto.CasingInputs.Casing_Wall_Thickness,
                Stiffening_Factor_Casing = dto.CasingInputs.Stiffening_Factor_Casing,
                Casing_Height = dto.CasingInputs.Casing_Height,
                Casing_Area = dto.CasingInputs.Casing_Area,
                Casing_Weight = dto.CasingInputs.Casing_Weight,

            };
        }
    }
}

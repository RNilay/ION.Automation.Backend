using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Cage_Inputs;

namespace IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Cage_Inputs
{
    public static class CageInputsMapper
    {
        public static CageInputsMainDto ToMainDto(CageInputs entity)
        {
            if (entity == null) return null;
            return new CageInputsMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                CageInputs = new CageInputsDto
                {
                    Cage_Wire_Dia = entity.Cage_Wire_Dia,
                    No_Of_Cage_Wires = entity.No_Of_Cage_Wires,
                    Ring_Spacing = entity.Ring_Spacing,
                },

            };
        }

        public static CageInputs ToEntity(CageInputsMainDto dto)
        {
            if (dto == null) return null;
            return new CageInputs
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Cage_Wire_Dia = dto.CageInputs.Cage_Wire_Dia,
                No_Of_Cage_Wires = dto.CageInputs.No_Of_Cage_Wires,
                Ring_Spacing = dto.CageInputs.Ring_Spacing,

            };
        }
    }
}

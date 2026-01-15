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
                    Cage_Type = entity.Cage_Type,
                    Cage_Sub_Type = entity.Cage_Sub_Type,
                    Cage_Material = entity.Cage_Material,
                    Cage_Wire_Dia = entity.Cage_Wire_Dia,
                    No_Of_Cage_Wires = entity.No_Of_Cage_Wires,
                    Ring_Spacing = entity.Ring_Spacing,
                    Cage_Diameter = entity.Cage_Diameter,
                    Cage_Length = entity.Cage_Length,
                    Spare_Cages = entity.Spare_Cages,
                    Cage_Configuration = entity.Cage_Configuration,
                    No_Of_Rings = entity.No_Of_Rings,
                    Tot_Wire_Length = entity.Tot_Wire_Length,
                    Weight_Of_Cage_Wires = entity.Weight_Of_Cage_Wires,
                    Weight_Of_Cage_Rings = entity.Weight_Of_Cage_Rings,
                    Weight_Of_One_Cage = entity.Weight_Of_One_Cage,
                    Cage_Weight = entity.Cage_Weight,
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
                Cage_Type = dto.CageInputs.Cage_Type,
                Cage_Sub_Type = dto.CageInputs.Cage_Sub_Type,
                Cage_Material = dto.CageInputs.Cage_Material,
                Cage_Wire_Dia = dto.CageInputs.Cage_Wire_Dia,
                No_Of_Cage_Wires = dto.CageInputs.No_Of_Cage_Wires,
                Ring_Spacing = dto.CageInputs.Ring_Spacing,
                Cage_Diameter = dto.CageInputs.Cage_Diameter,
                Cage_Length = dto.CageInputs.Cage_Length,
                Spare_Cages = dto.CageInputs.Spare_Cages,
                Cage_Configuration = dto.CageInputs.Cage_Configuration,
                No_Of_Rings = dto.CageInputs.No_Of_Rings,
                Tot_Wire_Length = dto.CageInputs.Tot_Wire_Length,
                Weight_Of_Cage_Wires = dto.CageInputs.Weight_Of_Cage_Wires,
                Weight_Of_Cage_Rings = dto.CageInputs.Weight_Of_Cage_Rings,
                Weight_Of_One_Cage = dto.CageInputs.Weight_Of_One_Cage,
                Cage_Weight = dto.CageInputs.Cage_Weight,
            };
        }
    }
}

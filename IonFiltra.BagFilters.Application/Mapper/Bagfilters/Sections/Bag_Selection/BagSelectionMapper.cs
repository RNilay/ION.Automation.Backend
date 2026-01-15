using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Bag_Selection;

namespace IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Bag_Selection
{
    public static class BagSelectionMapper
    {
        public static BagSelectionMainDto ToMainDto(BagSelection entity)
        {
            if (entity == null) return null;
            return new BagSelectionMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                BagSelection = new BagSelectionDto
                {
                    Filter_Bag_Dia = entity.Filter_Bag_Dia,
                    Fil_Bag_Length = entity.Fil_Bag_Length,
                    ClothAreaPerBag = entity.ClothAreaPerBag,
                    //noOfBags = entity.noOfBags,
                    Fil_Bag_Recommendation = entity.Fil_Bag_Recommendation,
                    Bag_Per_Row = entity.Bag_Per_Row,
                    Number_Of_Rows = entity.Number_Of_Rows,
                    Actual_Bag_Req = entity.Actual_Bag_Req,
                    Wire_Cross_Sec_Area = entity.Wire_Cross_Sec_Area,
                    //No_Of_Rings = entity.No_Of_Rings,
                    //Tot_Wire_Length = entity.Tot_Wire_Length,
                    //Cage_Weight = entity.Cage_Weight,
                },

            };
        }

        public static BagSelection ToEntity(BagSelectionMainDto dto)
        {
            if (dto == null) return null;
            return new BagSelection
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Filter_Bag_Dia = dto.BagSelection.Filter_Bag_Dia,
                Fil_Bag_Length = dto.BagSelection.Fil_Bag_Length,
                ClothAreaPerBag = dto.BagSelection.ClothAreaPerBag,
                //noOfBags = dto.BagSelection.noOfBags,
                Fil_Bag_Recommendation = dto.BagSelection.Fil_Bag_Recommendation,
                Bag_Per_Row = dto.BagSelection.Bag_Per_Row,
                Number_Of_Rows = dto.BagSelection.Number_Of_Rows,
                Actual_Bag_Req = dto.BagSelection.Actual_Bag_Req,
                Wire_Cross_Sec_Area = dto.BagSelection.Wire_Cross_Sec_Area,
                //No_Of_Rings = dto.BagSelection.No_Of_Rings,
                //Tot_Wire_Length = dto.BagSelection.Tot_Wire_Length,
                //Cage_Weight = dto.BagSelection.Cage_Weight,

            };
        }
    }
}

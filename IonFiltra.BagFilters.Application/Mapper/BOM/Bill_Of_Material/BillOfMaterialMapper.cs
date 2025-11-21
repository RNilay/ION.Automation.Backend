using IonFiltra.BagFilters.Application.DTOs.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Core.Entities.BOM.Bill_Of_Material;

namespace IonFiltra.BagFilters.Application.Mappers.BOM.Bill_Of_Material
{
    public static class BillOfMaterialMapper
    {
        public static BillOfMaterialMainDto ToMainDto(BillOfMaterial entity)
        {
            if (entity == null) return null;
            return new BillOfMaterialMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                BillOfMaterial = new BillOfMaterialDto
                {
                    Item = entity.Item,
                    Material = entity.Material,
                    Weight = entity.Weight,
                    Units = entity.Units,
                    Rate = entity.Rate,
                    Cost = entity.Cost,
                    SortOrder = entity.SortOrder,
                },

            };
        }

        public static BillOfMaterial ToEntity(BillOfMaterialMainDto dto)
        {
            if (dto == null) return null;
            return new BillOfMaterial
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Item = dto.BillOfMaterial.Item,
                Material = dto.BillOfMaterial.Material,
                Weight = dto.BillOfMaterial.Weight,
                Units = dto.BillOfMaterial.Units,
                Rate = dto.BillOfMaterial.Rate,
                Cost = dto.BillOfMaterial.Cost,
                SortOrder = dto.BillOfMaterial.SortOrder,

            };
        }
    }
}

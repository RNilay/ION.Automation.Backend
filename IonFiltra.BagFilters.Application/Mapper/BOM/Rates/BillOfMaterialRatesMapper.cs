using IonFiltra.BagFilters.Application.DTOs.BOM.Rates;
using IonFiltra.BagFilters.Core.Entities.BOM.Rates;

namespace IonFiltra.BagFilters.Application.Mappers.BOM.Rates
{
    public static class BillOfMaterialRatesMapper
    {
        public static BillOfMaterialRatesMainDto ToMainDto(BillOfMaterialRates entity)
        {
            if (entity == null) return null;
            return new BillOfMaterialRatesMainDto
            {
                Id = entity.Id,
               
                BillOfMaterialRates = new BillOfMaterialRatesDto
                {
                    ItemKey = entity.ItemKey,
                    Rate = entity.Rate,
                    Unit = entity.Unit,
                },

            };
        }

        public static BillOfMaterialRates ToEntity(BillOfMaterialRatesMainDto dto)
        {
            if (dto == null) return null;
            return new BillOfMaterialRates
            {
                Id = dto.Id,
                
                ItemKey = dto.BillOfMaterialRates.ItemKey,
                Rate = dto.BillOfMaterialRates.Rate,
                Unit = dto.BillOfMaterialRates.Unit,

            };
        }
    }
}

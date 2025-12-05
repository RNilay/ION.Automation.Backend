using IonFiltra.BagFilters.Application.DTOs.MasterData.FilterBagData;
using IonFiltra.BagFilters.Core.Entities.MasterData.FilterBagData;

namespace IonFiltra.BagFilters.Application.Mappers.MasterData.FilterBagData
{
    public static class FilterBagMapper
    {
        public static FilterBagMainDto ToMainDto(FilterBag entity)
        {
            if (entity == null) return null;
            return new FilterBagMainDto
            {
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                FilterBag = new FilterBagDto
                {
                    Material = entity.Material,
                    Gsm = entity.Gsm,
                    Size = entity.Size,
                    Make = entity.Make,
                    Cost = entity.Cost,
                    
                },

            };
        }

        public static FilterBag ToEntity(FilterBagMainDto dto)
        {
            if (dto == null) return null;
            return new FilterBag
            {
                Id = dto.Id,
                Material = dto.FilterBag.Material,
                Gsm = dto.FilterBag.Gsm,
                Size = dto.FilterBag.Size,
                Make = dto.FilterBag.Make,
                Cost = dto.FilterBag.Cost,
                IsDeleted = dto.IsDeleted,

            };
        }
    }
}

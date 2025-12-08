using IonFiltra.BagFilters.Application.DTOs.MasterData.DPTData;
using IonFiltra.BagFilters.Core.Entities.MasterData.DPTData;

namespace IonFiltra.BagFilters.Application.Mappers.MasterData.DPTData
{
    public static class DPTEntityMapper
    {
        public static DPTMainDto ToMainDto(DPTEntity entity)
        {
            if (entity == null) return null;
            return new DPTMainDto
            {
                Id = entity.Id,
                            DPTEntity = new DPTEntityDto {
                    Make = entity.Make,
                    Model = entity.Model,
                    Cost = entity.Cost,
                },

            };
        }

        public static DPTEntity ToEntity(DPTMainDto dto)
        {
            if (dto == null) return null;
            return new DPTEntity
            {
                Id = dto.Id,
                                Make = dto.DPTEntity.Make,
                Model = dto.DPTEntity.Model,
                Cost = dto.DPTEntity.Cost,

             };
        }
    }
}
    
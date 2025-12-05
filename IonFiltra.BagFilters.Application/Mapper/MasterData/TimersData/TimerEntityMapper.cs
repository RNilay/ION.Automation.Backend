using IonFiltra.BagFilters.Application.DTOs.MasterData.TimerData;
using IonFiltra.BagFilters.Core.Entities.MasterData.TimerData;

namespace IonFiltra.BagFilters.Application.Mappers.MasterData.TimerData
{
    public static class TimerEntityMapper
    {
        public static TimerEntityMainDto ToMainDto(TimerEntity entity)
        {
            if (entity == null) return null;
            return new TimerEntityMainDto
            {
                Id = entity.Id,
                TimerEntity = new TimerEntityDto
                {
                    Make = entity.Make,
                    Model = entity.Model,
                    Cost = entity.Cost,
                },

            };
        }

        public static TimerEntity ToEntity(TimerEntityMainDto dto)
        {
            if (dto == null) return null;
            return new TimerEntity
            {
                Id = dto.Id,
                Make = dto.TimerEntity.Make,
                Model = dto.TimerEntity.Model,
                Cost = dto.TimerEntity.Cost,

            };
        }
    }
}

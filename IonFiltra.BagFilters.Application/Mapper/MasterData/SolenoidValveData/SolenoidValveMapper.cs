using IonFiltra.BagFilters.Application.DTOs.MasterData.SolenoidValveData;
using IonFiltra.BagFilters.Core.Entities.MasterData.SolenoidValveData;

namespace IonFiltra.BagFilters.Application.Mappers.MasterData.SolenoidValveData
{
    public static class SolenoidValveMapper
    {
        public static SolenoidValveMainDto ToMainDto(SolenoidValve entity)
        {
            if (entity == null) return null;
            return new SolenoidValveMainDto
            {
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                SolenoidValve = new SolenoidValveDto
                {
                    Make = entity.Make,
                    Size = entity.Size,
                    Model = entity.Model,
                    Cost = entity.Cost,
                },

            };
        }

        public static SolenoidValve ToEntity(SolenoidValveMainDto dto)
        {
            if (dto == null) return null;
            return new SolenoidValve
            {
                Id = dto.Id,
                IsDeleted = dto.IsDeleted,
                Make = dto.SolenoidValve.Make,
                Size = dto.SolenoidValve.Size,
                Model = dto.SolenoidValve.Model,
                Cost = dto.SolenoidValve.Cost,


            };
        }
    }
}

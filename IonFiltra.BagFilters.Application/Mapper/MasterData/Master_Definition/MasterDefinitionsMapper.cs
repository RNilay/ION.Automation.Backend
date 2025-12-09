using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.MasterData.Master_Definition;
using IonFiltra.BagFilters.Core.Entities.MasterData.Master_Definition;

namespace IonFiltra.BagFilters.Application.Mapper.MasterData.Master_Definition
{
    public static class MasterDefinitionsMapper
    {
        public static MasterDefinitionsDto ToMainDto(MasterDefinitions entity)
        {
            if (entity == null) return null;
            return new MasterDefinitionsDto
            {
                Id = entity.Id,
                MasterKey = entity.MasterKey,
                DisplayName = entity.DisplayName,
                ApiRoute = entity.ApiRoute,
                SectionOrder = entity.SectionOrder,
                IsActive = entity.IsActive,
                ColumnsJson = entity.ColumnsJson,
            };
        }

        public static MasterDefinitions ToEntity(MasterDefinitionsDto dto)
        {
            if (dto == null) return null;
            return new MasterDefinitions
            {
                Id = dto.Id,
                MasterKey = dto.MasterKey,
                DisplayName = dto.DisplayName,
                ApiRoute = dto.ApiRoute,
                SectionOrder = dto.SectionOrder,
                IsActive = dto.IsActive,
                ColumnsJson = dto.ColumnsJson,
            };
        }
    }
}

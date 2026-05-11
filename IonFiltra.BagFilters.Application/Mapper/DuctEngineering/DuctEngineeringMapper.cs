using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.DuctEngineering;
using IonFiltra.BagFilters.Core.Entities.DuctEngineering;

namespace IonFiltra.BagFilters.Application.Mapper.DuctEngineering
{
    public static class DuctEngineeringMapper
    {
        public static EnquiryDuctEngineering ToEntity(SaveDuctEngineeringRequestDto dto) =>
            new()
            {
                EnquiryId = dto.EnquiryId,
                IsDuctEngineering = dto.IsDuctEngineering,
                Cost = dto.Cost,
                IsDeleted = false
            };

        public static DuctEngineeringResponseDto ToResponseDto(EnquiryDuctEngineering entity) =>
            new()
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                IsDuctEngineering = entity.IsDuctEngineering,
                Cost = entity.Cost
            };
    }
}

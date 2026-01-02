using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.EV;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.EV;

namespace IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.EV
{
    public static class ExplosionVentEntityMapper
    {
        public static ExplosionVentEntityMainDto ToMainDto(ExplosionVentEntity entity)
        {
            if (entity == null) return null;
            return new ExplosionVentEntityMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                ExplosionVentEntity = new ExplosionVentEntityDto
                {
                    Explosion_Vent_Design_Pressure = entity.Explosion_Vent_Design_Pressure,
                    Explosion_Vent_Quantity = entity.Explosion_Vent_Quantity,
                    Explosion_Vent_Size = entity.Explosion_Vent_Size,
                },

            };
        }

        public static ExplosionVentEntity ToEntity(ExplosionVentEntityMainDto dto)
        {
            if (dto == null) return null;
            return new ExplosionVentEntity
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                Explosion_Vent_Design_Pressure = dto.ExplosionVentEntity.Explosion_Vent_Design_Pressure,
                Explosion_Vent_Quantity = dto.ExplosionVentEntity.Explosion_Vent_Quantity,
                Explosion_Vent_Size = dto.ExplosionVentEntity.Explosion_Vent_Size,

            };
        }
    }
}

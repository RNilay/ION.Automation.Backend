using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.Supervision_Charges;
using IonFiltra.BagFilters.Core.Entities.Supervision_Charges;

namespace IonFiltra.BagFilters.Application.Mapper.Supervision_Charges
{
    public static class SupervisionChargesMapper
    {
        // ── DTO → Entity (Save / Update) ─────────────────────────────
        public static EnquirySupervisionCharges ToEntity(
            SaveSupervisionChargesRequestDto dto) => new()
            {
                EnquiryId = dto.EnquiryId,
                VisitEngineeringCharges = dto.VisitEngineeringCharges,
                FreeManDays = dto.FreeManDays,
                FreeManDaysRate = dto.FreeManDaysRate,
                FreeManDaysToAndFro = dto.FreeManDaysToAndFro,
                FreeManDaysLodgingBoarding = dto.FreeManDaysLodgingBoarding,
                ChargeableDays = dto.ChargeableDays,
                ChargeableRate = dto.ChargeableRate,
                ChargeableToAndFro = dto.ChargeableToAndFro,
                ChargeableLodgingBoarding = dto.ChargeableLodgingBoarding,
                IsDeleted = false
            };

        // ── Entity → DTO (Get) ────────────────────────────────────────
        public static SupervisionChargesResponseDto ToResponseDto(
            EnquirySupervisionCharges entity) => new()
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                VisitEngineeringCharges = entity.VisitEngineeringCharges,
                FreeManDays = entity.FreeManDays,
                FreeManDaysRate = entity.FreeManDaysRate,
                FreeManDaysToAndFro = entity.FreeManDaysToAndFro,
                FreeManDaysLodgingBoarding = entity.FreeManDaysLodgingBoarding,
                ChargeableDays = entity.ChargeableDays,
                ChargeableRate = entity.ChargeableRate,
                ChargeableToAndFro = entity.ChargeableToAndFro,
                ChargeableLodgingBoarding = entity.ChargeableLodgingBoarding
            };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.Transportation_Settings;
using IonFiltra.BagFilters.Core.Entities.Transportation_Settings;

namespace IonFiltra.BagFilters.Application.Mapper.Transportation_Settings
{
    public static class TransportationSettingsMapper
    {
        // ── DTO → Entity (Save / Update) ─────────────────────────────
        public static EnquiryTransportationSettings ToEntity(
            SaveTransportationSettingsRequestDto dto) => new()
            {
                EnquiryId = dto.EnquiryId,
                DefaultMode = dto.DefaultMode ?? "EX works",
                DapFixedCost = dto.DapFixedCost,
                IsDeleted = false
            };

        // ── Entity → Response DTO (Get) ───────────────────────────────
        public static TransportationSettingsResponseDto ToResponseDto(
            EnquiryTransportationSettings entity) => new()
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                DefaultMode = entity.DefaultMode,
                DapFixedCost = entity.DapFixedCost
            };
    }
}

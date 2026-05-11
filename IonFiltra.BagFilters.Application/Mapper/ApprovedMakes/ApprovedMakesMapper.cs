using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.ApprovedMakes;
using IonFiltra.BagFilters.Core.Entities.ApprovedMakes;

namespace IonFiltra.BagFilters.Application.Mapper.ApprovedMakes
{
    public static class ApprovedMakesMapper
    {
        // ══════════════════════════════════════════════════════════════════════
        //  DTO  →  GRAPH  (used by Save / Update)
        // ══════════════════════════════════════════════════════════════════════

        public static ApprovedMakeGraph ToGraph(SaveApprovedMakesRequestDto dto)
        {
            var header = new EnquiryApprovedMake
            {
                EnquiryId = dto.EnquiryId,
                IsDeleted = false
            };

            // Flatten dictionary → list of item entities.
            // Skip masterKeys that have no approved makes.
            var items = dto.ApprovedMakes
                .Where(kvp => kvp.Value != null && kvp.Value.Count > 0)
                .SelectMany(kvp => kvp.Value
                    .Where(make => !string.IsNullOrWhiteSpace(make))
                    .Select(make => new EnquiryApprovedMakeItem
                    {
                        MasterKey = kvp.Key,
                        MakeValue = make.Trim(),
                        ItemType = dto.ItemTypes.TryGetValue(kvp.Key, out var t)
                                    ? t
                                    : "primary"
                    }))
                .ToList();

            return new ApprovedMakeGraph
            {
                Header = header,
                Items = items
            };
        }

        // ══════════════════════════════════════════════════════════════════════
        //  GRAPH  →  DTO  (used by Get)
        // ══════════════════════════════════════════════════════════════════════

        public static ApprovedMakesResponseDto ToResponseDto(ApprovedMakeGraph graph)
        {
            // Re-group flat items back into { masterKey: [make1, make2, ...] }
            var approvedMakes = graph.Items
                .GroupBy(i => i.MasterKey)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(i => i.MakeValue).ToList()
                );

            // Build per-masterKey type map (take first row since all rows for a key share the same type)
            var itemTypes = graph.Items
                .GroupBy(i => i.MasterKey)
                .ToDictionary(
                    g => g.Key,
                    g => g.First().ItemType
                );

            return new ApprovedMakesResponseDto
            {
                Id = graph.Header.Id,
                EnquiryId = graph.Header.EnquiryId,
                ApprovedMakes = approvedMakes,
                ItemTypes = itemTypes
            };
        }
    }
}

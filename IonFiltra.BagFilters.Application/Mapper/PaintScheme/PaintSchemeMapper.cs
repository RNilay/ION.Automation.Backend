using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.PaintScheme;
using IonFiltra.BagFilters.Core.Entities.PaintScheme;

namespace IonFiltra.BagFilters.Application.Mapper.PaintScheme
{
    public static class PaintSchemeMapper
    {
        // ══════════════════════════════════════════════════════════════════════
        //  DTO  →  ENTITIES  (used by Save / Update)
        // ══════════════════════════════════════════════════════════════════════

        public static PaintSchemeGraph ToGraph(SavePaintSchemeRequestDto dto)
        {
            var header = new EnquiryPaintScheme
            {
                EnquiryId = dto.EnquiryId,
                PaintingSchemeId = dto.PaintingSchemeId,
                IsDeleted = false
            };

            var sections = dto.Sections
                .Select(ToSectionEntity)
                .ToList();

            var assignments = dto.BfAssignments
                .Select(a => new EnquiryPaintSchemeBfAssignment
                {
                    BfName = a.BfName,
                    AssignmentType = a.AssignmentType
                })
                .ToList();

            // Only build overrides for BFs that are marked "custom"
            var customBfNames = dto.BfAssignments
                .Where(a => a.AssignmentType == "custom")
                .Select(a => a.BfName)
                .ToHashSet();

            var overrides = dto.Overrides
                .Where(o => customBfNames.Contains(o.BfName))
                .Select(o => new PaintSchemeOverrideGraph
                {
                    Override = new EnquiryPaintSchemeOverride
                    {
                        PaintingSchemeId = o.PaintingSchemeId,
                        BfName = o.BfName   // [NotMapped] transient — used by repo to link assignment Id
                    },
                    Sections = o.Sections
                        .Select(ToOverrideSectionEntity)
                        .ToList()
                })
                .ToList();

            return new PaintSchemeGraph
            {
                Header = header,
                Sections = sections,
                Assignments = assignments,
                Overrides = overrides
            };
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PaintSchemeGraph  →  DTO  (used by Get)
        // ══════════════════════════════════════════════════════════════════════

        public static PaintSchemeResponseDto ToResponseDto(PaintSchemeGraph graph)
        {
            return new PaintSchemeResponseDto
            {
                Id = graph.Header.Id,
                EnquiryId = graph.Header.EnquiryId,
                PaintingSchemeId = graph.Header.PaintingSchemeId,
                //SchemeName = null, // resolved from master on the frontend
                SchemeName = graph.SchemeName,
                CostPerKg = graph.CostPerKg,

                Sections = graph.Sections
                    .Select(ToSectionDto)
                    .ToList(),

                BfAssignments = graph.Assignments
                    .Select(a => new BfAssignmentDto
                    {
                        BfName = a.BfName,
                        AssignmentType = a.AssignmentType
                    })
                    .ToList(),

                Overrides = graph.Overrides
                    .Select(og =>
                    {
                        // find the parent assignment to resolve BfName on the response
                        var parentAssignment = graph.Assignments
                            .FirstOrDefault(a => a.Id == og.Override.BfAssignmentId);

                        return new BfOverrideResponseDto
                        {
                            OverrideId = og.Override.Id,
                            BfName = parentAssignment?.BfName ?? og.Override.BfName ?? string.Empty,
                            PaintingSchemeId = og.Override.PaintingSchemeId,
                            SchemeName = og.SchemeName,
                            CostPerKg = og.CostPerKg,
                            Sections = og.Sections
                                .Select(ToOverrideSectionDto)
                                .ToList()
                        };
                    })
                    .ToList()
            };
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PRIVATE HELPERS
        // ══════════════════════════════════════════════════════════════════════

        private static EnquiryPaintSchemeSection ToSectionEntity(PaintSchemeSectionDto s) => new()
        {
            SectionKey = s.SectionKey,
            ItemMasterId = s.ItemMasterId,
            ItemModel = s.ItemModel,
            CostPerLiter = s.CostPerLiter,
            NoOfCoats = s.NoOfCoats
        };

        private static EnquiryPaintSchemeOverrideSection ToOverrideSectionEntity(PaintSchemeSectionDto s) => new()
        {
            SectionKey = s.SectionKey,
            ItemMasterId = s.ItemMasterId,
            ItemModel = s.ItemModel,
            CostPerLiter = s.CostPerLiter,
            NoOfCoats = s.NoOfCoats
        };

        private static PaintSchemeSectionDto ToSectionDto(EnquiryPaintSchemeSection s) => new()
        {
            SectionKey = s.SectionKey,
            ItemMasterId = s.ItemMasterId,
            ItemModel = s.ItemModel,
            CostPerLiter = s.CostPerLiter,
            NoOfCoats = s.NoOfCoats
        };

        private static PaintSchemeSectionDto ToOverrideSectionDto(EnquiryPaintSchemeOverrideSection s) => new()
        {
            SectionKey = s.SectionKey,
            ItemMasterId = s.ItemMasterId,
            ItemModel = s.ItemModel,
            CostPerLiter = s.CostPerLiter,
            NoOfCoats = s.NoOfCoats
        };


    }
}

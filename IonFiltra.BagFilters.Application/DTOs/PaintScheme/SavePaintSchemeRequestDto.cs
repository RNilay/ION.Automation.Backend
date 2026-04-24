using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.PaintScheme
{
    public class SavePaintSchemeRequestDto
    {
        public int EnquiryId { get; set; }

        /// <summary>PaintingSchemeMaster.Id — null if user skipped the scheme dropdown</summary>
        public int? PaintingSchemeId { get; set; }

        public string? SchemeName { get; set; }

        /// <summary>Global coat sections (primer / intermediate / finalCoat)</summary>
        public List<PaintSchemeSectionDto> Sections { get; set; } = new();

        /// <summary>Which BFs are included and how (global / custom / none)</summary>
        public List<BfAssignmentDto> BfAssignments { get; set; } = new();

        /// <summary>Per-BF overrides — only populated when AssignmentType == "custom"</summary>
        public List<BfOverrideDto> Overrides { get; set; } = new();
    }

    /// <summary>
    /// One coat section (primer / intermediate / finalCoat).
    /// Shared between the global scheme and BF overrides.
    /// </summary>
    public class PaintSchemeSectionDto
    {
        /// <summary>"primer" | "intermediate" | "finalCoat"</summary>
        public string SectionKey { get; set; } = string.Empty;

        /// <summary>PK of the selected row in PrimerMaster / IntermediateCoatingMaster / FinalCoatingMaster</summary>
        public int ItemMasterId { get; set; }

        /// <summary>Denormalized label — stored for fast display without a JOIN</summary>
        public string? ItemModel { get; set; }

        public decimal CostPerLiter { get; set; }
        public int NoOfCoats { get; set; }
    }

    /// <summary>BF-level assignment row</summary>
    public class BfAssignmentDto
    {
        /// <summary>"001", "002", etc.</summary>
        public string BfName { get; set; } = string.Empty;

        /// <summary>"global" | "custom" | "none"</summary>
        public string AssignmentType { get; set; } = "global";
    }

    /// <summary>
    /// Custom paint scheme for a single BF.
    /// Only present when AssignmentType == "custom".
    /// </summary>
    public class BfOverrideDto
    {
        public string BfName { get; set; } = string.Empty;
        public int? PaintingSchemeId { get; set; }
        public List<PaintSchemeSectionDto> Sections { get; set; } = new();
    }
}

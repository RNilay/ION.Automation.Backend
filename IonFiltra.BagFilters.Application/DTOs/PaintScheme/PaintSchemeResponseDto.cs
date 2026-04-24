using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.PaintScheme
{
    /// <summary>
    /// Full paint scheme state returned to the frontend.
    /// Shape matches what PaintSchemeModal expects in its initialState prop.
    /// </summary>
    public class PaintSchemeResponseDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int? PaintingSchemeId { get; set; }
        public string? SchemeName { get; set; }
        public decimal CostPerKg { get; set; }
        public List<PaintSchemeSectionDto> Sections { get; set; } = new();
        public List<BfAssignmentDto> BfAssignments { get; set; } = new();
        public List<BfOverrideResponseDto> Overrides { get; set; } = new();
    }

    /// <summary>
    /// Override response — same as request but includes the DB Id for traceability.
    /// </summary>
    public class BfOverrideResponseDto
    {
        public int OverrideId { get; set; }
        public string BfName { get; set; } = string.Empty;
        public int? PaintingSchemeId { get; set; }
        public string? SchemeName { get; set; }
        public decimal CostPerKg { get; set; }
        public List<PaintSchemeSectionDto> Sections { get; set; } = new();
    }
}

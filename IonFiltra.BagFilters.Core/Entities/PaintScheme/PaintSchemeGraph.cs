using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.PaintScheme
{
    /// <summary>
    /// Carrier for the full paint scheme entity graph.
    /// Used as the return type of GetByEnquiryIdAsync to avoid
    /// nullable-tuple inference issues with TransactionHelper.
    /// </summary>
    public class PaintSchemeGraph
    {
        public EnquiryPaintScheme Header { get; set; } = null!;
        public string SchemeName { get; set; } = string.Empty;
        public decimal CostPerKg { get; set; }
        public List<EnquiryPaintSchemeSection> Sections { get; set; } = new();
        public List<EnquiryPaintSchemeBfAssignment> Assignments { get; set; } = new();
        public List<PaintSchemeOverrideGraph> Overrides { get; set; } = new();
    }

    /// <summary>
    /// One custom-override entry: the override header + its coat sections.
    /// Replaces the (Override, Sections) value tuple throughout the codebase.
    /// </summary>
    public class PaintSchemeOverrideGraph
    {
        public EnquiryPaintSchemeOverride Override { get; set; } = null!;
        public string SchemeName { get; set; } = string.Empty;
        public decimal CostPerKg { get; set; }   // resolved from PaintingSchemeMaster via override's PaintingSchemeId
        public List<EnquiryPaintSchemeOverrideSection> Sections { get; set; } = new();
    }
}

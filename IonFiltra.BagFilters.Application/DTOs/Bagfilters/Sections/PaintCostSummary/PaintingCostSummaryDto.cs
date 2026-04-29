using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.PaintCostSummary
{
    /// <summary>
    /// Embedded in each BF batch dto.
    /// Carries the resolved scheme name + computed grand total for that BF.
    /// </summary>
    public class PaintingCostSummaryDto
    {
        public string? SchemeName { get; set; }
        public decimal GrandTotal { get; set; }
    }
}

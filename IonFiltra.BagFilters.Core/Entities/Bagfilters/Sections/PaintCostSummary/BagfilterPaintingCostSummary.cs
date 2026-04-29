using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.PaintCostSummary
{
    public class BagfilterPaintingCostSummary
    {
        public int Id { get; set; }
        public int BagfilterMasterId { get; set; }
        public int EnquiryId { get; set; }
        public string? SchemeName { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

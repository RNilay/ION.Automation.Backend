using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.PaintScheme
{
    public class EnquiryPaintSchemeSection
    {
        public int Id { get; set; }
        public int EnquiryPaintSchemeId { get; set; }
        public string SectionKey { get; set; }        // "primer" | "intermediate" | "finalCoat"
        public int ItemMasterId { get; set; }
        public string? ItemModel { get; set; }
        public decimal CostPerLiter { get; set; }
        public int NoOfCoats { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

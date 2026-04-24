using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.PaintScheme
{
    public class EnquiryPaintSchemeBfAssignment
    {
        public int Id { get; set; }
        public int EnquiryPaintSchemeId { get; set; }
        public int? BagfilterMasterId { get; set; }
        public string BfName { get; set; }            // "001", "002", etc.
        public string AssignmentType { get; set; }    // "global" | "custom" | "none"
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

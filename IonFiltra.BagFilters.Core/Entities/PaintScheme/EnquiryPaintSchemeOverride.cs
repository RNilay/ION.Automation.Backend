using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.PaintScheme
{
    public class EnquiryPaintSchemeOverride
    {
        public int Id { get; set; }
        public int BfAssignmentId { get; set; }
        public int? PaintingSchemeId { get; set; }

        [NotMapped]
        public string? BfName { get; set; }   // transient — used only during Save/Update, never persisted
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

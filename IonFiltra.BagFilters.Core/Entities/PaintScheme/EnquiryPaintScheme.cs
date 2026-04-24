using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.PaintScheme
{
    public class EnquiryPaintScheme
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int? PaintingSchemeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.DuctEngineering
{
    public class EnquiryDuctEngineering
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }

        /// <summary>true = Yes, false = No</summary>
        public bool? IsDuctEngineering { get; set; }

        /// <summary>
        /// Snapshot of the AdminCostConfig "Duct Engineering Cost" value
        /// at the time the user saved. Read-only in UI — sourced from master.
        /// </summary>
        public decimal? Cost { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

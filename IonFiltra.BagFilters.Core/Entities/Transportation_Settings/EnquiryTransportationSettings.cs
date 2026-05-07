using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.Transportation_Settings
{
    public class EnquiryTransportationSettings
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }

        /// <summary>"EX works" or "DAP"</summary>
        public string? DefaultMode { get; set; } = "EX works";

        /// <summary>Manual DAP cost in ₹ — used when DefaultMode is "DAP"</summary>
        public decimal? DapFixedCost { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.ApprovedMakes
{
    public class EnquiryApprovedMakeItem
    {
        public int Id { get; set; }

        /// <summary>FK to EnquiryApprovedMakes.Id</summary>
        public int EnquiryApprovedMakeId { get; set; }

        /// <summary>masterKey from the frontend — e.g. "CENTRIFUGAL_FAN", "PULSE_JET_VALVE"</summary>
        public string MasterKey { get; set; } = string.Empty;

        /// <summary>The approved make string — e.g. "Kirloskar", "L&T"</summary>
        public string MakeValue { get; set; } = string.Empty;

        /// <summary>"primary" | "secondary"</summary>
        public string ItemType { get; set; } = "primary";

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

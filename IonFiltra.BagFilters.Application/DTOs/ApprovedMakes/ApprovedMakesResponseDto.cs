using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.ApprovedMakes
{
    /// <summary>
    /// Returned to the frontend on GET by-enquiry.
    /// ApprovedMakes is in the same dictionary shape the frontend state expects,
    /// so it can be set directly: setApprovedMakes(data.approvedMakes)
    /// </summary>
    public class ApprovedMakesResponseDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }

        /// <summary>
        /// { "CENTRIFUGAL_FAN": ["Kirloskar", "Bharat"], ... }
        /// </summary>
        public Dictionary<string, List<string>> ApprovedMakes { get; set; } = new();

        /// <summary>
        /// Item type per masterKey — "primary" or "secondary"
        /// </summary>
        public Dictionary<string, string> ItemTypes { get; set; } = new();
    }
}

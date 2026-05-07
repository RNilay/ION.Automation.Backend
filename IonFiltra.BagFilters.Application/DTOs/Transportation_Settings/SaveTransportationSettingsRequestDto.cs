using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.Transportation_Settings
{
    /// <summary>
    /// Payload sent by the frontend on save and update.
    /// </summary>
    public class SaveTransportationSettingsRequestDto
    {
        public int EnquiryId { get; set; }

        /// <summary>"EX works" or "DAP"</summary>
        public string? DefaultMode { get; set; } = "EX works";

        /// <summary>Manual DAP cost in ₹</summary>
        public decimal? DapFixedCost { get; set; }
    }

    /// <summary>
    /// Shape returned to the frontend on GET.
    /// camelCase-compatible with frontend transportationGlobalState keys.
    /// </summary>
    public class TransportationSettingsResponseDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public string? DefaultMode { get; set; } = "EX works";
        public decimal? DapFixedCost { get; set; }
    }
}

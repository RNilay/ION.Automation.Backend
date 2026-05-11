using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.ApprovedMakes
{
    /// <summary>
    /// Payload sent by the frontend when saving / updating approved makes.
    /// The ApprovedMakes dictionary mirrors the frontend state shape exactly:
    ///   { "CENTRIFUGAL_FAN": ["Kirloskar", "Bharat"], "EXP_JOINT_INLET": ["L&T"] }
    /// </summary>
    public class SaveApprovedMakesRequestDto
    {
        public int EnquiryId { get; set; }

        /// <summary>
        /// Key   = masterKey (e.g. "CENTRIFUGAL_FAN")
        /// Value = list of approved make strings for that item
        /// Keys with empty lists are ignored during save.
        /// </summary>
        public Dictionary<string, List<string>> ApprovedMakes { get; set; } = new();

        /// <summary>
        /// Item type per masterKey — "primary" or "secondary".
        /// If a key is absent the value defaults to "primary".
        /// </summary>
        public Dictionary<string, string> ItemTypes { get; set; } = new();
    }
}

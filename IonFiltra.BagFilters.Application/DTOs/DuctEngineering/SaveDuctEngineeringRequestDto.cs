using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.DuctEngineering
{
    public class SaveDuctEngineeringRequestDto
    {
        public int EnquiryId { get; set; }

        /// <summary>true = Yes, false = No</summary>
        public bool? IsDuctEngineering { get; set; }

        /// <summary>
        /// Cost snapshot sent from the frontend (sourced from AdminCostConfig).
        /// Stored as-is so the saved record is independent of future master changes.
        /// </summary>
        public decimal? Cost { get; set; }
    }


    public class DuctEngineeringResponseDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public bool? IsDuctEngineering { get; set; }
        public decimal? Cost { get; set; }
    }
}

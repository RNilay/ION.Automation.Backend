using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.Supervision_Charges
{
    /// <summary>
    /// Payload sent by the frontend on save and update.
    /// </summary>
    public class SaveSupervisionChargesRequestDto
    {
        public int EnquiryId { get; set; }
        public decimal VisitEngineeringCharges { get; set; }
        public decimal FreeManDays { get; set; }
        public decimal FreeManDaysRate { get; set; }
        public decimal FreeManDaysToAndFro { get; set; }
        public decimal FreeManDaysLodgingBoarding { get; set; }
        public decimal ChargeableDays { get; set; }
        public decimal ChargeableRate { get; set; }
        public decimal ChargeableToAndFro { get; set; }
        public decimal ChargeableLodgingBoarding { get; set; }
    }

    /// <summary>
    /// Shape returned to the frontend on GET.
    /// Field names are camelCase-compatible with the frontend state keys.
    /// </summary>
    public class SupervisionChargesResponseDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public decimal VisitEngineeringCharges { get; set; }
        public decimal FreeManDays { get; set; }
        public decimal FreeManDaysRate { get; set; }
        public decimal FreeManDaysToAndFro { get; set; }
        public decimal FreeManDaysLodgingBoarding { get; set; }
        public decimal ChargeableDays { get; set; }
        public decimal ChargeableRate { get; set; }
        public decimal ChargeableToAndFro { get; set; }
        public decimal ChargeableLodgingBoarding { get; set; }
    }
}

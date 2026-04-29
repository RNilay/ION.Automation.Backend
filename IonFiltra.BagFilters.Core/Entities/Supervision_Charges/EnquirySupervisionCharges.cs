using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.Supervision_Charges
{
    public class EnquirySupervisionCharges
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
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

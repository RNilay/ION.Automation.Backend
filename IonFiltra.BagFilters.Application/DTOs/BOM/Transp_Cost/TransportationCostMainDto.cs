using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.BOM.Transp_Cost
{
    public class TransportationCostMainDto
    {
        public int Id { get; set; }
        public int? EnquiryId { get; set; }
        public int? BagfilterMasterId { get; set; }
        public TransportationCostEntityDto TransportationCostEntity { get; set; }

    }
}


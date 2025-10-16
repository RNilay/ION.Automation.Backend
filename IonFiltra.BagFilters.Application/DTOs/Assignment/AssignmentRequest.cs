using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.Assignment
{
    public class AssignmentRequest
    {
        public string EnquiryId { get; set; }
        public string Customer { get; set; }
        public int ProcessVolumes { get; set; }
        public int RequiredBagFilters { get; set; }
    }
}

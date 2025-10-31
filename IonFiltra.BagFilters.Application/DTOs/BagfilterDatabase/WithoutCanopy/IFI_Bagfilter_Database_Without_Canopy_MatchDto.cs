using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.BagfilterDatabase.WithoutCanopy
{
    public class IFI_Bagfilter_Database_Without_Canopy_MatchDto
    {
      
        public string? Process_Volume_m3hr { get; set; }
        public string? Hopper_type { get; set; }

   
        public decimal? Number_of_columns { get; set; }
    }

}

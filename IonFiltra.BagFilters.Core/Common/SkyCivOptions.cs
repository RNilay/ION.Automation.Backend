using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Common
{
    public class SkyCivOptions
    {
        public string ApiUrl { get; set; } = "https://api.skyciv.com/v3";
        public string Username { get; set; }
        public string Key { get; set; }
        public int TimeoutSeconds { get; set; } = 120; // default timeout
        public int TimeoutMinutes { get; set; } = 10; //optional
    }

}

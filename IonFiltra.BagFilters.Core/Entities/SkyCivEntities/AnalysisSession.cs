using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.SkyCivEntities
{
    public class AnalysisSession
    {
        public int Id { get; set; }
        public string SessionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}

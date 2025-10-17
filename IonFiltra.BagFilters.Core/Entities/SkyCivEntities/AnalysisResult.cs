using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.SkyCivEntities
{
    public class AnalysisResult
    {
        public int Id { get; set; }
        public int AnalysisSessionId { get; set; }
        public AnalysisSession Session { get; set; }
        public string ModelGetDataJson { get; set; }
        public string FullResponseJson { get; set; }
        public DateTime SavedAt { get; set; }
    }
}

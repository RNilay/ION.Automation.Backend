using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.MasterData.Master_Definition
{
    public class MasterDefinitions
    {
        public int Id { get; set; }
        public string MasterKey { get; set; }
        public string DisplayName { get; set; }
        public string ApiRoute { get; set; }
        public int SectionOrder { get; set; }
        public bool IsActive { get; set; }
        public string ColumnsJson { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.MasterData.Master_Definition
{
    public class ColumnDefDto
    {
        public string Field { get; set; }      // e.g. "material"
        public string Label { get; set; }      // e.g. "Material"
        public string Type { get; set; }       // e.g. "string", "number", "date", etc.
                                               // optional: more flags later, e.g. Editable, Width, etc.
                                               // public bool Editable { get; set; }
    }
}

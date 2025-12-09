using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Application.DTOs.MasterData.Master_Definition
{
    public class MasterDefinitionsDto
    {
        public int Id { get; set; }
        public string MasterKey { get; set; }
        public string DisplayName { get; set; }
        public string ApiRoute { get; set; }
        public int SectionOrder { get; set; }
        public bool IsActive { get; set; }


        [JsonIgnore]
        public string ColumnsJson { get; set; }

        // NEW: strongly-typed columns for the frontend
        public List<ColumnDefDto> Columns { get; set; } = new();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace IonFiltra.BagFilters.Application.DTOs.SkyCiv
{
    public class AnalysisResponseDto
    {
        public string SessionId { get; set; }
        public JObject ModelData { get; set; } // S3D.model.get -> data
        public string Status { get; set; }     // Succeeded / Failed
        public string Message { get; set; }    // optional
    }

    public class AnalysisRequestDto
    {
        [Required]
        public JObject s3dModel { get; set; }
    }

}

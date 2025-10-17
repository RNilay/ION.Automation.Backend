using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.SkyCiv;
using Newtonsoft.Json.Linq;

namespace IonFiltra.BagFilters.Core.Interfaces.SkyCiv
{
    public interface ISkyCivAnalysisService
    {
        Task<AnalysisResponseDto> RunAnalysisAsync(JObject s3dModel, CancellationToken ct);
    }
}

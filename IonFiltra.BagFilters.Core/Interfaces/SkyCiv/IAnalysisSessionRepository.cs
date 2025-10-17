using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.SkyCivEntities;

namespace IonFiltra.BagFilters.Core.Interfaces.SkyCiv
{
    public interface IAnalysisSessionRepository
    {
        Task AddAsync(AnalysisSession session);
        Task UpdateAsync(AnalysisSession session);
        Task AddResultAsync(AnalysisResult result);
    }
}

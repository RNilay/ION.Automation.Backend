using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.SkyCivEntities;
using IonFiltra.BagFilters.Core.Interfaces.SkyCiv;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.SkyCiv
{
    public class AnalysisSessionRepository : IAnalysisSessionRepository
    {
        public Task AddAsync(AnalysisSession session)
        {
            throw new NotImplementedException();
        }

        public Task AddResultAsync(AnalysisResult result)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AnalysisSession session)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.MasterData.Master_Definition;

namespace IonFiltra.BagFilters.Core.Interfaces.MasterData.Master_Definition
{
    public interface IMasterDefinitionsRepository
    {
        Task<IEnumerable<MasterDefinitions>> GetAllActiveAsync();
    }
}

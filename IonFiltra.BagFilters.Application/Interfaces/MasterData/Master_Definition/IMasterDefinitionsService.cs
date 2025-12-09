using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.MasterData.Master_Definition;

namespace IonFiltra.BagFilters.Application.Interfaces.MasterData.Master_Definition
{
    public interface IMasterDefinitionsService
    {
        Task<IEnumerable<MasterDefinitionsDto>> GetAllActiveAsync();
    }
}

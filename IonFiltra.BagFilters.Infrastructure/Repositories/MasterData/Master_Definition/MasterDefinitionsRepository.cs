using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.MasterData.Master_Definition;
using IonFiltra.BagFilters.Core.Interfaces.MasterData.Master_Definition;
using IonFiltra.BagFilters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Infrastructure.Repositories.MasterData.Master_Definition
{
    public class MasterDefinitionsRepository : IMasterDefinitionsRepository
    {
        private readonly TransactionHelper _transactionHelper;
        private readonly ILogger<MasterDefinitionsRepository> _logger;

        public MasterDefinitionsRepository(
            TransactionHelper transactionHelper,
            ILogger<MasterDefinitionsRepository> logger)
        {
            _transactionHelper = transactionHelper;
            _logger = logger;
        }

        public async Task<IEnumerable<MasterDefinitions>> GetAllActiveAsync()
        {
            return await _transactionHelper.ExecuteAsync(async dbContext =>
            {
                _logger.LogInformation("Fetching all active MasterDefinitions metadata.");

                return await dbContext.MasterDefinitions
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.SectionOrder)
                    .ThenBy(x => x.Id)
                    .ToListAsync();
            });
        }
    }
}

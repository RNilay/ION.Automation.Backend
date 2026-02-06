using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.Interfaces.BoughtOutItems;
using IonFiltra.BagFilters.Core.Entities.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.BoughtOutItems;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.BoughtOutItems
{
    public class SecondaryBoughtOutItemService : ISecondaryBoughtOutItemService
    {
        private readonly ISecondaryBoughtOutItemRepository _repository;
        private readonly ILogger<SecondaryBoughtOutItemService> _logger;

        public SecondaryBoughtOutItemService(
            ISecondaryBoughtOutItemRepository repository,
            ILogger<SecondaryBoughtOutItemService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<SecondaryBoughtOutItem>> GetByEnquiryAsync(int enquiryId)
        {
            if (enquiryId <= 0)
                throw new ArgumentException("Invalid enquiry id.");

            _logger.LogInformation(
                "Fetching SecondaryBoughtOutItems for EnquiryId {EnquiryId}",
                enquiryId);

            return await _repository.GetByEnquiryAsync(enquiryId);
        }
    }

}

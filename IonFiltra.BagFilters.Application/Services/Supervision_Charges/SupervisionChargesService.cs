using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.Supervision_Charges;
using IonFiltra.BagFilters.Application.Interfaces.Supervision_Charges;
using IonFiltra.BagFilters.Application.Mapper.Supervision_Charges;
using IonFiltra.BagFilters.Core.Interfaces.Supervision_Charges;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Supervision_Charges
{
    public class SupervisionChargesService : ISupervisionChargesService
    {
        private readonly ISupervisionChargesRepository _repository;
        private readonly ILogger<SupervisionChargesService> _logger;

        public SupervisionChargesService(
            ISupervisionChargesRepository repository,
            ILogger<SupervisionChargesService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<int> SaveAsync(SaveSupervisionChargesRequestDto dto)
        {
            _logger.LogInformation(
                "Saving supervision charges for EnquiryId {EnquiryId}", dto.EnquiryId);

            var entity = SupervisionChargesMapper.ToEntity(dto);
            return await _repository.SaveAsync(entity);
        }

        public async Task<bool> UpdateAsync(SaveSupervisionChargesRequestDto dto)
        {
            _logger.LogInformation(
                "Updating supervision charges for EnquiryId {EnquiryId}", dto.EnquiryId);

            var entity = SupervisionChargesMapper.ToEntity(dto);
            return await _repository.UpdateAsync(dto.EnquiryId, entity);
        }

        public async Task<SupervisionChargesResponseDto?> GetByEnquiryIdAsync(int enquiryId)
        {
            _logger.LogInformation(
                "Fetching supervision charges for EnquiryId {EnquiryId}", enquiryId);

            var entity = await _repository.GetByEnquiryIdAsync(enquiryId);
            if (entity == null) return null;

            return SupervisionChargesMapper.ToResponseDto(entity);
        }

        public async Task<bool> ExistsByEnquiryIdAsync(int enquiryId)
        {
            return await _repository.ExistsByEnquiryIdAsync(enquiryId);
        }
    }
}

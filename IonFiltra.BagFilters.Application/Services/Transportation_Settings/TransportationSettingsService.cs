using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.Transportation_Settings;
using IonFiltra.BagFilters.Application.Interfaces.Transportation_Settings;
using IonFiltra.BagFilters.Application.Mapper.Transportation_Settings;
using IonFiltra.BagFilters.Core.Interfaces.Transportation_Settings;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Transportation_Settings
{
    public class TransportationSettingsService : ITransportationSettingsService
    {
        private readonly ITransportationSettingsRepository _repository;
        private readonly ILogger<TransportationSettingsService> _logger;

        public TransportationSettingsService(
            ITransportationSettingsRepository repository,
            ILogger<TransportationSettingsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<int> SaveAsync(SaveTransportationSettingsRequestDto dto)
        {
            _logger.LogInformation(
                "Saving transportation settings for EnquiryId {EnquiryId}",
                dto.EnquiryId);

            var entity = TransportationSettingsMapper.ToEntity(dto);
            return await _repository.SaveAsync(entity);
        }

        public async Task<bool> UpdateAsync(SaveTransportationSettingsRequestDto dto)
        {
            _logger.LogInformation(
                "Updating transportation settings for EnquiryId {EnquiryId}",
                dto.EnquiryId);

            var entity = TransportationSettingsMapper.ToEntity(dto);
            return await _repository.UpdateAsync(dto.EnquiryId, entity);
        }

        public async Task<TransportationSettingsResponseDto?> GetByEnquiryIdAsync(
            int enquiryId)
        {
            _logger.LogInformation(
                "Fetching transportation settings for EnquiryId {EnquiryId}",
                enquiryId);

            var entity = await _repository.GetByEnquiryIdAsync(enquiryId);
            if (entity == null) return null;

            return TransportationSettingsMapper.ToResponseDto(entity);
        }

        public async Task<bool> ExistsByEnquiryIdAsync(int enquiryId)
        {
            return await _repository.ExistsByEnquiryIdAsync(enquiryId);
        }
    }
}

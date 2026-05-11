using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.DuctEngineering;
using IonFiltra.BagFilters.Application.Interfaces.DuctEngineering;
using IonFiltra.BagFilters.Application.Mapper.DuctEngineering;
using IonFiltra.BagFilters.Core.Interfaces.DuctEngineering;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.DuctEngineering
{
    public class DuctEngineeringService : IDuctEngineeringService
    {
        private readonly IEnquiryDuctEngineeringRepository _repository;
        private readonly ILogger<DuctEngineeringService> _logger;

        public DuctEngineeringService(
            IEnquiryDuctEngineeringRepository repository,
            ILogger<DuctEngineeringService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<int> SaveAsync(SaveDuctEngineeringRequestDto dto)
        {
            _logger.LogInformation(
                "Saving duct engineering for EnquiryId {EnquiryId}", dto.EnquiryId);

            var entity = DuctEngineeringMapper.ToEntity(dto);
            return await _repository.SaveAsync(entity);
        }

        public async Task<bool> UpdateAsync(SaveDuctEngineeringRequestDto dto)
        {
            _logger.LogInformation(
                "Updating duct engineering for EnquiryId {EnquiryId}", dto.EnquiryId);

            var entity = DuctEngineeringMapper.ToEntity(dto);
            return await _repository.UpdateAsync(dto.EnquiryId, entity);
        }

        public async Task<DuctEngineeringResponseDto?> GetByEnquiryIdAsync(int enquiryId)
        {
            _logger.LogInformation(
                "Fetching duct engineering for EnquiryId {EnquiryId}", enquiryId);

            var entity = await _repository.GetByEnquiryIdAsync(enquiryId);
            if (entity == null) return null;

            return DuctEngineeringMapper.ToResponseDto(entity);
        }

        public async Task<bool> ExistsByEnquiryIdAsync(int enquiryId)
        {
            return await _repository.ExistsByEnquiryIdAsync(enquiryId);
        }
    }
}

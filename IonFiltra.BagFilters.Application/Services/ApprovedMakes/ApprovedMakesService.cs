using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.ApprovedMakes;
using IonFiltra.BagFilters.Application.Interfaces.ApprovedMakes;
using IonFiltra.BagFilters.Application.Mapper.ApprovedMakes;
using IonFiltra.BagFilters.Core.Interfaces.ApprovedMakes;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.ApprovedMakes
{
    public class ApprovedMakesService : IApprovedMakesService
    {
        private readonly IEnquiryApprovedMakesRepository _repository;
        private readonly ILogger<ApprovedMakesService> _logger;

        public ApprovedMakesService(
            IEnquiryApprovedMakesRepository repository,
            ILogger<ApprovedMakesService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<int> SaveAsync(SaveApprovedMakesRequestDto dto)
        {
            _logger.LogInformation(
                "Saving approved makes for EnquiryId {EnquiryId}", dto.EnquiryId);

            var graph = ApprovedMakesMapper.ToGraph(dto);
            return await _repository.SaveAsync(graph);
        }

        public async Task<bool> UpdateAsync(SaveApprovedMakesRequestDto dto)
        {
            _logger.LogInformation(
                "Updating approved makes for EnquiryId {EnquiryId}", dto.EnquiryId);

            var graph = ApprovedMakesMapper.ToGraph(dto);
            return await _repository.UpdateAsync(dto.EnquiryId, graph);
        }

        public async Task<ApprovedMakesResponseDto?> GetByEnquiryIdAsync(int enquiryId)
        {
            _logger.LogInformation(
                "Fetching approved makes for EnquiryId {EnquiryId}", enquiryId);

            var graph = await _repository.GetByEnquiryIdAsync(enquiryId);

            if (graph == null) return null;

            return ApprovedMakesMapper.ToResponseDto(graph);
        }

        public async Task<bool> ExistsByEnquiryIdAsync(int enquiryId)
        {
            return await _repository.ExistsByEnquiryIdAsync(enquiryId);
        }
    }
}

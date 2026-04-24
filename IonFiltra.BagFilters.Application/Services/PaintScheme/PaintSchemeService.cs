using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.PaintScheme;
using IonFiltra.BagFilters.Application.Interfaces.PaintScheme;
using IonFiltra.BagFilters.Application.Mapper.PaintScheme;
using IonFiltra.BagFilters.Core.Interfaces.PaintScheme;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.PaintScheme
{
    public class PaintSchemeService : IPaintSchemeService
    {
        private readonly IEnquiryPaintSchemeRepository _repository;
        private readonly ILogger<PaintSchemeService> _logger;

        public PaintSchemeService(
            IEnquiryPaintSchemeRepository repository,
            ILogger<PaintSchemeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        public async Task<int> SaveAsync(SavePaintSchemeRequestDto dto)
        {
            _logger.LogInformation(
                "Saving paint scheme for EnquiryId {EnquiryId}", dto.EnquiryId);

            var graph = PaintSchemeMapper.ToGraph(dto);
            return await _repository.SaveAsync(graph);
        }

        public async Task<bool> UpdateAsync(SavePaintSchemeRequestDto dto)
        {
            _logger.LogInformation(
                "Updating paint scheme for EnquiryId {EnquiryId}", dto.EnquiryId);

            var graph = PaintSchemeMapper.ToGraph(dto);
            return await _repository.UpdateAsync(dto.EnquiryId, graph);
        }

        public async Task<PaintSchemeResponseDto?> GetByEnquiryIdAsync(int enquiryId)
        {
            _logger.LogInformation(
                "Fetching paint scheme for EnquiryId {EnquiryId}", enquiryId);

            var graph = await _repository.GetByEnquiryIdAsync(enquiryId);

            if (graph == null) return null;

            return PaintSchemeMapper.ToResponseDto(graph);
        }

        public async Task<bool> ExistsByEnquiryIdAsync(int enquiryId)
        {
            return await _repository.ExistsByEnquiryIdAsync(enquiryId);
        }


    }
}

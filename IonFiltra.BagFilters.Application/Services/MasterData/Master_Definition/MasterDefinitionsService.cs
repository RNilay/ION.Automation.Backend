using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.MasterData.Master_Definition;
using IonFiltra.BagFilters.Application.Interfaces.MasterData.Master_Definition;
using IonFiltra.BagFilters.Application.Mapper.MasterData.Master_Definition;
using IonFiltra.BagFilters.Core.Interfaces.MasterData.Master_Definition;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.MasterData.Master_Definition
{
    public class MasterDefinitionsService : IMasterDefinitionsService
    {
        private readonly IMasterDefinitionsRepository _repository;
        private readonly ILogger<MasterDefinitionsService> _logger;

        public MasterDefinitionsService(
            IMasterDefinitionsRepository repository,
            ILogger<MasterDefinitionsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        //public async Task<IEnumerable<MasterDefinitionsDto>> GetAllActiveAsync()
        //{
        //    _logger.LogInformation("Service: Fetching all active MasterDefinitions metadata.");

        //    var entities = await _repository.GetAllActiveAsync();

        //    // Map to DTOs using your mapper
        //    return entities.Select(MasterDefinitionsMapper.ToMainDto);
        //}

        public async Task<IEnumerable<MasterDefinitionsDto>> GetAllActiveAsync()
        {
            _logger.LogInformation("Service: Fetching all active MasterDefinitions metadata.");

            var entities = await _repository.GetAllActiveAsync();

            var dtos = entities
                .Select(MasterDefinitionsMapper.ToMainDto)
                .ToList();

            foreach (var dto in dtos)
            {
                dto.Columns = ParseColumnsJson(dto.ColumnsJson, dto.MasterKey);
            }

            return dtos;
        }


        private List<ColumnDefDto> ParseColumnsJson(string json, string masterKey)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<ColumnDefDto>();

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var columns = JsonSerializer.Deserialize<List<ColumnDefDto>>(json, options);
                return columns ?? new List<ColumnDefDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to deserialize ColumnsJson for MasterKey {MasterKey}. Raw JSON: {Json}",
                    masterKey,
                    json);

                // Fail soft: return empty list instead of blowing up the whole endpoint
                return new List<ColumnDefDto>();
            }
        }

    }

}

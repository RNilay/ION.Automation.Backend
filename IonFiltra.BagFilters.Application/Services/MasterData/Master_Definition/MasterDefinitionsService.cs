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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.MasterData.Master_Definition
{
    public class MasterDefinitionsService : IMasterDefinitionsService
    {
        private readonly IMasterDefinitionsRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<MasterDefinitionsService> _logger;

        // Cache key — all master definitions share one cached result
        private const string CacheKey = "MasterDefinitions_All_Active";

        

        public MasterDefinitionsService(
        IMasterDefinitionsRepository repository,
        IMemoryCache cache,
        ILogger<MasterDefinitionsService> logger)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<MasterDefinitionsDto>> GetAllActiveAsync()
        {
            _logger.LogInformation("Service: Fetching all active MasterDefinitions metadata.");

            // Return from cache if available
            if (_cache.TryGetValue(CacheKey, out IEnumerable<MasterDefinitionsDto>? cached)
                && cached != null)
            {
                _logger.LogInformation("MasterDefinitions metadata served from cache.");
                return cached;
            }

            var entities = await _repository.GetAllActiveAsync();

            var dtos = entities
                .Select(MasterDefinitionsMapper.ToMainDto)
                .ToList();

            foreach (var dto in dtos)
                dto.Columns = ParseColumnsJson(dto.ColumnsJson, dto.MasterKey);

            // Cache for 2 hours — master data changes only when admin edits it
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(2))
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            _cache.Set(CacheKey, (IEnumerable<MasterDefinitionsDto>)dtos, options);

            _logger.LogInformation(
                "MasterDefinitions metadata cached ({Count} entries).", dtos.Count);

            return dtos;
        }

        // ── Cache invalidation — call after admin saves a MasterDefinition ────
        public void InvalidateCache()
        {
            _cache.Remove(CacheKey);
            _logger.LogInformation("MasterDefinitions cache invalidated.");
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

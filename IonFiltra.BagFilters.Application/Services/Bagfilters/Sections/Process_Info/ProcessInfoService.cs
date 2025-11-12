using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Process_Info;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Process_Info
{
    public class ProcessInfoService : IProcessInfoService
    {
        private readonly IProcessInfoRepository _repository;
        private readonly ILogger<ProcessInfoService> _logger;

        public ProcessInfoService(
            IProcessInfoRepository repository,
            ILogger<ProcessInfoService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ProcessInfoMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching ProcessInfo for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return ProcessInfoMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(ProcessInfoMainDto dto)
        {
            _logger.LogInformation("Adding ProcessInfo for Id {Id}", dto.Id);
            var entity = ProcessInfoMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(ProcessInfoMainDto dto)
        {
            _logger.LogInformation("Updating ProcessInfo for Id {Id}", dto.Id);
            var entity = ProcessInfoMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    
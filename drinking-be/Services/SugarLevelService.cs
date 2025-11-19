// Services/SugarLevelService.cs
using AutoMapper;
using drinking_be.Dtos.OptionDtos;
using drinking_be.Interfaces;
using drinking_be.Interfaces.OptionInterfaces;
using drinking_be.Models;

namespace drinking_be.Services
{
    public class SugarLevelService : ISugarLevelService
    {
        private readonly ISugarLevelRepository _sugarRepo;
        private readonly IMapper _mapper;

        public SugarLevelService(ISugarLevelRepository sugarRepo, IMapper mapper)
        {
            _sugarRepo = sugarRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SugarLevelDto>> GetAllSugarLevelsAsync()
        {
            var levels = await _sugarRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<SugarLevelDto>>(levels);
        }
    }
}
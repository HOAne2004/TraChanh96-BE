// Services/IceLevelService.cs
using AutoMapper;
using drinking_be.Dtos.OptionDtos;
using drinking_be.Interfaces;
using drinking_be.Interfaces.OptionInterfaces;
using drinking_be.Models;

namespace drinking_be.Services
{
    public class IceLevelService : IIceLevelService
    {
        private readonly IIceLevelRepository _iceRepo;
        private readonly IMapper _mapper;

        public IceLevelService(IIceLevelRepository iceRepo, IMapper mapper)
        {
            _iceRepo = iceRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IceLevelDto>> GetAllIceLevelsAsync()
        {
            var levels = await _iceRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<IceLevelDto>>(levels);
        }
    }
}
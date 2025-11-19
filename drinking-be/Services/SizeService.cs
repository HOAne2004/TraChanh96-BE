// Services/SizeService.cs (TẠO MỚI)
using AutoMapper;
using drinking_be.Dtos.OptionDtos;
using drinking_be.Interfaces.OptionInterfaces;
using drinking_be.Models;

namespace drinking_be.Services
{
    public class SizeService : ISizeService
    {
        private readonly ISizeRepository _sizeRepo;
        private readonly IMapper _mapper;

        public SizeService(ISizeRepository sizeRepo, IMapper mapper)
        {
            _sizeRepo = sizeRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SizeDto>> GetAllSizesAsync()
        {
            var sizes = await _sizeRepo.GetAllAsync();
            // Cần thêm Map trong MappingProfile
            return _mapper.Map<IEnumerable<SizeDto>>(sizes);
        }
    }
}
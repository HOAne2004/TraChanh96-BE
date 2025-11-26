// Services/MembershipLevelService.cs
using drinking_be.Dtos.MembershipLevelDtos;
using drinking_be.Interfaces;
using drinking_be.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Services
{
    public class MembershipLevelService : IMembershipLevelService
    {
        private readonly IMembershipLevelRepository _levelRepo;
        private readonly IMapper _mapper;

        public MembershipLevelService(IMembershipLevelRepository levelRepo, IMapper mapper)
        {
            _levelRepo = levelRepo;
            _mapper = mapper;
        }

        // --- PUBLIC API ---

        public async Task<IEnumerable<MembershipLevelReadDto>> GetAllLevelsAsync()
        {
            var levels = await _levelRepo.GetAllSortedAsync();
            return _mapper.Map<IEnumerable<MembershipLevelReadDto>>(levels);
        }

        // --- ADMIN API ---

        public async Task<MembershipLevelReadDto?> GetLevelByIdAsync(byte id)
        {
            // Do IGenericRepository.GetByIdAsync(int id) nhận int, ta cần ép kiểu
            var level = await _levelRepo.GetByIdAsync(id);
            if (level == null)
            {
                throw new KeyNotFoundException("Không tìm thấy cấp độ thành viên.");
            }
            return _mapper.Map<MembershipLevelReadDto>(level);
        }

        public async Task<MembershipLevelReadDto> CreateLevelAsync(MembershipLevelCreateDto levelDto)
        {
            // Kiểm tra tên trùng lặp (nếu cần)
            var existing = await _levelRepo.FindAsync(l => l.Name.ToLower() == levelDto.Name.ToLower());
            if (existing.Any())
            {
                throw new Exception("Tên cấp độ này đã tồn tại.");
            }

            var level = _mapper.Map<MembershipLevel>(levelDto);

            await _levelRepo.AddAsync(level);
            await _levelRepo.SaveChangesAsync();

            return _mapper.Map<MembershipLevelReadDto>(level);
        }

        public async Task<MembershipLevelReadDto> UpdateLevelAsync(byte id, MembershipLevelCreateDto levelDto)
        {
            var existingLevel = await _levelRepo.GetByIdAsync(id);
            if (existingLevel == null)
            {
                throw new KeyNotFoundException("Không tìm thấy cấp độ thành viên để cập nhật.");
            }

            // Ánh xạ DTO sang Entity đã tồn tại
            _mapper.Map(levelDto, existingLevel);

            _levelRepo.Update(existingLevel);
            await _levelRepo.SaveChangesAsync();

            return _mapper.Map<MembershipLevelReadDto>(existingLevel);
        }
    }
}
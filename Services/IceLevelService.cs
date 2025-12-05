// Services/IceLevelService.cs
using AutoMapper;
using drinking_be.Dtos.OptionDtos;
using drinking_be.Interfaces;
using drinking_be.Interfaces.OptionInterfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Services
{
    public class IceLevelService : IIceLevelService
    {
        private readonly IIceLevelRepository _iceRepo;
        private readonly IMapper _mapper;
        private readonly DBDrinkContext _context;
        public IceLevelService(IIceLevelRepository iceRepo, IMapper mapper, DBDrinkContext context)
        {
            _iceRepo = iceRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<IceLevelDto>> GetAllIceLevelsAsync()
        {
            var levels = await _iceRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<IceLevelDto>>(levels);
        }

        public async Task<IceLevelDto> CreateIceLevelAsync(IceLevelCreateDto iceLevelDto)
        {
            if(await _iceRepo.ExistsByNameAsync(iceLevelDto.Label))
            {
                throw new ArgumentException($"Mức đá {iceLevelDto.Label} đã tồn tại.");
            }
            var iceLevel = _mapper.Map<IceLevel>(iceLevelDto);
            await _iceRepo.AddAsync(iceLevel);
            // After adding, you may want to save changes and/or retrieve the entity with its generated Id.
            // For now, we assume iceLevel is updated with its Id after AddAsync.
            return _mapper.Map<IceLevelDto>(iceLevel);
        }
        public async Task<IceLevelDto?> UpdateIceLevelAsync(short id, IceLevelCreateDto iceLevelDto)
        {
            var existingIceLevel = await _iceRepo.GetByIdAsync(id);
            if (existingIceLevel == null) return null;
            _mapper.Map(iceLevelDto, existingIceLevel);
            _iceRepo.Update(existingIceLevel); // Update is void, do not await
            await _iceRepo.SaveChangesAsync();   // Persist changes asynchronously
            return _mapper.Map<IceLevelDto>(existingIceLevel);
        }

        public async Task<int> CountProductsUsingIceLevelAsync(short id)
        {
            // Đếm trong bảng trung gian Product_IceLevel
            return await _context.ProductIceLevels.CountAsync(ps => ps.IceLevelId == id);
        }

        public async Task<bool> DeleteIceLevelAsync(short id)
        {
            var iceLevel = await _context.IceLevels.FindAsync(id);
            if (iceLevel == null) return false;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // 1. Xóa các liên kết trong bảng trung gian trước (Product_IceLevel)
                // Sản phẩm sẽ vẫn tồn tại nhưng không còn IceLevel này nữa -> Đúng yêu cầu "mất trong sản phẩm"
                var relations = _context.ProductIceLevels.Where(ps => ps.IceLevelId == id);
                _context.ProductIceLevels.RemoveRange(relations);

                // 2. Xóa IceLevel chính
                _context.IceLevels.Remove(iceLevel);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

}
// Services/SugarLevelService.cs
using AutoMapper;
using drinking_be.Dtos.OptionDtos;
using drinking_be.Interfaces;
using drinking_be.Interfaces.OptionInterfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Services
{
    public class SugarLevelService : ISugarLevelService
    {
        private readonly ISugarLevelRepository _sugarRepo;
        private readonly IMapper _mapper;
        private readonly DBDrinkContext _context;

        public SugarLevelService(ISugarLevelRepository sugarRepo, IMapper mapper, DBDrinkContext context)
        {
            _sugarRepo = sugarRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<SugarLevelReadDto>> GetAllSugarLevelsAsync()
        {
            var levels = await _sugarRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<SugarLevelReadDto>>(levels);
        }

        public async Task<SugarLevelReadDto> CreateSugarLevelAsync(SugarLevelCreateDto sugarLevelDto)
        {
            var sugarLevel = _mapper.Map<SugarLevel>(sugarLevelDto);
            await _sugarRepo.AddAsync(sugarLevel);
            // After adding, you may want to save changes and/or retrieve the entity with its generated Id.
            // For now, we assume sugarLevel is updated with its Id after AddAsync.
            return _mapper.Map<SugarLevelReadDto>(sugarLevel);
        }

        public async Task<SugarLevelReadDto?> UpdateSugarLevelAsync(short id, SugarLevelCreateDto sugarLevelDto)
        {
            var existingSugarLevel = await _sugarRepo.GetByIdAsync(id);
            if (existingSugarLevel == null) return null;
            _mapper.Map(sugarLevelDto, existingSugarLevel);
            _sugarRepo.Update(existingSugarLevel); // Update is void, do not await
            await _sugarRepo.SaveChangesAsync();   // Persist changes asynchronously
            return _mapper.Map<SugarLevelReadDto>(existingSugarLevel);
        }

        public async Task<int> CountProductsUsingSugarLevelAsync(short id)
        {
            // Đếm trong bảng trung gian Product_SugarLevel
            return await _context.ProductSugarLevels.CountAsync(ps => ps.SugarLevelId == id);
        }

        public async Task<bool> DeleteSugarLevelAsync(short id)
        {
            var sugarLevel = await _context.SugarLevels.FindAsync(id);
            if (sugarLevel == null) return false;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // 1. Xóa các liên kết trong bảng trung gian trước (Product_SugarLevel)
                // Sản phẩm sẽ vẫn tồn tại nhưng không còn SugarLevel này nữa -> Đúng yêu cầu "mất trong sản phẩm"
                var relations = _context.ProductSugarLevels.Where(ps => ps.SugarLevelId == id);
                _context.ProductSugarLevels.RemoveRange(relations);

                // 2. Xóa SugarLevel chính
                _context.SugarLevels.Remove(sugarLevel);

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
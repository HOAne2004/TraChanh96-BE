// Services/SizeService.cs (TẠO MỚI)
using AutoMapper;
using drinking_be.Dtos.OptionDtos;
using drinking_be.Interfaces.OptionInterfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Services
{
    public class SizeService : ISizeService
    {
        private readonly ISizeRepository _sizeRepo;
        private readonly IMapper _mapper;
        private readonly DBDrinkContext _context;

        public SizeService(ISizeRepository sizeRepo, IMapper mapper, DBDrinkContext context)
        {
            _sizeRepo = sizeRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<SizeReadDto>> GetAllSizesAsync()
        {
            var sizes = await _sizeRepo.GetAllAsync();
            // Cần thêm Map trong MappingProfile
            return _mapper.Map<IEnumerable<SizeReadDto>>(sizes);
        }

        public async Task<SizeReadDto> CreateSizeAsync(SizeCreateDto sizeDto)
        {
            var size = _mapper.Map<Size>(sizeDto);
            await _sizeRepo.AddAsync(size);
            await _sizeRepo.SaveChangesAsync(); // Lưu thay đổi vào DB
            return _mapper.Map<SizeReadDto>(size);
        }

        public async Task<SizeReadDto?> UpdateSizeAsync(short id, SizeCreateDto sizeDto)
        {
            var existingSize = await _sizeRepo.GetByIdAsync(id);
            if (existingSize == null) return null;
            _mapper.Map(sizeDto, existingSize);
            _sizeRepo.Update(existingSize); // Update is void, do not await
            await _sizeRepo.SaveChangesAsync();   // Persist changes asynchronously
            return _mapper.Map<SizeReadDto>(existingSize);
        }

        public async Task<int> CountProductsUsingSizeAsync(short id)
        {
            // Đếm trong bảng trung gian Product_Size
            return await _context.ProductSizes.CountAsync(ps => ps.SizeId == id);
        }

        public async Task<bool> DeleteSizeAsync(short id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null) return false;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // 1. Xóa các liên kết trong bảng trung gian trước (Product_Size)
                // Sản phẩm sẽ vẫn tồn tại nhưng không còn Size này nữa -> Đúng yêu cầu "mất trong sản phẩm"
                var relations = _context.ProductSizes.Where(ps => ps.SizeId == id);
                _context.ProductSizes.RemoveRange(relations);

                // 2. Xóa Size chính
                _context.Sizes.Remove(size);

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
// Services/BrandService.cs
using drinking_be.Dtos.BrandDtos;
using drinking_be.Interfaces;
using drinking_be.Models;
using AutoMapper;
using System.Threading.Tasks;
using System;

namespace drinking_be.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepo;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepo, IMapper mapper)
        {
            _brandRepo = brandRepo;
            _mapper = mapper;
        }

        public async Task<BrandReadDto?> GetPrimaryBrandInfoAsync()
        {
            var brand = await _brandRepo.GetPrimaryBrandAsync();

            return _mapper.Map<BrandReadDto>(brand);
        }

        public async Task<BrandReadDto> CreateBrandAsync(BrandCreateDto brandDto)
        {
            // 1. Kiểm tra Brand chính đã tồn tại chưa
            var existingBrand = await _brandRepo.GetPrimaryBrandAsync();
                // TẠO MỚI (Thường chỉ chạy lần đầu)
                var newBrand = _mapper.Map<Brand>(brandDto);
                await _brandRepo.AddAsync(newBrand);
                await _brandRepo.SaveChangesAsync();
                return _mapper.Map<BrandReadDto>(newBrand);
        }

        public async Task<BrandReadDto?> UpdateBrandAsync(int id, BrandUpdateDto brandDto)
        {
            var brand = await _brandRepo.GetByIdAsync(id);
            if (brand == null)
            {
                return null;
            }
            _mapper.Map(brandDto, brand);
            brand.UpdatedAt = DateTime.UtcNow;
            _brandRepo.Update(brand);
            await _brandRepo.SaveChangesAsync();
            return _mapper.Map<BrandReadDto>(brand);
        }
    }
}
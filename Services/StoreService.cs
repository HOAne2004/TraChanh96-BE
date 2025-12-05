// Services/StoreService.cs
using drinking_be.Dtos.StoreDtos;
using drinking_be.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using drinking_be.Interfaces.StoreInterfaces;

namespace drinking_be.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepo;
        private readonly IMapper _mapper;

        public StoreService(IStoreRepository storeRepo, IMapper mapper)
        {
            _storeRepo = storeRepo;
            _mapper = mapper;
        }

        // --- PUBLIC API ---

        public async Task<IEnumerable<StoreReadDto>> GetActiveStoresAsync()
        {
            var stores = await _storeRepo.GetActiveStoresAsync();

            // Ánh xạ Store Entity (đã có Brand) sang StoreReadDto
            return _mapper.Map<IEnumerable<StoreReadDto>>(stores);
        }

        public async Task<StoreReadDto?> GetStoreBySlugAsync(string slug)
        {
            var store = await _storeRepo.GetBySlugAsync(slug);
            if (store == null || !store.IsActive == true)
            {
                return null;
            }

            // Ánh xạ Store Entity (đã có Brand) sang StoreReadDto
            return _mapper.Map<StoreReadDto>(store);
        }

        // TODO: Triển khai các phương thức Admin (Create, Update)

        public async Task<StoreReadDto> CreateStoreAsync(StoreCreateDto storeDto)
        {
            var store = _mapper.Map<Store>(storeDto);

            // Thêm logic xác thực BrandId có tồn tại hay không

            await _storeRepo.AddAsync(store);
            await _storeRepo.SaveChangesAsync();

            // NOTE: Để trả về BrandName chính xác, ta cần viết GetByIdWithBrand() trong Repo
            // Giả định: Ta chỉ trả về DTO cơ bản sau khi tạo
            return _mapper.Map<StoreReadDto>(store);
        }

        // ⭐️ THÊM MỚI: Logic Cập nhật
        public async Task<StoreReadDto?> UpdateStoreAsync(long id, StoreUpdateDto storeDto)
        {
            // 1. Tìm Store cũ
            var existingStore = await _storeRepo.GetByIdAsync((int)id); // Ép kiểu nếu Repo dùng int
            if (existingStore == null) return null;

            // 2. Cập nhật dữ liệu (Map từ DTO đè lên Entity cũ)
            _mapper.Map(storeDto, existingStore);

            // 3. Lưu thay đổi
            _storeRepo.Update(existingStore);
            await _storeRepo.SaveChangesAsync();

            return _mapper.Map<StoreReadDto>(existingStore);
        }

        // ⭐️ THÊM MỚI: Logic Xóa
        public async Task<bool> DeleteStoreAsync(long id)
        {
            var existingStore = await _storeRepo.GetByIdAsync((int)id);
            if (existingStore == null) return false;

            // Cách 1: Xóa cứng (Hard Delete)
            _storeRepo.Delete(existingStore);

            // Cách 2: Xóa mềm (Soft Delete - Khuyên dùng)
            // existingStore.IsActive = false;
            // _storeRepo.Update(existingStore);

            await _storeRepo.SaveChangesAsync();
            return true;
        }
    }
}
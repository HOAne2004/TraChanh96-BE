using AutoMapper;
using drinking_be.Dtos.ShopTableDtos;
using drinking_be.Interfaces;
using drinking_be.Interfaces.ShopTableInterfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace drinking_be.Services
{
    public class ShopTableService : IShopTableService
    {
        private readonly IShopTableRepository _repository;
        private readonly IMapper _mapper;

        public ShopTableService(IShopTableRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShopTableReadDto>> GetTablesByStoreAsync(int storeId)
        {
            var tables = await _repository.GetTablesByStoreIdAsync(storeId);
            return _mapper.Map<IEnumerable<ShopTableReadDto>>(tables);
        }

        public async Task<ShopTableReadDto?> GetTableByIdAsync(int id)
        {
            // Sử dụng FindAsync của GenericRepository với Include Store để lấy tên Store
            var tables = await _repository.FindAsync(t => t.Id == id, "Store");
            var table = tables.FirstOrDefault();

            if (table == null) return null;

            return _mapper.Map<ShopTableReadDto>(table);
        }

        public async Task<ShopTableReadDto> CreateTableAsync(ShopTableCreateDto createDto)
        {
            // Kiểm tra trùng tên bàn trong quán (Optional)
            if (await _repository.IsTableNameExistsAsync(createDto.StoreId, createDto.Name))
            {
                throw new Exception($"Bàn có tên '{createDto.Name}' đã tồn tại trong cửa hàng này.");
            }

            var tableEntity = _mapper.Map<ShopTable>(createDto);

            // Set giá trị mặc định nếu cần
            tableEntity.CreatedAt = DateTime.Now;
            tableEntity.IsActive = true;

            await _repository.AddAsync(tableEntity);
            await _repository.SaveChangesAsync();

            // Load lại để lấy thông tin Store (nếu cần thiết cho ReadDto trả về ngay lập tức)
            // Ở đây map trực tiếp trả về cho nhanh
            return _mapper.Map<ShopTableReadDto>(tableEntity);
        }

        public async Task<bool> UpdateTableAsync(int id, ShopTableUpdateDto updateDto)
        {
            var tableEntity = await _repository.GetByIdAsync(id);
            if (tableEntity == null) return false;

            // Map dữ liệu từ UpdateDto sang Entity
            _mapper.Map(updateDto, tableEntity);

            _repository.Update(tableEntity);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTableAsync(int id)
        {
            var tableEntity = await _repository.GetByIdAsync(id);
            if (tableEntity == null) return false;

            // Xóa mềm: Chỉ set IsActive = false
            // tableEntity.IsActive = false;
            // _repository.Update(tableEntity);

            // Xóa cứng (Xóa khỏi DB):
            _repository.Delete(tableEntity);

            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
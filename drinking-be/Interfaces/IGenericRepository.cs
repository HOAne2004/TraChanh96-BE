using System.Linq.Expressions;

namespace drinking_be.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        // Đọc dữ liệu
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);

        // Thêm, Sửa, Xóa
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        // Lưu thay đổi vào CSDL
        Task<int> SaveChangesAsync();

        // Tìm kiếm nâng cao
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null); 
        Task AddRangeAsync(IEnumerable<T> entities); // Cho phép thêm nhiều
        void DeleteRange(IEnumerable<T> entities); // Cho phép xóa nhiều
    }
}
// Interfaces/IMembershipService.cs
using drinking_be.Dtos.MembershipDtos;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface IMembershipService
    {
        // Lấy thông tin thành viên của người dùng (dựa trên token)
        Task<MembershipReadDto?> GetMyMembershipAsync(int userId);

        // (Chúng ta sẽ thêm các phương thức cập nhật/tạo sau khi tích hợp với Order/Auth)
    }
}
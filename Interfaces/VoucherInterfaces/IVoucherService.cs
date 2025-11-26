// Interfaces/IVoucherService.cs
using drinking_be.Dtos.VoucherDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface IVoucherService
    {
        // --- Admin (Template) ---
        Task<IEnumerable<VoucherTemplateReadDto>> GetAllTemplatesAsync();
        Task<VoucherTemplateReadDto> CreateTemplateAsync(VoucherTemplateCreateDto templateDto);
        Task<VoucherTemplateReadDto?> GetTemplateByIdAsync(int id);

        // --- User Voucher (MỚI) ---

        // [ADMIN] Phát hành voucher cho User
        Task<UserVoucherReadDto> IssueVoucherAsync(VoucherIssueDto issueDto);

        // [USER] Lấy danh sách voucher của tôi
        Task<IEnumerable<UserVoucherReadDto>> GetUserVouchersAsync(int userId);

        // [USER/SYSTEM] Áp dụng voucher
        Task<VoucherApplyResultDto> ApplyVoucherAsync(int userId, VoucherApplyDto applyDto);

        // [SYSTEM] Đánh dấu voucher đã sử dụng (gọi bởi OrderService)
        Task MarkVoucherAsUsedAsync(string voucherCode);
    }
}
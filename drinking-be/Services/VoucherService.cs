// Services/VoucherService.cs (CẬP NHẬT)
using drinking_be.Dtos.VoucherDtos;
using drinking_be.Interfaces;
using drinking_be.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace drinking_be.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherTemplateRepository _templateRepo;
        private readonly IUserVoucherRepository _userVoucherRepo; // THÊM
        private readonly IGenericRepository<User> _userRepo; // THÊM
        private readonly IMapper _mapper;

        public VoucherService(IVoucherTemplateRepository templateRepo,
                              IUserVoucherRepository userVoucherRepo, // THÊM
                              IGenericRepository<User> userRepo, // THÊM
                              IMapper mapper)
        {
            _templateRepo = templateRepo;
            _userVoucherRepo = userVoucherRepo; // KHỞI TẠO
            _userRepo = userRepo; // KHỞI TẠO
            _mapper = mapper;
        }
        // --- Admin (Template) ---

        public async Task<IEnumerable<VoucherTemplateReadDto>> GetAllTemplatesAsync()
        {
            var templates = await _templateRepo.GetAllWithLevelAsync();
            return _mapper.Map<IEnumerable<VoucherTemplateReadDto>>(templates);
        }

        public async Task<VoucherTemplateReadDto?> GetTemplateByIdAsync(int id)
        {
            var template = await _templateRepo.GetByIdWithLevelAsync(id);
            if (template == null)
            {
                throw new KeyNotFoundException("Không tìm thấy mẫu voucher.");
            }
            return _mapper.Map<VoucherTemplateReadDto>(template);
        }

        public async Task<VoucherTemplateReadDto> CreateTemplateAsync(VoucherTemplateCreateDto templateDto)
        {
            // TODO: Thêm logic xác thực LevelId có tồn tại hay không
            // TODO: Thêm logic xác thực DiscountType ("Percent" hoặc "Fixed")

            if (templateDto.EndDate <= templateDto.StartDate)
            {
                throw new Exception("Ngày kết thúc phải sau ngày bắt đầu.");
            }

            var template = _mapper.Map<VoucherTemplate>(templateDto);

            await _templateRepo.AddAsync(template);
            await _templateRepo.SaveChangesAsync();

            // Cần lấy lại dữ liệu với Include(Level) để trả về DTO đầy đủ
            var createdTemplate = await _templateRepo.GetByIdWithLevelAsync(template.Id);

            return _mapper.Map<VoucherTemplateReadDto>(createdTemplate);
        }

        // ----------------------------------------------------------------
        // --- LOGIC USER VOUCHER (MỚI) ---
        // ----------------------------------------------------------------

        /// <summary>
        /// [ADMIN] Phát hành voucher cho User
        /// </summary>
        public async Task<UserVoucherReadDto> IssueVoucherAsync(VoucherIssueDto issueDto)
        {
            // 1. Kiểm tra Template và User
            var template = await _templateRepo.GetByIdAsync(issueDto.VoucherTemplateId);
            if (template == null)
            {
                throw new KeyNotFoundException("Mẫu voucher không tồn tại.");
            }

            var user = await _userRepo.GetByIdAsync(issueDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException("Người dùng không tồn tại.");
            }

            // 2. Tạo UserVoucher Entity
            var userVoucher = new UserVoucher
            {
                UserId = issueDto.UserId,
                VoucherTemplateId = issueDto.VoucherTemplateId,
                // Tạo mã code ngẫu nhiên (ví dụ)
                VoucherCode = $"{template.Name.Substring(0, 3).ToUpper()}-{Guid.NewGuid().ToString().Substring(0, 8)}",
                IssuedDate = DateTime.UtcNow,
                ExpiryDate = template.EndDate, // Lấy hạn dùng từ Template
                Status = 1 // 1: Unused
            };

            // 3. Lưu vào DB
            await _userVoucherRepo.AddAsync(userVoucher);
            await _userVoucherRepo.SaveChangesAsync();

            // 4. Ánh xạ DTO (cần gán Template thủ công để AutoMapper hoạt động)
            userVoucher.VoucherTemplate = template; // Gán lại Template để AutoMapper đọc được
            return _mapper.Map<UserVoucherReadDto>(userVoucher);
        }

        /// <summary>
        /// [USER] Lấy danh sách voucher của tôi
        /// </summary>
        public async Task<IEnumerable<UserVoucherReadDto>> GetUserVouchersAsync(int userId)
        {
            var vouchers = await _userVoucherRepo.GetValidVouchersByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<UserVoucherReadDto>>(vouchers);
        }

        /// <summary>
        /// [USER/SYSTEM] Áp dụng voucher
        /// </summary>
        public async Task<VoucherApplyResultDto> ApplyVoucherAsync(int userId, VoucherApplyDto applyDto)
        {
            var voucher = await _userVoucherRepo.GetValidVoucherByCodeAsync(applyDto.VoucherCode);

            // 1. Kiểm tra Voucher có tồn tại và hợp lệ không
            if (voucher == null)
            {
                throw new Exception("Mã voucher không hợp lệ hoặc đã hết hạn.");
            }

            // 2. Kiểm tra Voucher có thuộc về User này không
            if (voucher.UserId != userId)
            {
                throw new Exception("Mã voucher này không thuộc về bạn.");
            }

            // 3. Kiểm tra điều kiện đơn hàng tối thiểu
            if (applyDto.OrderTotalAmount < voucher.VoucherTemplate.MinOrderValue)
            {
                throw new Exception($"Đơn hàng phải đạt tối thiểu {voucher.VoucherTemplate.MinOrderValue}đ để áp dụng voucher này.");
            }

            // 4. Tính toán giảm giá
            decimal discountAmount = 0;
            if (voucher.VoucherTemplate.DiscountType == "Fixed")
            {
                discountAmount = voucher.VoucherTemplate.DiscountValue;
            }
            else if (voucher.VoucherTemplate.DiscountType == "Percent")
            {
                discountAmount = (applyDto.OrderTotalAmount * voucher.VoucherTemplate.DiscountValue) / 100;

                // Kiểm tra mức giảm tối đa
                if (voucher.VoucherTemplate.MaxDiscountAmount.HasValue &&
                    discountAmount > voucher.VoucherTemplate.MaxDiscountAmount.Value)
                {
                    discountAmount = voucher.VoucherTemplate.MaxDiscountAmount.Value;
                }
            }

            // Đảm bảo không giảm giá nhiều hơn tổng tiền hàng
            discountAmount = Math.Min(applyDto.OrderTotalAmount, discountAmount);

            return new VoucherApplyResultDto
            {
                DiscountAmount = discountAmount,
                FinalAmount = applyDto.OrderTotalAmount - discountAmount,
                VoucherCode = voucher.VoucherCode
            };
        }

        /// <summary>
        /// [SYSTEM] Đánh dấu voucher đã sử dụng (gọi bởi OrderService)
        /// </summary>
        public async Task MarkVoucherAsUsedAsync(string voucherCode)
        {
            var voucher = await _userVoucherRepo.GetValidVoucherByCodeAsync(voucherCode);
            if (voucher != null)
            {
                voucher.Status = 2; // 2: Used
                voucher.UsedDate = DateTime.UtcNow;
                _userVoucherRepo.Update(voucher);
                await _userVoucherRepo.SaveChangesAsync();

                // TODO: Cập nhật UsedCount trong VoucherTemplate (nếu cần)
            }
        }
    }
}
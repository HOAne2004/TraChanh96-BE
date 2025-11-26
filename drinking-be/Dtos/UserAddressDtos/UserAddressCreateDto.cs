// Dtos/UserAddressDtos/UserAddressCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.UserAddressDtos
{
    // DTO dùng cho POST và PUT
    public class UserAddressCreateDto
    {
        [Required(ErrorMessage = "Địa chỉ đầy đủ là bắt buộc.")]
        [MaxLength(500)]
        public string FullAddress { get; set; } = string.Empty;

        // Chỉ định địa chỉ này là mặc định
        public bool IsDefault { get; set; } = false;
    }
}
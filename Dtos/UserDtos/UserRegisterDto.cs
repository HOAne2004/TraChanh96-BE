// Dtos/UserDtos/UserRegisterDto.cs
using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.UserDtos
{
    public class UserRegisterDto
    {
        [Required]
        [MinLength(4), MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6), MaxLength(50)]
        public string Password { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; }
    }
}
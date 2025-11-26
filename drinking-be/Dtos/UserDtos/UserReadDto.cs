// Dtos/UserDtos/UserReadDto.cs
using System;

namespace drinking_be.Dtos.UserDtos
{
    public class UserReadDto
    {
        public Guid PublicId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string Role { get; set; } = string.Empty;
        public int CurrentCoins { get; set; }
        public bool EmailVerified { get; set; }
        public byte? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
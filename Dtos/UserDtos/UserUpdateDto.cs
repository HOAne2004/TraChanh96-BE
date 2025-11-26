namespace drinking_be.Dtos.UserDtos
{
    public class UserUpdateDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string Role { get; set; } = string.Empty;
        public byte? Status { get; set; }
    }
}

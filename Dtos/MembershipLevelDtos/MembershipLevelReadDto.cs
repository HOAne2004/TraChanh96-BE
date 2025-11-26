// Dtos/MembershipLevelDtos/MembershipLevelReadDto.cs

namespace drinking_be.Dtos.MembershipLevelDtos
{
    public class MembershipLevelReadDto
    {
        public byte Id { get; set; } // TINYINT
        public string Name { get; set; } = string.Empty;
        public decimal MinSpendRequired { get; set; }
        public short DurationDays { get; set; }
        public string? Benefits { get; set; } // Giữ là chuỗi (JSON)
        public DateTime CreatedAt { get; set; }
    }
}
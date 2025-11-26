// Dtos/MembershipDtos/MembershipReadDto.cs
using System;

namespace drinking_be.Dtos.MembershipDtos
{
    public class MembershipReadDto
    {
        public long Id { get; set; }
        public string CardCode { get; set; } = string.Empty; // Mã thẻ
        public decimal TotalSpent { get; set; } // Tổng chi tiêu
        public DateTime LevelStartDate { get; set; }
        public DateTime LevelEndDate { get; set; }

        // Thông tin Cấp độ (từ Join)
        public byte LevelId { get; set; }
        public string LevelName { get; set; } = string.Empty;

        // Thông tin Người dùng (từ Join)
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
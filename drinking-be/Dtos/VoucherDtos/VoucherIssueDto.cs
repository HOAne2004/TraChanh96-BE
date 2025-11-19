// Dtos/VoucherDtos/VoucherIssueDto.cs
using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.VoucherDtos
{
    public class VoucherIssueDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int VoucherTemplateId { get; set; }
    }
}
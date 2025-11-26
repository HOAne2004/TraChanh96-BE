// Dtos/PolicyDtos/PolicyCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.PolicyDtos
{
    public class PolicyCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int BrandId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
// Dtos/PaymentMethodDtos/PaymentMethodCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.PaymentMethodDtos
{
    public class PaymentMethodCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
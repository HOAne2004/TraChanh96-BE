// Dtos/BrandDtos/BrandCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.BrandDtos
{
    public class BrandCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? LogoUrl { get; set; }
        public string? Address { get; set; }
        public string? Hotline { get; set; }
        public string? EmailSupport { get; set; }
        public string? TaxCode { get; set; }
        public string? CompanyName { get; set; }
        public string? Slogan { get; set; }
        public string? CopyrightText { get; set; }
    }
}
// Dtos/BrandDtos/BrandReadDto.cs
using System;

namespace drinking_be.Dtos.BrandDtos
{
    public class BrandReadDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? Address { get; set; }
        public string? Hotline { get; set; }
        public string? EmailSupport { get; set; }
        public string? TaxCode { get; set; }
        public string? CompanyName { get; set; }
        public string? Slogan { get; set; }
        public string? CopyrightText { get; set; }

        public List<SocialMediaDto> SocialMedia { get; set; } = new List<SocialMediaDto>();
    }
}
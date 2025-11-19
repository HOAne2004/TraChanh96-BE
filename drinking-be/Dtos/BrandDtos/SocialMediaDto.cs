namespace drinking_be.Dtos.BrandDtos
{
    public class SocialMediaDto
    {
        public string PlatformName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? IconUrl { get; set; }
    }
}
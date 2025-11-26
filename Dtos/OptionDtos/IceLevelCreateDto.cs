using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.OptionDtos
{
    public class IceLevelCreateDto
    {
        [Required]
        [MaxLength(20)]
        public string Label { get; set; } = string.Empty;

        public int Value { get; set; }
    }
}

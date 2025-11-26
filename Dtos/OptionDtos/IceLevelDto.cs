// Dtos/OptionDtos/IceLevelDto.cs
namespace drinking_be.Dtos.OptionDtos
{
    public class IceLevelDto
    {
        public short Id { get; set; }
        public string Label { get; set; } = string.Empty;
        public short Value { get; set; }
    }
}
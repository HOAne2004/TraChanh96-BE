namespace drinking_be.Dtos.OptionDtos
{
    // Tạo một DTO đơn giản cho Size (nếu cần)
    public class SizeReadDto
    {
        public short Id { get; set; }
        public string Label { get; set; } = string.Empty;
        public decimal PriceModifier { get; set; }
    }
}

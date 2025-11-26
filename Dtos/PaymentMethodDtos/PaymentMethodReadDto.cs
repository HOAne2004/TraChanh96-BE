// Dtos/PaymentMethodDtos/PaymentMethodReadDto.cs

namespace drinking_be.Dtos.PaymentMethodDtos
{
    public class PaymentMethodReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
// Models/ProductIceLevel.cs
namespace drinking_be.Models
{
    public class ProductIceLevel
    {
        public int ProductId { get; set; }
        public short IceLevelId { get; set; }

        public Product Product { get; set; } = null!;
        public IceLevel IceLevel { get; set; } = null!;
    }
}
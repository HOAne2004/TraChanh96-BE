// Models/ProductSugarLevel.cs
namespace drinking_be.Models
{
    public class ProductSugarLevel
    {
        public int ProductId { get; set; }
        public short SugarLevelId { get; set; }

        public Product Product { get; set; } = null!;
        public SugarLevel SugarLevel { get; set; } = null!;
    }
}
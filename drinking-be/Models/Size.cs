using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace drinking_be.Models;

public partial class Size
{
    public short Id { get; set; }

    public string Label { get; set; } = null!;

    public decimal? PriceModifier { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty(nameof(ProductSize.Size))]
    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
}

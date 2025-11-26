using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace drinking_be.Models;

public partial class SugarLevel
{
    public short Id { get; set; }

    public string Label { get; set; } = null!;

    public short Value { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty(nameof(ProductSugarLevel.SugarLevel))]
    public virtual ICollection<ProductSugarLevel> ProductSugarLevels { get; set; } = new List<ProductSugarLevel>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace drinking_be.Models;

public partial class IceLevel
{
    public short Id { get; set; }

    public string Label { get; set; } = null!;

    public short Value { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty(nameof(ProductIceLevel.IceLevel))]
    public virtual ICollection<ProductIceLevel> ProductIceLevels { get; set; } = new List<ProductIceLevel>();
}

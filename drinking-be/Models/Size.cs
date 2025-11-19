using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("Size")]
public partial class Size
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("label")]
    [StringLength(20)]
    public string Label { get; set; } = null!;

    [Column("price_modifier", TypeName = "decimal(5, 2)")]
    public decimal? PriceModifier { get; set; }

    [InverseProperty("Size")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [InverseProperty("Size")]
    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
}

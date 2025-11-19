using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("Sugar_Level")]
public partial class SugarLevel
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("label")]
    [StringLength(20)]
    public string Label { get; set; } = null!;

    [Column("value")]
    public short Value { get; set; }

    [InverseProperty("SugarLevel")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [InverseProperty("SugarLevel")]
    public virtual ICollection<ProductSugarLevel> ProductSugarLevels { get; set; } = new List<ProductSugarLevel>();
}

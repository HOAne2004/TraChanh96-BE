using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("Ice_Level")]
public partial class IceLevel
{
    [Key]
    [Column("id")]
    public short Id { get; set; }

    [Column("label")]
    [StringLength(20)]
    public string Label { get; set; } = null!;

    [Column("value")]
    public short Value { get; set; }

    [InverseProperty("IceLevel")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();


    [InverseProperty("IceLevel")]
    public virtual ICollection<ProductIceLevel> ProductIceLevels { get; set; } = new List<ProductIceLevel>();
}

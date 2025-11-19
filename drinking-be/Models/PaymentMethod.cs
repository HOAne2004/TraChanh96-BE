using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("Payment_Method")]
public partial class PaymentMethod
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("image_url")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ImageUrl { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [InverseProperty("PaymentMethod")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

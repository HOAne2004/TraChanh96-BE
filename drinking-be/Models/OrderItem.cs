using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("Order_item")]
public partial class OrderItem
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("order_id")]
    public long OrderId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("base_price", TypeName = "decimal(10, 2)")]
    public decimal BasePrice { get; set; }

    [Column("final_price", TypeName = "decimal(10, 2)")]
    public decimal FinalPrice { get; set; }

    [Column("note")]
    [StringLength(255)]
    public string? Note { get; set; }

    [Column("parent_item_id")]
    public long? ParentItemId { get; set; }

    [Column("size_id")]
    public short? SizeId { get; set; }

    [Column("sugar_level_id")]
    public short? SugarLevelId { get; set; }

    [Column("ice_level_id")]
    public short? IceLevelId { get; set; }

    [ForeignKey("IceLevelId")]
    [InverseProperty("OrderItems")]
    public virtual IceLevel? IceLevel { get; set; }

    [InverseProperty("ParentItem")]
    public virtual ICollection<OrderItem> InverseParentItem { get; set; } = new List<OrderItem>();

    [ForeignKey("OrderId")]
    [InverseProperty("OrderItems")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ParentItemId")]
    [InverseProperty("InverseParentItem")]
    public virtual OrderItem? ParentItem { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("OrderItems")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("SizeId")]
    [InverseProperty("OrderItems")]
    public virtual Size? Size { get; set; }

    [ForeignKey("SugarLevelId")]
    [InverseProperty("OrderItems")]
    public virtual SugarLevel? SugarLevel { get; set; }
}

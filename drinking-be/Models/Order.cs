using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("Order")]
[Index("OrderCode", Name = "UQ__Order__99D12D3F9A37D2CB", IsUnique = true)]
public partial class Order
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("order_code")]
    [StringLength(50)]
    [Unicode(false)]
    public string OrderCode { get; set; } = null!;

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("store_id")]
    public int StoreId { get; set; }

    [Column("payment_method_id")]
    public int? PaymentMethodId { get; set; }

    [Column("order_date", TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [Column("delivery_date", TypeName = "datetime")]
    public DateTime? DeliveryDate { get; set; }

    [Column("total_amount", TypeName = "decimal(10, 2)")]
    public decimal TotalAmount { get; set; }

    [Column("discount_amount", TypeName = "decimal(10, 2)")]
    public decimal? DiscountAmount { get; set; }

    [Column("shipping_fee", TypeName = "decimal(8, 2)")]
    public decimal? ShippingFee { get; set; }

    [Column("grand_total", TypeName = "decimal(10, 2)")]
    public decimal GrandTotal { get; set; }

    [Column("coins_earned")]
    public int? CoinsEarned { get; set; }

    [Column("status")]
    public byte? Status { get; set; }

    [Column("delivery_address")]
    [StringLength(500)]
    public string DeliveryAddress { get; set; } = null!;

    [Column("customer_phone")]
    [StringLength(20)]
    [Unicode(false)]
    public string CustomerPhone { get; set; } = null!;

    [Column("customer_name")]
    [StringLength(100)]
    public string CustomerName { get; set; } = null!;

    [Column("voucher_code_used")]
    [StringLength(20)]
    [Unicode(false)]
    public string? VoucherCodeUsed { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [ForeignKey("PaymentMethodId")]
    [InverseProperty("Orders")]
    public virtual PaymentMethod? PaymentMethod { get; set; }

    [ForeignKey("StoreId")]
    [InverseProperty("Orders")]
    public virtual Store Store { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User? User { get; set; }
}

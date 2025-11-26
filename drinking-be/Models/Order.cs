using System;
using System.Collections.Generic;

namespace drinking_be.Models;

public partial class Order
{
    public long Id { get; set; }

    public string OrderCode { get; set; } = null!;

    public int? UserId { get; set; }

    public int StoreId { get; set; }

    public int? PaymentMethodId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? ShippingFee { get; set; }

    public decimal GrandTotal { get; set; }

    public int? CoinsEarned { get; set; }

    public byte? Status { get; set; }

    public string DeliveryAddress { get; set; } = null!;

    public string CustomerPhone { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public string? VoucherCodeUsed { get; set; }

    public string? StoreName { get; set; }

    public string? UserNotes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual PaymentMethod? PaymentMethod { get; set; }

    public virtual Store Store { get; set; } = null!;

    public virtual User? User { get; set; }
}

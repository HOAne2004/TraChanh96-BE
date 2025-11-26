using System;
using System.Collections.Generic;

namespace drinking_be.Models;

public partial class PaymentMethod
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public bool? IsActive { get; set; }

    public byte? SortOrder { get; set; }

    public decimal? ProcessingFee { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

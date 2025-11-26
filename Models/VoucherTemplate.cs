using System;
using System.Collections.Generic;

namespace drinking_be.Models;

public partial class VoucherTemplate
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public byte? LevelId { get; set; }

    public decimal DiscountValue { get; set; }

    public string DiscountType { get; set; } = null!;

    public decimal? MinOrderValue { get; set; }

    public decimal? MaxDiscountAmount { get; set; }

    public byte? QuantityPerLevel { get; set; }

    public int? UsageLimit { get; set; }

    public int? UsedCount { get; set; }

    public bool? IsActive { get; set; }

    public byte? UsageLimitPerUser { get; set; }

    public string? CouponCode { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual MembershipLevel? Level { get; set; }

    public virtual ICollection<UserVoucher> UserVouchers { get; set; } = new List<UserVoucher>();
}

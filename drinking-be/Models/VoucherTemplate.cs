using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("Voucher_Template")]
public partial class VoucherTemplate
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("level_id")]
    public byte? LevelId { get; set; }

    [Column("discount_value", TypeName = "decimal(5, 2)")]
    public decimal DiscountValue { get; set; }

    [Column("discount_type")]
    [StringLength(10)]
    [Unicode(false)]
    public string DiscountType { get; set; } = null!;

    [Column("min_order_value", TypeName = "decimal(10, 2)")]
    public decimal? MinOrderValue { get; set; }

    [Column("max_discount_amount", TypeName = "decimal(10, 2)")]
    public decimal? MaxDiscountAmount { get; set; }

    [Column("quantity_per_level")]
    public byte? QuantityPerLevel { get; set; }

    [Column("usage_limit")]
    public int? UsageLimit { get; set; }

    [Column("used_count")]
    public int? UsedCount { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("start_date", TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column("end_date", TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("LevelId")]
    [InverseProperty("VoucherTemplates")]
    public virtual MembershipLevel? Level { get; set; }

    [InverseProperty("VoucherTemplate")]
    public virtual ICollection<UserVoucher> UserVouchers { get; set; } = new List<UserVoucher>();
}

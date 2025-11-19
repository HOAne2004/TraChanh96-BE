using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("Membership_Level")]
[Index("Name", Name = "UQ__Membersh__72E12F1B815D04AC", IsUnique = true)]
public partial class MembershipLevel
{
    [Key]
    [Column("id")]
    public byte Id { get; set; }

    [Column("name")]
    [StringLength(35)]
    public string Name { get; set; } = null!;

    [Column("min_spend_required", TypeName = "decimal(10, 2)")]
    public decimal MinSpendRequired { get; set; }

    [Column("duration_days")]
    public short DurationDays { get; set; }

    [Column("benefits")]
    public string? Benefits { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Level")]
    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    [InverseProperty("Level")]
    public virtual ICollection<VoucherTemplate> VoucherTemplates { get; set; } = new List<VoucherTemplate>();
}

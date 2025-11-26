using System;
using System.Collections.Generic;

namespace drinking_be.Models;

public partial class MembershipLevel
{
    public byte Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal MinSpendRequired { get; set; }

    public short DurationDays { get; set; }

    public string? Benefits { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    public virtual ICollection<VoucherTemplate> VoucherTemplates { get; set; } = new List<VoucherTemplate>();
}

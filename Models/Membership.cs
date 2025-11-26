using System;
using System.Collections.Generic;

namespace drinking_be.Models;

public partial class Membership
{
    public long Id { get; set; }

    public int UserId { get; set; }

    public string CardCode { get; set; } = null!;

    public byte LevelId { get; set; }

    public decimal? TotalSpent { get; set; }

    public DateOnly? LevelStartDate { get; set; }

    public DateOnly LevelEndDate { get; set; }

    public DateOnly? LastLevelSpentReset { get; set; }

    public byte? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual MembershipLevel Level { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

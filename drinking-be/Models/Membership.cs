using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("Membership")]
[Index("CardCode", Name = "UQ__Membersh__81703D7218B9CEEC", IsUnique = true)]
[Index("UserId", Name = "UQ__Membersh__B9BE370E50C42904", IsUnique = true)]
public partial class Membership
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("card_code")]
    [StringLength(50)]
    [Unicode(false)]
    public string CardCode { get; set; } = null!;

    [Column("level_id")]
    public byte LevelId { get; set; }

    [Column("total_spent", TypeName = "decimal(12, 2)")]
    public decimal? TotalSpent { get; set; }

    [Column("level_start_date")]
    public DateOnly? LevelStartDate { get; set; }

    [Column("level_end_date")]
    public DateOnly LevelEndDate { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("LevelId")]
    [InverseProperty("Memberships")]
    public virtual MembershipLevel Level { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Membership")]
    public virtual User User { get; set; } = null!;
}

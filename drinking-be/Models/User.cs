using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("User")]
[Index("PublicId", Name = "UQ__User__5699A530A49B065B", IsUnique = true)]
[Index("Email", Name = "UQ__User__AB6E6164996BBF93", IsUnique = true)]
[Index("Username", Name = "UQ__User__F3DBC572914F5728", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("public_id")]
    public Guid? PublicId { get; set; }

    [Column("role_id")]
    public byte RoleId { get; set; }

    [Column("username")]
    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("thumbnail_url")]
    [StringLength(200)]
    [Unicode(false)]
    public string? ThumbnailUrl { get; set; }

    [Column("email")]
    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("phone")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [Column("password_hash")]
    [StringLength(255)]
    [Unicode(false)]
    public string PasswordHash { get; set; } = null!;

    [Column("current_coins")]
    public int? CurrentCoins { get; set; }

    [Column("email_verified")]
    public bool? EmailVerified { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [Column("last_login", TypeName = "datetime")]
    public DateTime? LastLogin { get; set; }

    [InverseProperty("User")]
    public virtual Cart? Cart { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [InverseProperty("User")]
    public virtual Membership? Membership { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<News> News { get; set; } = new List<News>();

    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("User")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [InverseProperty("User")]
    public virtual ICollection<UserVoucher> UserVouchers { get; set; } = new List<UserVoucher>();
}

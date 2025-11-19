using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Index("Slug", Name = "UQ__News__32DD1E4C6E82B36D", IsUnique = true)]
[Index("PublicId", Name = "UQ__News__5699A5301974685B", IsUnique = true)]
public partial class News
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("public_id")]
    public Guid? PublicId { get; set; }

    [Column("slug")]
    [StringLength(200)]
    public string? Slug { get; set; }

    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("content")]
    public string Content { get; set; } = null!;

    [Column("thumbnail_url")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ThumbnailUrl { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string? Status { get; set; }

    [Column("published_date", TypeName = "datetime")]
    public DateTime? PublishedDate { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("News")]
    public virtual NewsCategory Category { get; set; } = null!;

    [InverseProperty("News")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [ForeignKey("UserId")]
    [InverseProperty("News")]
    public virtual User User { get; set; } = null!;
}

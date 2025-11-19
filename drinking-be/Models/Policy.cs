using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("Policy")]
[Index("Slug", Name = "UQ__Policy__32DD1E4C8BE28374", IsUnique = true)]
public partial class Policy
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("slug")]
    [StringLength(100)]
    public string Slug { get; set; } = null!;

    [Column("brand_id")]
    public int BrandId { get; set; }

    [Column("title")]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    [Column("content")]
    public string Content { get; set; } = null!;

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("BrandId")]
    [InverseProperty("Policies")]
    public virtual Brand Brand { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("News_Category")]
[Index("Slug", Name = "UQ__News_Cat__32DD1E4C78FF7627", IsUnique = true)]
public partial class NewsCategory
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("slug")]
    [StringLength(100)]
    public string Slug { get; set; } = null!;

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<News> News { get; set; } = new List<News>();
}

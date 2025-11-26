using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("Social_media")]
public partial class SocialMedia
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("brand_id")]
    public int BrandId { get; set; }

    [Column("platform_name")]
    [StringLength(30)]
    [Unicode(false)]
    public string PlatformName { get; set; } = null!;

    [Column("url")]
    [StringLength(500)]
    [Unicode(false)]
    public string Url { get; set; } = null!;

    [Column("icon_url")]
    [StringLength(500)]
    [Unicode(false)]
    public string? IconUrl { get; set; }

    [Column("sort_order")]
    public byte? SortOrder { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [ForeignKey("BrandId")]
    [InverseProperty("SocialMedia")]
    public virtual Brand Brand { get; set; } = null!;
}

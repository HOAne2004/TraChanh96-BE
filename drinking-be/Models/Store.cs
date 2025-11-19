using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("Store")]
[Index("Slug", Name = "UQ__Store__32DD1E4CDD2AEBCA", IsUnique = true)]
[Index("PublicId", Name = "UQ__Store__5699A5306604AD10", IsUnique = true)]
public partial class Store
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("public_id")]
    public Guid? PublicId { get; set; }

    [Column("slug")]
    [StringLength(200)]
    public string? Slug { get; set; }

    [Column("brand_id")]
    public int BrandId { get; set; }

    [Column("name")]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Column("image_url")]
    [StringLength(255)]
    [Unicode(false)]
    public string? ImageUrl { get; set; }

    [Column("address")]
    [StringLength(200)]
    public string Address { get; set; } = null!;

    [Column("latitude")]
    public double? Latitude { get; set; }

    [Column("longitude")]
    public double? Longitude { get; set; }

    [Column("open_time")]
    public TimeOnly? OpenTime { get; set; }

    [Column("close_time")]
    public TimeOnly? CloseTime { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("BrandId")]
    [InverseProperty("Stores")]
    public virtual Brand Brand { get; set; } = null!;

    [InverseProperty("Store")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

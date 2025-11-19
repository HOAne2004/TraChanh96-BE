using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("Brand")]
public partial class Brand
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("logo_url")]
    [StringLength(500)]
    [Unicode(false)]
    public string? LogoUrl { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("hotline")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Hotline { get; set; }

    [Column("email_support")]
    [StringLength(100)]
    [Unicode(false)]
    public string? EmailSupport { get; set; }

    [Column("tax_code")]
    [StringLength(30)]
    [Unicode(false)]
    public string? TaxCode { get; set; }

    [Column("company_name")]
    [StringLength(100)]
    public string? CompanyName { get; set; }

    [Column("slogan")]
    [StringLength(255)]
    public string? Slogan { get; set; }

    [Column("copyright_text")]
    [StringLength(255)]
    public string? CopyrightText { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Brand")]
    public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();

    [InverseProperty("Brand")]
    public virtual ICollection<SocialMedia> SocialMedia { get; set; } = new List<SocialMedia>();

    [InverseProperty("Brand")]
    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}

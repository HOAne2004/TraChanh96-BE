using System;
using System.Collections.Generic;

namespace drinking_be.Models;

public partial class Policy
{
    public int Id { get; set; }

    public string Slug { get; set; } = null!;

    public int BrandId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Brand Brand { get; set; } = null!;
}

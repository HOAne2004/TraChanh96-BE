using System;
using System.Collections.Generic;

namespace drinking_be.Models;

public partial class News
{
    public int Id { get; set; }

    public Guid? PublicId { get; set; }

    public string? Slug { get; set; }

    public string Type { get; set; } = null!;

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? ThumbnailUrl { get; set; }

    public string? Status { get; set; }

    public bool? IsFeatured { get; set; }

    public string? SeoDescription { get; set; }

    public DateTime? PublishedDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual User User { get; set; } = null!;
}

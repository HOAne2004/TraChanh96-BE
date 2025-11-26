using System;
using System.Collections.Generic;

namespace drinking_be.Models;

public partial class NewsCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();
}

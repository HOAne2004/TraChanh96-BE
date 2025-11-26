using System;
using System.Collections.Generic;

namespace drinking_be.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public int UserId { get; set; }

    public int NewsId { get; set; }

    public string Content { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Comment> InverseParent { get; set; } = new List<Comment>();

    public virtual News News { get; set; } = null!;

    public virtual Comment? Parent { get; set; }

    public virtual User User { get; set; } = null!;
}

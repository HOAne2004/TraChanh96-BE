using System;
using System.Collections.Generic;

namespace drinking_be.Models;

public partial class Review
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public string? Content { get; set; }

    public byte Rating { get; set; }

    public string? Status { get; set; }

    public string? MediaUrl { get; set; }

    public string? AdminResponse { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

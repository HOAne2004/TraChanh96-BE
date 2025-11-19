using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("Comment")]
public partial class Comment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("parent_id")]
    public int? ParentId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("news_id")]
    public int NewsId { get; set; }

    [Column("content")]
    [StringLength(500)]
    public string Content { get; set; } = null!;

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Parent")]
    public virtual ICollection<Comment> InverseParent { get; set; } = new List<Comment>();

    [ForeignKey("NewsId")]
    [InverseProperty("Comments")]
    public virtual News News { get; set; } = null!;

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual Comment? Parent { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Comments")]
    public virtual User User { get; set; } = null!;
}

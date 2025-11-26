using System;
using System.Collections.Generic;

namespace drinking_be.Models;

public partial class CartItem
{
    public long Id { get; set; }

    public long CartId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal BasePrice { get; set; }

    public decimal FinalPrice { get; set; }

    public string? Note { get; set; }

    public long? ParentItemId { get; set; }

    public short? SizeId { get; set; }

    public short? SugarLevelId { get; set; }

    public short? IceLevelId { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual IceLevel? IceLevel { get; set; }

    public virtual ICollection<CartItem> InverseParentItem { get; set; } = new List<CartItem>();

    public virtual CartItem? ParentItem { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Size? Size { get; set; }

    public virtual SugarLevel? SugarLevel { get; set; }
}

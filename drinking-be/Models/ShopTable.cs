using System;
using System.Collections.Generic;

namespace drinking_be.Models;

public partial class ShopTable
{
    public int Id { get; set; }

    public int StoreId { get; set; }

    public string Name { get; set; } = null!;

    public byte Capacity { get; set; }

    public bool? CanBeMerged { get; set; }

    public int? MergedWithTableId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ShopTable> InverseMergedWithTable { get; set; } = new List<ShopTable>();

    public virtual ShopTable? MergedWithTable { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual Store Store { get; set; } = null!;
}

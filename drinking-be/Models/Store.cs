using System;
using System.Collections.Generic;

namespace drinking_be.Models;

public partial class Store
{
    public int Id { get; set; }

    public Guid? PublicId { get; set; }

    public string? Slug { get; set; }

    public int BrandId { get; set; }

    public string Name { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string Address { get; set; } = null!;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public TimeOnly? OpenTime { get; set; }

    public TimeOnly? CloseTime { get; set; }

    public bool? IsActive { get; set; }

    public byte? SortOrder { get; set; }

    public bool? MapVerified { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<ShopTable> ShopTables { get; set; } = new List<ShopTable>();
}

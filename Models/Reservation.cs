using System;
using System.Collections.Generic;

namespace drinking_be.Models;

public partial class Reservation
{
    public long Id { get; set; }

    public string ReservationCode { get; set; } = null!;

    public int? UserId { get; set; }

    public int StoreId { get; set; }

    public DateTime ReservationDatetime { get; set; }

    public byte NumberOfGuests { get; set; }

    public string CustomerName { get; set; } = null!;

    public string CustomerPhone { get; set; } = null!;

    public string? Note { get; set; }

    public byte? Status { get; set; }

    public int? AssignedTableId { get; set; }
    public decimal DepositAmount { get; set; }
    public bool IsDepositPaid { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ShopTable? AssignedTable { get; set; }

    public virtual Store Store { get; set; } = null!;

    public virtual User? User { get; set; }
}

using System;
using System.Collections.Generic;

namespace drinking_be.Models;

public partial class UserVoucher
{
    public long Id { get; set; }

    public int UserId { get; set; }

    public int VoucherTemplateId { get; set; }

    public string VoucherCode { get; set; } = null!;

    public DateTime? IssuedDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public byte? Status { get; set; }

    public DateTime? UsedDate { get; set; }

    public long? OrderIdUsed { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual VoucherTemplate VoucherTemplate { get; set; } = null!;
}

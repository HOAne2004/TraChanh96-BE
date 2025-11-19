using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("User_Voucher")]
[Index("VoucherCode", Name = "UQ__User_Vou__21731069273E3857", IsUnique = true)]
public partial class UserVoucher
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("voucher_template_id")]
    public int VoucherTemplateId { get; set; }

    [Column("voucher_code")]
    [StringLength(20)]
    [Unicode(false)]
    public string VoucherCode { get; set; } = null!;

    [Column("issued_date", TypeName = "datetime")]
    public DateTime? IssuedDate { get; set; }

    [Column("expiry_date", TypeName = "datetime")]
    public DateTime ExpiryDate { get; set; }

    [Column("status")]
    public byte? Status { get; set; }

    [Column("used_date", TypeName = "datetime")]
    public DateTime? UsedDate { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserVouchers")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("VoucherTemplateId")]
    [InverseProperty("UserVouchers")]
    public virtual VoucherTemplate VoucherTemplate { get; set; } = null!;
}

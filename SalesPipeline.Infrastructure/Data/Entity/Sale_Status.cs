using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// สถานะการขาย
/// </summary>
public partial class Sale_Status
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public string? CreateByName { get; set; }

    public Guid SaleId { get; set; }

    public int? StatusMainId { get; set; }

    /// <summary>
    /// สถานะการขายหลัก
    /// </summary>
    public string? StatusNameMain { get; set; }

    public int StatusId { get; set; }

    /// <summary>
    /// สถานะการขาย
    /// </summary>
    public string? StatusName { get; set; }

    public string? Description { get; set; }

    public Guid? Master_Reason_CloseSaleId { get; set; }

    public virtual User CreateByNavigation { get; set; } = null!;

    public virtual Master_Reason_CloseSale? Master_Reason_CloseSale { get; set; }

    public virtual Sale Sale { get; set; } = null!;

    public virtual Master_StatusSale StatusNavigation { get; set; } = null!;
}

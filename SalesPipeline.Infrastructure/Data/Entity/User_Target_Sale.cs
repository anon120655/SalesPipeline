using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// เป้ายอดการขาย
/// </summary>
public partial class User_Target_Sale
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdateBy { get; set; }

    /// <summary>
    /// พนักงาน
    /// </summary>
    public int UserId { get; set; }

    public int Year { get; set; }

    /// <summary>
    /// ยอดเป้าหมาย
    /// </summary>
    public decimal AmountTarget { get; set; }

    /// <summary>
    /// ยอดที่ทำได้
    /// </summary>
    public decimal AmountActual { get; set; }

    public virtual User CreateByNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

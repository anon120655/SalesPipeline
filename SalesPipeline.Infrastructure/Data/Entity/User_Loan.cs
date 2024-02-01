using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// พนักงานสินเชื่อภายใต้การดูแล
/// </summary>
public partial class User_Loan
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    /// <summary>
    /// พนักงานที่ดูแล
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// พนักงานสินเชื่อที่ดูแล
    /// </summary>
    public int UserLoanId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual User UserLoan { get; set; } = null!;
}

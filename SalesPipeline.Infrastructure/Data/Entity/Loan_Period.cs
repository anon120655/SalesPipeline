using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// รายละเอียดงวดสินเชื่อ
/// </summary>
public partial class Loan_Period
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    /// <summary>
    /// ชื่อสินเชื่อ
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// FK สินเชื่อ
    /// </summary>
    public Guid LoanId { get; set; }

    /// <summary>
    /// ระยะที่
    /// </summary>
    public int PeriodNo { get; set; }

    public virtual Loan Loan { get; set; } = null!;
}

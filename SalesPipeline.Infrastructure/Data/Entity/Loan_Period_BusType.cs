using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ประเภทธุรกิจในระยะที่
/// </summary>
public partial class Loan_Period_BusType
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid Loan_PeriodId { get; set; }

    public Guid Master_Pre_BusinessTypeId { get; set; }

    public string? Master_Pre_BusinessTypeName { get; set; }

    public virtual Loan_Period Loan_Period { get; set; } = null!;
}

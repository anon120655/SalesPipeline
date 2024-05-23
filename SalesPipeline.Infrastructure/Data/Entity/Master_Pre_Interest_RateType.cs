using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ประเภทอัตราดอกเบี้ย
/// </summary>
public partial class Master_Pre_Interest_RateType
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

    public string? Code { get; set; }

    /// <summary>
    /// ชื่อเต็ม
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// อัตราดอกเบี้ย
    /// </summary>
    public decimal? Rate { get; set; }

    public virtual ICollection<Loan_Period> Loan_Periods { get; set; } = new List<Loan_Period>();
}

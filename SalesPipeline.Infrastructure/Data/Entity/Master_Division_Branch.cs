using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ฝ่ายกิจการสาขา
/// </summary>
public partial class Master_Division_Branch
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
    /// รหัส
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// ชื่อฝ่าย
    /// </summary>
    public string? Name { get; set; }

    public virtual ICollection<Master_Division_Loan> Master_Division_Loans { get; set; } = new List<Master_Division_Loan>();
}

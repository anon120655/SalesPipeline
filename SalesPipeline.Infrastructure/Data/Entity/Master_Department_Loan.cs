using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ศูนย์ธุรกิจสินเชื่อ
/// </summary>
public partial class Master_Department_Loan
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
    /// FK ฝ่ายกิจการสาขาภาค
    /// </summary>
    public Guid? Master_Department_BranchId { get; set; }

    public string? Master_Department_BranchName { get; set; }

    /// <summary>
    /// รหัส
    /// </summary>
    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public virtual Master_Department_Branch? Master_Department_Branch { get; set; }
}

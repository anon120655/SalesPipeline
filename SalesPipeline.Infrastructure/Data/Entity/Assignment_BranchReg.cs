using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Assignment_BranchReg
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    /// <summary>
    /// กิจการสาขาภาค
    /// </summary>
    public Guid? Master_Branch_RegionId { get; set; }

    public int? BranchId { get; set; }

    /// <summary>
    /// รหัสสาขา
    /// </summary>
    public string? BranchCode { get; set; }

    /// <summary>
    /// ชื่อสาขา
    /// </summary>
    public string? BranchName { get; set; }

    /// <summary>
    /// UserId กิจการสาขาภาค
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// รหัสพนักงาน
    /// </summary>
    public string? EmployeeId { get; set; }

    /// <summary>
    /// ชื่อพนักงาน
    /// </summary>
    public string? EmployeeName { get; set; }

    public string? Tel { get; set; }

    /// <summary>
    /// จำนวนลูกค้าปัจจุบันที่ดูแล
    /// </summary>
    public int? CurrentNumber { get; set; }

    public virtual InfoBranch? Branch { get; set; }

    public virtual Master_Branch_Region? Master_Branch_Region { get; set; }

    public virtual User User { get; set; } = null!;
}

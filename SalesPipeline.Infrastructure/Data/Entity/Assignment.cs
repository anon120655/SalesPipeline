using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Assignment
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
    public Guid? Master_Department_BranchId { get; set; }

    /// <summary>
    /// รหัสศูนย์
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// ศูนย์สาขา
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// UserId ผู้จัดการศูนย์ที่ได้รับมอบหมาย
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// รหัสพนักงานผู้จัดการศูนย์
    /// </summary>
    public string? EmployeeId { get; set; }

    /// <summary>
    /// ชื่อผู้จัดการศูนย์
    /// </summary>
    public string? EmployeeName { get; set; }

    public string? Tel { get; set; }

    /// <summary>
    /// จำนวนพนง.สินเชื่อ
    /// </summary>
    public int? RMNumber { get; set; }

    /// <summary>
    /// จำนวนลูกค้าปัจจุบันที่ดูแล
    /// </summary>
    public int? CurrentNumber { get; set; }

    public virtual Master_Department_Branch? Master_Department_Branch { get; set; }

    public virtual User User { get; set; } = null!;
}

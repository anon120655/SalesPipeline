using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// พนักงานที่ถูกมอบหมาย
/// </summary>
public partial class Assignment
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    /// <summary>
    /// พนักงานที่ได้รับมอบหมาย
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// รหัสพนักงาน
    /// </summary>
    public string? EmployeeId { get; set; }

    /// <summary>
    /// ชื่อพนักงานที่ได้รับมอบหมาย
    /// </summary>
    public string? EmployeeName { get; set; }

    /// <summary>
    /// จำนวนลูกค้าปัจจุบันที่ดูแล
    /// </summary>
    public int? CurrentNumber { get; set; }

    public virtual ICollection<Assignment_Sale> Assignment_Sales { get; set; } = new List<Assignment_Sale>();

    public virtual User User { get; set; } = null!;
}

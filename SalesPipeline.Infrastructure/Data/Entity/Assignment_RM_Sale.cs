using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ลูกค้าที่พนักงานดูแล
/// </summary>
public partial class Assignment_RM_Sale
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    /// <summary>
    /// ผู้มอบหมาย
    /// </summary>
    public int CreateBy { get; set; }

    /// <summary>
    /// ชื่อผู้มอบหมาย
    /// </summary>
    public string? CreateByName { get; set; }

    public Guid AssignmentRMId { get; set; }

    /// <summary>
    /// 1=อยู่ในความรับผิดชอบ 0=ถูกเปลี่ยนผู้รับผิดชอบ
    /// </summary>
    public short IsActive { get; set; }

    /// <summary>
    /// สินเชื่อที่ถูกมอบหมาย
    /// </summary>
    public Guid SaleId { get; set; }

    public string? Description { get; set; }

    public virtual Assignment_RM AssignmentRM { get; set; } = null!;

    public virtual User CreateByNavigation { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}

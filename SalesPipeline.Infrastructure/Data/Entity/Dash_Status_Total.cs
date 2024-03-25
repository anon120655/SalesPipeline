using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Dash_Status_Total
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public bool IsUpdate { get; set; }

    public int UserId { get; set; }

    /// <summary>
    /// จำนวนลูกค้านำเข้าทั้งหมด
    /// </summary>
    public int NumCusAll { get; set; }

    /// <summary>
    /// รอผู้จัดการศูนย์มอบหมาย
    /// </summary>
    public int NumCusWaitMCenterAssign { get; set; }

    /// <summary>
    /// ผู้จัดการศูนย์มอบหมาย
    /// </summary>
    public int NumCusMCenterAssign { get; set; }

    /// <summary>
    /// อยู่ในกระบวนการ
    /// </summary>
    public int NumCusInProcess { get; set; }

    /// <summary>
    /// รายการส่งกลับ
    /// </summary>
    public int NumCusReturn { get; set; }

    /// <summary>
    /// พนักงานที่ไม่บรรลุเป้าหมาย
    /// </summary>
    public int NumCusTargeNotSuccess { get; set; }

    public virtual User User { get; set; } = null!;
}

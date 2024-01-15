using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// พนักงานในสาขา
/// </summary>
public partial class User_Branch
{
    public int Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public int UserId { get; set; }

    public int BranchId { get; set; }

    public virtual Master_Branch Branch { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// กิจการสาขาภาค
/// </summary>
public partial class Master_Department_Branch
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
    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public virtual ICollection<Master_Department_Center> Master_Department_Centers { get; set; } = new List<Master_Department_Center>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

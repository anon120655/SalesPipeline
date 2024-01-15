using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// สิทธิ์การเข้าถึง
/// </summary>
public partial class User_Permission
{
    public int Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int RoleId { get; set; }

    public int MenuNumber { get; set; }

    public bool IsView { get; set; }

    public virtual User_Role Role { get; set; } = null!;
}

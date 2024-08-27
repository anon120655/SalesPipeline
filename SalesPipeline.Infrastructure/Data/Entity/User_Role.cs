using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ระดับหน้าที่
/// </summary>
public partial class User_Role
{
    public int Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdateBy { get; set; }

    /// <summary>
    /// อนุญาตให้แก้ไข
    /// </summary>
    public bool IsModify { get; set; }

    public string? Code { get; set; }

    /// <summary>
    /// ชื่อหน้าที่
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// รายละเอียดหน้าที่
    /// </summary>
    public string? Description { get; set; }

    public string? iAuthenRoleCode { get; set; }

    public string? iAuthenRoleName { get; set; }

    /// <summary>
    /// 1=มีสิทธ์มอบหมาย ผจศ.
    /// </summary>
    public bool IsAssignCenter { get; set; }

    /// <summary>
    /// 1=มีสิทธ์มอบหมาย rm
    /// </summary>
    public bool IsAssignRM { get; set; }

    public string? org_id { get; set; }

    public string? org_name { get; set; }

    public virtual ICollection<User_Permission> User_Permissions { get; set; } = new List<User_Permission>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

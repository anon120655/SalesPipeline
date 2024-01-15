using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ผู้ใช้งาน
/// </summary>
public partial class User
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

    public string? EmployeeId { get; set; }

    public string? TitleName { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public int? RoleId { get; set; }

    public int? PositionId { get; set; }

    public int? LevelId { get; set; }

    public string? PasswordHash { get; set; }

    public short? LoginFail { get; set; }

    public virtual User_Level? Level { get; set; }

    public virtual Master_Position? Position { get; set; }

    public virtual User_Role? Role { get; set; }

    public virtual ICollection<User_Branch> User_Branches { get; set; } = new List<User_Branch>();
}

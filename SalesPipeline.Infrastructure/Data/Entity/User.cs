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

    /// <summary>
    /// รหัสพนักงาน
    /// </summary>
    public string? EmployeeId { get; set; }

    public string? TitleName { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    /// <summary>
    /// เบอร์โทร
    /// </summary>
    public string? Tel { get; set; }

    /// <summary>
    /// ฝ่ายส่วนงานธุรกิจสินเชื่อ
    /// </summary>
    public Guid? Master_DepartmentId { get; set; }

    /// <summary>
    /// กิจการสาขาภาค
    /// </summary>
    public Guid? Master_Department_BranchId { get; set; }

    /// <summary>
    /// ศูนย์ธุรกิจสินเชื่อ
    /// </summary>
    public Guid? Master_Department_CenterId { get; set; }

    /// <summary>
    /// จังหวัด
    /// </summary>
    public int? ProvinceId { get; set; }

    public string? ProvinceName { get; set; }

    /// <summary>
    /// อำเภอ
    /// </summary>
    public int? AmphurId { get; set; }

    public string? AmphurName { get; set; }

    /// <summary>
    /// สาขา
    /// </summary>
    public int? BranchId { get; set; }

    public string? BranchName { get; set; }

    /// <summary>
    /// ตำแหน่ง
    /// </summary>
    public int? PositionId { get; set; }

    /// <summary>
    /// ระดับ
    /// </summary>
    public int? LevelId { get; set; }

    /// <summary>
    /// ระดับหน้าที่
    /// </summary>
    public int? RoleId { get; set; }

    public string? PasswordHash { get; set; }

    public short? LoginFail { get; set; }

    public virtual ICollection<Assignment_MCenter> Assignment_MCenters { get; set; } = new List<Assignment_MCenter>();

    public virtual ICollection<Assignment_RM_Sale> Assignment_RM_Sales { get; set; } = new List<Assignment_RM_Sale>();

    public virtual ICollection<Assignment_RM> Assignment_RMs { get; set; } = new List<Assignment_RM>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual User_Level? Level { get; set; }

    public virtual Master_Department? Master_Department { get; set; }

    public virtual Master_Department_Branch? Master_Department_Branch { get; set; }

    public virtual Master_Department_Center? Master_Department_Center { get; set; }

    public virtual ICollection<Notification> NotificationFromUsers { get; set; } = new List<Notification>();

    public virtual ICollection<Notification> NotificationToUsers { get; set; } = new List<Notification>();

    public virtual Master_Position? Position { get; set; }

    public virtual User_Role? Role { get; set; }

    public virtual ICollection<Sale> SaleAssCenterUsers { get; set; } = new List<Sale>();

    public virtual ICollection<Sale> SaleAssUsers { get; set; } = new List<Sale>();

    public virtual ICollection<Sale_Status_Total> Sale_Status_Totals { get; set; } = new List<Sale_Status_Total>();

    public virtual ICollection<Sale_Status> Sale_Statuses { get; set; } = new List<Sale_Status>();
}

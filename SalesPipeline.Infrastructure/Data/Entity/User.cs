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

    public string? UserName { get; set; }

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
    public Guid? Master_Branch_RegionId { get; set; }

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

    public short? IsSentMail { get; set; }

    /// <summary>
    /// url ลายเซ็น
    /// </summary>
    public string? UrlSignature { get; set; }

    public string? TokenApi { get; set; }

    /// <summary>
    /// 1=iAuthen
    /// </summary>
    public int? Create_Type { get; set; }

    public int? authen_fail_time { get; set; }

    public string? branch_code { get; set; }

    public string? branch_name { get; set; }

    public string? cbs_id { get; set; }

    public string? change_password_url { get; set; }

    public string? create_password_url { get; set; }

    public string? email_baac { get; set; }

    public string? employee_id { get; set; }

    public string? employee_position_id { get; set; }

    public string? employee_position_level { get; set; }

    public string? employee_position_name { get; set; }

    public string? employee_status { get; set; }

    public string? first_name_th { get; set; }

    public bool? image_existing { get; set; }

    public string? job_field_id { get; set; }

    public string? job_field_name { get; set; }

    public string? job_id { get; set; }

    public string? job_name { get; set; }

    public string? last_name_th { get; set; }

    public DateTime? lastauthen_timestamp { get; set; }

    public string? mobile_no { get; set; }

    public string? name_en { get; set; }

    public string? org_id { get; set; }

    public string? org_name { get; set; }

    public string? organization_48 { get; set; }

    public string? organization_abbreviation { get; set; }

    public string? organization_upper_id { get; set; }

    public string? organization_upper_id2 { get; set; }

    public string? organization_upper_id3 { get; set; }

    public string? organization_upper_name { get; set; }

    public string? organization_upper_name2 { get; set; }

    public string? organization_upper_name3 { get; set; }

    public bool? password_unexpire { get; set; }

    public bool? requester_active { get; set; }

    public bool? requester_existing { get; set; }

    public DateTime? timeresive { get; set; }

    public DateTime? timesend { get; set; }

    public string? title_th { get; set; }

    public string? title_th_2 { get; set; }

    public string? user_class { get; set; }

    public bool? username_active { get; set; }

    public bool? username_existing { get; set; }

    public string? working_status { get; set; }

    public virtual ICollection<Assignment_BranchReg> Assignment_BranchRegs { get; set; } = new List<Assignment_BranchReg>();

    public virtual ICollection<Assignment_Center> Assignment_Centers { get; set; } = new List<Assignment_Center>();

    public virtual ICollection<Assignment_RM_Sale> Assignment_RM_Sales { get; set; } = new List<Assignment_RM_Sale>();

    public virtual ICollection<Assignment_RM> Assignment_RMs { get; set; } = new List<Assignment_RM>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Dash_Avg_Number> Dash_Avg_Numbers { get; set; } = new List<Dash_Avg_Number>();

    public virtual ICollection<Dash_Map_Thailand> Dash_Map_Thailands { get; set; } = new List<Dash_Map_Thailand>();

    public virtual ICollection<Dash_Status_Total> Dash_Status_Totals { get; set; } = new List<Dash_Status_Total>();

    public virtual User_Level? Level { get; set; }

    public virtual Master_Branch_Region? Master_Branch_Region { get; set; }

    public virtual ICollection<Notification> NotificationFromUsers { get; set; } = new List<Notification>();

    public virtual ICollection<Notification> NotificationToUsers { get; set; } = new List<Notification>();

    public virtual Master_Position? Position { get; set; }

    public virtual User_Role? Role { get; set; }

    public virtual ICollection<Sale> SaleAssCenterUsers { get; set; } = new List<Sale>();

    public virtual ICollection<Sale> SaleAssUsers { get; set; } = new List<Sale>();

    public virtual ICollection<Sale_Status_Total> Sale_Status_Totals { get; set; } = new List<Sale_Status_Total>();

    public virtual ICollection<Sale_Status> Sale_Statuses { get; set; } = new List<Sale_Status>();

    public virtual ICollection<User_Area> User_Areas { get; set; } = new List<User_Area>();

    public virtual ICollection<User_Target_Sale> User_Target_SaleCreateByNavigations { get; set; } = new List<User_Target_Sale>();

    public virtual ICollection<User_Target_Sale> User_Target_SaleUsers { get; set; } = new List<User_Target_Sale>();
}

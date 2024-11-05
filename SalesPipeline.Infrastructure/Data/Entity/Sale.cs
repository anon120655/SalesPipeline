using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// การขาย
/// </summary>
public partial class Sale
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public string? CreateByName { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdateBy { get; set; }

    public string? UpdateByName { get; set; }

    public Guid CustomerId { get; set; }

    public string? CIF { get; set; }

    /// <summary>
    /// ชื่อบริษัท
    /// </summary>
    public string? CompanyName { get; set; }

    public int? StatusSaleMainId { get; set; }

    /// <summary>
    /// สถานะการขายหลัก
    /// </summary>
    public string? StatusSaleNameMain { get; set; }

    public int StatusSaleId { get; set; }

    /// <summary>
    /// สถานะการขาย
    /// </summary>
    public string? StatusSaleName { get; set; }

    /// <summary>
    /// รายละเอียดสถานะ
    /// </summary>
    public string? StatusDescription { get; set; }

    /// <summary>
    /// เหตุผลไม่ประสงค์กู้
    /// </summary>
    public Guid? Master_Reason_CloseSaleId { get; set; }

    /// <summary>
    /// วันที่เริ่มติดต่อ
    /// </summary>
    public DateTime? ContactStartDate { get; set; }

    /// <summary>
    /// วันที่นัดหมาย
    /// </summary>
    public DateTime? DateAppointment { get; set; }

    /// <summary>
    /// จำนวนการกู้
    /// </summary>
    public decimal? LoanAmount { get; set; }

    /// <summary>
    /// เปอร์เซ็นโอกาสกู้ผ่าน
    /// </summary>
    public double? PercentChanceLoanPass { get; set; }

    /// <summary>
    /// เปอร์เซ็นโอกาสกู้ผ่าน
    /// </summary>
    public string? PercentChanceLoanPassName { get; set; }

    public Guid? Pre_FactorId { get; set; }

    /// <summary>
    /// ชื่อโครงการสินเชื่อ
    /// </summary>
    public string? ProjectLoanName { get; set; }

    /// <summary>
    /// ระยะเวลาสินเชื่อ
    /// </summary>
    public int? LoanPeriod { get; set; }

    /// <summary>
    /// กิจการสาขาภาค
    /// </summary>
    public Guid? Master_Branch_RegionId { get; set; }

    public string? Master_Branch_RegionName { get; set; }

    /// <summary>
    /// จังหวัด
    /// </summary>
    public int? ProvinceId { get; set; }

    public string? ProvinceName { get; set; }

    /// <summary>
    /// สาขา
    /// </summary>
    public int? BranchId { get; set; }

    public string? BranchName { get; set; }

    /// <summary>
    /// ผู้จัดการศูนย์ได้รับมอบหมายแล้ว
    /// </summary>
    public bool? AssCenterAlready { get; set; }

    /// <summary>
    /// ผู้จัดการศูนย์ที่ดูแล
    /// </summary>
    public int? AssCenterUserId { get; set; }

    /// <summary>
    /// ชื่อผู้จัดการศูนย์ที่ดูแล
    /// </summary>
    public string? AssCenterUserName { get; set; }

    /// <summary>
    /// ผู้มอบหมายผู้จัดการศูนย์
    /// </summary>
    public int? AssCenterCreateBy { get; set; }

    /// <summary>
    /// วันที่มอบหมายผู้จัดการศูนย์
    /// </summary>
    public DateTime? AssCenterDate { get; set; }

    /// <summary>
    /// ได้รับมอบหมายแล้ว
    /// </summary>
    public bool? AssUserAlready { get; set; }

    /// <summary>
    /// พนักงานที่ได้รับมอบหมาย
    /// </summary>
    public int? AssUserId { get; set; }

    /// <summary>
    /// ชื่อพนักงานที่ได้รับมอบหมาย
    /// </summary>
    public string? AssUserName { get; set; }

    /// <summary>
    /// วันที่มอบหมาย
    /// </summary>
    public DateTime? AssUserDate { get; set; }

    /// <summary>
    /// สร้างโดยกด Re-Purpose
    /// </summary>
    public bool? IsRePurpose { get; set; }

    public virtual User? AssCenterUser { get; set; }

    public virtual User? AssUser { get; set; }

    public virtual ICollection<Assignment_RM_Sale> Assignment_RM_Sales { get; set; } = new List<Assignment_RM_Sale>();

    public virtual InfoBranch? Branch { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Master_Branch_Region? Master_Branch_Region { get; set; }

    public virtual Master_Reason_CloseSale? Master_Reason_CloseSale { get; set; }

    public virtual ICollection<Pre_Factor> Pre_Factors { get; set; } = new List<Pre_Factor>();

    public virtual ICollection<Sale_Activity> Sale_Activities { get; set; } = new List<Sale_Activity>();

    public virtual ICollection<Sale_Close_Sale> Sale_Close_Sales { get; set; } = new List<Sale_Close_Sale>();

    public virtual ICollection<Sale_Contact_History> Sale_Contact_Histories { get; set; } = new List<Sale_Contact_History>();

    public virtual ICollection<Sale_Contact_Info> Sale_Contact_Infos { get; set; } = new List<Sale_Contact_Info>();

    public virtual ICollection<Sale_Contact> Sale_Contacts { get; set; } = new List<Sale_Contact>();

    public virtual ICollection<Sale_Deliver> Sale_Delivers { get; set; } = new List<Sale_Deliver>();

    public virtual ICollection<Sale_Document_Upload> Sale_Document_Uploads { get; set; } = new List<Sale_Document_Upload>();

    public virtual ICollection<Sale_Document> Sale_Documents { get; set; } = new List<Sale_Document>();

    public virtual ICollection<Sale_Duration> Sale_Durations { get; set; } = new List<Sale_Duration>();

    public virtual ICollection<Sale_Meet> Sale_Meets { get; set; } = new List<Sale_Meet>();

    public virtual ICollection<Sale_Partner> Sale_Partners { get; set; } = new List<Sale_Partner>();

    public virtual ICollection<Sale_Phoenix> Sale_Phoenixes { get; set; } = new List<Sale_Phoenix>();

    public virtual ICollection<Sale_Reply> Sale_Replies { get; set; } = new List<Sale_Reply>();

    public virtual ICollection<Sale_Result> Sale_Results { get; set; } = new List<Sale_Result>();

    public virtual ICollection<Sale_Status> Sale_Statuses { get; set; } = new List<Sale_Status>();

    public virtual Master_StatusSale StatusSale { get; set; } = null!;
}

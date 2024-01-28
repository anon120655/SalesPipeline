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

    /// <summary>
    /// ชื่อบริษัท
    /// </summary>
    public string? CompanyName { get; set; }

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
    /// วันที่นัดหมาย
    /// </summary>
    public DateTime? DateAppointment { get; set; }

    /// <summary>
    /// เปอร์เซ็นโอกาสกู้ผ่าน
    /// </summary>
    public int? PercentChanceLoanPass { get; set; }

    /// <summary>
    /// พนักงานที่ได้รับมอบหมาย
    /// </summary>
    public int? AssignedUserId { get; set; }

    /// <summary>
    /// ชื่อพนักงานที่ได้รับมอบหมาย
    /// </summary>
    public string? AssignedUserName { get; set; }

    public virtual ICollection<Assignment_Sale> Assignment_Sales { get; set; } = new List<Assignment_Sale>();

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Sale_Reply> Sale_Replies { get; set; } = new List<Sale_Reply>();

    public virtual ICollection<Sale_Status> Sale_Statuses { get; set; } = new List<Sale_Status>();

    public virtual Master_StatusSale StatusSale { get; set; } = null!;
}

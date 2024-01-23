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

    /// <summary>
    /// ผู้รับผิดชอบ
    /// </summary>
    public string? ResponsibleName { get; set; }

    public int StatusSaleId { get; set; }

    /// <summary>
    /// สถานะการขาย
    /// </summary>
    public string? StatusSaleName { get; set; }

    /// <summary>
    /// รายละเอียดสถานะการขาย
    /// </summary>
    public string? StatusSaleDescription { get; set; }

    /// <summary>
    /// วันที่นัดหมาย
    /// </summary>
    public DateTime? DateAppointment { get; set; }

    /// <summary>
    /// เปอร์เซ็นโอกาสกู้ผ่าน
    /// </summary>
    public int? PercentChanceLoanPass { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Sale_Reply> Sale_Replies { get; set; } = new List<Sale_Reply>();

    public virtual ICollection<Sale_Status> Sale_Statuses { get; set; } = new List<Sale_Status>();

    public virtual Master_StatusSale StatusSale { get; set; } = null!;
}

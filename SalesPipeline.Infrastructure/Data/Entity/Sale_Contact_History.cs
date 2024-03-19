using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ประวัติการติดต่อ
/// </summary>
public partial class Sale_Contact_History
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public string? CreateByName { get; set; }

    public Guid SaleId { get; set; }

    public int StatusSaleId { get; set; }

    /// <summary>
    /// การดำเนินการ
    /// </summary>
    public string ProceedName { get; set; } = null!;

    /// <summary>
    /// ผลการติดต่อ
    /// </summary>
    public string? ResultContactName { get; set; }

    /// <summary>
    /// ผลการเข้าพบ
    /// </summary>
    public string? ResultMeetName { get; set; }

    public string? NextActionName { get; set; }

    /// <summary>
    /// วันที่นัดหมาย
    /// </summary>
    public DateTime? AppointmentDate { get; set; }

    /// <summary>
    /// เวลาที่นัดหมาย
    /// </summary>
    public TimeOnly? AppointmentTime { get; set; }

    /// <summary>
    /// สถานที่
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// บันทึกเพิ่มเติม
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// สถานะ
    /// </summary>
    public string? StatusName { get; set; }

    /// <summary>
    /// วงเงิน
    /// </summary>
    public decimal? CreditLimit { get; set; }

    /// <summary>
    /// ร้อยละ
    /// </summary>
    public string? Percent { get; set; }

    public virtual Sale Sale { get; set; } = null!;

    public virtual Master_StatusSale StatusSale { get; set; } = null!;
}

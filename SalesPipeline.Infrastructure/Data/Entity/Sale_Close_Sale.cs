using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Sale_Close_Sale
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid SaleId { get; set; }

    public Guid? SaleReplyId { get; set; }

    /// <summary>
    /// วันที่ติดต่อ
    /// </summary>
    public DateTime? ContactDate { get; set; }

    /// <summary>
    /// ชื่อผู้ติดต่อ
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// เบอร์ติดต่อ
    /// </summary>
    public string? Tel { get; set; }

    /// <summary>
    /// 1=รับสาย 2=ไม่รับสาย
    /// </summary>
    public int? ResultMeetId { get; set; }

    /// <summary>
    /// 1=ปิดการขาย 2=ติดต่ออีกครั้ง
    /// </summary>
    public int? NextActionId { get; set; }

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
    /// 1=ประสงค์กู้ 2=ไม่ประสงค์กู้
    /// </summary>
    public int? DesireLoanId { get; set; }

    public Guid? Master_Reason_CloseSaleId { get; set; }

    public virtual Master_Reason_CloseSale? Master_Reason_CloseSale { get; set; }

    public virtual Sale Sale { get; set; } = null!;
}

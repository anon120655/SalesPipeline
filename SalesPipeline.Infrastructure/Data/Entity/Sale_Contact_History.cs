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

    public Guid? SaleReplyId { get; set; }

    public int StatusSaleId { get; set; }

    public string? ProcessSaleCode { get; set; }

    /// <summary>
    /// ชื่อหัวข้อ
    /// </summary>
    public string? TopicName { get; set; }

    /// <summary>
    /// ชื่อผู้ติดต่อ
    /// </summary>
    public string? ContactFullName { get; set; }

    /// <summary>
    /// ชื่อการดำเนินการ
    /// </summary>
    public string? ProceedName { get; set; }

    /// <summary>
    /// วันที่ติดต่อ
    /// </summary>
    public DateTime? ContactDate { get; set; }

    /// <summary>
    /// เบอร์ติดต่อ
    /// </summary>
    public string? ContactTel { get; set; }

    /// <summary>
    /// ผลการติดต่อ
    /// </summary>
    public string? ResultContactName { get; set; }

    /// <summary>
    /// ชื่อผู้เข้าพบ
    /// </summary>
    public string? MeetFullName { get; set; }

    /// <summary>
    /// ผลการเข้าพบ
    /// </summary>
    public string? ResultMeetName { get; set; }

    /// <summary>
    /// Next Action
    /// </summary>
    public string? NextActionName { get; set; }

    /// <summary>
    /// ไฟล์แนบ
    /// </summary>
    public string? AttachmentPath { get; set; }

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

    /// <summary>
    /// ความประสงค์กู้
    /// </summary>
    public string? DesireLoanName { get; set; }

    /// <summary>
    /// เหตุผล
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Note system
    /// </summary>
    public string? NoteSystem { get; set; }

    /// <summary>
    /// 1=กำหนดเวลาแล้ว
    /// </summary>
    public short? IsScheduledJob { get; set; }

    /// <summary>
    /// 1=แจ้งเตือนแล้ว
    /// </summary>
    public short? IsScheduledJobSucceed { get; set; }

    /// <summary>
    /// เปอร์เซ็นโอกาสกู้ผ่าน
    /// </summary>
    public double? PercentChanceLoanPass { get; set; }

    public virtual Sale Sale { get; set; } = null!;

    public virtual Master_StatusSale StatusSale { get; set; } = null!;
}

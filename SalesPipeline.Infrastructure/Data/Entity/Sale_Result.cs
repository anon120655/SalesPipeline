using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Sale_Result
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
    /// 1=แจ้งข้อมูลเพิ่มเติม 2=ติดต่อขอเอกสาร 3=เข้าพบรับเอกสาร 4=ไม่ผ่านการพิจารณา 5=รอปิดการขาย
    /// </summary>
    public int? ProceedId { get; set; }

    /// <summary>
    /// 1=เข้าพบสำเร็จ 2=เข้าพบไม่สำเร็จ
    /// </summary>
    public int? ResultMeetId { get; set; }

    /// <summary>
    /// 1=ทำการนัดหมาย 2=รอปิดการขาย
    /// </summary>
    public int? NextActionId { get; set; }

    /// <summary>
    /// วันที่ติดต่อ
    /// </summary>
    public DateTime? DateContact { get; set; }

    /// <summary>
    /// ช่องทางการติดต่อ
    /// </summary>
    public Guid? Master_ContactChannelId { get; set; }

    /// <summary>
    /// ผู้เข้าพบ
    /// </summary>
    public string? MeetName { get; set; }

    /// <summary>
    /// เบอร์โทร
    /// </summary>
    public string? Tel { get; set; }

    /// <summary>
    /// เอกสาร
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

    public virtual Master_ContactChannel? Master_ContactChannel { get; set; }

    public virtual Sale Sale { get; set; } = null!;
}

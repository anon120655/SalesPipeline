using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Sale_Document_Upload
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public Guid SaleId { get; set; }

    /// <summary>
    /// 1=รูปบัตรประชาชน 
    /// 2=ทะเบียนนบ้าน 
    /// 3=เอกสารอื่นๆ 
    /// 4=ลายเซ็นผู้กู้ยืม 
    /// 5=ลายเซ็นพนักงานสินเชื่อ 
    /// 6=เอกสารเพิ่มเติม
    /// </summary>
    public short Type { get; set; }

    public string? Url { get; set; }

    /// <summary>
    /// ชื่อเดิมไฟล์
    /// </summary>
    public string? OriginalFileName { get; set; }

    /// <summary>
    /// ชื่อไฟล์ที่ใช้ในระบบ
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// ขนาดไฟล์
    /// </summary>
    public long? FileSize { get; set; }

    /// <summary>
    /// นามสกุลไฟล์
    /// </summary>
    public string? MimeType { get; set; }

    public virtual Sale Sale { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class FileUpload
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public string? Url { get; set; }

    /// <summary>
    /// ชื่อเดิมไฟล์
    /// </summary>
    public string OriginalFileName { get; set; } = null!;

    /// <summary>
    /// ชื่อไฟล์ที่ใช้ในระบบ
    /// </summary>
    public string FileName { get; set; } = null!;

    /// <summary>
    /// ขนาดไฟล์
    /// </summary>
    public int FileSize { get; set; }

    /// <summary>
    /// นามสกุลไฟล์
    /// </summary>
    public string MimeType { get; set; } = null!;

    public virtual ICollection<Sale_Reply_Section_ItemValue> Sale_Reply_Section_ItemValues { get; set; } = new List<Sale_Reply_Section_ItemValue>();
}

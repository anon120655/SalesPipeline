using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ลายเซ็นอนุมัติ
/// </summary>
public partial class System_Signature
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public string? Name { get; set; }

    /// <summary>
    /// Url รูป
    /// </summary>
    public string? ImgUrl { get; set; }

    /// <summary>
    /// Url รูปขนาดย่อ
    /// </summary>
    public string? ImgThumbnailUrl { get; set; }
}

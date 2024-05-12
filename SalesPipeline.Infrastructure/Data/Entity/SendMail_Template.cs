using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class SendMail_Template
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    /// <summary>
    /// วันที่สร้าง
    /// </summary>
    public DateTime CreateDate { get; set; }

    public string Code { get; set; } = null!;

    public string? Subject { get; set; }

    public string Message { get; set; } = null!;

    public virtual ICollection<SendMail_Log> SendMail_Logs { get; set; } = new List<SendMail_Log>();
}

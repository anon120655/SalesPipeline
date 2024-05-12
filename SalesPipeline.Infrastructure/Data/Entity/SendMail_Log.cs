using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class SendMail_Log
{
    public Guid Id { get; set; }

    /// <summary>
    /// วันที่สร้าง
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// ผู้สร้าง/เป็น null ได้กรณีคนนอกไม่ได้ login
    /// </summary>
    public int? CreateById { get; set; }

    public Guid? SendMail_TemplateId { get; set; }

    public string EmailTo { get; set; } = null!;

    public string? EmailToCc { get; set; }

    public string Subject { get; set; } = null!;

    public string Message { get; set; } = null!;

    public bool IsCompleted { get; set; }

    public string? StatusMessage { get; set; }

    public virtual SendMail_Template? SendMail_Template { get; set; }
}

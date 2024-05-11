using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Log_SendMail
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

    public string Template { get; set; } = null!;

    public string EmailTo { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Message { get; set; } = null!;

    public bool IsCompleted { get; set; }

    public string? StatusMessage { get; set; }
}

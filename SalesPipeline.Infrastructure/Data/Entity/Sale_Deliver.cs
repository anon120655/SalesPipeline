using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ระยะเวลาในการส่งมอบ
/// </summary>
public partial class Sale_Deliver
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid SaleId { get; set; }

    /// <summary>
    /// ชื่อผู้ติดต่อ
    /// </summary>
    public string? ContactName { get; set; }

    /// <summary>
    /// ติดต่อ(ครั้ง)
    /// </summary>
    public int LoanToBranch { get; set; }

    /// <summary>
    /// เข้าพบ(ครั้ง)
    /// </summary>
    public int BranchToMcenter { get; set; }

    /// <summary>
    /// ยื่นเอกสาร(ครั้ง)
    /// </summary>
    public int McenterToRM { get; set; }

    /// <summary>
    /// ปิดการขาย(ครั้ง)
    /// </summary>
    public int CloseSale { get; set; }
}

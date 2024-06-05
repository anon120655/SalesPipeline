using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Pre_Result_Item
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid Pre_ResultId { get; set; }

    /// <summary>
    /// ปัจจัยการวิเคราะห์
    /// </summary>
    public string? AnalysisFactor { get; set; }

    /// <summary>
    /// คุณสมบัติ
    /// </summary>
    public string? Feature { get; set; }

    /// <summary>
    /// คะแนน
    /// </summary>
    public decimal? Score { get; set; }

    /// <summary>
    /// อัตราส่วน
    /// </summary>
    public decimal? Ratio { get; set; }

    /// <summary>
    /// ผลคะแนน
    /// </summary>
    public decimal? ScoreResult { get; set; }

    public virtual Pre_Result Pre_Result { get; set; } = null!;
}

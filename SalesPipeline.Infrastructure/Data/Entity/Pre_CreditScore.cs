using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// Credit Score
/// </summary>
public partial class Pre_CreditScore
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdateBy { get; set; }

    public string? Level { get; set; }

    public int? CreditScore { get; set; }

    public string? Grade { get; set; }

    public string? LimitMultiplier { get; set; }

    public string? RateMultiplier { get; set; }

    public string? CreditScoreColor { get; set; }
}

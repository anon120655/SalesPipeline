using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// โอกาสขอสินเชื่อผ่าน
/// </summary>
public partial class Pre_ChancePass
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

    /// <summary>
    /// ลำดับ
    /// </summary>
    public int SequenceNo { get; set; }

    public string? Z { get; set; }

    public string? CreditScore { get; set; }

    public string? Prob { get; set; }
}

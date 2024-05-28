using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Pre_Cal_Info_Score
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid Pre_Cal_InfoId { get; set; }

    /// <summary>
    /// จำนวน
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// คะแนน
    /// </summary>
    public decimal? Score { get; set; }

    public virtual Pre_Cal_Info Pre_Cal_Info { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Pre_Factor_App
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid Pre_FactorId { get; set; }

    public Guid? Pre_Cal_Fetu_App_ItemId { get; set; }

    public string? Pre_Cal_Fetu_App_ItemName { get; set; }

    public Guid? Pre_Cal_Fetu_App_Item_ScoreId { get; set; }

    public string? Pre_Cal_Fetu_App_Item_ScoreName { get; set; }

    public virtual Pre_Factor Pre_Factor { get; set; } = null!;
}

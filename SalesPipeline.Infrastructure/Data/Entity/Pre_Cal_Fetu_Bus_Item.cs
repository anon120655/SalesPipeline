using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Pre_Cal_Fetu_Bus_Item
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid Pre_Cal_Fetu_BusId { get; set; }

    /// <summary>
    /// ลำดับ
    /// </summary>
    public int SequenceNo { get; set; }

    public string? Name { get; set; }

    public virtual Pre_Cal_Fetu_Bu Pre_Cal_Fetu_Bus { get; set; } = null!;

    public virtual ICollection<Pre_Cal_Fetu_Bus_Item_Score> Pre_Cal_Fetu_Bus_Item_Scores { get; set; } = new List<Pre_Cal_Fetu_Bus_Item_Score>();
}

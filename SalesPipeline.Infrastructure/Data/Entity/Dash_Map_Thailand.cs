using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Dash_Map_Thailand
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public bool IsUpdate { get; set; }

    public int UserId { get; set; }

    /// <summary>
    /// 1=ยอดขายสูงสุด 2=แพ้ให้กับคู่แข่งสูงสุด
    /// </summary>
    public int Type { get; set; }

    public int ProvinceId { get; set; }

    public string ProvinceName { get; set; } = null!;

    /// <summary>
    /// ยอดขาย
    /// </summary>
    public decimal SalesAmount { get; set; }

    public virtual User User { get; set; } = null!;
}

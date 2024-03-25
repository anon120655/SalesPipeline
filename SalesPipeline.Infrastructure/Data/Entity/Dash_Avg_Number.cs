using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Dash_Avg_Number
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
    /// มูลค่าเฉลี่ยต่อหนึ่งดีล
    /// </summary>
    public decimal AvgPerDeal { get; set; }

    /// <summary>
    /// ระยะเวลาเฉลี่ยที่ใช้ในการปิดการขาย
    /// </summary>
    public int AvgTimeCloseSale { get; set; }

    /// <summary>
    /// ระยะเวลาเฉลี่ยที่ใช้ในการขายที่แพ้ให้กับคู่แข่ง
    /// </summary>
    public int AvgTimeLostSale { get; set; }

    /// <summary>
    /// ดีลโดยเฉลี่ยต่อองค์กร
    /// </summary>
    public int AvgDealOrg { get; set; }

    /// <summary>
    /// กิจกรรมการขายโดยเฉลี่ยต่อดีลที่ปิดการขาย
    /// </summary>
    public int AvgSaleActcloseDeal { get; set; }

    /// <summary>
    /// ระยะเวลาเฉลี่ยในการส่งมอบ
    /// </summary>
    public int AvgDeliveryTime { get; set; }

    /// <summary>
    /// ดีลโดยเฉลี่ยต่อพนักงานสินเชื่อ
    /// </summary>
    public int AvgDealRM { get; set; }

    public virtual User User { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// น้ำหนักของแต่ละปัจจัย
/// </summary>
public partial class Pre_Cal_WeightFactor
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid Pre_CalId { get; set; }

    /// <summary>
    /// 1=ข้อมูลการขอสินเชื่อ
    /// 2=คุณสมบัติมารตฐาน
    /// 3=คุณสมบัติตามประเภทผู้ขอ
    /// 4=คุณสมบัติตามประเภทธุรกิจ
    /// </summary>
    public short Type { get; set; }

    public decimal TotalPercent { get; set; }

    public virtual Pre_Cal Pre_Cal { get; set; } = null!;

    public virtual ICollection<Pre_Cal_WeightFactor_Item> Pre_Cal_WeightFactor_Items { get; set; } = new List<Pre_Cal_WeightFactor_Item>();
}

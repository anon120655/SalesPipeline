using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// น้ำหนักของแต่ละปัจจัย
/// </summary>
public partial class Pre_Cal_Weight_Factor
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
    /// ชื่อสินเชื่อ
    /// </summary>
    public string? Name { get; set; }
}

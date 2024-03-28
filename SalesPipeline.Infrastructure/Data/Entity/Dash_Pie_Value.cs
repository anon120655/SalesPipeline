using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Dash_Pie_Value
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid? Dash_PieId { get; set; }

    public string? Name { get; set; }

    public decimal? Value { get; set; }

    public virtual Dash_Pie? Dash_Pie { get; set; }
}

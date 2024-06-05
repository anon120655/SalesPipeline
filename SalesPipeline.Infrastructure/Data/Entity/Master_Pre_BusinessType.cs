using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ประเภทธุรกิจ pre approve
/// </summary>
public partial class Master_Pre_BusinessType
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

    public string? Name { get; set; }

    public virtual ICollection<Pre_Cal> Pre_Cals { get; set; } = new List<Pre_Cal>();

    public virtual ICollection<Pre_Factor_Info> Pre_Factor_Infos { get; set; } = new List<Pre_Factor_Info>();
}

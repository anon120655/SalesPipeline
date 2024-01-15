using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// สาขา
/// </summary>
public partial class Master_Branch
{
    public int Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdateBy { get; set; }

    public int RegionId { get; set; }

    public string? Name { get; set; }

    public virtual Master_Region Region { get; set; } = null!;

    public virtual ICollection<User_Branch> User_Branches { get; set; } = new List<User_Branch>();
}

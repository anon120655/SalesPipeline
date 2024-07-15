using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class InfoBranch
{
    public int BranchID { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdateBy { get; set; }

    public int ProvinceID { get; set; }

    public string? BranchCode { get; set; }

    public string? BranchName { get; set; }

    public string? BranchNameMain { get; set; }

    public virtual ICollection<Assignment_BranchReg> Assignment_BranchRegs { get; set; } = new List<Assignment_BranchReg>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}

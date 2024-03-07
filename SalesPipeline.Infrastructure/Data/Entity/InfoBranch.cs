using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class InfoBranch
{
    public int BranchID { get; set; }

    public int ProvinceID { get; set; }

    public string? BranchCode { get; set; }

    public string? BranchName { get; set; }

    public string? BranchNameMain { get; set; }

    public virtual ICollection<Assignment_RM> Assignment_RMs { get; set; } = new List<Assignment_RM>();

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}

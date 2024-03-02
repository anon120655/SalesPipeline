using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class InfoBranch
{
    public int BranchID { get; set; }

    public int ProvinceID { get; set; }

    public string? BranchName { get; set; }

    public string? BranchNameMain { get; set; }
}

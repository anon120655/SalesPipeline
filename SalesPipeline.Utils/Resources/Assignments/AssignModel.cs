using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Assignments
{
	public class AssignModel : CommonModel
	{
		public Assignment_CenterCustom AssignMCenter { get; set; } = new();
		public Assignment_BranchRegCustom AssignMBranch { get; set; } = new();
		public List<SaleCustom> Sales { get; set; } = null!;
	}
}

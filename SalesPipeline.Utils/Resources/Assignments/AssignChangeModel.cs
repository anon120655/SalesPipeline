using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Assignments
{
	public class AssignChangeModel : CommonModel
	{
		public SaleCustom Original { get; set; } = null!;
		public Assignment_RMCustom New { get; set; } = null!;
	}
}

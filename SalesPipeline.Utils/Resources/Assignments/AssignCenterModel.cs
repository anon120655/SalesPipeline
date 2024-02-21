using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Assignments
{
	public class AssignCenterModel : CommonModel
	{
		public AssignmentCustom Assign { get; set; } = null!;
		public List<SaleCustom> Sales { get; set; } = null!;
	}
}

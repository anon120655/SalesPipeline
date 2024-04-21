using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Dashboards
{
	public class FilterAvgPerDeal
	{
		public List<SelectModel>? DepartmentBranch { get; set; }
		public List<SelectModel>? Branchs { get; set; }
		public List<SelectModel>? RMUser { get; set; }

		public DateTime? startdate { get; set; }
		public DateTime? enddate { get; set; }
	}
}

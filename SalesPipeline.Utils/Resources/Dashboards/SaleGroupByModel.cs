using SalesPipeline.Utils.Resources.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Dashboards
{
	public class SaleGroupByModel
	{
        public string? GroupID { get; set; }
        public List<SaleCustom>? Sales { get; set; }
	}
}

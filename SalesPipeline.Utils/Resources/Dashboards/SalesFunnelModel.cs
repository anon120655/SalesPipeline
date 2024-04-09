using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Dashboards
{
	public class SalesFunnelModel
	{
		public int Contact { get; set; }
		public int Meet { get; set; }
		public int Document { get; set; }
		public int CloseSale { get; set; }
		public int CloseSaleFail { get; set; }
	}
}

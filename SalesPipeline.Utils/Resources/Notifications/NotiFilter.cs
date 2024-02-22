using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Notifications
{
	public class NotiFilter : PagerFilter
	{
		public int? touserid { get; set; }
		public int? eventid { get; set; }
		public int? isread { get; set; }
	}
}

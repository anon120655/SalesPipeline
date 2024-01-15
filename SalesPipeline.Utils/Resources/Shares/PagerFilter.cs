using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class PagerFilter : CommonModel
	{
		public int page { get; set; } = 1;
		public int pagesize { get; set; } = 10;
	}
}

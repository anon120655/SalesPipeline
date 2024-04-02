using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class ChartJsDataLabelsModel
	{
		public List<string?> labels { get; set; } = new();
		public List<decimal?> datas { get; set; } = new();
	}
}

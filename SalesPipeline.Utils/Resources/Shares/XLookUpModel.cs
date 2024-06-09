using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class XLookUpModel
	{
        public Guid ID { get; set; }
        public double CheckValue { get; set; }
		public string? ReturnValue { get; set; }
	}
}

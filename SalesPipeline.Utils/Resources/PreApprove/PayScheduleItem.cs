using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class PayScheduleItem
	{
        public int Period { get; set; }
        public double? Rate { get; set; }
        public double? Payment { get; set; }
        public double? Interest { get; set; }
        public double? Principle { get; set; }
        public double? Balance { get; set; }
	}
}

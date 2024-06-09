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
        public string? RateStr { get; set; }
		public double? Payment { get; set; }
        public string? PaymentStr { get; set; }
		public double? Interest { get; set; }
		public string? InterestStr { get; set; }
		public double? Principle { get; set; }
		public string? PrincipleStr { get; set; }
		public double? Balance { get; set; }
		public string? BalanceStr { get; set; }
	}
}

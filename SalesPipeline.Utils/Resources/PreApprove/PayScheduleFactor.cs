using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class PayScheduleFactor
	{
		public decimal lookupValue { get; set; }
		/// <summary>
		/// สินเชื่อ
		/// </summary>
		public Guid LoanId { get; set; }
		/// <summary>
		/// จำนวนงวดที่ต้องการผ่อน
		/// </summary>
		public int NumberOfPayments { get; set; }
		/// <summary>
		/// จำนวนเงินต้น
		/// </summary>
		public double Principal { get; set; }

		//      public List<PeriodRates>? PeriodRate { get; set; }

		//      public class PeriodRates
		//{
		//	public double Period { get; set; }
		//          public double Rate { get; set; }
		//      }

	}
}

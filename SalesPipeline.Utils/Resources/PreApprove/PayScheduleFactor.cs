using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class PayScheduleFactor
	{
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
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils
{
	public class NotifyEventIdModel
	{
		/// <summary>
		/// ลูกค้าใหม่
		/// </summary>
		public const short NewCus = 1;
		/// <summary>
		/// อนุมัติคำขอ
		/// </summary>
		public const short ApproveLoan = 2;
		/// <summary>
		/// ส่งกลับ
		/// </summary>
		public const short Return = 3;
	}
}

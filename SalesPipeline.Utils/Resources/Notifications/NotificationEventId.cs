using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Notifications
{
	public class NotificationEventId
	{
		/// <summary>
		/// รายการลูกค้าใหม่
		/// </summary>
		public const int AssignNew = 1;
		/// <summary>
		/// อนุมัติคำขอ
		/// </summary>
		public const int ApproveRequest = 2;
		/// <summary>
		/// ส่งกลับ
		/// </summary>
		public const int Return = 3;
	}
}

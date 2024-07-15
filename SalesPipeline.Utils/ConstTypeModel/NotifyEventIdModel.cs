using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.ConstTypeModel
{
    public class NotifyEventIdModel
    {
        /// <summary>
        /// รายการลูกค้าใหม่
        /// </summary>
        public const int AssignNew = 1;
        /// <summary>
        /// อนุมัติกลุ่มเป้าหมาย
        /// </summary>
        public const int ApproveTarget = 2;
		/// <summary>
		/// อนุมัติคำขอสินเชื่อ
		/// </summary>
		public const short ApproveLoan = 3;
        /// <summary>
        /// ส่งกลับ
        /// </summary>
        public const int Return = 4;
        /// <summary>
        /// นัดหมาย
        /// </summary>
        public const int Calendar = 5;
		/// <summary>
		/// ไม่อนุมัติกลุ่มเป้าหมาย
		/// </summary>
		public const int NotApproveTarget = 6;
	}
}

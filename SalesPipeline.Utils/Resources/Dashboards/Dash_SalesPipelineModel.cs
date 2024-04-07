using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Dashboards
{
	public class Dash_SalesPipelineModel
	{
		/// <summary>
		/// จำนวนลูกค้าทั้งหมด
		/// </summary>
		public int CusAll { get; set; }
		/// <summary>
		/// จำนวนลูกค้าสนใจ (มีการติดต่อและนัดหมายเข้าพบ)
		/// </summary>
        public int Interested { get; set; }
		/// <summary>
		/// ยื่นอนุมัติ (รอ ผจศ. อนุมัติคำขอสินเชื่อ)
		/// </summary>
		public int SubmitApproval { get; set; }
		/// <summary>
		/// ผ่าน (ผจศ. อนุมัติคำขอสินเชื่อ)
		/// </summary>
		public int Approval { get; set; }
		/// <summary>
		/// กู้ (ปิดการขาย)
		/// </summary>
		public int CloseSale { get; set; }
	}
}

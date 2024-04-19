using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Dashboards
{
	public class Dash_AvgBottom_NumberCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public bool IsUpdate { get; set; }

		public int UserId { get; set; }

		/// <summary>
		/// ดีลโดยเฉลี่ยต่อสาขา
		/// </summary>
		public int AvgDealBranch { get; set; }

		/// <summary>
		/// กิจกรรมการขายโดยเฉลี่ยต่อดีลที่ปิดการขาย
		/// </summary>
		public int AvgSaleActcloseDeal { get; set; }

		/// <summary>
		/// ระยะเวลาเฉลี่ยในการส่งมอบ
		/// </summary>
		public int AvgDeliveryDuration { get; set; }

		/// <summary>
		/// ดีลโดยเฉลี่ยต่อพนักงานสินเชื่อ
		/// </summary>
		public int AvgDealRM { get; set; }
	}
}

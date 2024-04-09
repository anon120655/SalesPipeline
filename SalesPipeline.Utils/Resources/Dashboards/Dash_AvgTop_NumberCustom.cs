using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Dashboards
{
	public class Dash_AvgTop_NumberCustom
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
		/// มูลค่าเฉลี่ยต่อหนึ่งดีล
		/// </summary>
		public decimal AvgPerDeal { get; set; }

		/// <summary>
		/// ระยะเวลาเฉลี่ยที่ใช้ในการปิดการขาย
		/// </summary>
		public int AvgDurationCloseSale { get; set; }

		/// <summary>
		/// ระยะเวลาเฉลี่ยที่ใช้ในการขายที่แพ้ให้กับคู่แข่ง
		/// </summary>
		public int AvgDurationLostSale { get; set; }
	}
}

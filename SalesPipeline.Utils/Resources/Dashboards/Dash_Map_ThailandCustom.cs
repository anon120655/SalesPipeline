using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Dashboards
{
	public class Dash_Map_ThailandCustom
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
		/// 1=ยอดขายสูงสุด 2=แพ้ให้กับคู่แข่งสูงสุด
		/// </summary>
		public int Type { get; set; }

		public int ProvinceId { get; set; }

		public string ProvinceName { get; set; } = null!;

		/// <summary>
		/// ยอดขาย
		/// </summary>
		public decimal SalesAmount { get; set; }

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Masters
{
	public class Master_StatusSaleCustom
	{
		public int Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public int SequenceNo { get; set; }

		public int? MainId { get; set; }

		public string? NameMain { get; set; }

		public string? Name { get; set; }

		public string? Description { get; set; }

		/// <summary>
		/// 1=แสดงใน filter
		/// </summary>
		public short? IsShowFilter { get; set; }
	}
}

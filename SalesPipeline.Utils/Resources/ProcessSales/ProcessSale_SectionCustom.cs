using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.ProcessSales
{
	public class ProcessSale_SectionCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public Guid ProcessSaleId { get; set; }

		public int SequenceNo { get; set; }

		public string? Name { get; set; }

		/// <summary>
		/// 1=แสดงผลตลอด
		/// </summary>
		public int ShowAlways { get; set; }

		public virtual ProcessSaleCustom? ProcessSale { get; set; }

		public virtual List<ProcessSale_Section_ItemCustom>? ProcessSale_Section_Items { get; set; }
	}
}

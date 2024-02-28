using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.ProcessSales
{
	public class ProcessSale_Section_ItemCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public Guid PSaleSectionId { get; set; }

		public int SequenceNo { get; set; }

		public string? ItemLabel { get; set; }

		public string? ItemType { get; set; }

		/// <summary>
		/// 0=ไม่จำเป็น ,1=จำเป็น
		/// </summary>
		public bool Required { get; set; }

		/// <summary>
		/// 0=แสดงครึ่ง ,1=แสดงเต็ม
		/// </summary>
		public int ShowType { get; set; }

		public virtual ProcessSale_SectionCustom? PSaleSection { get; set; }

		public virtual List<ProcessSale_Section_ItemOptionCustom>? ProcessSale_Section_ItemOptions { get; set; }
	}
}

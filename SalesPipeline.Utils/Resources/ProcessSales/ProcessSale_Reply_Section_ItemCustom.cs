using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.ProcessSales
{
	public class ProcessSale_Reply_Section_ItemCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public Guid PSaleReplySectionId { get; set; }

		public Guid PSaleSectionItemId { get; set; }

		public string? ItemLabel { get; set; }

		public string? ItemType { get; set; }

		public virtual ProcessSale_Reply_SectionCustom? PsaleReplySection { get; set; } = null!;

		public virtual ProcessSale_Section_ItemCustom? PsaleSectionItem { get; set; } = null!;

		public virtual List<ProcessSale_Reply_Section_ItemValueCustom>? ProcessSale_Reply_Section_ItemValues { get; set; }
	}
}

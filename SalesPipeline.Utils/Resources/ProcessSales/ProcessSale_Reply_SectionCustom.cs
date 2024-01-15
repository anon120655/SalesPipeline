using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.ProcessSales
{
	public class ProcessSale_Reply_SectionCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public Guid PSaleReplyId { get; set; }

		public Guid PSaleSectionId { get; set; }

		public string? Name { get; set; }

		public virtual ProcessSale_ReplyCustom? PsaleReply { get; set; } = null!;

		public virtual ProcessSale_SectionCustom? PsaleSection { get; set; } = null!;

		public virtual List<ProcessSale_Reply_Section_ItemCustom>? ProcessSale_Reply_Section_Items { get; set; }

		//Custom
		public bool IsSave { get; set; } = true;
	}
}

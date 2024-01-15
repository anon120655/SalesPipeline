using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.ProcessSales
{
	public class ProcessSale_ReplyCustom : CommonModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int CreateBy { get; set; }

		public string? CreateByName { get; set; }

		public DateTime UpdateDate { get; set; }

		public int UpdateBy { get; set; }

		public string? UpdateByName { get; set; }

		public Guid ProcessSaleId { get; set; }

		public string? ProcessSaleName { get; set; }

		public virtual List<ProcessSale_Reply_SectionCustom>? ProcessSale_Reply_Sections { get; set; }

	}
}

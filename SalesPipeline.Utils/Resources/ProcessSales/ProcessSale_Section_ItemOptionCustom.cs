using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.ProcessSales
{
	public class ProcessSale_Section_ItemOptionCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public Guid PSaleSectionItemId { get; set; }

		public int SequenceNo { get; set; }

		public string? OptionLabel { get; set; }

		public string? DefaultValue { get; set; }

		/// <summary>
		/// แสดงผลตาม Section
		/// </summary>
		public Guid? ShowSectionId { get; set; }

		public Guid? Master_ListId { get; set; }

		public virtual ProcessSale_Section_ItemCustom? PSaleSectionItem { get; set; }

	}
}

using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Sales
{
	public class Sale_Document_FileCustom : CommonModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int CreateBy { get; set; }

		public Guid SaleId { get; set; }

		public string Url { get; set; } = null!;

		public string Name { get; set; } = null!;

		/// <summary>
		/// 1=รูปบัตรประชาชน 2=ทะเบียนนบ้าน 3=เอกสารอื่นๆ 4=เอกสารเพิ่มเติม
		/// </summary>
		public short Type { get; set; }

	}
}

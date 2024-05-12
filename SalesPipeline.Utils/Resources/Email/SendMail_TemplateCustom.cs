using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Email
{
	public class SendMail_TemplateCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		/// <summary>
		/// วันที่สร้าง
		/// </summary>
		public DateTime CreateDate { get; set; }

		public string Code { get; set; } = null!;

		public string? Subject { get; set; }

		public string Message { get; set; } = null!;

	}
}

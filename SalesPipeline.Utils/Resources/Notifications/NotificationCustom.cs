using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Notifications
{
	public class NotificationCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public Guid? SaleId { get; set; }

		/// <summary>
		/// 1=รายการลูกค้าใหม่ ,2=อนุมัติคำขอ ,3=ส่งกลับ
		/// </summary>
		public int EventId { get; set; }

		public string? EventName { get; set; }

		/// <summary>
		/// FK รหัสผู้ใช้ ที่สร้างการแจ้งเตือน
		/// </summary>
		public int FromUserId { get; set; }

		public string? FromUserName { get; set; }

		/// <summary>
		/// FK รหัสผู้ใช้ ที่จะได้รับการแจ้งเตือน
		/// </summary>
		public int ToUserId { get; set; }

		public string? ToUserName { get; set; }

		/// <summary>
		/// 0=ยังไม่ได้อ่าน ,1=อ่านแล้ว
		/// </summary>
		public short? IsRead { get; set; }

		/// <summary>
		/// วันที่อ่าน
		/// </summary>
		public DateTime? ReadDate { get; set; }

		public string? RedirectUrl { get; set; }

		public string? ActionName1 { get; set; }

		public string? ActionName2 { get; set; }

		public string? ActionName3 { get; set; }
	}
}

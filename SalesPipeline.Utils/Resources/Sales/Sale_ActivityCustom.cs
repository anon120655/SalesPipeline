using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Sales
{
	public class Sale_ActivityCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public Guid SaleId { get; set; }

		/// <summary>
		/// ชื่อผู้ติดต่อ
		/// </summary>
		public string? ContactName { get; set; }

		/// <summary>
		/// ติดต่อ(ครั้ง)
		/// </summary>
		public int Contact { get; set; }

		/// <summary>
		/// เข้าพบ(ครั้ง)
		/// </summary>
		public int Meet { get; set; }

		/// <summary>
		/// ยื่นเอกสาร(ครั้ง)
		/// </summary>
		public int Document { get; set; }

		/// <summary>
		/// ผลลัพธ์(ครั้ง)
		/// </summary>
		public int Result { get; set; }

		/// <summary>
		/// ปิดการขาย(ครั้ง)
		/// </summary>
		public int CloseSale { get; set; }

		public virtual SaleCustom? Sale { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Customers
{
	public class Customer_ShareholderCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public int SequenceNo { get; set; }

		public Guid CustomerId { get; set; }

		/// <summary>
		/// ชื่อผู้ถือหุ้น
		/// </summary>
		public string? Name { get; set; }

		/// <summary>
		/// สัญชาติ
		/// </summary>
		public string? Nationality { get; set; }

		/// <summary>
		/// สัดส่วนการถือหุ้น
		/// </summary>
		public string? Proportion { get; set; }

		/// <summary>
		/// จำนวนหุ้นที่ถือ
		/// </summary>
		public int? NumberShareholder { get; set; }

		/// <summary>
		/// มูลค่าหุ้นทั้งหมด
		/// </summary>
		public decimal? TotalShareValue { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Sales
{
	public class Sale_Status_TotalCustom
	{
		public int Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public bool IsUpdate { get; set; }

		public int UserId { get; set; }

		/// <summary>
		/// ลูกค้าทั้งหมด
		/// </summary>
		public int AllCustomer { get; set; }

		/// <summary>
		/// รอการติดต่อ
		/// </summary>
		public int WaitContact { get; set; }

		/// <summary>
		/// ติดต่อ
		/// </summary>
		public int Contact { get; set; }

		/// <summary>
		/// รอการเข้าพบ
		/// </summary>
		public int WaitMeet { get; set; }

		/// <summary>
		/// เข้าพบ
		/// </summary>
		public int Meet { get; set; }

		/// <summary>
		/// ยื่นเอกสาร
		/// </summary>
		public int SubmitDocument { get; set; }

		/// <summary>
		/// ผลลัพธ์
		/// </summary>
		public int Results { get; set; }

		/// <summary>
		/// ปิดการขาย
		/// </summary>
		public int CloseSale { get; set; }
	}
}

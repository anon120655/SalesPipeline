using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Authorizes.Users
{
	public class User_LoanCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		/// <summary>
		/// พนักงานที่ดูแล
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// พนักงานสินเชื่อที่ดูแล
		/// </summary>
		public int UserLoanId { get; set; }
	}
}

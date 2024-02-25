using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Assignments
{
	public class StepAssignLoanModel
	{
		/// <summary>
		/// หน้าหลักมอบหมาย
		/// </summary>
		public const int Home = 1;
		/// <summary>
		/// ลูกค้าที่ได้รับมอบหมาย
		/// </summary>
		public const int Customer = 2;
		/// <summary>
		/// ส่งกลับ
		/// </summary>
		public const int Return = 3;
		/// <summary>
		/// ผู้รับผิดชอบ
		/// </summary>
		public const int Assigned = 4;
		/// <summary>
		/// สรุปผู้รับผิดชอบและลูกค้าที่ได้รับมอบหมาย
		/// </summary>
		public const int Summary = 5;
		/// <summary>
		/// กด Confirm
		/// </summary>
		public const int Confirm = 6;
	}
}

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
		/// ผู้รับผิดชอบ
		/// </summary>
		public const int Assigned = 3;
		/// <summary>
		/// สรุปผู้รับผิดชอบและลูกค้าที่ได้รับมอบหมาย
		/// </summary>
		public const int Summary = 4;
	}
}

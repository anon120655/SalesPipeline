using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Thailands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Assignments
{
	public class AssignmentCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int? BranchId { get; set; }

		/// <summary>
		/// รหัสสาขา
		/// </summary>
		public string? BranchCode { get; set; }

		/// <summary>
		/// สาขา
		/// </summary>
		public string? BranchName { get; set; }

		/// <summary>
		/// UserId ผู้จัดการศูนย์ที่ได้รับมอบหมาย
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// รหัสพนักงานผู้จัดการศูนย์
		/// </summary>
		public string? EmployeeId { get; set; }

		/// <summary>
		/// ชื่อผู้จัดการศูนย์
		/// </summary>
		public string? EmployeeName { get; set; }

		public string? Tel { get; set; }

		/// <summary>
		/// จำนวนพนง.สินเชื่อ
		/// </summary>
		public int? RMNumber { get; set; }

		/// <summary>
		/// จำนวนลูกค้าปัจจุบันที่ดูแล
		/// </summary>
		public int? CurrentNumber { get; set; }

		public virtual InfoBranchCustom? Branch { get; set; }

		public virtual UserCustom? User { get; set; }

		//Custom
		public bool IsSelected { get; set; } = false;

	}
}

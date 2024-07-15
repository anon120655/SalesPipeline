using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Assignments
{
    /// <summary>
    /// พนักงานที่ถูกมอบหมาย
    /// </summary>
    public class Assignment_RMCustom : CommonModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		/// <summary>
		/// พนักงานที่ได้รับมอบหมาย
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// รหัสพนักงาน
		/// </summary>
		public string? EmployeeId { get; set; }

		/// <summary>
		/// ชื่อพนักงานที่ได้รับมอบหมาย
		/// </summary>
		public string? EmployeeName { get; set; }

		/// <summary>
		/// จำนวนลูกค้าปัจจุบันที่ดูแล
		/// </summary>
		public int? CurrentNumber { get; set; }

		public virtual List<Assignment_RM_SaleCustom>? Assignment_RM_Sales { get; set; }
		public virtual UserCustom? User { get; set; }

		//Custom
		public string? Tel { get; set; }
		public string? ProvinceName { get; set; }
		public string? BranchName { get; set; }
		public string? AreaNameJoin { get; set; }
		
		/// <summary>
		/// จำนวนการมอบหมาย
		/// </summary>
		public int? NumberAssignment {
			get
			{
				if (Assignment_RM_Sales != null)
				{
					return Assignment_RM_Sales.Count(x=>x.Status != StatusModel.Delete);
				}
				return null;
			}
		}
		/// <summary>
		/// ลูกค้าหลังมอบหมาย
		/// </summary>
		public int? NumberAfterAssignment
		{
			get
			{
				if (CurrentNumber.HasValue && NumberAssignment.HasValue)
				{
					return CurrentNumber + NumberAssignment;
				}
				return CurrentNumber;
			}
		}

		public bool IsSelect { get; set; }
		public bool IsShow { get; set; } = true;
	}
}

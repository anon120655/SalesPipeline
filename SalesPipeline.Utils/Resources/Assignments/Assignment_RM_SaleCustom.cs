using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Assignments
{
	/// <summary>
	/// ลูกค้าที่พนักงานดูแล
	/// </summary>
	public class Assignment_RM_SaleCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		/// <summary>
		/// ผู้มอบหมาย
		/// </summary>
		public int CreateBy { get; set; }

		/// <summary>
		/// ชื่อผู้มอบหมาย
		/// </summary>
		public string? CreateByName { get; set; }

		public Guid AssignmentRMId { get; set; }

		/// <summary>
		/// 1=อยู่ในความรับผิดชอบ 0=ถูกเปลี่ยนผู้รับผิดชอบ
		/// </summary>
		public short IsActive { get; set; }

		public Guid SaleId { get; set; }

		public string? Description { get; set; }

		public virtual Assignment_RMCustom? AssignmentRM { get; set; }

		public virtual SaleCustom? Sale { get; set; }

		//Custom
		public bool IsSelect { get; set; }
		public bool IsSelectMove { get; set; } = false;
		public bool IsShow { get; set; } = true;

	}
}

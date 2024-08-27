using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Authorizes.Users
{
	public class User_RoleCustom : CommonModel
	{
		public int Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int CreateBy { get; set; }

		public DateTime UpdateDate { get; set; }

		public int UpdateBy { get; set; }

		/// <summary>
		/// อนุญาตให้แก้ไข
		/// </summary>
		public bool IsModify { get; set; }

		public string? Code { get; set; }

		/// <summary>
		/// ชื่อหน้าที่
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public string? Name { get; set; }

		/// <summary>
		/// รายละเอียดหน้าที่
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public string? Description { get; set; }

		public string? iAuthenRoleCode { get; set; }

		public string? iAuthenRoleName { get; set; }

		/// <summary>
		/// 1=มีสิทธ์มอบหมาย ผจศ.
		/// </summary>
		public bool IsAssignCenter { get; set; }

		/// <summary>
		/// 1=มีสิทธ์มอบหมาย rm
		/// </summary>
		public bool IsAssignRM { get; set; }

		public string? org_id { get; set; }

		public string? org_name { get; set; }

		public virtual ICollection<User_PermissionCustom>? User_Permissions { get; set; }

        //Custom
        public bool IsAccess { get; set; }

    }
}

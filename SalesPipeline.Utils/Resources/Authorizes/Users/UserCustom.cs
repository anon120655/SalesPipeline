using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationAtt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SalesPipeline.Utils.Resources.Authorizes.Users
{
	public class UserCustom : CommonModel
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
		/// รหัสพนักงาน
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public string? EmployeeId { get; set; }

		public string? TitleName { get; set; }

		public string? FirstName { get; set; }

		public string? LastName { get; set; }

		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public string? FullName { get; set; }

		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public string? Email { get; set; }

		/// <summary>
		/// เบอร์โทร
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public string? Tel { get; set; }

		/// <summary>
		/// สาขา
		/// </summary>
		//[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public Guid? BranchId { get; set; }

		/// <summary>
		/// ฝ่ายส่วนงานธุรกิจสินเชื่อ
		/// </summary>
		public Guid? Master_DepartmentId { get; set; }

		/// <summary>
		/// ฝ่ายกิจการสาขาภาค
		/// </summary>
		[UserAtt(FieldName = "Master_Department_BranchId")]
		public Guid? Master_Department_BranchId { get; set; }

		/// <summary>
		/// ศูนย์ธุรกิจสินเชื่อ
		/// </summary>
		[UserAtt(FieldName = "Master_Department_CenterId")]
		public Guid? Master_Department_CenterId { get; set; }

		/// <summary>
		/// ผู้จัดการศูนย์ที่ดูแล พนักงาน RM
		/// </summary>
		//[UserAtt(FieldName = "AssignmentId")]
		//public Guid? AssignmentId { get; set; }

		/// <summary>
		/// จังหวัด
		/// </summary>
		[UserAtt(FieldName = "ProvinceId")]
		public int? ProvinceId { get; set; }

		public string? ProvinceName { get; set; }

		/// <summary>
		/// อำเภอ
		/// </summary>
		[UserAtt(FieldName = "AmphurId")]
		public int? AmphurId { get; set; }

		public string? AmphurName { get; set; }

		/// <summary>
		/// ตำแหน่ง
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public int? PositionId { get; set; }

		/// <summary>
		/// ระดับ
		/// </summary>
		[UserAtt(FieldName = "LevelId")]
		public int? LevelId { get; set; }

		/// <summary>
		/// ระดับหน้าที่
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public int? RoleId { get; set; }

		public string? PasswordHash { get; set; }

		public sbyte? LoginFail { get; set; }

		public virtual Master_BranchCustom? Branch { get; set; }
		public virtual Master_Department_BranchCustom? Master_Department_Branch { get; set; }
		public virtual Master_Department_CenterCustom? Master_Department_Center { get; set; }
		public virtual Master_PositionCustom? Position { get; set; }
		public virtual User_RoleCustom? Role { get; set; }
		public virtual ICollection<Assignment_RMCustom>? Assignment_RMs { get; set; }


		//Custom --------------------------------------
		public bool? IsValidate { get; set; }
		public List<string?>? ValidateError { get; set; }
		public string? FirstNames
		{
			get
			{
				if (!String.IsNullOrEmpty(FullName))
				{
					string[] name = FullName.Split(null);
					name = name.Where(x => !string.IsNullOrEmpty(x)).ToArray();
					bool matchEnglish = GeneralUtils.IsEngOnly(FullName);
					if (name.Count() == 2 && !matchEnglish)
					{
						return name[0];
					}
				}
				return FullName;
			}
		}

		public string? LastNames
		{
			get
			{
				if (!String.IsNullOrEmpty(FullName))
				{
					string[] name = FullName.Split(null);
					name = name.Where(x => !string.IsNullOrEmpty(x)).ToArray();
					bool matchEnglish = GeneralUtils.IsEngOnly(FullName);
					if (name.Count() == 2 && !matchEnglish)
					{
						return name[1];
					}
				}
				return string.Empty;
			}
		}

		[UserAtt(FieldName = "AssignmentId")]
		public Guid? AssignmentId { get; set; }

  //      private Guid? _AssignmentId;
		//[UserAtt(FieldName = "AssignmentId")]
		//public Guid? AssignmentId
		//{
		//	get
		//	{
		//		if (Assignment_RMs?.Count > 0)
		//		{
		//			var response = Assignment_RMs.FirstOrDefault();
		//			if (response != null)
		//			{
		//				return response.AssignmentId;
		//			}
		//		}
		//		return _AssignmentId;
		//	}
		//	set
		//	{
		//		_AssignmentId = value;				
		//	}
		//}


	}
}

using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationAtt;
using System.ComponentModel.DataAnnotations;

namespace SalesPipeline.Utils.Resources.Authorizes.Users
{
	public class UserCustom : CommonModel
	{
		public int Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int CreateBy { get; set; }

		public DateTime UpdateDate { get; set; }

		public int UpdateBy { get; set; }

		public string? UserName { get; set; }

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
		/// ฝ่ายส่วนงานธุรกิจสินเชื่อ
		/// </summary>
		public Guid? Master_DepartmentId { get; set; }

		/// <summary>
		/// กิจการสาขาภาค
		/// </summary>
		//[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		//[UserAtt(FieldName = "Master_Branch_RegionId")]
		public Guid? Master_Branch_RegionId { get; set; }

		/// <summary>
		/// จังหวัด
		/// </summary>
		//[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		//[UserAtt(FieldName = "ProvinceId")]
		public int? ProvinceId { get; set; }

		public string? ProvinceName { get; set; }

		/// <summary>
		/// อำเภอ
		/// </summary>
		public int? AmphurId { get; set; }

		public string? AmphurName { get; set; }

		/// <summary>
		/// สาขา
		/// </summary>
		//[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		//[UserAtt(FieldName = "BranchId")]
		public int? BranchId { get; set; }

		public string? BranchName { get; set; }

		/// <summary>
		/// ตำแหน่ง
		/// </summary>
		//[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public int? PositionId { get; set; }

		/// <summary>
		/// ระดับ
		/// </summary>
		//[UserAtt(FieldName = "LevelId")]
		public int? LevelId { get; set; }

		/// <summary>
		/// ระดับหน้าที่
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public int? RoleId { get; set; }

		public string? PasswordHash { get; set; }

		public sbyte? LoginFail { get; set; }

		public short? IsSentMail { get; set; }

		/// <summary>
		/// url ลายเซ็น
		/// </summary>
		public string? UrlSignature { get; set; }

		public string? TokenApi { get; set; }

		/// <summary>
		/// 1=iAuthen
		/// </summary>
		public int Create_Type { get; set; }


		//*********** BAAC Model Begin **************

		public int? authen_fail_time { get; set; }

		public string? branch_code { get; set; }

		public string? branch_name { get; set; }

		public string? cbs_id { get; set; }

		public string? change_password_url { get; set; }

		public string? create_password_url { get; set; }

		public string? email_baac { get; set; }

		public string? employee_id { get; set; }

		public string? employee_position_id { get; set; }

		public string? employee_position_level { get; set; }

		public string? employee_position_name { get; set; }

		public string? employee_status { get; set; }

		public string? first_name_th { get; set; }

		public bool? image_existing { get; set; }

		public string? job_field_id { get; set; }

		public string? job_field_name { get; set; }

		public string? job_id { get; set; }

		public string? job_name { get; set; }

		public string? last_name_th { get; set; }

		public DateTime? lastauthen_timestamp { get; set; }

		public string? mobile_no { get; set; }

		public string? name_en { get; set; }

		public string? org_id { get; set; }

		public string? org_name { get; set; }

		public string? organization_48 { get; set; }

		public string? organization_abbreviation { get; set; }

		public string? organization_upper_id { get; set; }

		public string? organization_upper_id2 { get; set; }

		public string? organization_upper_id3 { get; set; }

		public string? organization_upper_name { get; set; }

		public string? organization_upper_name2 { get; set; }

		public string? organization_upper_name3 { get; set; }

		public bool? password_unexpire { get; set; }

		public bool? requester_active { get; set; }

		public bool? requester_existing { get; set; }

		public DateTime? timeresive { get; set; }

		public DateTime? timesend { get; set; }

		public string? title_th { get; set; }

		public string? title_th_2 { get; set; }

		public string? user_class { get; set; }

		public bool? username_active { get; set; }

		public bool? username_existing { get; set; }

		public string? working_status { get; set; }

		//*********** BAAC Model End **************

		public virtual Master_Branch_RegionCustom? Master_Branch_Region { get; set; }
		public virtual Master_PositionCustom? Position { get; set; }
		public virtual User_RoleCustom? Role { get; set; }
		public virtual List<User_Target_SaleCustom>? User_Target_SaleUsers { get; set; }
		public virtual List<User_AreaCustom>? User_Areas { get; set; }


		//Custom

		/// <summary>
		/// 1=IAuth
		/// </summary>
		public int? UpdateChannel { get; set; }
		public string? DefaultPassword { get; set; }
        public string? Master_DepartmentName { get; set; }
		public string? Master_Branch_RegionName { get; set; }
		public string? PositionName { get; set; }
		public string? RoleName { get; set; }
		public bool IsSelected { get; set; }
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

	}
}

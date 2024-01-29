using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
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
		public Guid? BranchId { get; set; }

		/// <summary>
		/// ฝ่ายงานกิจการสาขา
		/// </summary>
		public Guid? DivBranchId { get; set; }

		/// <summary>
		/// ฝ่ายงานศูนย์ธุรกิจสินเชื่อ
		/// </summary>
		public Guid? DivLoanId { get; set; }

		/// <summary>
		/// ตำแหน่ง
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public int? PositionId { get; set; }

		/// <summary>
		/// ระดับ
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public int? LevelId { get; set; }

		/// <summary>
		/// ระดับหน้าที่
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public int? RoleId { get; set; }

		public string? PasswordHash { get; set; }

		public sbyte? LoginFail { get; set; }

		public virtual Master_Division_BranchCustom? DivBranch { get; set; }
		public virtual Master_Division_LoanCustom? DivLoan { get; set; }
		public virtual Master_PositionCustom? Position { get; set; }
		public virtual User_RoleCustom? Role { get; set; }

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

	}
}

using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.iAuthen;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SalesPipeline.Utils.Resources.Authorizes.Auths
{
	public class LoginResponseModel
	{
		public LoginResponseModel()
		{
			User_Permissions = new List<User_PermissionCustom>();
		}

		public int Id { get; set; }
		public string? EmployeeId { get; set; }
		public string? TitleName { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public int? PositionId { get; set; }
		public string? PositionName { get; set; }
		public Guid? Master_Department_BranchId { get; set; }
		public string? Master_Department_BranchName { get; set; }
		public int? ProvinceId { get; set; }
		public string? ProvinceName { get; set; }
		public int? BranchId { get; set; }
		public string? BranchName { get; set; }
		public int? RoleId { get; set; }
		public string? RoleCode { get; set; }
		public string? RoleName { get; set; }
		public bool IsAssignCenter { get; set; }
		public bool IsAssignRM { get; set; }
		public int? LevelId { get; set; }
		public string? access_token { get; set; }
		public string? expires_in { get; set; }
		public int? OverdueNotify { get; set; }
		public List<User_PermissionCustom> User_Permissions { get; set; }
		public List<User_AreaCustom>? User_Areas { get; set; }
		public iAuthenResponse? iauthen { get; set; }


		public string? CheckData { get; set; }

		public string? FullName
		{
			get
			{
				return $"{TitleName}{FirstName} {LastName}";
			}
		}
		public string? AreasJoin
		{
			get
			{
				if (User_Areas?.Count > 0)
				{
					return string.Join(",", User_Areas.Select(x => x.ProvinceName));
				}
				return null;
			}
		}

	}
}

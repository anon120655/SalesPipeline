using SalesPipeline.Utils.Resources.Authorizes.Users;
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
		public string? TitleName { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public int? PositionId { get; set; }
		public string? PositionName { get; set; }
		public Guid? DivBranchId { get; set; }
		public string? DivBranchName { get; set; }
		public Guid? DivLoanId { get; set; }
		public string? DivLoanName { get; set; }
		public int? RoleId { get; set; }
		public int? RoleCode { get; set; }
		public string? RoleName { get; set; }
		public string? access_token { get; set; }
		public string? expires_in { get; set; }
		public List<User_PermissionCustom> User_Permissions { get; set; }

		public string? FullName
		{
			get
			{
				return $"{TitleName}{FirstName} {LastName}";
			}
		}

	}
}

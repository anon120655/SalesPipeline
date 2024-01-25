using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Thailands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
    public class LookUpResource
	{
		public List<MenuItemCustom>? MenuItem { get; set; }
		public List<Master_PositionCustom>? Positions { get; set; }
		public List<Master_DepartmentCustom>? Departments { get; set; }
		public List<User_LevelCustom>? UserLevels { get; set; }
		public List<Master_RegionCustom>? Regions { get; set; }
		public List<Master_BranchCustom>? Branchs { get; set; }
		public List<User_BranchCustom>? RMUser { get; set; }
		public List<Master_YieldCustom>? Yield { get; set; }
		public List<Master_ChainCustom>? Chain { get; set; }
		public List<Master_Division_BranchCustom>? DivisionBranch { get; set; }
		public List<Master_Division_LoanCustom>? DivisionLoan { get; set; }
		public List<Master_BusinessSizeCustom>? BusinessSize { get; set; }
		public List<Master_BusinessTypeCustom>? BusinessType { get; set; }
		public List<Master_ContactChannelCustom>? ContactChannel { get; set; }
		public List<Master_ISICCodeCustom>? ISICCode { get; set; }
		public List<Master_StatusSaleCustom>? StatusSale { get; set; }
		public List<InfoProvinceCustom>? Provinces { get; set; }
		public List<InfoAmphurCustom>? Amphurs { get; set; }
		public List<InfoTambolCustom>? Tambols { get; set; }
	}
}

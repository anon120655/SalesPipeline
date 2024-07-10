using SalesPipeline.Utils.Resources.Assignments;
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
		public List<Master_ListCustom>? MasterList { get; set; }
		public List<MenuItemCustom>? MenuItem { get; set; }
		public List<Master_PositionCustom>? Positions { get; set; }
		public List<User_LevelCustom>? UserLevels { get; set; }
		public List<User_RoleCustom>? UserRoles { get; set; }
		public List<Assignment_RMCustom>? RMUser { get; set; }
		public List<Assignment_CenterCustom>? AssignmentCenter { get; set; }
		public List<Assignment_RMCustom>? AssignmentUser { get; set; }
		public List<Master_YieldCustom>? Yield { get; set; }
		public List<Master_ChainCustom>? Chain { get; set; }
		public List<Master_DepartmentCustom>? Departments { get; set; }
		public List<Master_Branch_RegionCustom>? DepartmentBranch { get; set; }
		public List<Master_BusinessSizeCustom>? BusinessSize { get; set; }
		public List<Master_BusinessTypeCustom>? BusinessType { get; set; }
		public List<Master_ContactChannelCustom>? ContactChannel { get; set; }
		public List<Master_ISICCodeCustom>? ISICCode { get; set; }
		public List<Master_TSICCustom>? TSIC { get; set; }
		public List<Master_LoanTypeCustom>? LoanType { get; set; }
		public List<Master_ReasonReturnCustom>? ReasonReturn { get; set; }
		public List<Master_YearCustom>? Years { get; set; }
		public List<Master_StatusSaleCustom>? StatusSale { get; set; }
		public List<InfoProvinceCustom>? Provinces { get; set; }
		public List<InfoAmphurCustom>? Amphurs { get; set; }
		public List<InfoTambolCustom>? Tambols { get; set; }
		public List<InfoBranchCustom>? Branchs { get; set; }
		public List<Master_Pre_Interest_PayTypeCustom>? Pre_Interest_PayType { get; set; }
		public List<Master_Pre_Interest_RateTypeCustom>? Pre_Interest_RateType { get; set; }
		public List<Master_Pre_Applicant_LoanCustom>? Pre_Applicant_Loan { get; set; }
		public List<Master_Pre_BusinessTypeCustom>? Pre_BusinessType { get; set; }
		public List<SelectModel>? Pre_Applicant_LoanModel { get; set; }
		public List<SelectModel>? Pre_BusinessTypeModel { get; set; }
		public List<SelectModel>? Periods { get; set; }
	}
}

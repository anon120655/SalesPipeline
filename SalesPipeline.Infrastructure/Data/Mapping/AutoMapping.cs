using AutoMapper;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.ProcessSales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Data.Mapping
{
	public class AutoMapping : Profile
	{
		public AutoMapping()
		{
			//User
			CreateMap<Entity.User, UserCustom>().ReverseMap();
			CreateMap<Entity.User_Level, User_LevelCustom>().ReverseMap();
			CreateMap<Entity.User_Role, User_RoleCustom>().ReverseMap();
			CreateMap<Entity.User_Permission, User_PermissionCustom>().ReverseMap();
			CreateMap<Entity.User_Branch, User_BranchCustom>().ReverseMap();

			//Master
			CreateMap<Entity.Master_Position, Master_PositionCustom>().ReverseMap();
			CreateMap<Entity.Master_Region, Master_RegionCustom>().ReverseMap();
			CreateMap<Entity.Master_Branch, Master_BranchCustom>().ReverseMap();
			CreateMap<Entity.MenuItem, MenuItemCustom>().ReverseMap();
			CreateMap<Entity.Master_Division_Branch, Master_Division_BranchCustom>().ReverseMap();
			CreateMap<Entity.Master_Division_Loan, Master_Division_LoanCustom>().ReverseMap();
			CreateMap<Entity.Master_LoanType, Master_LoanTypeCustom>().ReverseMap();
			CreateMap<Entity.Master_ReasonReturn, Master_ReasonReturnCustom>().ReverseMap();
			CreateMap<Entity.Master_SLAOperation, Master_SLAOperationCustom>().ReverseMap();
			CreateMap<Entity.Master_Yield, Master_YieldCustom>().ReverseMap();
			CreateMap<Entity.Master_Chain, Master_ChainCustom>().ReverseMap();

			//ProcessSales
			CreateMap<Entity.ProcessSale, ProcessSaleCustom>().ReverseMap();
			CreateMap<Entity.ProcessSale_Section, ProcessSale_SectionCustom>().ReverseMap();
			CreateMap<Entity.ProcessSale_Section_Item, ProcessSale_Section_ItemCustom>().ReverseMap();
			CreateMap<Entity.ProcessSale_Section_ItemOption, ProcessSale_Section_ItemOptionCustom>().ReverseMap();

			CreateMap<Entity.ProcessSale_Reply, ProcessSale_ReplyCustom>().ReverseMap();
			CreateMap<Entity.ProcessSale_Reply_Section, ProcessSale_Reply_SectionCustom>().ReverseMap();
			CreateMap<Entity.ProcessSale_Reply_Section_Item, ProcessSale_Reply_Section_ItemCustom>().ReverseMap();
			CreateMap<Entity.ProcessSale_Reply_Section_ItemValue, ProcessSale_Reply_Section_ItemValueCustom>().ReverseMap();

			//Customer
			CreateMap<Entity.Customer, CustomerCustom>().ReverseMap();
			CreateMap<Entity.Customer_Committee, Customer_CommitteeCustom>().ReverseMap();
			CreateMap<Entity.Customer_Shareholder, Customer_ShareholderCustom>().ReverseMap();

			//Systems
			CreateMap<Entity.System_Signature, System_SignatureCustom>().ReverseMap();
			CreateMap<Entity.System_SLA, System_SLACustom>().ReverseMap();

		}
	}
}

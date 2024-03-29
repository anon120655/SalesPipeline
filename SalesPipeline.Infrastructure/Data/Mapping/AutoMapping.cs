using AutoMapper;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Notifications;
using SalesPipeline.Utils.Resources.ProcessSales;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;

namespace SalesPipeline.Infrastructure.Data.Mapping
{
    public class AutoMapping : Profile
	{
		public AutoMapping()
		{
			//Dashboard
			CreateMap<Entity.Dash_Status_Total, Dash_Status_TotalCustom>().ReverseMap();
			CreateMap<Entity.Dash_Avg_Number, Dash_Avg_NumberCustom>().ReverseMap();
			CreateMap<Entity.Dash_Map_Thailand, Dash_Map_ThailandCustom>().ReverseMap();

			//User
			CreateMap<Entity.User, UserCustom>().ReverseMap();
			CreateMap<Entity.User_Level, User_LevelCustom>().ReverseMap();
			CreateMap<Entity.User_Role, User_RoleCustom>().ReverseMap();
			CreateMap<Entity.User_Permission, User_PermissionCustom>().ReverseMap();
			//CreateMap<Entity.User_Loan, User_LoanCustom>().ReverseMap();
			//CreateMap<Entity.User_Branch, User_BranchCustom>().ReverseMap();

			//Master
			CreateMap<Entity.Master_List, Master_ListCustom>().ReverseMap();
			CreateMap<Entity.Master_ProductProgramBank, Master_ProductProgramBankCustom>().ReverseMap();
			CreateMap<Entity.Master_TypeLoanRequest, Master_TypeLoanRequestCustom>().ReverseMap();
			CreateMap<Entity.Master_Proceed, Master_ProceedCustom>().ReverseMap();
			CreateMap<Entity.Master_Position, Master_PositionCustom>().ReverseMap();
			CreateMap<Entity.Master_Region, Master_RegionCustom>().ReverseMap();
			CreateMap<Entity.MenuItem, MenuItemCustom>().ReverseMap();
			CreateMap<Entity.Master_Department, Master_DepartmentCustom>().ReverseMap();
			CreateMap<Entity.Master_Department_Branch, Master_Department_BranchCustom>().ReverseMap();
			CreateMap<Entity.Master_Department_Center, Master_Department_CenterCustom>().ReverseMap();
			CreateMap<Entity.Master_LoanType, Master_LoanTypeCustom>().ReverseMap();
			CreateMap<Entity.Master_ReasonReturn, Master_ReasonReturnCustom>().ReverseMap();
			CreateMap<Entity.Master_SLAOperation, Master_SLAOperationCustom>().ReverseMap();
			CreateMap<Entity.Master_Yield, Master_YieldCustom>().ReverseMap();
			CreateMap<Entity.Master_Chain, Master_ChainCustom>().ReverseMap();
			CreateMap<Entity.Master_BusinessSize, Master_BusinessSizeCustom>().ReverseMap();
			CreateMap<Entity.Master_BusinessType, Master_BusinessTypeCustom>().ReverseMap();
			CreateMap<Entity.Master_ContactChannel, Master_ContactChannelCustom>().ReverseMap();
			CreateMap<Entity.Master_ISICCode, Master_ISICCodeCustom>().ReverseMap();
			CreateMap<Entity.Master_StatusSale, Master_StatusSaleCustom>().ReverseMap();

			//Thailand
			CreateMap<Entity.InfoProvince, InfoProvinceCustom>().ReverseMap();
			CreateMap<Entity.InfoAmphur, InfoAmphurCustom>().ReverseMap();
			CreateMap<Entity.InfoTambol, InfoTambolCustom>().ReverseMap();
			CreateMap<Entity.InfoBranch, InfoBranchCustom>().ReverseMap();


			//ProcessSales
			CreateMap<Entity.ProcessSale, ProcessSaleCustom>().ReverseMap();
			CreateMap<Entity.ProcessSale_Section, ProcessSale_SectionCustom>().ReverseMap();
			CreateMap<Entity.ProcessSale_Section_Item, ProcessSale_Section_ItemCustom>().ReverseMap();
			CreateMap<Entity.ProcessSale_Section_ItemOption, ProcessSale_Section_ItemOptionCustom>().ReverseMap();

			//Sale
			CreateMap<Entity.Sale, SaleCustom>().ReverseMap();
			CreateMap<Entity.Sale_Return, Sale_ReturnCustom>().ReverseMap();
			CreateMap<Entity.Sale_Status, Sale_StatusCustom>().ReverseMap();
			CreateMap<Entity.Sale_Reply, Sale_ReplyCustom>().ReverseMap();
			CreateMap<Entity.Sale_Reply_Section, Sale_Reply_SectionCustom>().ReverseMap();
			CreateMap<Entity.Sale_Reply_Section_Item, Sale_Reply_Section_ItemCustom>().ReverseMap();
			CreateMap<Entity.Sale_Reply_Section_ItemValue, Sale_Reply_Section_ItemValueCustom>().ReverseMap();
			CreateMap<Entity.Sale_Reply_Section_ItemValue, Sale_Reply_Section_ItemValueCustom>().ReverseMap();
			CreateMap<Entity.Sale_Contact, Sale_ContactCustom>().ReverseMap();
			CreateMap<Entity.Sale_Meet, Sale_MeetCustom>().ReverseMap();
			CreateMap<Entity.Sale_Document, Sale_DocumentCustom>().ReverseMap();
			CreateMap<Entity.Sale_Result, Sale_ResultCustom>().ReverseMap();
			CreateMap<Entity.Sale_Close_Sale, Sale_Close_SaleCustom>().ReverseMap();
			CreateMap<Entity.Sale_Contact_History, Sale_Contact_HistoryCustom>().ReverseMap();
			CreateMap<Entity.Sale_Status_Total, Sale_Status_TotalCustom>().ReverseMap();
			CreateMap<Entity.Sale_Duration, Sale_DurationCustom>().ReverseMap();
			CreateMap<Entity.Sales_Activity, Sales_ActivityCustom>().ReverseMap();

			//Customer
			CreateMap<Entity.Customer, CustomerCustom>().ReverseMap();
			CreateMap<Entity.Customer_Committee, Customer_CommitteeCustom>().ReverseMap();
			CreateMap<Entity.Customer_Shareholder, Customer_ShareholderCustom>().ReverseMap();

			//Systems
			CreateMap<Entity.System_Signature, System_SignatureCustom>().ReverseMap();
			CreateMap<Entity.System_SLA, System_SLACustom>().ReverseMap();

			//Assignment
			CreateMap<Entity.Assignment_Branch, Assignment_BranchCustom>().ReverseMap();
			CreateMap<Entity.Assignment_MCenter, Assignment_MCenterCustom>().ReverseMap();
			CreateMap<Entity.Assignment_RM, Assignment_RMCustom>().ReverseMap();
			CreateMap<Entity.Assignment_RM_Sale, Assignment_RM_SaleCustom>().ReverseMap();

			//Notification
			CreateMap<Entity.Notification, NotificationCustom>().ReverseMap();

		}
	}
}

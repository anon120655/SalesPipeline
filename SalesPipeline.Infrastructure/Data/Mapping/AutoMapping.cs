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
using SalesPipeline.Utils.Resources.Email;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;
using SalesPipeline.Utils.Resources.Loans;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Phoenixs;

namespace SalesPipeline.Infrastructure.Data.Mapping
{
    public class AutoMapping : Profile
	{
		public AutoMapping()
		{
			//PreApprove
			CreateMap<Entity.Pre_Cal, Pre_CalCustom>().ReverseMap();
			CreateMap<Entity.Pre_Cal_Info, Pre_Cal_InfoCustom>().ReverseMap();
			CreateMap<Entity.Pre_Cal_Info_Score, Pre_Cal_Info_ScoreCustom>().ReverseMap();
			CreateMap<Entity.Pre_Cal_Fetu_Stan, Pre_Cal_Fetu_StanCustom>().ReverseMap();
			CreateMap<Entity.Pre_Cal_Fetu_Stan_ItemOption, Pre_Cal_Fetu_Stan_ItemOptionCustom>().ReverseMap();
			CreateMap<Entity.Pre_Cal_Fetu_Stan_Score, Pre_Cal_Fetu_Stan_ScoreCustom>().ReverseMap();
			CreateMap<Entity.Pre_Cal_Fetu_App, Pre_Cal_Fetu_AppCustom>().ReverseMap();
			CreateMap<Entity.Pre_Cal_Fetu_App_Item, Pre_Cal_Fetu_App_ItemCustom>().ReverseMap();
			CreateMap<Entity.Pre_Cal_Fetu_App_Item_Score, Pre_Cal_Fetu_App_Item_ScoreCustom>().ReverseMap();
			CreateMap<Entity.Pre_Cal_Fetu_Bu, Pre_Cal_Fetu_BuCustom>().ReverseMap();
			CreateMap<Entity.Pre_Cal_Fetu_Bus_Item, Pre_Cal_Fetu_Bus_ItemCustom>().ReverseMap();
			CreateMap<Entity.Pre_Cal_Fetu_Bus_Item_Score, Pre_Cal_Fetu_Bus_Item_ScoreCustom>().ReverseMap();
			CreateMap<Entity.Pre_Cal_WeightFactor, Pre_Cal_WeightFactorCustom>().ReverseMap();
			CreateMap<Entity.Pre_Cal_WeightFactor_Item, Pre_Cal_WeightFactor_ItemCustom>().ReverseMap();
			CreateMap<Entity.Pre_CreditScore, Pre_CreditScoreCustom>().ReverseMap();
			CreateMap<Entity.Pre_ChancePass, Pre_ChancePassCustom>().ReverseMap();
			CreateMap<Entity.Pre_Factor, Pre_FactorCustom>().ReverseMap();
			CreateMap<Entity.Pre_Factor_Info, Pre_Factor_InfoCustom>().ReverseMap();
			CreateMap<Entity.Pre_Factor_Stan, Pre_Factor_StanCustom>().ReverseMap();
			CreateMap<Entity.Pre_Factor_App, Pre_Factor_AppCustom>().ReverseMap();
			CreateMap<Entity.Pre_Factor_Bu, Pre_Factor_BuCustom>().ReverseMap();
			CreateMap<Entity.Pre_Result, Pre_ResultCustom>().ReverseMap();
			CreateMap<Entity.Pre_Result_Item, Pre_Result_ItemCustom>().ReverseMap();


			//Loan
			CreateMap<Entity.Loan, LoanCustom>().ReverseMap();
			CreateMap<Entity.Loan_Period, Loan_PeriodCustom>().ReverseMap();
			CreateMap<Entity.Loan_AppLoan, Loan_AppLoanCustom>().ReverseMap();
			CreateMap<Entity.Loan_BusType, Loan_BusTypeCustom>().ReverseMap();

			//Dashboard
			CreateMap<Entity.Dash_Status_Total, Dash_Status_TotalCustom>().ReverseMap();
			CreateMap<Entity.Dash_Avg_Number, Dash_AvgTop_NumberCustom>().ReverseMap();
			CreateMap<Entity.Dash_Map_Thailand, Dash_Map_ThailandCustom>().ReverseMap();
			CreateMap<Entity.Dash_Pie, Dash_PieCustom>().ReverseMap();

			//User
			CreateMap<Entity.User, UserCustom>().ReverseMap();
			CreateMap<Entity.User_Level, User_LevelCustom>().ReverseMap();
			CreateMap<Entity.User_Role, User_RoleCustom>().ReverseMap();
			CreateMap<Entity.User_Permission, User_PermissionCustom>().ReverseMap();
			CreateMap<Entity.User_Target_Sale, User_Target_SaleCustom>().ReverseMap();
			CreateMap<Entity.User_Login_Log, User_Login_LogCustom>().ReverseMap();
			CreateMap<Entity.User_Login_TokenNoti, User_Login_TokenNotiCustom>().ReverseMap();
			CreateMap<Entity.User_Area, User_AreaCustom>().ReverseMap();
			CreateMap<Entity.User_RefreshToken, User_RefreshTokenCustom>().ReverseMap();

			//Master
			CreateMap<Entity.Master_List, Master_ListCustom>().ReverseMap();
			CreateMap<Entity.Master_ProductProgramBank, Master_ProductProgramBankCustom>().ReverseMap();
			CreateMap<Entity.Master_TypeLoanRequest, Master_TypeLoanRequestCustom>().ReverseMap();
			CreateMap<Entity.Master_Proceed, Master_ProceedCustom>().ReverseMap();
			CreateMap<Entity.Master_Position, Master_PositionCustom>().ReverseMap();
			CreateMap<Entity.MenuItem, MenuItemCustom>().ReverseMap();
			CreateMap<Entity.Master_Department, Master_DepartmentCustom>().ReverseMap();
			CreateMap<Entity.Master_Branch_Region, Master_Branch_RegionCustom>().ReverseMap();
			CreateMap<Entity.Master_LoanType, Master_LoanTypeCustom>().ReverseMap();
			CreateMap<Entity.Master_ReasonReturn, Master_ReasonReturnCustom>().ReverseMap();
			CreateMap<Entity.Master_Yield, Master_YieldCustom>().ReverseMap();
			CreateMap<Entity.Master_Chain, Master_ChainCustom>().ReverseMap();
			CreateMap<Entity.Master_BusinessSize, Master_BusinessSizeCustom>().ReverseMap();
			CreateMap<Entity.Master_BusinessType, Master_BusinessTypeCustom>().ReverseMap();
			CreateMap<Entity.Master_ContactChannel, Master_ContactChannelCustom>().ReverseMap();
			CreateMap<Entity.Master_ISICCode, Master_ISICCodeCustom>().ReverseMap();
			CreateMap<Entity.Master_TSIC, Master_TSICCustom>().ReverseMap();
			CreateMap<Entity.Master_Reason_CloseSale, Master_Reason_CloseSaleCustom>().ReverseMap();
			CreateMap<Entity.Master_StatusSale, Master_StatusSaleCustom>().ReverseMap();
			CreateMap<Entity.Master_Year, Master_YearCustom>().ReverseMap();
			CreateMap<Entity.Master_Pre_Interest_RateType, Master_Pre_Interest_RateTypeCustom>().ReverseMap();
			CreateMap<Entity.Master_Pre_Applicant_Loan, Master_Pre_Applicant_LoanCustom>().ReverseMap();
			CreateMap<Entity.Master_Pre_BusinessType, Master_Pre_BusinessTypeCustom>().ReverseMap();
			CreateMap<Entity.Master_Pre_Interest_PayType, Master_Pre_Interest_PayTypeCustom>().ReverseMap();

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
			CreateMap<Entity.Sale_Contact_History, Sale_Contact_HistoryCustom>().ReverseMap();
            CreateMap<Entity.Sale_Contact_Info, Sale_Contact_InfoCustom>().ReverseMap();
            CreateMap<Entity.Sale_Meet, Sale_MeetCustom>().ReverseMap();
			CreateMap<Entity.Sale_Document, Sale_DocumentCustom>().ReverseMap();
			CreateMap<Entity.Sale_Document_Upload, Sale_Document_UploadCustom>().ReverseMap();
			CreateMap<Entity.Sale_Result, Sale_ResultCustom>().ReverseMap();
			CreateMap<Entity.Sale_Close_Sale, Sale_Close_SaleCustom>().ReverseMap();
			CreateMap<Entity.Sale_Status_Total, Sale_Status_TotalCustom>().ReverseMap();
			CreateMap<Entity.Sale_Duration, Sale_DurationCustom>().ReverseMap();
			CreateMap<Entity.Sale_Activity, Sale_ActivityCustom>().ReverseMap();
			CreateMap<Entity.Sale_Deliver, Sale_DeliverCustom>().ReverseMap();
			CreateMap<Entity.Sale_Partner, Sale_PartnerCustom>().ReverseMap();
			CreateMap<Entity.Sale_Phoenix, Sale_PhoenixCustom>().ReverseMap();

			//Customer
			CreateMap<Entity.Customer, CustomerCustom>().ReverseMap();
			CreateMap<Entity.Customer_Committee, Customer_CommitteeCustom>().ReverseMap();
			CreateMap<Entity.Customer_Shareholder, Customer_ShareholderCustom>().ReverseMap();
			CreateMap<Entity.Customer_History, Customer_HistoryCustom>().ReverseMap();

			//Systems
			CreateMap<Entity.System_Signature, System_SignatureCustom>().ReverseMap();
			CreateMap<Entity.System_SLA, System_SLACustom>().ReverseMap();

			//Assignment
			CreateMap<Entity.Assignment_BranchReg, Assignment_BranchRegCustom>().ReverseMap();
			CreateMap<Entity.Assignment_Center, Assignment_CenterCustom>().ReverseMap();
			CreateMap<Entity.Assignment_RM, Assignment_RMCustom>().ReverseMap();
			CreateMap<Entity.Assignment_RM_Sale, Assignment_RM_SaleCustom>().ReverseMap();

			//Notification
			CreateMap<Entity.Notification, NotificationCustom>().ReverseMap();

			//SendMail
			CreateMap<Entity.SendMail_Template, SendMail_TemplateCustom>().ReverseMap();

			//Config
			CreateMap<Entity.System_Config, System_ConfigCustom>().ReverseMap();

		}
	}
}

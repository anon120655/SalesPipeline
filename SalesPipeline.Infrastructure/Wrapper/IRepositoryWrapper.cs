using Microsoft.EntityFrameworkCore.Storage;
using SalesPipeline.Infrastructure.Data.Context;
using SalesPipeline.Infrastructure.Data.Logger.Context;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Wrapper
{
	public interface IRepositoryWrapper
	{
		SalesPipelineContext Context { get; }
		SalesPipelineLogContext ContextLog { get; }
		IDbContextTransaction BeginTransaction();
		void Commit();

		//Repository 
		IRepositoryBase _db { get; }
		ILoggerRepo Logger { get; }
		IAuthorizes Authorizes { get; }
		IJwtUtils jwtUtils { get; }
		IFileRepository Files { get; }
		INotifys Notifys { get; }
		IMaster Master { get; }
		IDashboard Dashboard { get; }
		IMasterDepartment MasterDepartment { get; }
		IMasterBranchReg MasterBranchReg { get; }
		IMasterBranch MasterBranch { get; }
		IMasterLoanTypes MasterLoanTypes { get; }
		IMasterReasonReturns MasterReasonReturn { get; }
		IMasterYields MasterYield { get; }
		IMasterChains MasterChain { get; }
		IMasterBusinessSize MasterBusinessSize { get; }
		IMasterBusinessType MasterBusinessType { get; }
		IMasterContactChannel MasterContactChannel { get; }
		IMasterISICCode MasterISICCode { get; }
		IMasterTSIC MasterTSIC { get; }
		IMasterReasonCloseSale MasterReasonCloseSale { get; }
		IMasterStatusSale MasterStatusSale { get; }
		IMaster_Pre_PayType Master_Pre_PayType { get; }
		IMaster_Pre_RateType Master_Pre_RateType { get; }
		IMaster_Pre_App_Loan Master_Pre_App_Loan { get; }
		IMaster_Pre_BusType Master_Pre_BusType { get; }
		IThailand Thailand { get; }
		IAssignmentCenter AssignmentCenter { get; }
		IAssignmentRM AssignmentRM { get; }
		IReturnRepo Return { get; }
		ILoanRepo Loan { get; }
		IPreCal PreCal { get; }
		IPreCalInfo PreCalInfo { get; }
		IPreCalStan PreCalStan { get; }
		IPreCalApp PreCalApp { get; }
		IPreCalBus PreCalBus { get; }
		IPreCalWeight PreCalWeight { get; }
		IPreCreditScore PreCreditScore { get; }
		IPreChancePass PreChancePass { get; }
		IPreFactor PreFactor { get; }

		IProcessSales ProcessSale { get; }
		ISales Sales { get; }
		IUserRepo User { get; }
		ICustomers Customer { get; }
		ISystemRepo System { get; }
		IEmailSender EmailSender { get; }
	}
}

using Microsoft.EntityFrameworkCore.Storage;
using SalesPipeline.Infrastructure.Data.Context;
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
		IDbContextTransaction BeginTransaction();
		void Commit();

		//Repository 
		IRepositoryBase _db { get; }
		ILoggerRepo Logger { get; }
		IAuthorizes Authorizes { get; }
		IFileRepository Files { get; }
		INotifys Notifys { get; }
		IMaster Master { get; }
		IDashboard Dashboard { get; }
		IMasterDepartment MasterDepartment { get; }
		IMasterBranchReg MasterBranchReg { get; }
		IMasterLoanTypes MasterLoanTypes { get; }
		IMasterReasonReturns MasterReasonReturn { get; }
		IMasterSLAOperations MasterSLAOperation { get; }
		IMasterYields MasterYield { get; }
		IMasterChains MasterChain { get; }
		IMasterBusinessSize MasterBusinessSize { get; }
		IMasterBusinessType MasterBusinessType { get; }
		IMasterContactChannel MasterContactChannel { get; }
		IMasterISICCode MasterISICCode { get; }
		IMasterReasonCloseSale MasterReasonCloseSale { get; }
		IMasterStatusSale MasterStatusSale { get; }
		IThailand Thailand { get; }
		IAssignmentBranch AssignmentBranch { get; }
		IAssignmentCenter AssignmentCenter { get; }
		IAssignmentRM AssignmentRM { get; }
		IReturnRepo Return { get; }

		IProcessSales ProcessSale { get; }
		ISales Sales { get; }
		IUserRepo User { get; }
		ICustomers Customer { get; }
		ISystemRepo System { get; }
		IEmailSender EmailSender { get; }
	}
}

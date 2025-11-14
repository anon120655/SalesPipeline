using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Context;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Data.Logger.Context;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Repositorys;
using SalesPipeline.Utils;

namespace SalesPipeline.Infrastructure.Wrapper
{
	public class RepositoryWrapper : IRepositoryWrapper
	{
		private IDbContextTransaction transaction = null!;
		private readonly IHttpContextAccessor _accessor;
		private readonly HttpClient _httpClient;
		private readonly IMapper _mapper;
		//private readonly IJwtUtils _jwtUtils;
		private bool _isDisposed;
		private readonly NotificationService _notiService;
		private readonly IBackgroundJobClient _backgroundJobClient;

		public SalesPipelineContext Context { get; }
		public SalesPipelineLogContext ContextLog { get; }
		public IRepositoryBase _db { get; }
		public ILoggerRepo Logger { get; }
		public IAuthorizes Authorizes { get; }
		public IJwtUtils jwtUtils { get; }
		public INotifys Notifys { get; }
		public IFileRepository Files { get; }
		public IMaster Master { get; }
		public IDashboard Dashboard { get; }
		public IMasterBranchReg MasterBranchReg { get; }
		public IMasterBranch MasterBranch { get; }
		public IMasterDepartment MasterDepartment { get; }
		public IMasterLoanTypes MasterLoanTypes { get; }
		public IMasterReasonReturns MasterReasonReturn { get; }
		public IMasterYields MasterYield { get; }
		public IMasterChains MasterChain { get; }
		public IMasterBusinessSize MasterBusinessSize { get; }
		public IMasterBusinessType MasterBusinessType { get; }
		public IMasterContactChannel MasterContactChannel { get; }
		public IMasterISICCode MasterISICCode { get; }
		public IMasterTSIC MasterTSIC { get; }
		public IMasterReasonCloseSale MasterReasonCloseSale { get; }
		public IMasterStatusSale MasterStatusSale { get; }
		public IMaster_Pre_PayType Master_Pre_PayType { get; }
		public IMaster_Pre_RateType Master_Pre_RateType { get; }
		public IMaster_Pre_App_Loan Master_Pre_App_Loan { get; }
		public IMaster_Pre_BusType Master_Pre_BusType { get; }
		public IThailand Thailand { get; }
		public IAssignmentCenter AssignmentCenter { get; }
		public IAssignmentRM AssignmentRM { get; }
		public IReturnRepo Return { get; }
		public ILoanRepo Loan { get; }
		public IPreCal PreCal { get; }
		public IPreCalInfo PreCalInfo { get; }
		public IPreCalStan PreCalStan { get; }
		public IPreCalApp PreCalApp { get; }
		public IPreCalBus PreCalBus { get; }
		public IPreCalWeight PreCalWeight { get; }
		public IPreCreditScore PreCreditScore { get; }
		public IPreChancePass PreChancePass { get; }
		public IPreFactor PreFactor { get; }
		public IProcessSales ProcessSale { get; }
		public ISales Sales { get; }
		public IUserRepo User { get; }
		public ICustomers Customer { get; }
		public ISystemRepo System { get; }
		public IEmailSender EmailSender { get; }

		public RepositoryWrapper(SalesPipelineContext _context, SalesPipelineLogContext _contextLog, IOptions<AppSettings> settings, IMapper mapper,
													IHttpContextAccessor accessor, 
													HttpClient httpClient, 
													//IJwtUtils jwtUtils, 
													NotificationService notificationService
			, IBackgroundJobClient backgroundJobClient)
		{
			Context = _context;
			ContextLog = _contextLog;
			_accessor = accessor;
			_mapper = mapper;
			_httpClient = httpClient;
			//_jwtUtils = jwtUtils;
			_notiService = notificationService;
			_backgroundJobClient = backgroundJobClient;

			_db = new RepositoryBase(this);
			Logger = new LoggerRepo(this, settings, ContextLog);
			Authorizes = new Authorizes(this, _db, settings, _mapper);
			jwtUtils = new JwtUtils(this, settings);
			Notifys = new Notifys(this, _db, settings, _mapper, _notiService, _backgroundJobClient);
			Files = new FileRepository(this, _db, settings, _mapper);
			Master = new Master(this, _db, settings, _mapper);
			Dashboard = new Dashboard(this, _db, settings, _mapper);
			MasterBranchReg = new MasterBranchReg(this, _db, settings, _mapper);
			MasterBranch = new MasterBranch(this, _db, settings, _mapper);
			MasterDepartment = new MasterDepartment(this, _db, settings, _mapper);
			MasterLoanTypes = new MasterLoanTypes(this, _db, settings, _mapper);
			MasterReasonReturn = new MasterReasonReturns(this, _db, settings, _mapper);
			MasterYield = new MasterYields(this, _db, settings, _mapper);
			MasterChain = new MasterChains(this, _db, settings, _mapper);
			MasterBusinessSize = new MasterBusinessSize(this, _db, settings, _mapper);
			MasterBusinessType = new MasterBusinessType(this, _db, settings, _mapper);
			MasterContactChannel = new MasterContactChannel(this, _db, settings, _mapper);
			MasterISICCode = new MasterISICCode(this, _db, settings, _mapper);
			MasterTSIC = new MasterTSIC(this, _db, settings, _mapper);
			MasterReasonCloseSale = new MasterReasonCloseSale(this, _db, settings, _mapper);
			MasterStatusSale = new MasterStatusSale(this, _db, settings, _mapper);
			Master_Pre_PayType = new Master_Pre_PayType(this, _db, settings, _mapper);
			Master_Pre_RateType = new Master_Pre_RateType(this, _db, settings, _mapper);
			Master_Pre_App_Loan = new Master_Pre_App_Loan(this, _db, settings, _mapper);
			Master_Pre_BusType = new Master_Pre_BusType(this, _db, settings, _mapper);
			Thailand = new Thailand(this, _db, settings, _mapper);
			AssignmentCenter = new AssignmentCenter(this, _db, settings, _mapper);
			AssignmentRM = new AssignmentRM(this, _db, settings, _mapper);
			Return = new ReturnRepo(this, _db, settings, _mapper);
			Loan = new LoanRepo(this, _db, settings, _mapper);
			PreCal = new PreCal(this, _db, settings, _mapper);
			PreCalInfo = new PreCalInfo(this, _db, settings, _mapper);
			PreCalStan = new PreCalStan(this, _db, settings, _mapper);
			PreCalApp = new PreCalApp(this, _db, settings, _mapper);
			PreCalBus = new PreCalBus(this, _db, settings, _mapper);
			PreCalWeight = new PreCalWeight(this, _db, settings, _mapper);
			PreCreditScore = new PreCreditScore(this, _db, settings, _mapper);
			PreChancePass = new PreChancePass(this, _db, settings, _mapper);
			PreFactor = new PreFactor(this, _db, _mapper);
			ProcessSale = new ProcessSales(this, _db, settings, _mapper);
			Sales = new Sales(this, _db, settings, _mapper);
			User = new UserRepo(this, _db, settings, _mapper);
			Customer = new Customers(this, _db, settings, _mapper);
			System = new SystemRepo(this, _db, settings, _mapper);
			EmailSender = new EmailSender(this, _db, settings, _mapper, _accessor);
		}

		public IDbContextTransaction BeginTransaction()
		{
			transaction = Context.Database.BeginTransaction();
			return transaction;
		}

		public void Commit()
		{
			transaction.Commit();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed)
				return;

			if (disposing)
			{
				Context.Dispose();
				ContextLog.Dispose();

				_isDisposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

	}
}

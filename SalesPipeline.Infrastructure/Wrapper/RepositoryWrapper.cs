using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Context;
using SalesPipeline.Infrastructure.Data.Entity;
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
		private readonly IJwtUtils _jwtUtils;
		private bool _isDisposed;

		public SalesPipelineContext Context { get; }
		public IRepositoryBase _db { get; }
		public ILoggerRepo Logger { get; }
		public IAuthorizes Authorizes { get; }
		public INotifys Notifys { get; }
		public IFileRepository Files { get; }
		public IMaster Master { get; }
		public IMasterDepBranch MasterDepBranch { get; }
		public IMasterDepartment MasterDepartment { get; }
		public IMasterDepCenter MasterDepCenter { get; }
		public IMasterLoanTypes MasterLoanTypes { get; }
		public IMasterReasonReturns MasterReasonReturn { get; }
		public IMasterSLAOperations MasterSLAOperation { get; }
		public IMasterYields MasterYield { get; }
		public IMasterChains MasterChain { get; }
		public IMasterBusinessSize MasterBusinessSize { get; }
		public IMasterBusinessType MasterBusinessType { get; }
		public IMasterContactChannel MasterContactChannel { get; }
		public IMasterISICCode MasterISICCode { get; }
		public IMasterStatusSale MasterStatusSale { get; }
		public IThailand Thailand { get; }
		public IAssignmentCenter AssignmentCenter { get; }
		public IAssignmentRM AssignmentRM { get; }
		public IProcessSales ProcessSale { get; }
		public ISales Sales { get; }
		public IUserRepo User { get; }
		public ICustomers Customer { get; }
		public ISystemRepo System { get; }

		public RepositoryWrapper(SalesPipelineContext _context, IOptions<AppSettings> settings, IMapper mapper,
													IHttpContextAccessor accessor, HttpClient httpClient, IJwtUtils jwtUtils)
		{
			Context = _context;
			_accessor = accessor;
			_mapper = mapper;
			_httpClient = httpClient;
			_jwtUtils = jwtUtils;

			_db = new RepositoryBase(this);
			Logger = new LoggerRepo(this, _db, settings);
			Authorizes = new Authorizes(this, _db, settings, _jwtUtils, _mapper);
			Notifys = new Notifys(this, _db, settings, _mapper);
			Files = new FileRepository(this, _db, settings, _mapper);
			Master = new Master(this, _db, settings, _mapper);
			MasterDepBranch = new MasterDepBranch(this, _db, settings, _mapper);
			MasterDepartment = new MasterDepartment(this, _db, settings, _mapper);
			MasterDepCenter = new MasterDepCenter(this, _db, settings, _mapper);
			MasterLoanTypes = new MasterLoanTypes(this, _db, settings, _mapper);
			MasterReasonReturn = new MasterReasonReturns(this, _db, settings, _mapper);
			MasterSLAOperation = new MasterSLAOperations(this, _db, settings, _mapper);
			MasterYield = new MasterYields(this, _db, settings, _mapper);
			MasterChain = new MasterChains(this, _db, settings, _mapper);
			MasterBusinessSize = new MasterBusinessSize(this, _db, settings, _mapper);
			MasterBusinessType = new MasterBusinessType(this, _db, settings, _mapper);
			MasterContactChannel = new MasterContactChannel(this, _db, settings, _mapper);
			MasterISICCode = new MasterISICCode(this, _db, settings, _mapper);
			MasterStatusSale = new MasterStatusSale(this, _db, settings, _mapper);
			Thailand = new Thailand(this, _db, settings, _mapper);
			AssignmentCenter = new AssignmentCenter(this, _db, settings, _mapper);
			AssignmentRM = new AssignmentRM(this, _db, settings, _mapper);			
			ProcessSale = new ProcessSales(this, _db, settings, _mapper);
			Sales = new Sales(this, _db, settings, _mapper);
			User = new UserRepo(this, _db, settings, _mapper);
			Customer = new Customers(this, _db, settings, _mapper);
			System = new SystemRepo(this, _db, settings, _mapper);
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

		//Dispose
		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed)
				return;

			if (disposing)
			{
				Context.Dispose();

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

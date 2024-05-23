using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Loans;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class LoanRepo : ILoanRepo
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public LoanRepo(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<LoanCustom> Create(LoanCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				string? master_Pre_Interest_PayTypeName = null;

				if (model.Master_Pre_Interest_PayTypeId.HasValue)
				{
					master_Pre_Interest_PayTypeName = await _repo.Master_Pre_PayType.GetNameById(model.Master_Pre_Interest_PayTypeId.Value);
				}

				DateTime _dateNow = DateTime.Now;

				var loan = new Data.Entity.Loan();
				loan.Status = StatusModel.Active;
				loan.CreateDate = _dateNow;
				loan.CreateBy = model.CurrentUserId;
				loan.UpdateDate = _dateNow;
				loan.UpdateBy = model.CurrentUserId;
				loan.Name = model.Name;
				loan.Master_Pre_Interest_PayTypeId = model.Master_Pre_Interest_PayTypeId;
				loan.Master_Pre_Interest_PayTypeName = master_Pre_Interest_PayTypeName;
				loan.PeriodNumber = model.PeriodNumber;
				loan.RiskPremiumYear = model.RiskPremiumYear;
				await _db.InsterAsync(loan);
				await _db.SaveAsync();

				if (model.Loan_Periods?.Count > 0)
				{
					foreach (var period in model.Loan_Periods)
					{
						string? master_Pre_Interest_RateTypeName = null;

						if (period.Master_Pre_Interest_RateTypeId.HasValue)
						{
							master_Pre_Interest_RateTypeName = await _repo.Master_Pre_RateType.GetNameById(period.Master_Pre_Interest_RateTypeId.Value);
						}

						var loan_Period = new Data.Entity.Loan_Period();
						loan_Period.Status = StatusModel.Active;
						loan_Period.CreateDate = _dateNow;
						loan_Period.LoanId = loan.Id;
						loan_Period.PeriodNo = period.PeriodNo;
						loan_Period.Master_Pre_Interest_RateTypeId = period.Master_Pre_Interest_RateTypeId;
						loan_Period.Master_Pre_Interest_RateTypeName = master_Pre_Interest_RateTypeName;
						loan_Period.SpecialType = period.SpecialType;
						loan_Period.SpecialRate = period.SpecialRate;
						loan_Period.RateValue = period.RateValue;
						loan_Period.StartYear = period.StartYear;
						loan_Period.Condition = period.Condition;
						await _db.InsterAsync(loan_Period);
						await _db.SaveAsync();

						if (period.Loan_Period_AppLoans?.Count > 0)
						{
							foreach (var appLoan in period.Loan_Period_AppLoans)
							{
								string? master_Pre_Applicant_LoanName = null;

								if (appLoan.Master_Pre_Applicant_LoanId != Guid.Empty)
								{
									master_Pre_Applicant_LoanName = await _repo.Master_Pre_App_Loan.GetNameById(appLoan.Master_Pre_Applicant_LoanId);
								}

								var loan_Period_AppLoan = new Data.Entity.Loan_Period_AppLoan();
								loan_Period_AppLoan.Status = StatusModel.Active;
								loan_Period_AppLoan.CreateDate = _dateNow;
								loan_Period_AppLoan.Loan_PeriodId = loan_Period.Id;
								loan_Period_AppLoan.Master_Pre_Applicant_LoanId = appLoan.Master_Pre_Applicant_LoanId;
								loan_Period_AppLoan.Master_Pre_Applicant_LoanName = master_Pre_Applicant_LoanName;
								await _db.InsterAsync(loan_Period_AppLoan);
								await _db.SaveAsync();
							}
						}

						if (period.Loan_Period_BusTypes?.Count > 0)
						{
							foreach (var busType in period.Loan_Period_BusTypes)
							{
								string? master_Pre_BusinessTypeName = null;

								if (busType.Master_Pre_BusinessTypeId != Guid.Empty)
								{
									master_Pre_BusinessTypeName = await _repo.Master_Pre_BusType.GetNameById(busType.Master_Pre_BusinessTypeId);
								}

								var loan_Period_BusType = new Data.Entity.Loan_Period_BusType();
								loan_Period_BusType.Status = StatusModel.Active;
								loan_Period_BusType.CreateDate = _dateNow;
								loan_Period_BusType.Loan_PeriodId = loan_Period.Id;
								loan_Period_BusType.Master_Pre_BusinessTypeId = busType.Master_Pre_BusinessTypeId;
								loan_Period_BusType.Master_Pre_BusinessTypeName = master_Pre_BusinessTypeName;
								await _db.InsterAsync(loan_Period_BusType);
								await _db.SaveAsync();
							}
						}
					}
				}

				_transaction.Commit();

				return _mapper.Map<LoanCustom>(loan);
			}
		}

		public async Task<LoanCustom> Update(LoanCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				string? master_Pre_Interest_PayTypeName = null;

				if (model.Master_Pre_Interest_PayTypeId.HasValue)
				{
					master_Pre_Interest_PayTypeName = await _repo.Master_Pre_PayType.GetNameById(model.Master_Pre_Interest_PayTypeId.Value);
				}

				DateTime _dateNow = DateTime.Now;
				var loan = await _repo.Context.Loans.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (loan != null)
				{
					loan.UpdateDate = _dateNow;
					loan.UpdateBy = model.CurrentUserId;
					loan.Name = model.Name;
					loan.Master_Pre_Interest_PayTypeId = model.Master_Pre_Interest_PayTypeId;
					loan.Master_Pre_Interest_PayTypeName = master_Pre_Interest_PayTypeName;
					loan.PeriodNumber = model.PeriodNumber;
					loan.RiskPremiumYear = model.RiskPremiumYear;
					_db.Update(loan);
					await _db.SaveAsync();

					//Update Status To Delete All
					var loan_PeriodR = _repo.Context.Loan_Periods.Where(x => x.LoanId == model.Id).ToList();
					if (loan_PeriodR.Count > 0)
					{
						foreach (var period in loan_PeriodR)
						{
							var loan_Period_AppLoansR = _repo.Context.Loan_Period_AppLoans.Where(x => x.Loan_PeriodId == period.Id).ToList();
							if (loan_Period_AppLoansR.Count > 0)
							{
								foreach (var appLoan in loan_Period_AppLoansR)
								{
									appLoan.Status = StatusModel.Delete;
								}
								await _db.SaveAsync();
							}
							var loan_Period_BusTypesR = _repo.Context.Loan_Period_BusTypes.Where(x => x.Loan_PeriodId == period.Id).ToList();
							if (loan_Period_BusTypesR.Count > 0)
							{
								foreach (var sharehold_item in loan_Period_BusTypesR)
								{
									sharehold_item.Status = StatusModel.Delete;
								}
								await _db.SaveAsync();
							}
							period.Status = StatusModel.Delete;
						}
						await _db.SaveAsync();
					}

					if (model.Loan_Periods?.Count > 0)
					{
						foreach (var period in model.Loan_Periods)
						{
							string? master_Pre_Interest_RateTypeName = null;

							if (period.Master_Pre_Interest_RateTypeId.HasValue)
							{
								master_Pre_Interest_RateTypeName = await _repo.Master_Pre_RateType.GetNameById(period.Master_Pre_Interest_RateTypeId.Value);
							}

							var loan_Period = new Data.Entity.Loan_Period();
							loan_Period.Status = StatusModel.Active;
							loan_Period.CreateDate = _dateNow;
							loan_Period.LoanId = loan.Id;
							loan_Period.PeriodNo = period.PeriodNo;
							loan_Period.Master_Pre_Interest_RateTypeId = period.Master_Pre_Interest_RateTypeId;
							loan_Period.Master_Pre_Interest_RateTypeName = master_Pre_Interest_RateTypeName;
							loan_Period.SpecialType = period.SpecialType;
							loan_Period.SpecialRate = period.SpecialRate;
							loan_Period.RateValue = period.RateValue;
							loan_Period.StartYear = period.StartYear;
							loan_Period.Condition = period.Condition;
							await _db.InsterAsync(loan_Period);
							await _db.SaveAsync();

							if (period.Loan_Period_AppLoans?.Count > 0)
							{
								foreach (var appLoan in period.Loan_Period_AppLoans)
								{
									string? master_Pre_Applicant_LoanName = null;

									if (appLoan.Master_Pre_Applicant_LoanId != Guid.Empty)
									{
										master_Pre_Applicant_LoanName = await _repo.Master_Pre_App_Loan.GetNameById(appLoan.Master_Pre_Applicant_LoanId);
									}

									var loan_Period_AppLoan = new Data.Entity.Loan_Period_AppLoan();
									loan_Period_AppLoan.Status = StatusModel.Active;
									loan_Period_AppLoan.CreateDate = _dateNow;
									loan_Period_AppLoan.Loan_PeriodId = loan_Period.Id;
									loan_Period_AppLoan.Master_Pre_Applicant_LoanId = appLoan.Master_Pre_Applicant_LoanId;
									loan_Period_AppLoan.Master_Pre_Applicant_LoanName = master_Pre_Applicant_LoanName;
									await _db.InsterAsync(loan_Period_AppLoan);
									await _db.SaveAsync();
								}
							}

							if (period.Loan_Period_BusTypes?.Count > 0)
							{
								foreach (var busType in period.Loan_Period_BusTypes)
								{
									string? master_Pre_BusinessTypeName = null;

									if (busType.Master_Pre_BusinessTypeId != Guid.Empty)
									{
										master_Pre_BusinessTypeName = await _repo.Master_Pre_BusType.GetNameById(busType.Master_Pre_BusinessTypeId);
									}

									var loan_Period_BusType = new Data.Entity.Loan_Period_BusType();
									loan_Period_BusType.Status = StatusModel.Active;
									loan_Period_BusType.CreateDate = _dateNow;
									loan_Period_BusType.Loan_PeriodId = loan_Period.Id;
									loan_Period_BusType.Master_Pre_BusinessTypeId = busType.Master_Pre_BusinessTypeId;
									loan_Period_BusType.Master_Pre_BusinessTypeName = master_Pre_BusinessTypeName;
									await _db.InsterAsync(loan_Period_BusType);
									await _db.SaveAsync();
								}
							}
						}
					}

				}

				_transaction.Commit();

				return _mapper.Map<LoanCustom>(loan);
			}
		}

		public async Task DeleteById(UpdateModel model)
		{
			Guid id = Guid.Parse(model.id);
			var query = await _repo.Context.Loans.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
			if (query != null)
			{
				query.UpdateDate = DateTime.Now;
				query.UpdateBy = model.userid;
				query.Status = StatusModel.Delete;
				_db.Update(query);
				await _db.SaveAsync();
			}
		}

		public async Task UpdateStatusById(UpdateModel model)
		{
			if (model != null && Boolean.TryParse(model.value, out bool parsedValue))
			{
				var _status = parsedValue ? (short)1 : (short)0;
				Guid id = Guid.Parse(model.id);
				var query = await _repo.Context.Loans.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
				if (query != null)
				{
					query.UpdateBy = model.userid;
					query.Status = _status;
					_db.Update(query);
					await _db.SaveAsync();
				}
			}
		}

		public async Task<LoanCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Loans
										 .Include(x => x.Loan_Periods).ThenInclude(t => t.Loan_Period_AppLoans)
										 .Include(x => x.Loan_Periods).ThenInclude(t => t.Loan_Period_BusTypes)
										 .OrderByDescending(o => o.CreateDate)
										 .FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<LoanCustom>(query);
		}

		public async Task<string?> GetNameById(Guid id)
		{
			var name = await _repo.Context.Loans.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
			return name;
		}

		public async Task<PaginationView<List<LoanCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Loans
									 .Where(x => x.Status != StatusModel.Delete)
									 .Include(x => x.Loan_Periods).ThenInclude(t => t.Loan_Period_AppLoans)
									 .Include(x => x.Loan_Periods).ThenInclude(t => t.Loan_Period_BusTypes)
									 .OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
									 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (!String.IsNullOrEmpty(model.val1))
			{
				query = query.Where(x => x.Name != null && x.Name.Contains(model.val1));
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<LoanCustom>>()
			{
				Items = _mapper.Map<List<LoanCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

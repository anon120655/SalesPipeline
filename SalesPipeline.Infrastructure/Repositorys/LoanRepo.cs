using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
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
				loan.Condition = model.Condition;
				await _db.InsterAsync(loan);
				await _db.SaveAsync();

				if (model.Loan_AppLoans?.Count > 0)
				{
					foreach (var appLoan in model.Loan_AppLoans)
					{
						string? master_Pre_Applicant_LoanName = null;

						if (appLoan.Master_Pre_Applicant_LoanId != Guid.Empty)
						{
							master_Pre_Applicant_LoanName = await _repo.Master_Pre_App_Loan.GetNameById(appLoan.Master_Pre_Applicant_LoanId);
						}

						var loan_AppLoan = new Data.Entity.Loan_AppLoan();
						loan_AppLoan.Status = StatusModel.Active;
						loan_AppLoan.CreateDate = _dateNow;
						loan_AppLoan.LoanId = loan.Id;
						loan_AppLoan.Master_Pre_Applicant_LoanId = appLoan.Master_Pre_Applicant_LoanId;
						loan_AppLoan.Master_Pre_Applicant_LoanName = master_Pre_Applicant_LoanName;
						await _db.InsterAsync(loan_AppLoan);
						await _db.SaveAsync();
					}
				}

				if (model.Loan_BusTypes?.Count > 0)
				{
					foreach (var busType in model.Loan_BusTypes)
					{
						string? master_Pre_BusinessTypeName = null;

						if (busType.Master_Pre_BusinessTypeId != Guid.Empty)
						{
							master_Pre_BusinessTypeName = await _repo.Master_Pre_BusType.GetNameById(busType.Master_Pre_BusinessTypeId);
						}

						var loan_BusType = new Data.Entity.Loan_BusType();
						loan_BusType.Status = StatusModel.Active;
						loan_BusType.CreateDate = _dateNow;
						loan_BusType.LoanId = loan.Id;
						loan_BusType.Master_Pre_BusinessTypeId = busType.Master_Pre_BusinessTypeId;
						loan_BusType.Master_Pre_BusinessTypeName = master_Pre_BusinessTypeName;
						await _db.InsterAsync(loan_BusType);
						await _db.SaveAsync();
					}
				}

				if (model.Loan_Periods?.Count > 0)
				{
					foreach (var period in model.Loan_Periods)
					{
						string? master_Pre_Interest_RateTypeName = null;
						string? master_Pre_Interest_RateTypeCode = null;

						if (!period.Master_Pre_Interest_RateTypeId.HasValue) throw new ExceptionCustom("ระบุประเภทอัตราดอกเบี้ย");

						var pre_App_Loan = await _repo.Master_Pre_RateType.GetById(period.Master_Pre_Interest_RateTypeId.Value);
						if (pre_App_Loan != null)
						{
							master_Pre_Interest_RateTypeName = pre_App_Loan.Name;
							master_Pre_Interest_RateTypeCode = pre_App_Loan.Code;
						}

						if (period.SpecialType.HasValue && !period.SpecialRate.HasValue) throw new ExceptionCustom("ระบุอัตราดอกเบี้ย");
						//11e23023-18cd-11ef-93aa-30e37aef72fb=Special - ระบุ
						if (period.Master_Pre_Interest_RateTypeId != Guid.Parse("11e23023-18cd-11ef-93aa-30e37aef72fb"))
						{
							if (!period.StartYear.HasValue) throw new ExceptionCustom("ระบุเริ่มปีที่");
						}

						var loan_Period = new Data.Entity.Loan_Period();
						loan_Period.Status = StatusModel.Active;
						loan_Period.CreateDate = _dateNow;
						loan_Period.LoanId = loan.Id;
						loan_Period.PeriodNo = period.PeriodNo;
						loan_Period.Master_Pre_Interest_RateTypeId = period.Master_Pre_Interest_RateTypeId;
						loan_Period.Master_Pre_Interest_RateTypeName = master_Pre_Interest_RateTypeName;
						loan_Period.Master_Pre_Interest_RateTypeCode = master_Pre_Interest_RateTypeCode;
						loan_Period.RateValueOriginal = period.RateValueOriginal;
						loan_Period.SpecialType = period.SpecialType;
						loan_Period.SpecialRate = period.SpecialRate;
						loan_Period.RateValue = period.RateValue;
						loan_Period.StartYear = period.StartYear;
						await _db.InsterAsync(loan_Period);
						await _db.SaveAsync();
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
					loan.Condition = model.Condition;
					_db.Update(loan);
					await _db.SaveAsync();

					//Update Status To Delete All
					var loan_AppLoanR = _repo.Context.Loan_AppLoans.Where(x => x.LoanId == model.Id).ToList();
					if (loan_AppLoanR.Count > 0)
					{
						_db.DeleteRange(loan_AppLoanR);
						await _db.SaveAsync();
					}
					var loan_BusTypeR = _repo.Context.Loan_BusTypes.Where(x => x.LoanId == model.Id).ToList();
					if (loan_BusTypeR.Count > 0)
					{
						_db.DeleteRange(loan_BusTypeR);
						await _db.SaveAsync();
					}
					var loan_PeriodR = _repo.Context.Loan_Periods.Where(x => x.LoanId == model.Id).ToList();
					if (loan_PeriodR.Count > 0)
					{
						_db.DeleteRange(loan_PeriodR);
						await _db.SaveAsync();
					}

					if (model.Loan_AppLoans?.Count > 0)
					{
						foreach (var appLoan in model.Loan_AppLoans)
						{
							string? master_Pre_Applicant_LoanName = null;

							if (appLoan.Master_Pre_Applicant_LoanId != Guid.Empty)
							{
								master_Pre_Applicant_LoanName = await _repo.Master_Pre_App_Loan.GetNameById(appLoan.Master_Pre_Applicant_LoanId);
							}

							var loan_AppLoan = new Data.Entity.Loan_AppLoan();
							loan_AppLoan.Status = StatusModel.Active;
							loan_AppLoan.CreateDate = _dateNow;
							loan_AppLoan.LoanId = loan.Id;
							loan_AppLoan.Master_Pre_Applicant_LoanId = appLoan.Master_Pre_Applicant_LoanId;
							loan_AppLoan.Master_Pre_Applicant_LoanName = master_Pre_Applicant_LoanName;
							await _db.InsterAsync(loan_AppLoan);
							await _db.SaveAsync();
						}
					}

					if (model.Loan_BusTypes?.Count > 0)
					{
						foreach (var busType in model.Loan_BusTypes)
						{
							string? master_Pre_BusinessTypeName = null;

							if (busType.Master_Pre_BusinessTypeId != Guid.Empty)
							{
								master_Pre_BusinessTypeName = await _repo.Master_Pre_BusType.GetNameById(busType.Master_Pre_BusinessTypeId);
							}

							var loan_BusType = new Data.Entity.Loan_BusType();
							loan_BusType.Status = StatusModel.Active;
							loan_BusType.CreateDate = _dateNow;
							loan_BusType.LoanId = loan.Id;
							loan_BusType.Master_Pre_BusinessTypeId = busType.Master_Pre_BusinessTypeId;
							loan_BusType.Master_Pre_BusinessTypeName = master_Pre_BusinessTypeName;
							await _db.InsterAsync(loan_BusType);
							await _db.SaveAsync();
						}
					}

					if (model.Loan_Periods?.Count > 0)
					{
						foreach (var period in model.Loan_Periods)
						{
							string? master_Pre_Interest_RateTypeName = null;
							string? master_Pre_Interest_RateTypeCode = null;

							if (!period.Master_Pre_Interest_RateTypeId.HasValue) throw new ExceptionCustom("ระบุประเภทอัตราดอกเบี้ย");

							var pre_App_Loan = await _repo.Master_Pre_RateType.GetById(period.Master_Pre_Interest_RateTypeId.Value);
							if (pre_App_Loan != null)
							{
								master_Pre_Interest_RateTypeName = pre_App_Loan.Name;
								master_Pre_Interest_RateTypeCode = pre_App_Loan.Code;
							}

							if (period.SpecialType.HasValue && !period.SpecialRate.HasValue) throw new ExceptionCustom("ระบุอัตราดอกเบี้ย");
							//11e23023-18cd-11ef-93aa-30e37aef72fb=Special - ระบุ
							if (period.Master_Pre_Interest_RateTypeId != Guid.Parse("11e23023-18cd-11ef-93aa-30e37aef72fb"))
							{
								if (!period.StartYear.HasValue) throw new ExceptionCustom("ระบุเริ่มปีที่");
							}

							var loan_Period = new Data.Entity.Loan_Period();
							loan_Period.Status = StatusModel.Active;
							loan_Period.CreateDate = _dateNow;
							loan_Period.LoanId = loan.Id;
							loan_Period.PeriodNo = period.PeriodNo;
							loan_Period.Master_Pre_Interest_RateTypeId = period.Master_Pre_Interest_RateTypeId;
							loan_Period.Master_Pre_Interest_RateTypeName = master_Pre_Interest_RateTypeName;
							loan_Period.Master_Pre_Interest_RateTypeCode = master_Pre_Interest_RateTypeCode;
							loan_Period.RateValueOriginal = period.RateValueOriginal;
							loan_Period.SpecialType = period.SpecialType;
							loan_Period.SpecialRate = period.SpecialRate;
							loan_Period.RateValue = period.RateValue;
							loan_Period.StartYear = period.StartYear;
							await _db.InsterAsync(loan_Period);
							await _db.SaveAsync();
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
										 .Include(x => x.Loan_AppLoans)
										 .Include(x => x.Loan_BusTypes)
										 .Include(x => x.Loan_Periods.OrderBy(o => o.PeriodNo))
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
									 .Include(x => x.Loan_AppLoans)
									 .Include(x => x.Loan_BusTypes)
									 .Include(x => x.Loan_Periods)
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

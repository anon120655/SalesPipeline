using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class Master_Pre_App_Loan : IMaster_Pre_App_Loan
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public Master_Pre_App_Loan(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Master_Pre_Applicant_LoanCustom> Create(Master_Pre_Applicant_LoanCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var pre_Applicant_Loan = new Data.Entity.Master_Pre_Applicant_Loan()
				{
					Status = StatusModel.Active,
					CreateDate = _dateNow,
					CreateBy = model.CurrentUserId,
					UpdateDate = _dateNow,
					UpdateBy = model.CurrentUserId,
					Name = model.Name,
				};
				await _db.InsterAsync(pre_Applicant_Loan);
				await _db.SaveAsync();

				_transaction.Commit();

				return _mapper.Map<Master_Pre_Applicant_LoanCustom>(pre_Applicant_Loan);
			}
		}

		public async Task<Master_Pre_Applicant_LoanCustom> Update(Master_Pre_Applicant_LoanCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var master_Pre_Applicant_Loan = await _repo.Context.Master_Pre_Applicant_Loans.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (master_Pre_Applicant_Loan != null)
				{
					master_Pre_Applicant_Loan.UpdateDate = _dateNow;
					master_Pre_Applicant_Loan.UpdateBy = model.CurrentUserId;
					master_Pre_Applicant_Loan.Name = model.Name;
					_db.Update(master_Pre_Applicant_Loan);
					await _db.SaveAsync();

					_transaction.Commit();
				}

				return _mapper.Map<Master_Pre_Applicant_LoanCustom>(master_Pre_Applicant_Loan);
			}
		}

		public async Task DeleteById(UpdateModel model)
		{
			Guid id = Guid.Parse(model.id);
			var query = await _repo.Context.Master_Pre_Applicant_Loans.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
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
				var query = await _repo.Context.Master_Pre_Applicant_Loans.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
				if (query != null)
				{
					query.UpdateBy = model.userid;
					query.Status = _status;
					_db.Update(query);
					await _db.SaveAsync();
				}
			}
		}

		public async Task<Master_Pre_Applicant_LoanCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Master_Pre_Applicant_Loans
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Master_Pre_Applicant_LoanCustom>(query);
		}

		public async Task<string?> GetNameById(Guid id)
		{
			var name = await _repo.Context.Master_Pre_Applicant_Loans.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
			return name;
		}

		public async Task<PaginationView<List<Master_Pre_Applicant_LoanCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Master_Pre_Applicant_Loans
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (model.isMatchCal == 1)
			{
				var pre_Cals = _repo.Context.Pre_Cals
									 .Where(x => x.Status == StatusModel.Active)
									 .Select(a => a.Master_Pre_Applicant_LoanId)
									 .Distinct(); // ใช้ Distinct เพื่อลดการจับคู่ที่ไม่จำเป็น

				query = from q in query
						join c in pre_Cals on q.Id equals c
						select q;

				//query = query.Where(x => pre_Cals.Any(loanid => loanid == x.Id));
			}

			if (!String.IsNullOrEmpty(model.val1))
			{
				query = query.Where(x => x.Name != null && x.Name.Contains(model.val1));
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Master_Pre_Applicant_LoanCustom>>()
			{
				Items = _mapper.Map<List<Master_Pre_Applicant_LoanCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

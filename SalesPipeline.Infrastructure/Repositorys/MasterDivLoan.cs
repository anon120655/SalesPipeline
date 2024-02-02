using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class MasterDivLoan : IMasterDepLoan
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public MasterDivLoan(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		//ฝ่ายธุรกิจสินเชื่อ
		public async Task<Master_Department_LoanCustom> Create(Master_Department_LoanCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var masterDivisionLoan = new Data.Entity.Master_Department_Loan()
				{
					Status = StatusModel.Active,
					CreateDate = _dateNow,
					CreateBy = model.CurrentUserId,
					UpdateDate = _dateNow,
					UpdateBy = model.CurrentUserId,
					Code = model.Code,
					Name = model.Name
				};
				await _db.InsterAsync(masterDivisionLoan);
				await _db.SaveAsync();

				_transaction.Commit();

				return _mapper.Map<Master_Department_LoanCustom>(masterDivisionLoan);
			}
		}

		public async Task<Master_Department_LoanCustom> Update(Master_Department_LoanCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var masterDivisionLoan = await _repo.Context.Master_Department_Loans.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (masterDivisionLoan != null)
				{
					masterDivisionLoan.UpdateDate = _dateNow;
					masterDivisionLoan.UpdateBy = model.CurrentUserId;
					masterDivisionLoan.Code = model.Code;
					masterDivisionLoan.Name = model.Name;
					_db.Update(masterDivisionLoan);
					await _db.SaveAsync();

					_transaction.Commit();
				}

				return _mapper.Map<Master_Department_LoanCustom>(masterDivisionLoan);
			}
		}

		public async Task DeleteById(UpdateModel model)
		{
			Guid id = Guid.Parse(model.id);
			var query = await _repo.Context.Master_Department_Loans.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
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
				var query = await _repo.Context.Master_Department_Loans.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
				if (query != null)
				{
					query.UpdateBy = model.userid;
					query.Status = _status;
					_db.Update(query);
					await _db.SaveAsync();
				}
			}
		}

		public async Task<Master_Department_LoanCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Master_Department_Loans
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Master_Department_LoanCustom>(query);
		}

		public async Task<PaginationView<List<Master_Department_LoanCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Master_Department_Loans
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (!String.IsNullOrEmpty(model.val1))
			{
				query = query.Where(x => x.Code != null && x.Code.Contains(model.val1));
			}

			if (!String.IsNullOrEmpty(model.val2))
			{
				query = query.Where(x => x.Name != null && x.Name.Contains(model.val2));
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Master_Department_LoanCustom>>()
			{
				Items = _mapper.Map<List<Master_Department_LoanCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

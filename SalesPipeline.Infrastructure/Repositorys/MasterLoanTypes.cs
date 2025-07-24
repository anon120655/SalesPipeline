using AutoMapper;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using Microsoft.EntityFrameworkCore;
using SalesPipeline.Utils.ConstTypeModel;

namespace SalesPipeline.Infrastructure.Repositorys
{
    public class MasterLoanTypes : IMasterLoanTypes
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public MasterLoanTypes(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Master_LoanTypeCustom> Create(Master_LoanTypeCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var masterLoanType = new Data.Entity.Master_LoanType()
				{
					Status = StatusModel.Active,
					CreateDate = _dateNow,
					CreateBy = model.CurrentUserId,
					UpdateDate = _dateNow,
					UpdateBy = model.CurrentUserId,
					Name = model.Name,
				};
				await _db.InsterAsync(masterLoanType);
				await _db.SaveAsync();

				_transaction.Commit();

				return _mapper.Map<Master_LoanTypeCustom>(masterLoanType);
			}
		}

		public async Task<Master_LoanTypeCustom> Update(Master_LoanTypeCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var masterDivisionLoan = await _repo.Context.Master_LoanTypes.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (masterDivisionLoan != null)
				{
					masterDivisionLoan.UpdateDate = _dateNow;
					masterDivisionLoan.UpdateBy = model.CurrentUserId;
					masterDivisionLoan.Name = model.Name;
					_db.Update(masterDivisionLoan);
					await _db.SaveAsync();

					_transaction.Commit();
				}

				return _mapper.Map<Master_LoanTypeCustom>(masterDivisionLoan);
			}
		}

		public async Task DeleteById(UpdateModel model)
		{
			Guid id = Guid.Parse(model.id);
			var query = await _repo.Context.Master_LoanTypes.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
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
				var query = await _repo.Context.Master_LoanTypes.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
				if (query != null)
				{
					query.UpdateBy = model.userid;
					query.Status = _status;
					_db.Update(query);
					await _db.SaveAsync();
				}
			}
		}

		public async Task<Master_LoanTypeCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Master_LoanTypes.AsNoTracking()
                .OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Master_LoanTypeCustom>(query);
		}

		public async Task<PaginationView<List<Master_LoanTypeCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Master_LoanTypes.AsNoTracking()
                                                 .Where(x => x.Status != StatusModel.Delete)
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

			return new PaginationView<List<Master_LoanTypeCustom>>()
			{
				Items = _mapper.Map<List<Master_LoanTypeCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task<string?> GetNameById(Guid id)
		{
			var name = await _repo.Context.Master_LoanTypes.AsNoTracking().Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
			return name;
		}
	}
}

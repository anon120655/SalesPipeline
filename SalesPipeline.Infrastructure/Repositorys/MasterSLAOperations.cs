using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class MasterSLAOperations : IMasterSLAOperations
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public MasterSLAOperations(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		//ฝ่ายกิจการสาขา
		public async Task<Master_SLAOperationCustom> Create(Master_SLAOperationCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var masterSlaoperation = new Data.Entity.Master_SLAOperation()
				{
					Status = StatusModel.Active,
					CreateDate = _dateNow,
					CreateBy = model.CurrentUserId,
					UpdateDate = _dateNow,
					UpdateBy = model.CurrentUserId,
					Name = model.Name,
					Day = model.Day,
				};
				await _db.InsterAsync(masterSlaoperation);
				await _db.SaveAsync();

				_transaction.Commit();

				return _mapper.Map<Master_SLAOperationCustom>(masterSlaoperation);
			}
		}

		public async Task<Master_SLAOperationCustom> Update(Master_SLAOperationCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var masterSlaoperations = await _repo.Context.Master_SLAOperations.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (masterSlaoperations != null)
				{
					masterSlaoperations.UpdateDate = _dateNow;
					masterSlaoperations.UpdateBy = model.CurrentUserId;
					masterSlaoperations.Name = model.Name;
					masterSlaoperations.Day = model.Day;
					_db.Update(masterSlaoperations);
					await _db.SaveAsync();

					_transaction.Commit();
				}

				return _mapper.Map<Master_SLAOperationCustom>(masterSlaoperations);
			}
		}

		public async Task DeleteById(UpdateModel model)
		{
			Guid id = Guid.Parse(model.id);
			var query = await _repo.Context.Master_SLAOperations.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
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
				var query = await _repo.Context.Master_SLAOperations.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
				if (query != null)
				{
					query.UpdateBy = model.userid;
					query.Status = _status;
					_db.Update(query);
					await _db.SaveAsync();
				}
			}
		}

		public async Task<Master_SLAOperationCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Master_SLAOperations
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Master_SLAOperationCustom>(query);
		}

		public async Task<PaginationView<List<Master_SLAOperationCustom>>> GetSLAOperations(allFilter model)
		{
			var query = _repo.Context.Master_SLAOperations
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

			return new PaginationView<List<Master_SLAOperationCustom>>()
			{
				Items = _mapper.Map<List<Master_SLAOperationCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

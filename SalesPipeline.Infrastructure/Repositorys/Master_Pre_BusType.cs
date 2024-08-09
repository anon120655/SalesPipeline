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
	public class Master_Pre_BusType : IMaster_Pre_BusType
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public Master_Pre_BusType(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Master_Pre_BusinessTypeCustom> Create(Master_Pre_BusinessTypeCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var pre_BusinessType = new Data.Entity.Master_Pre_BusinessType()
				{
					Status = StatusModel.Active,
					CreateDate = _dateNow,
					CreateBy = model.CurrentUserId,
					UpdateDate = _dateNow,
					UpdateBy = model.CurrentUserId,
					Name = model.Name,
				};
				await _db.InsterAsync(pre_BusinessType);
				await _db.SaveAsync();

				_transaction.Commit();

				return _mapper.Map<Master_Pre_BusinessTypeCustom>(pre_BusinessType);
			}
		}

		public async Task<Master_Pre_BusinessTypeCustom> Update(Master_Pre_BusinessTypeCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var pre_BusinessType = await _repo.Context.Master_Pre_BusinessTypes.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (pre_BusinessType != null)
				{
					pre_BusinessType.UpdateDate = _dateNow;
					pre_BusinessType.UpdateBy = model.CurrentUserId;
					pre_BusinessType.Name = model.Name;
					_db.Update(pre_BusinessType);
					await _db.SaveAsync();

					_transaction.Commit();
				}

				return _mapper.Map<Master_Pre_BusinessTypeCustom>(pre_BusinessType);
			}
		}

		public async Task DeleteById(UpdateModel model)
		{
			Guid id = Guid.Parse(model.id);
			var query = await _repo.Context.Master_Pre_BusinessTypes.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
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
				var query = await _repo.Context.Master_Pre_BusinessTypes.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
				if (query != null)
				{
					query.UpdateBy = model.userid;
					query.Status = _status;
					_db.Update(query);
					await _db.SaveAsync();
				}
			}
		}

		public async Task<Master_Pre_BusinessTypeCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Master_Pre_BusinessTypes
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Master_Pre_BusinessTypeCustom>(query);
		}

		public async Task<string?> GetNameById(Guid id)
		{
			var name = await _repo.Context.Master_Pre_BusinessTypes.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
			return name;
		}

		public async Task<PaginationView<List<Master_Pre_BusinessTypeCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Master_Pre_BusinessTypes
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (Guid.TryParse(model.preapploanid, out Guid _preapploanid))
			{
				if (_preapploanid != Guid.Empty)
				{
					var pre_Cals = _repo.Context.Pre_Cals
										 .Where(x => x.Status == StatusModel.Active && x.Master_Pre_Applicant_LoanId == _preapploanid)
										 .Select(a => a.Master_Pre_BusinessTypeId)
										 .Distinct(); // ใช้ Distinct เพื่อลดการจับคู่ที่ไม่จำเป็น

					query = query.Where(x => pre_Cals.Any(busid => busid == x.Id));
				}
			}

			if (!String.IsNullOrEmpty(model.val1))
			{
				query = query.Where(x => x.Name != null && x.Name.Contains(model.val1));
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Master_Pre_BusinessTypeCustom>>()
			{
				Items = _mapper.Map<List<Master_Pre_BusinessTypeCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

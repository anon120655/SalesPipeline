using AutoMapper;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using Microsoft.EntityFrameworkCore;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Authorizes.Users;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class MasterYields : IMasterYields
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public MasterYields(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Master_YieldCustom> Validate(Master_YieldCustom model, bool isThrow = true, bool? isSetMaster = false)
		{
			string errorMessage = string.Empty;
			model.IsValidate = true;
			if (model.ValidateError == null) model.ValidateError = new();

			if (model.Id == Guid.Empty)
			{
				if (model.Name != null && _repo.Context.Master_Yields.Any(x => x.Status != StatusModel.Delete && x.Name == model.Name))
				{
					errorMessage = $"มีผลผลิต {model.Name} แล้ว";
					model.IsValidate = false;
					model.ValidateError.Add(errorMessage);
					if (isThrow) throw new ExceptionCustom(errorMessage);
				}
			}

			await Task.CompletedTask;

			return model;
		}

		public async Task<List<Master_YieldCustom>> ValidateUpload(List<Master_YieldCustom> model)
		{
			for (int i = 0; i < model.Count; i++)
			{
				model[i] = await Validate(model[i], false, true);
			}
			return model;
		}

		public async Task<Master_YieldCustom> Create(Master_YieldCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var masterYield = new Data.Entity.Master_Yield()
				{
					Status = StatusModel.Active,
					CreateDate = _dateNow,
					CreateBy = model.CurrentUserId,
					UpdateDate = _dateNow,
					UpdateBy = model.CurrentUserId,
					Name = model.Name,
				};
				await _db.InsterAsync(masterYield);
				await _db.SaveAsync();

				_transaction.Commit();

				return _mapper.Map<Master_YieldCustom>(masterYield);
			}
		}

		public async Task<Master_YieldCustom> Update(Master_YieldCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var masterYield = await _repo.Context.Master_Yields.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (masterYield != null)
				{
					masterYield.UpdateDate = _dateNow;
					masterYield.UpdateBy = model.CurrentUserId;
					masterYield.Name = model.Name;
					_db.Update(masterYield);
					await _db.SaveAsync();

					_transaction.Commit();
				}

				return _mapper.Map<Master_YieldCustom>(masterYield);
			}
		}

		public async Task DeleteById(UpdateModel model)
		{
			Guid id = Guid.Parse(model.id);
			var query = await _repo.Context.Master_Yields.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
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
				var query = await _repo.Context.Master_Yields.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
				if (query != null)
				{
					query.UpdateBy = model.userid;
					query.Status = _status;
					_db.Update(query);
					await _db.SaveAsync();
				}
			}
		}

		public async Task<Master_YieldCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Master_Yields.AsNoTracking()
                .OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Master_YieldCustom>(query);
		}

		public async Task<string?> GetNameById(Guid id)
		{
			var name = await _repo.Context.Master_Yields.AsNoTracking().Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
			return name;
		}

		public async Task<PaginationView<List<Master_YieldCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Master_Yields.AsNoTracking()
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

			return new PaginationView<List<Master_YieldCustom>>()
			{
				Items = _mapper.Map<List<Master_YieldCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

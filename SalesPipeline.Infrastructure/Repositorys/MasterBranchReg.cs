using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class MasterBranchReg : IMasterBranchReg
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public MasterBranchReg(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		//กิจการสาขาภาค
		public async Task<Master_Branch_RegionCustom> Create(Master_Branch_RegionCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var master_Branch_Region = new Data.Entity.Master_Branch_Region()
				{
					Status = StatusModel.Active,
					CreateDate = _dateNow,
					CreateBy = model.CurrentUserId,
					UpdateDate = _dateNow,
					UpdateBy = model.CurrentUserId,
					Code = model.Code,
					Name = model.Name,
				};
				await _db.InsterAsync(master_Branch_Region);
				await _db.SaveAsync();

				_transaction.Commit();

				return _mapper.Map<Master_Branch_RegionCustom>(master_Branch_Region);
			}
		}

		public async Task<Master_Branch_RegionCustom> Update(Master_Branch_RegionCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var master_Branch_Region = await _repo.Context.Master_Branch_Regions.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (master_Branch_Region != null)
				{
					master_Branch_Region.UpdateDate = _dateNow;
					master_Branch_Region.UpdateBy = model.CurrentUserId;
					master_Branch_Region.Code = model.Code;
					master_Branch_Region.Name = model.Name;
					_db.Update(master_Branch_Region);
					await _db.SaveAsync();

					_transaction.Commit();
				}

				return _mapper.Map<Master_Branch_RegionCustom>(master_Branch_Region);
			}
		}

		public async Task DeleteById(UpdateModel model)
		{
			Guid id = Guid.Parse(model.id);
			var query = await _repo.Context.Master_Branch_Regions.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
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
				var query = await _repo.Context.Master_Branch_Regions.Where(x =>  x.Id == id).FirstOrDefaultAsync();
				if (query != null)
				{
					query.UpdateBy = model.userid;
					query.Status = _status;
					_db.Update(query);
					await _db.SaveAsync();
				}
			}
		}

		public async Task<Master_Branch_RegionCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Master_Branch_Regions.AsNoTracking()
                .OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.Id == id);

			return _mapper.Map<Master_Branch_RegionCustom>(query);
		}

		public async Task<string?> GetNameById(Guid id)
		{
			var name = await _repo.Context.Master_Branch_Regions.AsNoTracking().Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
			return name;
		}

		public async Task<PaginationView<List<Master_Branch_RegionCustom>>> GetBranchRegs(allFilter model)
		{
			var query = _repo.Context.Master_Branch_Regions.AsNoTracking()
                                                 .Where(x => x.Status != StatusModel.Delete)
												 .OrderBy(x => x.CreateDate)
												 .AsQueryable();

			if (model.status.HasValue && model.isAll != 1)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (!String.IsNullOrEmpty(model.code))
			{
				query = query.Where(x => x.Code != null && x.Code.Contains(model.code));
			}

			if (!String.IsNullOrEmpty(model.branch_name))
			{
				query = query.Where(x => x.Name != null && x.Name.Contains(model.branch_name));
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Master_Branch_RegionCustom>>()
			{
				Items = _mapper.Map<List<Master_Branch_RegionCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

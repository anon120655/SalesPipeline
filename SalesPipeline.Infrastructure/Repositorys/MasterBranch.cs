using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class MasterBranch : IMasterBranch
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public MasterBranch(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}


		public async Task Validate(InfoBranchCustom model)
		{
			var infoBranches = await _repo.Context.InfoBranches.Where(x => x.Status == StatusModel.Active
			&& x.BranchID != model.BranchID
			&& x.BranchCode == model.BranchCode).FirstOrDefaultAsync();
			if (infoBranches != null)
			{
				throw new ExceptionCustom("มีรหัสสาขานี้แล้ว");
			}
		}

		//สาขา
		public async Task<InfoBranchCustom> Create(InfoBranchCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				await Validate(model);

				DateTime _dateNow = DateTime.Now;

				int id = _repo.Context.InfoBranches.Max(u => u.BranchID) + 1;

				var infoBranches = new Data.Entity.InfoBranch()
				{
					BranchID = id,
					Status = StatusModel.Active,
					CreateDate = _dateNow,
					CreateBy = model.CurrentUserId,
					UpdateDate = _dateNow,
					UpdateBy = model.CurrentUserId,
					BranchCode = model.BranchCode,
					BranchName = model.BranchName,
					ProvinceID = model.ProvinceID
				};
				await _db.InsterAsync(infoBranches);
				await _db.SaveAsync();

				_transaction.Commit();

				return _mapper.Map<InfoBranchCustom>(infoBranches);
			}
		}

		public async Task<InfoBranchCustom> Update(InfoBranchCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				await Validate(model);

				var _dateNow = DateTime.Now;

				var infoBranches = await _repo.Context.InfoBranches.Where(x => x.BranchID == model.BranchID).FirstOrDefaultAsync();
				if (infoBranches != null)
				{
					infoBranches.UpdateDate = _dateNow;
					infoBranches.UpdateBy = model.CurrentUserId;
					infoBranches.BranchCode = model.BranchCode;
					infoBranches.BranchName = model.BranchName;
					infoBranches.ProvinceID = model.ProvinceID;
					_db.Update(infoBranches);
					await _db.SaveAsync();

					_transaction.Commit();
				}

				return _mapper.Map<InfoBranchCustom>(infoBranches);
			}
		}

		public async Task DeleteById(UpdateModel model)
		{
			int id = int.Parse(model.id);
			var query = await _repo.Context.InfoBranches.Where(x => x.Status != StatusModel.Delete && x.BranchID == id).FirstOrDefaultAsync();
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
				int id = int.Parse(model.id);
				var query = await _repo.Context.InfoBranches.Where(x => x.Status != StatusModel.Delete && x.BranchID == id).FirstOrDefaultAsync();
				if (query != null)
				{
					query.UpdateDate = DateTime.Now;
					query.UpdateBy = model.userid;
					query.Status = _status;
					_db.Update(query);
					await _db.SaveAsync();
				}
			}
		}

		public async Task<InfoBranchCustom> GetById(int id)
		{
			var query = await _repo.Context.InfoBranches
				.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.BranchID == id);

			return _mapper.Map<InfoBranchCustom>(query);
		}

		public async Task<string?> GetNameById(int id)
		{
			var name = await _repo.Context.InfoBranches.AsNoTracking().Where(x => x.BranchID == id).Select(x => x.BranchName).FirstOrDefaultAsync();
			return name;
		}

		public async Task<PaginationView<List<InfoBranchCustom>>> GetBranchs(allFilter model)
		{
			var query = _repo.Context.InfoBranches
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderByDescending(x => x.UpdateDate)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (!String.IsNullOrEmpty(model.code))
			{
				query = query.Where(x => x.BranchCode != null && x.BranchCode.Contains(model.code));
			}

			if (!String.IsNullOrEmpty(model.branch_name))
			{
				query = query.Where(x => x.BranchName != null && x.BranchName.Contains(model.branch_name));
			}

			if (model.provinceid > 0)
			{
				query = query.Where(x => x.ProvinceID == model.provinceid);
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<InfoBranchCustom>>()
			{
				Items = _mapper.Map<List<InfoBranchCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

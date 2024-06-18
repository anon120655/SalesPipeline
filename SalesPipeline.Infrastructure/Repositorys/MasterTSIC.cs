using AutoMapper;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesPipeline.Utils.ConstTypeModel;

namespace SalesPipeline.Infrastructure.Repositorys
{
    public class MasterTSIC : IMasterTSIC
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public MasterTSIC(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Master_TSICCustom> Create(Master_TSICCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var master_TSIC = new Data.Entity.Master_TSIC()
				{
					Status = StatusModel.Active,
					CreateDate = _dateNow,
					CreateBy = model.CurrentUserId,
					UpdateDate = _dateNow,
					UpdateBy = model.CurrentUserId,
					Code = model.Code,
					Name = model.Name
				};
				await _db.InsterAsync(master_TSIC);
				await _db.SaveAsync();

				_transaction.Commit();

				return _mapper.Map<Master_TSICCustom>(master_TSIC);
			}
		}

		public async Task<Master_TSICCustom> Update(Master_TSICCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var master_TSIC = await _repo.Context.Master_TSICs.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (master_TSIC != null)
				{
					master_TSIC.UpdateDate = _dateNow;
					master_TSIC.UpdateBy = model.CurrentUserId;
					master_TSIC.Code = model.Code;
					master_TSIC.Name = model.Name;
					_db.Update(master_TSIC);
					await _db.SaveAsync();

					_transaction.Commit();
				}

				return _mapper.Map<Master_TSICCustom>(master_TSIC);
			}
		}

		public Task DeleteById(UpdateModel model)
		{
			throw new NotImplementedException();
		}

		public Task UpdateStatusById(UpdateModel model)
		{
			throw new NotImplementedException();
		}

		public async Task<Master_TSICCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Master_TSICs
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Master_TSICCustom>(query);
		}

		public async Task<Guid?> GetIDByCode(string code)
		{
			var query = await _repo.Context.Master_TSICs
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Code == code);

			if (query != null)
			{
				return query.Id;
			}

			return null;
		}

		public async Task<string?> GetNameById(Guid id)
		{
			var name = await _repo.Context.Master_TSICs.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
			return name;
		}

		public async Task<Guid?> GetIdByName(string name)
		{
			var query = await _repo.Context.Master_TSICs.Where(x => x.Name == name).FirstOrDefaultAsync();
			if (query != null)
			{
				return query.Id;
			}
			return null;
		}

		public async Task<PaginationView<List<Master_TSICCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Master_TSICs
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

			return new PaginationView<List<Master_TSICCustom>>()
			{
				Items = _mapper.Map<List<Master_TSICCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

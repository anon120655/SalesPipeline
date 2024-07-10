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
	public class MasterISICCode : IMasterISICCode
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public MasterISICCode(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Master_ISICCodeCustom> Create(Master_ISICCodeCustom model)
		{
			DateTime _dateNow = DateTime.Now;

			var master_ISICCode = new Data.Entity.Master_ISICCode()
			{
				Status = StatusModel.Active,
				CreateDate = _dateNow,
				CreateBy = model.CurrentUserId,
				UpdateDate = _dateNow,
				UpdateBy = model.CurrentUserId,
				Code = model.Code,
				Name = model.Name,
				GroupMaster_BusinessTypeId = model.GroupMaster_BusinessTypeId
			};
			await _db.InsterAsync(master_ISICCode);
			await _db.SaveAsync();

			return _mapper.Map<Master_ISICCodeCustom>(master_ISICCode);
		}

		public async Task<Master_ISICCodeCustom> Update(Master_ISICCodeCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var master_ISICCode = await _repo.Context.Master_ISICCodes.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (master_ISICCode != null)
				{
					master_ISICCode.UpdateDate = _dateNow;
					master_ISICCode.UpdateBy = model.CurrentUserId;
					master_ISICCode.Code = model.Code;
					master_ISICCode.Name = model.Name;
					master_ISICCode.GroupMaster_BusinessTypeId = model.GroupMaster_BusinessTypeId;
					_db.Update(master_ISICCode);
					await _db.SaveAsync();

					_transaction.Commit();
				}

				return _mapper.Map<Master_ISICCodeCustom>(master_ISICCode);
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

		public async Task<Master_ISICCodeCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Master_ISICCodes
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Master_ISICCodeCustom>(query);
		}

		public async Task<Guid?> GetIDByCode(string code)
		{
			var query = await _repo.Context.Master_ISICCodes
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
			var name = await _repo.Context.Master_ISICCodes.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
			return name;
		}

		public async Task<PaginationView<List<Master_ISICCodeCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Master_ISICCodes
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

			if (!string.IsNullOrEmpty(model.businesstype))
			{
				if (Guid.TryParse(model.businesstype, out Guid Groupid))
				{
					query = query.Where(x => x.GroupMaster_BusinessTypeId != null
					&& x.GroupMaster_BusinessTypeId.ToLower().Contains(Groupid.ToString().ToLower()));
				}
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Master_ISICCodeCustom>>()
			{
				Items = _mapper.Map<List<Master_ISICCodeCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

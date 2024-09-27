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
    public class Master_Pre_RateType : IMaster_Pre_RateType
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public Master_Pre_RateType(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Master_Pre_Interest_RateTypeCustom> Update(Master_Pre_Interest_RateTypeCustom model)
		{
			var _dateNow = DateTime.Now;

			var master_Pre_Interest_RateTypes = await _repo.Context.Master_Pre_Interest_RateTypes.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
			if (master_Pre_Interest_RateTypes != null)
			{
				master_Pre_Interest_RateTypes.UpdateDate = _dateNow;
				master_Pre_Interest_RateTypes.UpdateBy = model.CurrentUserId;
				//master_Pre_Interest_RateTypes.Code = model.Code;
				//master_Pre_Interest_RateTypes.Name = model.Name;
				master_Pre_Interest_RateTypes.Rate = model.Rate;
				_db.Update(master_Pre_Interest_RateTypes);
				await _db.SaveAsync();
			}
			return _mapper.Map<Master_Pre_Interest_RateTypeCustom>(master_Pre_Interest_RateTypes);
		}

		public async Task<Master_Pre_Interest_RateTypeCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Master_Pre_Interest_RateTypes
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Master_Pre_Interest_RateTypeCustom>(query);
		}

		public async Task<string?> GetNameById(Guid id)
		{
			var name = await _repo.Context.Master_Pre_Interest_RateTypes.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
			return name;
		}

		public async Task<PaginationView<List<Master_Pre_Interest_RateTypeCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Master_Pre_Interest_RateTypes
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderBy(x => x.CreateDate)
												 .AsQueryable();

			if (model.id != Guid.Empty)
			{
				query = query.Where(x => x.Id != model.id);
			}

			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (!String.IsNullOrEmpty(model.code))
			{
				query = query.Where(x => x.Code != null && x.Code.Contains(model.code));
			}

			if (!String.IsNullOrEmpty(model.searchtxt))
			{
				query = query.Where(x => x.Name != null && x.Name.Contains(model.searchtxt));
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Master_Pre_Interest_RateTypeCustom>>()
			{
				Items = _mapper.Map<List<Master_Pre_Interest_RateTypeCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}
	}
}

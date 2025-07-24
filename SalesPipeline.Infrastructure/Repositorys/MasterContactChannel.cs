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
    public class MasterContactChannel : IMasterContactChannel
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public MasterContactChannel(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public Task<Master_ContactChannelCustom> Create(Master_ContactChannelCustom model)
		{
			throw new NotImplementedException();
		}

		public Task<Master_ContactChannelCustom> Update(Master_ContactChannelCustom model)
		{
			throw new NotImplementedException();
		}

		public Task DeleteById(UpdateModel model)
		{
			throw new NotImplementedException();
		}

		public Task UpdateStatusById(UpdateModel model)
		{
			throw new NotImplementedException();
		}

		public async Task<Master_ContactChannelCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Master_ContactChannels
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Master_ContactChannelCustom>(query);
		}

		public async Task<string?> GetNameById(Guid id)
		{
			var name = await _repo.Context.Master_ContactChannels.AsNoTracking().Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
			return name;
		}

		public async Task<PaginationView<List<Master_ContactChannelCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Master_ContactChannels
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderBy(x => x.CreateDate)
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

			return new PaginationView<List<Master_ContactChannelCustom>>()
			{
				Items = _mapper.Map<List<Master_ContactChannelCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

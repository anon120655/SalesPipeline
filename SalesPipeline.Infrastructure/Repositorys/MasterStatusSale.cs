using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
    public class MasterStatusSale : IMasterStatusSale
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public MasterStatusSale(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public Task<Master_StatusSaleCustom> Create(Master_StatusSaleCustom model)
		{
			throw new NotImplementedException();
		}

		public Task<Master_StatusSaleCustom> Update(Master_StatusSaleCustom model)
		{
			throw new NotImplementedException();
		}

		public Task DeleteById(UpdateModel model)
		{
			throw new NotImplementedException();
		}

		public async Task<Master_StatusSaleCustom> GetById(int id)
		{
			var query = await _repo.Context.Master_StatusSales.Where(x => x.Id == id).FirstOrDefaultAsync();
			return _mapper.Map<Master_StatusSaleCustom>(query);
		}

		public async Task<string?> GetNameById(int id)
		{
			var name = await _repo.Context.Master_StatusSales.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
			return name;
		}

		public Task UpdateStatusById(UpdateModel model)
		{
			throw new NotImplementedException();
		}

		public async Task<PaginationView<List<Master_StatusSaleCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Master_StatusSales
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderBy(x => x.SequenceNo)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (!String.IsNullOrEmpty(model.val1))
			{
				query = query.Where(x => x.Name != null && x.Name.Contains(model.val1));
			}

			if (model.statussaleid.HasValue)
			{
				query = query.Where(x => x.Id == model.statussaleid.Value);
			}

			if (model.isshow.HasValue)
			{
				query = query.Where(x => x.IsShowFilter == model.isshow.Value);
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Master_StatusSaleCustom>>()
			{
				Items = _mapper.Map<List<Master_StatusSaleCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

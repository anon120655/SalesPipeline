using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
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
    public class MasterBusinessSize : IMasterBusinessSize
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public MasterBusinessSize(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public Task<Master_BusinessSizeCustom> Create(Master_BusinessSizeCustom model)
		{
			throw new NotImplementedException();
		}

		public Task<Master_BusinessSizeCustom> Update(Master_BusinessSizeCustom model)
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

		public async Task<Master_BusinessSizeCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Master_BusinessSizes
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Master_BusinessSizeCustom>(query);
		}

		public async Task<string?> GetNameById(Guid id)
		{
			var name = await _repo.Context.Master_BusinessSizes.AsNoTracking().Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
			return name;
		}

		public async Task<Guid?> GetIdByName(string name)
		{
			var query = await _repo.Context.Master_BusinessSizes.AsNoTracking().Where(x => x.Name == name).FirstOrDefaultAsync();
			if (query != null)
			{
				return query.Id;
			}
			return null;
		}

		public async Task<PaginationView<List<Master_BusinessSizeCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Master_BusinessSizes
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

			return new PaginationView<List<Master_BusinessSizeCustom>>()
			{
				Items = _mapper.Map<List<Master_BusinessSizeCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

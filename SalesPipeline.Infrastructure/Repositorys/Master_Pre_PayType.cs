using AutoMapper;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using Microsoft.EntityFrameworkCore;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.ConstTypeModel;

namespace SalesPipeline.Infrastructure.Repositorys
{
    public class Master_Pre_PayType : IMaster_Pre_PayType
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public Master_Pre_PayType(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<string?> GetNameById(Guid id)
		{
			var name = await _repo.Context.Master_Pre_Interest_PayTypes.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
			return name;
		}

		public async Task<PaginationView<List<Master_Pre_Interest_PayTypeCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Master_Pre_Interest_PayTypes
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderBy(x => x.CreateDate)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (!String.IsNullOrEmpty(model.searchtxt))
			{
				query = query.Where(x => x.Name != null && x.Name.Contains(model.searchtxt));
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Master_Pre_Interest_PayTypeCustom>>()
			{
				Items = _mapper.Map<List<Master_Pre_Interest_PayTypeCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

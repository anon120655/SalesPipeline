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
    public class MasterReasonCloseSale  : IMasterReasonCloseSale
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public MasterReasonCloseSale(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<string?> GetNameById(Guid id)
		{
			var name = await _repo.Context.Master_Reason_CloseSales.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
			return name;
		}

		public async Task<PaginationView<List<Master_Reason_CloseSaleCustom>>> GetReasonCloseSale(allFilter model)
		{
			var query = _repo.Context.Master_Reason_CloseSales
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderBy(x => x.CreateDate)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (!String.IsNullOrEmpty(model.val2))
			{
				query = query.Where(x => x.Name != null && x.Name.Contains(model.val2));
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Master_Reason_CloseSaleCustom>>()
			{
				Items = _mapper.Map<List<Master_Reason_CloseSaleCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}
	}
}

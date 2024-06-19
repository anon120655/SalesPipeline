using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
    public class PreChancePass : IPreChancePass
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public PreChancePass(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Pre_ChancePassCustom> Update(Pre_ChancePassCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var pre_ChancePasses = await _repo.Context.Pre_ChancePasses.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (pre_ChancePasses != null)
				{
					pre_ChancePasses.UpdateDate = _dateNow;
					pre_ChancePasses.UpdateBy = model.CurrentUserId;
					pre_ChancePasses.Prob = model.Prob;
					_db.Update(pre_ChancePasses);
					await _db.SaveAsync();

					_transaction.Commit();
				}

				return _mapper.Map<Pre_ChancePassCustom>(pre_ChancePasses);
			}
		}

		public async Task<Pre_ChancePassCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Pre_ChancePasses
										 .OrderByDescending(o => o.CreateDate)
										 .FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Pre_ChancePassCustom>(query);
		}

		public async Task<PaginationView<List<Pre_ChancePassCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Pre_ChancePasses
									 .Where(x => x.Status != StatusModel.Delete)
									 .OrderBy(x => x.SequenceNo)
									 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Pre_ChancePassCustom>>()
			{
				Items = _mapper.Map<List<Pre_ChancePassCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

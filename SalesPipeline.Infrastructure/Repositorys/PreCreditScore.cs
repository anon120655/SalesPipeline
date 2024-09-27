using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class PreCreditScore : IPreCreditScore
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public PreCreditScore(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Pre_CreditScoreCustom> Create(Pre_CreditScoreCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var pre_CreditScores = new Pre_CreditScore();

				pre_CreditScores.CreateBy = model.CurrentUserId;
				pre_CreditScores.CreateDate = _dateNow;
				pre_CreditScores.UpdateBy = model.CurrentUserId;
				pre_CreditScores.UpdateDate = _dateNow;
				pre_CreditScores.Level = model.Level;
				pre_CreditScores.CreditScore = model.CreditScore;
				pre_CreditScores.Grade = model.Grade;
				pre_CreditScores.LimitMultiplier = model.LimitMultiplier;
				pre_CreditScores.RateMultiplier = model.RateMultiplier;
				pre_CreditScores.CreditScoreColor = model.CreditScoreColor;
				await _db.InsterAsync(pre_CreditScores);
				await _db.SaveAsync();

				_transaction.Commit();

				return _mapper.Map<Pre_CreditScoreCustom>(pre_CreditScores);
			}
		}

		public async Task<Pre_CreditScoreCustom> Update(Pre_CreditScoreCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var pre_CreditScores = await _repo.Context.Pre_CreditScores.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (pre_CreditScores != null)
				{
					pre_CreditScores.UpdateDate = _dateNow;
					pre_CreditScores.UpdateBy = model.CurrentUserId;
					pre_CreditScores.Level = model.Level;
					pre_CreditScores.CreditScore = model.CreditScore;
					pre_CreditScores.Grade = model.Grade;
					pre_CreditScores.LimitMultiplier = model.LimitMultiplier;
					pre_CreditScores.RateMultiplier = model.RateMultiplier;
					pre_CreditScores.CreditScoreColor = model.CreditScoreColor;
					_db.Update(pre_CreditScores);
					await _db.SaveAsync();

					_transaction.Commit();
				}

				return _mapper.Map<Pre_CreditScoreCustom>(pre_CreditScores);
			}
		}

		public async Task DeleteById(UpdateModel model)
		{
			Guid id = Guid.Parse(model.id);
			var query = await _repo.Context.Pre_CreditScores.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
			if (query != null)
			{
				query.UpdateDate = DateTime.Now;
				query.UpdateBy = model.userid;
				query.Status = StatusModel.Delete;
				_db.Update(query);
				await _db.SaveAsync();
			}
		}

		public async Task<Pre_CreditScoreCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Pre_CreditScores
										 .OrderByDescending(o => o.CreateDate)
										 .FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Pre_CreditScoreCustom>(query);
		}

		public async Task<PaginationView<List<Pre_CreditScoreCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Pre_CreditScores
									 .Where(x => x.Status != StatusModel.Delete)
									 .OrderBy(x => x.CreateDate)
									 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Pre_CreditScoreCustom>>()
			{
				Items = _mapper.Map<List<Pre_CreditScoreCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}
	}
}

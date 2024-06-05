using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.PreApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class PreFactor : IPreFactor
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public PreFactor(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Pre_FactorCustom> Create(Pre_FactorCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var sales = await _repo.Context.Sales.Where(x => x.Id == model.SaleId).FirstOrDefaultAsync();

				DateTime _dateNow = DateTime.Now;

				var pre_Factor = new Data.Entity.Pre_Factor();
				pre_Factor.Status = StatusModel.Active;
				pre_Factor.CreateDate = _dateNow;
				pre_Factor.CreateBy = model.CurrentUserId;
				pre_Factor.SaleId = model.SaleId;
				pre_Factor.CompanyName = sales?.CompanyName;
				await _db.InsterAsync(pre_Factor);
				await _db.SaveAsync();

				_transaction.Commit();

				return _mapper.Map<Pre_FactorCustom>(pre_Factor);
			}
		}

		public Task<Pre_FactorCustom> GetById(Guid id)
		{
			throw new NotImplementedException();
		}

	}
}

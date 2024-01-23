using AutoMapper;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class Sales : ISales
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public Sales(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<SaleCustom> Create(SaleCustom model)
		{
			DateTime _dateNow = DateTime.Now;

			var sale = new Data.Entity.Sale();
			sale.Status = StatusModel.Active;
			sale.CreateDate = _dateNow;
			sale.CreateBy = model.CurrentUserId;
			sale.UpdateDate = _dateNow;
			sale.UpdateBy = model.CurrentUserId;
			await _db.InsterAsync(sale);
			await _db.SaveAsync();

			return _mapper.Map<SaleCustom>(sale);
		}
	}
}

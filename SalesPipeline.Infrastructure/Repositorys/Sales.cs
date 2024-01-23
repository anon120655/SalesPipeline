using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
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

			string? companyName = null;
			string? currentUserName = null;
			//string? statusSaleName = null;
			//string? statusSaleDescription = null;

			var customer = await _repo.Customer.GetById(model.CustomerId);
			if (customer != null) companyName = customer.CompanyName;

			var user = await _repo.User.GetById(model.CurrentUserId);
			if (user != null) currentUserName = user.FullName;

			//var masterStatus = await _repo.MasterStatusSale.GetById(model.StatusSaleId);
			//if (masterStatus != null)
			//{
			//	statusSaleName = masterStatus.Name;
			//	statusSaleDescription = masterStatus.Description;
			//}

			var sale = new Data.Entity.Sale();
			sale.Status = StatusModel.Active;
			sale.CreateDate = _dateNow;
			sale.CreateBy = model.CurrentUserId;
			sale.CreateByName = currentUserName;
			sale.UpdateDate = _dateNow;
			sale.UpdateBy = model.CurrentUserId;
			sale.UpdateByName = currentUserName;
			sale.CustomerId = model.CustomerId;
			sale.CompanyName = companyName;
			sale.ResponsibleName = currentUserName;
			sale.StatusSaleId = model.StatusSaleId;
			//sale.StatusSaleName = statusSaleName;
			//sale.StatusSaleDescription = statusSaleDescription;
			sale.DateAppointment = model.DateAppointment;
			sale.PercentChanceLoanPass = model.PercentChanceLoanPass;

			await _db.InsterAsync(sale);
			await _db.SaveAsync();

			//var sale_Status = new Data.Entity.Sale_Status();
			//sale_Status.Status = StatusModel.Active;
			//sale_Status.CreateDate = _dateNow;
			//sale_Status.SaleId  = sale.Id;
			//sale_Status.StatusId  = model.StatusSaleId;

			//await _db.InsterAsync(sale_Status);
			//await _db.SaveAsync();

			await UpdateStatusOnly(new()
			{
				SaleId = sale.Id,
				StatusId = model.StatusSaleId
			});

			return _mapper.Map<SaleCustom>(sale);
		}

		public Task<SaleCustom> Update(SaleCustom model)
		{
			throw new NotImplementedException();
		}

		public async Task UpdateStatusOnly(Sale_StatusCustom model)
		{
			DateTime _dateNow = DateTime.Now;
			var sale_Status = new Data.Entity.Sale_Status();
			sale_Status.Status = StatusModel.Active;
			sale_Status.CreateDate = _dateNow;
			sale_Status.SaleId = model.SaleId;
			sale_Status.StatusId = model.StatusId;
			sale_Status.Description = model.Description;

			await _db.InsterAsync(sale_Status);
			await _db.SaveAsync();

			var sales = await _repo.Context.Sales.Where(x => x.Id == model.SaleId).FirstOrDefaultAsync();
			if (sales != null)
			{
				string? statusSaleName = null;
				string? statusSaleDescription = null;

				var masterStatus = await _repo.MasterStatusSale.GetById(model.StatusId);
				if (masterStatus != null)
				{
					statusSaleName = masterStatus.Name;
					statusSaleDescription = masterStatus.Description;
				}

				sales.UpdateDate = _dateNow;
				sales.StatusSaleId = model.StatusId;
				sales.StatusSaleName = statusSaleName;
				sales.StatusDescription = model.Description;
				_db.Update(sales);
				await _db.SaveAsync();
			}
		}

		public async Task<SaleCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Sales
				.Include(x => x.Customer)
				.Include(x => x.StatusSale)
				.Where(x => x.Id == id).FirstOrDefaultAsync();
			return _mapper.Map<SaleCustom>(query);
		}

	}
}

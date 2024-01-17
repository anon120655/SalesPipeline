using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class Customers : ICustomers
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public Customers(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public Task<CustomerCustom> Validate(CustomerCustom model, bool isThrow = false)
		{
			throw new NotImplementedException();
		}

		public Task<List<CustomerCustom>> ValidateUpload(List<CustomerCustom> model)
		{
			throw new NotImplementedException();
		}

		public async Task<CustomerCustom> Create(CustomerCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var customer = new Data.Entity.Customer()
				{
					Status = StatusModel.Active,
					CreateDate = _dateNow,
					CreateBy = model.CurrentUserId,
					UpdateDate = _dateNow,
					UpdateBy = model.CurrentUserId,
					DateContact = model.DateContact,
					ContactChannelId = model.ContactChannelId,
					BranchId = model.BranchId,
					ProvincialOffice = model.ProvincialOffice,
					EmployeeName = model.EmployeeName,
					EmployeeId = model.EmployeeId,
					ContactName = model.ContactName,
					ContactTel = model.ContactTel,
					CompanyName = model.CompanyName,
					JuristicPersonRegNumber = model.JuristicPersonRegNumber,
					BusinessType = model.BusinessType,
					BusinessSize = model.BusinessSize,
					MainProduction = model.MainProduction,
					ValueChain = model.ValueChain,
					CompanyEmail = model.CompanyEmail,
					CompanyTel = model.CompanyTel,
					ParentCompanyGroup = model.ParentCompanyGroup,
					HouseNo = model.HouseNo,
					VillageNo = model.VillageNo,
					ProvinceId = model.ProvinceId,
					AmphurId = model.AmphurId,
					TambolId = model.TambolId,
					ZipCode = model.ZipCode,
					ShareholderMeetDay = model.ShareholderMeetDay,
					RegisteredCapital = model.RegisteredCapital,
					CreditScore = model.CreditScore,
					FiscalYear = model.FiscalYear,
					StatementDate = model.StatementDate,
					TradeAccReceivable = model.TradeAccReceivable,
					TradeAccRecProceedsNet = model.TradeAccRecProceedsNet,
					Inventories = model.Inventories,
					Nationality = model.Nationality,
					Proportion = model.Proportion,
					NumberShareholder = model.NumberShareholder,
					NameShareholder = model.NameShareholder
				};
				await _db.InsterAsync(customer);
				await _db.SaveAsync();

				int indexCommittee = 1;
				foreach (var item in model.Customer_Committees ?? new())
				{
					var customerCommittee = new Data.Entity.Customer_Committee()
					{
						Status = StatusModel.Active,
						SequenceNo = indexCommittee++,
						CustomerId = customer.Id,
						Name = item.Name,
					};
					await _db.InsterAsync(customerCommittee);
					await _db.SaveAsync();
				}

				int indexShareholder = 1;
				foreach (var item in model.Customer_Shareholders ?? new())
				{
					var customerShareholder = new Data.Entity.Customer_Shareholder()
					{
						Status = StatusModel.Active,
						SequenceNo = indexShareholder++,
						CustomerId = customer.Id,
						Name = item.Name,
						Nationality = item.Nationality,
						Proportion = item.Proportion,
						NumberShareholder = item.NumberShareholder,
						TotalShareValue = item.TotalShareValue,
					};
					await _db.InsterAsync(customerShareholder);
					await _db.SaveAsync();
				}

				_transaction.Commit();

				return _mapper.Map<CustomerCustom>(customer);
			}
		}

		public async Task<CustomerCustom> Update(CustomerCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var customer = await _repo.Context.Customers.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (customer != null)
				{
					customer.UpdateDate = _dateNow;
					customer.UpdateBy = model.CurrentUserId;
					customer.DateContact = model.DateContact;
					customer.ContactChannelId = model.ContactChannelId;
					customer.BranchId = model.BranchId;
					customer.ProvincialOffice = model.ProvincialOffice;
					customer.EmployeeName = model.EmployeeName;
					customer.EmployeeId = model.EmployeeId;
					customer.ContactName = model.ContactName;
					customer.ContactTel = model.ContactTel;
					customer.CompanyName = model.CompanyName;
					customer.JuristicPersonRegNumber = model.JuristicPersonRegNumber;
					customer.BusinessType = model.BusinessType;
					customer.BusinessSize = model.BusinessSize;
					customer.MainProduction = model.MainProduction;
					customer.ValueChain = model.ValueChain;
					customer.CompanyEmail = model.CompanyEmail;
					customer.CompanyTel = model.CompanyTel;
					customer.ParentCompanyGroup = model.ParentCompanyGroup;
					customer.HouseNo = model.HouseNo;
					customer.VillageNo = model.VillageNo;
					customer.ProvinceId = model.ProvinceId;
					customer.AmphurId = model.AmphurId;
					customer.TambolId = model.TambolId;
					customer.ZipCode = model.ZipCode;
					customer.ShareholderMeetDay = model.ShareholderMeetDay;
					customer.RegisteredCapital = model.RegisteredCapital;
					customer.CreditScore = model.CreditScore;
					customer.FiscalYear = model.FiscalYear;
					customer.StatementDate = model.StatementDate;
					customer.TradeAccReceivable = model.TradeAccReceivable;
					customer.TradeAccRecProceedsNet = model.TradeAccRecProceedsNet;
					customer.Inventories = model.Inventories;
					customer.Nationality = model.Nationality;
					customer.Proportion = model.Proportion;
					customer.NumberShareholder = model.NumberShareholder;
					customer.NameShareholder = model.NameShareholder;

					_db.Update(customer);
					await _db.SaveAsync();

					_db.DeleteRange(_repo.Context.Customer_Committees.Where(x => x.Status != StatusModel.Delete && x.CustomerId == model.Id).ToList());
					_db.DeleteRange(_repo.Context.Customer_Shareholders.Where(x => x.Status != StatusModel.Delete && x.CustomerId == model.Id).ToList());

					int indexCommittee = 1;
					foreach (var item in model.Customer_Committees ?? new())
					{
						var customerCommittee = new Data.Entity.Customer_Committee()
						{
							Status = StatusModel.Active,
							SequenceNo = indexCommittee++,
							CustomerId = customer.Id,
							Name = item.Name,
						};
						await _db.InsterAsync(customerCommittee);
						await _db.SaveAsync();
					}

					int indexShareholder = 1;
					foreach (var item in model.Customer_Shareholders ?? new())
					{
						var customerShareholder = new Data.Entity.Customer_Shareholder()
						{
							Status = StatusModel.Active,
							SequenceNo = indexShareholder++,
							CustomerId = customer.Id,
							Name = item.Name,
							Nationality = item.Nationality,
							Proportion = item.Proportion,
							NumberShareholder = item.NumberShareholder,
							TotalShareValue = item.TotalShareValue,
						};
						await _db.InsterAsync(customerShareholder);
						await _db.SaveAsync();
					}

					_transaction.Commit();
				}

				return _mapper.Map<CustomerCustom>(customer);
			}
		}

		public async Task DeleteById(UpdateModel model)
		{
			Guid id = Guid.Parse(model.id);
			var query = await _repo.Context.Customers.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
			if (query != null)
			{
				query.UpdateDate = DateTime.Now;
				query.UpdateBy = model.userid;
				query.Status = StatusModel.Delete;
				_db.Update(query);
				await _db.SaveAsync();
			}
		}

		public Task UpdateStatusById(UpdateModel model)
		{
			throw new NotImplementedException();
		}

		public async Task<CustomerCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Customers
				.Include(x=>x.Customer_Committees.OrderBy(o=>o.SequenceNo))
				.Include(x=>x.Customer_Shareholders.OrderBy(o => o.SequenceNo))
				.Where(x => x.Id == id).FirstOrDefaultAsync();
			return _mapper.Map<CustomerCustom>(query);
		}

		public async Task<PaginationView<List<CustomerCustom>>> GetList(CustomerFilter model)
		{
			var query = _repo.Context.Customers.Where(x => x.Status != StatusModel.Delete)
												 .OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<CustomerCustom>>()
			{
				Items = _mapper.Map<List<CustomerCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}


	}
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Index.HPRtree;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
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

			var companyName = await _repo.Customer.GetCompanyNameById(model.CustomerId);
			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);
			var master_Department_BranchName = await _repo.MasterDepBranch.GetNameById(model.Master_Department_BranchId ?? Guid.Empty);
			var provinceName = await _repo.Thailand.GetProvinceNameByid(model.ProvinceId ?? 0);
			var branchName = await _repo.Thailand.GetBranchNameByid(model.BranchId ?? 0);

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
			sale.StatusSaleId = model.StatusSaleId;
			sale.DateAppointment = model.DateAppointment;
			sale.PercentChanceLoanPass = model.PercentChanceLoanPass;
			sale.Master_Department_BranchId = model.Master_Department_BranchId;
			sale.Master_Department_BranchName = master_Department_BranchName;
			sale.ProvinceId = model.ProvinceId;
			sale.ProvinceName = provinceName;
			sale.BranchId = model.BranchId;
			sale.BranchName = branchName;
			sale.AssCenterUserId = model.AssCenterUserId;
			sale.AssCenterUserName = model.AssCenterUserName;
			if (model.AssCenterUserId.HasValue)
			{
				sale.AssCenterCreateBy = model.CurrentUserId;
				sale.AssCenterDate = _dateNow;
			}
			sale.AssUserId = model.AssUserId;
			sale.AssUserName = model.AssUserName;

			await _db.InsterAsync(sale);
			await _db.SaveAsync();

			await UpdateStatusOnly(new()
			{
				SaleId = sale.Id,
				StatusId = model.StatusSaleId,
				CreateBy = model.CurrentUserId,
				CreateByName = currentUserName,
			});

			return _mapper.Map<SaleCustom>(sale);
		}

		public async Task<SaleCustom> Update(SaleCustom model)
		{
			DateTime _dateNow = DateTime.Now;

			var companyName = await _repo.Customer.GetCompanyNameById(model.CustomerId);
			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);
			var branchName = await _repo.Thailand.GetBranchNameByid(model.BranchId ?? 0);

			var sale = await _repo.Context.Sales
				.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
			if (sale != null)
			{
				sale.Status = StatusModel.Active;
				sale.CreateDate = _dateNow;
				sale.CreateBy = model.CurrentUserId;
				sale.CreateByName = currentUserName;
				sale.UpdateDate = _dateNow;
				sale.UpdateBy = model.CurrentUserId;
				sale.UpdateByName = currentUserName;
				sale.CustomerId = model.CustomerId;
				sale.CompanyName = companyName;
				//sale.StatusSaleId = model.StatusSaleId; //ไม่ต้อง update กรณีแก้ไข
				sale.DateAppointment = model.DateAppointment;
				sale.PercentChanceLoanPass = model.PercentChanceLoanPass;

				_db.Update(sale);
				await _db.SaveAsync();
			}
			return _mapper.Map<SaleCustom>(sale);
		}

		public async Task UpdateStatusOnly(Sale_StatusCustom model)
		{
			var sale_Statuses = await _repo.Context.Sale_Statuses.Where(x => x.Status == StatusModel.Active && x.SaleId == model.SaleId).ToListAsync();
			if (sale_Statuses != null && sale_Statuses.Count > 0)
			{
				var last_status = sale_Statuses.OrderByDescending(x => x.CreateDate).FirstOrDefault();
				if (last_status != null)
				{
					if (last_status.StatusId == StatusSaleModel.NotApprove)
					{
						throw new ExceptionCustom("Not approve the process.");
					}

					if (last_status.StatusId == model.StatusId)
					{
						throw new ExceptionCustom("Duplicate the process.");
					}
				}
			}

			string? currentUserName = model.CreateByName;
			if (String.IsNullOrEmpty(currentUserName))
			{
				currentUserName = await _repo.User.GetFullNameById(model.CreateBy);
			}

			DateTime _dateNow = DateTime.Now;
			var sale_Status = new Data.Entity.Sale_Status();
			sale_Status.Status = StatusModel.Active;
			sale_Status.CreateDate = _dateNow;
			sale_Status.CreateBy = model.CreateBy;
			sale_Status.CreateByName = currentUserName;
			sale_Status.SaleId = model.SaleId;
			sale_Status.StatusId = model.StatusId;
			sale_Status.Description = model.Description;

			await _db.InsterAsync(sale_Status);
			await _db.SaveAsync();

			var sales = await _repo.Context.Sales.Where(x => x.Id == model.SaleId).FirstOrDefaultAsync();
			if (sales != null)
			{
				string? statusSaleName = null;
				string? statusSaleNameMain = null;

				var masterStatus = await _repo.MasterStatusSale.GetById(model.StatusId);
				if (masterStatus != null)
				{
					statusSaleName = masterStatus.Name;
					statusSaleNameMain = masterStatus.NameMain;
				}

				sales.UpdateDate = _dateNow;
				sales.StatusSaleId = model.StatusId;
				sales.StatusSaleName = statusSaleName;
				sales.StatusSaleNameMain = statusSaleNameMain;
				sales.StatusDescription = model.Description;
				_db.Update(sales);
				await _db.SaveAsync();

				//อนุมัติและรอการติดต่อเฉพาะ RM สร้าง
				if (model.StatusId == StatusSaleModel.WaitContact && sales.AssUserId.HasValue)
				{
					var assignment = await _repo.AssignmentRM.GetByUserId(sales.AssUserId.Value);
					if (assignment != null)
					{
						if (assignment.CurrentNumber >= 100)
						{
							throw new ExceptionCustom("ลูกค้าที่ดูแลปัจจุบันเกินจำนวนที่กำหนด");
						}

						if (!await _repo.AssignmentRM.CheckAssignmentSaleById(sales.Id))
						{
							var assignmentSale = await _repo.AssignmentRM.CreateSale(new()
							{
								CreateBy = model.CreateBy,
								CreateByName = currentUserName,
								AssignmentRMId = assignment.Id,
								SaleId = sales.Id
							});
						}

						await _repo.AssignmentRM.UpdateCurrentNumber(assignment.Id);
					}

					//Noti
					await _repo.Notifys.Create(new()
					{
						EventId = 2,
						FromUserId = model.CreateBy,
						ToUserId = sales.AssUserId.Value,
						ActionId = 2,
						ActionName1 = sales.CompanyName
					});
				}

				//Update Status Total
				if (sales.AssUserId.HasValue)
				{
					await _repo.Sales.SetIsUpdateStatusTotal(sales.AssUserId.Value);
				}

			}
		}

		public async Task<SaleCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Sales
				.Include(x => x.Customer).ThenInclude(x => x.Customer_Committees)
				.Include(x => x.Customer).ThenInclude(x => x.Customer_Shareholders)
				.Include(x => x.StatusSale)
				//.Include(x => x.Sale_Contact_Histories.OrderBy(x => x.CreateDate))
				.Where(x => x.Id == id).FirstOrDefaultAsync();
			return _mapper.Map<SaleCustom>(query);
		}

		public async Task<SaleCustom> GetStatusById(Guid id)
		{
			var query = await _repo.Context.Sales
				.Include(x => x.StatusSale)
				.Where(x => x.Id == id).FirstOrDefaultAsync();
			return _mapper.Map<SaleCustom>(query);
		}

		public async Task<PaginationView<List<SaleCustom>>> GetList(allFilter model)
		{
			IQueryable<Sale> query;
			string? roleCode = null;
			if (model.assigncenter.HasValue)
			{
				var roleList = await _repo.User.GetRoleByUserId(model.assigncenter.Value);
				if (roleList != null)
				{
					roleCode = roleList.Code;
				}
			}

			if (roleCode != null && roleCode.ToUpper().StartsWith(RoleCodes.RM))
			{
				query = _repo.Context.Sales.Where(x => x.Status != StatusModel.Delete)
													.Include(x => x.Customer)
													.Include(x => x.Sale_Statuses)
													.OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
													.AsQueryable();
			}
			else
			{
				query = _repo.Context.Sales.Where(x => x.Status != StatusModel.Delete)
													.Include(x => x.Customer)
													.Include(x => x.AssCenterUser).ThenInclude(s => s.Master_Department_Branch)
													.Include(x => x.Sale_Statuses)
													.OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
													.AsQueryable();
			}

			if (model.customerid.HasValue && model.customerid != Guid.Empty)
			{
				query = query.Where(x => x.CustomerId == model.customerid.Value);
			}

			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (model.statussaleid.HasValue)
			{
				query = query.Where(x => x.StatusSaleId == model.statussaleid);
			}

			if (model.assigncenter.HasValue)
			{
				query = query.Where(x => x.AssCenterUserId == model.assigncenter);
			}

			if (model.assignrm.HasValue)
			{
				query = query.Where(x => x.AssUserId == model.assignrm);
			}

			if (!String.IsNullOrEmpty(model.juristicnumber))
			{
				query = query.Where(x => x.Customer != null
				&& x.Customer.JuristicPersonRegNumber != null
				&& x.Customer.JuristicPersonRegNumber.Contains(model.juristicnumber));
			}

			if (!String.IsNullOrEmpty(model.sort))
			{
				if (model.sort == OrderByModel.ASC)
				{
					query = query.OrderBy(x => x.CreateDate);
				}
				else if (model.sort == OrderByModel.DESC)
				{
					query = query.OrderByDescending(x => x.CreateDate);
				}
			}

			//ISICCode
			if (!String.IsNullOrEmpty(model.isiccode))
			{
				if (Guid.TryParse(model.isiccode, out Guid id))
				{
					query = query.Where(x => x.Customer != null && x.Customer.Master_ISICCodeId == id);
				}
			}

			//ห่วงโซ่
			if (!String.IsNullOrEmpty(model.chain))
			{
				if (Guid.TryParse(model.chain, out Guid id))
				{
					query = query.Where(x => x.Customer != null && x.Customer.Master_ChainId == id);
				}
			}

			//ประเภทธุรกิจ
			if (!String.IsNullOrEmpty(model.businesstype))
			{
				if (Guid.TryParse(model.businesstype, out Guid id))
				{
					query = query.Where(x => x.Customer != null && x.Customer.Master_BusinessTypeId == id);
				}
			}

			//จังหวัด
			if (model.provinceid.HasValue)
			{
				query = query.Where(x => x.Customer != null && x.Customer.ProvinceId == model.provinceid);
			}

			//อำเภอ
			if (model.amphurid.HasValue)
			{
				query = query.Where(x => x.Customer != null && x.Customer.AmphurId == model.amphurid);
			}

			if (!String.IsNullOrEmpty(model.searchtxt))
				query = query.Where(x => x.CompanyName != null && x.CompanyName.Contains(model.searchtxt)
				|| x.Customer != null && x.Customer.JuristicPersonRegNumber != null && x.Customer.JuristicPersonRegNumber.Contains(model.searchtxt));

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<SaleCustom>>()
			{
				Items = _mapper.Map<List<SaleCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task<Sale_ReturnCustom> CreateReturn(Sale_ReturnCustom model)
		{
			DateTime _dateNow = DateTime.Now;

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

			var sale_Return = new Data.Entity.Sale_Return();
			sale_Return.Status = StatusModel.Active;
			sale_Return.CreateDate = _dateNow;
			sale_Return.CreateBy = model.CurrentUserId;
			sale_Return.CreateByName = currentUserName;
			sale_Return.CustomerId = model.CustomerId;
			sale_Return.CompanyName = model.CompanyName;
			sale_Return.SaleId = model.SaleId;
			sale_Return.StatusSaleId = model.StatusSaleId;
			sale_Return.StatusSaleName = model.StatusSaleName;
			sale_Return.StatusDescription = model.StatusDescription;
			sale_Return.Master_BusinessTypeId = model.Master_BusinessTypeId;
			sale_Return.Master_BusinessTypeName = model.Master_BusinessTypeName;
			sale_Return.Master_LoanTypeId = model.Master_LoanTypeId;
			sale_Return.Master_LoanTypeName = model.Master_LoanTypeName;
			sale_Return.AssUserId = model.AssUserId;
			sale_Return.AssUserName = model.AssUserName;
			await _db.InsterAsync(sale_Return);
			await _db.SaveAsync();

			return _mapper.Map<Sale_ReturnCustom>(sale_Return);
		}

		public async Task<PaginationView<List<Sale_ReturnCustom>>> GetListReturn(allFilter model)
		{
			IQueryable<Sale_Return> query;
			string? roleCode = null;
			if (model.assigncenter.HasValue)
			{
				var roleList = await _repo.User.GetRoleByUserId(model.assigncenter.Value);
				if (roleList != null)
				{
					roleCode = roleList.Code;
				}
			}

			query = _repo.Context.Sale_Returns.Where(x => x.Status != StatusModel.Delete)
												.OrderByDescending(x => x.CreateDate)
												.AsQueryable();

			if (model.customerid.HasValue && model.customerid != Guid.Empty)
			{
				query = query.Where(x => x.CustomerId == model.customerid.Value);
			}

			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (model.statussaleid.HasValue)
			{
				query = query.Where(x => x.StatusSaleId == model.statussaleid);
			}

			if (model.assignrm.HasValue)
			{
				query = query.Where(x => x.AssUserId == model.assignrm);
			}

			if (!String.IsNullOrEmpty(model.sort))
			{
				if (model.sort == OrderByModel.ASC)
				{
					query = query.OrderBy(x => x.CreateDate);
				}
				else if (model.sort == OrderByModel.DESC)
				{
					query = query.OrderByDescending(x => x.CreateDate);
				}
			}

			if (!String.IsNullOrEmpty(model.searchtxt))
				query = query.Where(x => x.CompanyName != null && x.CompanyName.Contains(model.searchtxt));

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Sale_ReturnCustom>>()
			{
				Items = _mapper.Map<List<Sale_ReturnCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task SetIsUpdateStatusTotal(int userid)
		{
			var sale_Status_Totals = await _repo.Context.Sale_Status_Totals.Where(x => x.UserId == userid && !x.IsUpdate).FirstOrDefaultAsync();
			if (sale_Status_Totals != null)
			{
				sale_Status_Totals.IsUpdate = true;
				_db.Update(sale_Status_Totals);
				await _db.SaveAsync();
			}
		}

		public async Task UpdateStatusTotalById(int userid)
		{
			var statusTotal = await _repo.Context.Sales.Where(x => x.Status != StatusModel.Delete && x.AssUserId == userid).GroupBy(info => info.StatusSaleId)
						.Select(group => new
						{
							StatusID = group.Key,
							Count = group.Count()
						}).OrderBy(x => x.StatusID).ToListAsync();

			int allCustomer = statusTotal.Sum(x => x.Count);
			int waitContact = 0;
			int contact = 0;
			int waitMeet = 0;
			int meet = 0;
			int submitDocument = 0;
			int results = 0;
			int closeSale = 0;

			foreach (var item in statusTotal)
			{
				if (item.StatusID == (int)StatusSaleModel.WaitContact) waitContact = item.Count;
				if (item.StatusID == (int)StatusSaleModel.Contact) contact = item.Count;
				if (item.StatusID == (int)StatusSaleModel.WaitMeet) waitMeet = item.Count;
				if (item.StatusID == (int)StatusSaleModel.Meet) meet = item.Count;
				if (item.StatusID == (int)StatusSaleModel.SubmitDocument) submitDocument = item.Count;
				if (item.StatusID == (int)StatusSaleModel.Results) results = item.Count;
				if (item.StatusID == (int)StatusSaleModel.CloseSale) closeSale = item.Count;
			}

			int CRUD = CRUDModel.Update;

			var sale_Status_Totals = await _repo.Context.Sale_Status_Totals.Where(x => x.UserId == userid).FirstOrDefaultAsync();
			if (sale_Status_Totals == null)
			{
				CRUD = CRUDModel.Create;
				sale_Status_Totals = new();
				sale_Status_Totals.Status = StatusModel.Active;
				sale_Status_Totals.CreateDate = DateTime.Now;
				sale_Status_Totals.UserId = userid;
			}

			sale_Status_Totals.AllCustomer = allCustomer;
			sale_Status_Totals.WaitContact = waitContact;
			sale_Status_Totals.Contact = contact;
			sale_Status_Totals.WaitMeet = waitMeet;
			sale_Status_Totals.Meet = meet;
			sale_Status_Totals.SubmitDocument = submitDocument;
			sale_Status_Totals.Results = results;
			sale_Status_Totals.CloseSale = closeSale;
			sale_Status_Totals.IsUpdate = false;

			if (CRUD == CRUDModel.Create)
			{
				await _db.InsterAsync(sale_Status_Totals);
				await _db.SaveAsync();
			}
			else
			{
				_db.Update(sale_Status_Totals);
				await _db.SaveAsync();
			}
		}

		public async Task UpdateStatusTotalAll()
		{
			var sale_Status_Totals = await _repo.Context.Sale_Status_Totals.Where(x => x.Status != StatusModel.Delete && x.IsUpdate).ToListAsync();
			if (sale_Status_Totals.Count > 0)
			{
				foreach (var item in sale_Status_Totals)
				{
					await _repo.Sales.UpdateStatusTotalById(item.UserId);
				}
			}

		}

		public async Task<Sale_Status_TotalCustom> GetStatusTotalById(int userid)
		{
			var query = await _repo.Context.Sale_Status_Totals
				.Where(x => x.UserId == userid).FirstOrDefaultAsync();
			return _mapper.Map<Sale_Status_TotalCustom>(query);
		}

	}
}

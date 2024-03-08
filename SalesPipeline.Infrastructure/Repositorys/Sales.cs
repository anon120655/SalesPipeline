using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
			var sale_Statuses = await _repo.Context.Sale_Statuses.Where(x => x.SaleId == model.SaleId).ToListAsync();
			if (sale_Statuses != null && sale_Statuses.Count > 0)
			{
				foreach (var item in sale_Statuses)
				{
					if (item.StatusId == model.StatusId)
					{
						throw new ExceptionCustom("Duplicate the process.");
					}

					if (item.StatusId == StatusSaleModel.NotApprove)
					{
						throw new ExceptionCustom("Not approve the process.");
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

				var masterStatus = await _repo.MasterStatusSale.GetById(model.StatusId);
				if (masterStatus != null)
				{
					statusSaleName = masterStatus.Name;
				}

				sales.UpdateDate = _dateNow;
				sales.StatusSaleId = model.StatusId;
				sales.StatusSaleName = statusSaleName;
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
				}

			}
		}

		public async Task<SaleCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Sales
				.Include(x => x.Customer).ThenInclude(x => x.Customer_Committees)
				.Include(x => x.Customer).ThenInclude(x => x.Customer_Shareholders)
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
			if (!String.IsNullOrEmpty(model.province))
			{
				if (int.TryParse(model.province, out int id))
				{
					query = query.Where(x => x.Customer != null && x.Customer.ProvinceId == id);
				}
			}

			//อำเภอ
			if (!String.IsNullOrEmpty(model.amphur))
			{
				if (int.TryParse(model.amphur, out int id))
				{
					query = query.Where(x => x.Customer != null && x.Customer.AmphurId == id);
				}
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

	}
}

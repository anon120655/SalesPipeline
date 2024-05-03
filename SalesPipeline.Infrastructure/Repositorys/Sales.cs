using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Index.HPRtree;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Drawing;
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

		public async Task UpdateStatusOnly(Sale_StatusCustom model, SaleCustom? modelSale = null)
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

			int? statusSaleMainId = null;
			string? statusSaleNameMain = null;
			string? statusSaleName = null;

			var masterStatus = await _repo.MasterStatusSale.GetById(model.StatusId);
			if (masterStatus != null)
			{
				statusSaleMainId = masterStatus.MainId;
				statusSaleNameMain = masterStatus.NameMain;
				statusSaleName = masterStatus.Name;
			}

			DateTime _dateNow = DateTime.Now;
			if (model.CreateDate != DateTime.MinValue)
			{
				_dateNow = model.CreateDate;
			}


			var sale_Status = new Data.Entity.Sale_Status();
			sale_Status.Status = StatusModel.Active;
			sale_Status.CreateDate = _dateNow;
			sale_Status.CreateBy = model.CreateBy;
			sale_Status.CreateByName = currentUserName;
			sale_Status.SaleId = model.SaleId;
			sale_Status.StatusMainId = statusSaleMainId;
			sale_Status.StatusNameMain = statusSaleNameMain;
			sale_Status.StatusId = model.StatusId;
			sale_Status.StatusName = statusSaleName;
			sale_Status.Description = model.Description;
			sale_Status.Master_Reason_CloseSaleId = model.Master_Reason_CloseSaleId;

			await _db.InsterAsync(sale_Status);
			await _db.SaveAsync();

			var sales = await _repo.Context.Sales.Where(x => x.Id == model.SaleId).FirstOrDefaultAsync();
			if (sales != null)
			{
				sales.UpdateDate = _dateNow;
				sales.StatusSaleMainId = statusSaleMainId;
				sales.StatusSaleNameMain = statusSaleNameMain;
				sales.StatusSaleId = model.StatusId;
				sales.StatusSaleName = statusSaleName;
				sales.StatusDescription = model.Description;
				sales.Master_Reason_CloseSaleId = model.Master_Reason_CloseSaleId;

				if (modelSale != null)
				{
					if (!sales.ContactStartDate.HasValue)
					{
						sales.ContactStartDate = modelSale.ContactStartDate;
					}
					sales.LoanAmount = modelSale.LoanAmount;
				}

				_db.Update(sales);
				await _db.SaveAsync();

				if (model.StatusId == StatusSaleModel.WaitContact)
				{
					//อนุมัติและรอการติดต่อเฉพาะ RM สร้าง
					if (sales.AssUserId.HasValue)
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

					await _repo.ProcessSale.CreateContactHistory(new()
					{
						CurrentUserId = model.CreateBy,
						SaleId = model.SaleId,
						ProcessSaleCode = ProcessSaleCodeModel.WaitContact,
						StatusSaleId = model.StatusId,
						TopicName = "รอติดต่อ",
						NoteSystem = "รอติดต่อลูกค้า"
					});
				}
				else if (model.StatusId == StatusSaleModel.WaitAPIPHOENIX)
				{
					await _repo.ProcessSale.CreateContactHistory(new()
					{
						CurrentUserId = model.CreateBy,
						SaleId = model.SaleId,
						ProcessSaleCode = ProcessSaleCodeModel.Document,
						StatusSaleId = model.StatusId,
						TopicName = "ผจศ. อนุมัติคำขอสินเชื่อ",
						NoteSystem = "รอวิเคราะห์สินเชื่อ(PHOENIX)"
					});

				}
				else if (model.StatusId == StatusSaleModel.WaitResults)
				{
					await _repo.ProcessSale.CreateContactHistory(new()
					{
						CurrentUserId = model.CreateBy,
						SaleId = model.SaleId,
						ProcessSaleCode = ProcessSaleCodeModel.Document,
						StatusSaleId = model.StatusId,
						TopicName = "รอบันทึกผลลัพธ์",
						NoteSystem = "ผ่านการวิเคราะห์สินเชื่อ(PHOENIX)"
					});

				}
				else if (model.StatusId == StatusSaleModel.NotApproveLoanRequest)
				{
					await _repo.ProcessSale.CreateContactHistory(new()
					{
						CurrentUserId = model.CreateBy,
						SaleId = model.SaleId,
						ProcessSaleCode = ProcessSaleCodeModel.Document,
						StatusSaleId = model.StatusId,
						TopicName = "ผจศ. ไม่อนุมัติคำขอสินเชื่อ"
					});
				}

				//Update Status Total
				if (sales.AssUserId.HasValue)
				{
					await _repo.Sales.SetIsUpdateStatusTotal(sales.AssUserId.Value);
				}

			}
		}

		public async Task<bool> CheckStatusById(Guid id,int statusid)
		{
			return await _repo.Context.Sale_Statuses.AnyAsync(x => x.Status == StatusModel.Active && x.SaleId == id && x.StatusId == statusid);
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

			if (model.isoverdue == 1)
			{
				var _dnow = DateTime.Now.Date;
				query = query.Include(x => x.Sale_Contacts.Where(c => c.AppointmentDate.HasValue))
							 .Include(x => x.Sale_Meets.Where(c => c.AppointmentDate.HasValue))
							 .Include(x => x.Sale_Results.Where(c => c.AppointmentDate.HasValue))
							 .Where(x => (x.Sale_Contacts.Any(a => a.AppointmentDate != null && a.AppointmentDate.Value.Date < _dnow))
							 || x.Sale_Meets.Any(a => a.AppointmentDate != null && a.AppointmentDate.Value.Date < _dnow)
							 || x.Sale_Results.Any(a => a.AppointmentDate != null && a.AppointmentDate.Value.Date < _dnow));
			}

			if (model.customerid.HasValue && model.customerid != Guid.Empty)
			{
				query = query.Where(x => x.CustomerId == model.customerid.Value);
			}

			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (model.statussaleid > 0)
			{
				query = query.Where(x => x.StatusSaleId == model.statussaleid);
			}

			//การปิดการขาย สำเร็จ/ไม่สำเร็จ
			if (model.isclosesale > 0)
			{
				if (model.isclosesale == 1)
					query = query.Where(x => x.StatusSaleId == StatusSaleModel.CloseSale);
				if (model.isclosesale == 2)
					query = query.Where(x => x.StatusSaleId != StatusSaleModel.CloseSale);
			}

			//การพิจารณา ผ่าน/ไม่ผ่าน
			if (model.isconsidered > 0)
			{
				//if (model.isconsidered == 1)
				//	query = query.Where(x => x.StatusSaleId == StatusSaleModel.CloseSale);
				if (model.isconsidered == 2)
					query = query.Where(x => x.StatusSaleId == StatusSaleModel.NotApproveLoanRequest || x.StatusSaleId == StatusSaleModel.ResultsNotConsidered);
			}

			if (model.assigncenter > 0)
			{
				query = query.Where(x => x.AssCenterUserId == model.assigncenter);
			}

			if (model.assignrm > 0)
			{
				query = query.Where(x => x.AssUserId == model.assignrm);
			}

			if (!String.IsNullOrEmpty(model.contact_name))
			{
				query = query.Where(x => x.Customer.ContactName != null && x.Customer.ContactName.Contains(model.contact_name));
			}

			if (model.contactstartdate.HasValue)
			{
				query = query.Where(x => x.ContactStartDate.HasValue && x.ContactStartDate.Value.Date >= model.contactstartdate.Value.Date).OrderByDescending(x => x.CreateDate);
			}

			if (model.returndate.HasValue)
			{
				query = query.Where(x => x.Sale_Statuses.Any(w => w.StatusId == StatusSaleModel.MCenterReturnBranch && w.CreateDate.Date == model.returndate.Value.Date));
			}

			if (!String.IsNullOrEmpty(model.reason))
			{
				query = query.Where(x => x.StatusDescription != null && x.StatusDescription.Contains(model.reason));
			}

			if (!String.IsNullOrEmpty(model.assignrm_name))
			{
				query = query.Where(x => x.AssUserName != null && x.AssUserName.Contains(model.assignrm_name));
			}

			if (model.isloanamount == 1)
			{
				query = query.Where(x => x.LoanAmount.HasValue && x.LoanAmount.Value > 0);
			}

			if (model.loanamount > 0)
			{
				query = query.Where(x => x.LoanAmount.HasValue && x.LoanAmount == model.loanamount);
			}

			if (!String.IsNullOrEmpty(model.juristicnumber))
			{
				query = query.Where(x => x.Customer != null
				&& x.Customer.JuristicPersonRegNumber != null
				&& x.Customer.JuristicPersonRegNumber.Contains(model.juristicnumber));
			}

			if (model.startdate.HasValue && !model.enddate.HasValue)
			{
				query = query.Where(x => x.CreateDate.Date >= model.startdate.Value.Date).OrderByDescending(x => x.CreateDate);
			}
			if (!model.startdate.HasValue && model.enddate.HasValue)
			{
				query = query.Where(x => x.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
			}
			if (model.startdate.HasValue && model.enddate.HasValue)
			{
				query = query.Where(x => x.CreateDate.Date >= model.startdate.Value.Date && x.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
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
				if (Guid.TryParse(model.isiccode, out Guid id) && id != Guid.Empty)
				{
					query = query.Where(x => x.Customer != null && x.Customer.Master_ISICCodeId == id);
				}
			}

			//ห่วงโซ่
			if (!String.IsNullOrEmpty(model.chain))
			{
				if (Guid.TryParse(model.chain, out Guid id) && id != Guid.Empty)
				{
					query = query.Where(x => x.Customer != null && x.Customer.Master_ChainId == id);
				}
			}

			//ประเภทธุรกิจ
			if (!String.IsNullOrEmpty(model.businesstype))
			{
				if (Guid.TryParse(model.businesstype, out Guid id) && id != Guid.Empty)
				{
					query = query.Where(x => x.Customer != null && x.Customer.Master_BusinessTypeId == id);
				}
			}

			//ประเภทธุรกิจ
			if (!String.IsNullOrEmpty(model.loantypeid))
			{
				if (Guid.TryParse(model.loantypeid, out Guid id) && id != Guid.Empty)
				{
					query = query.Where(x => x.Customer != null && x.Customer.Master_LoanTypeId == id);
				}
			}

			//จังหวัด
			if (model.provinceid > 0)
			{
				query = query.Where(x => x.Customer != null && x.Customer.ProvinceId == model.provinceid);
			}

			//อำเภอ
			if (model.amphurid > 0)
			{
				query = query.Where(x => x.Customer != null && x.Customer.AmphurId == model.amphurid);
			}

			//กิจการสาขาภาค[]
			if (model.DepBranchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToGuid(model.DepBranchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Master_Department_BranchId.HasValue && idList.Contains(x.Master_Department_BranchId));
				}
			}

			//จังหวัด[]
			if (model.Provinces?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Provinces);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.ProvinceId.HasValue && idList.Contains(x.ProvinceId));
				}
			}

			//สาขา[]
			if (model.Branchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Branchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.BranchId.HasValue && idList.Contains(x.BranchId));
				}
			}

			if (model.RMUsers?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.RMUsers);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.AssUserId.HasValue && idList.Contains(x.AssUserId));
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

		public async Task<Sale_Status_TotalCustom> GetStatusTotalById(int userid)
		{
			//var query = await _repo.Context.Sale_Status_Totals
			//	.Where(x => x.UserId == userid).FirstOrDefaultAsync();
			//return _mapper.Map<Sale_Status_TotalCustom>(query);

			var response = new Sale_Status_TotalCustom();

			var statusTotal = await _repo.Context.Sales.Where(x => x.Status != StatusModel.Delete && x.AssUserId == userid).GroupBy(info => info.StatusSaleId)
						.Select(group => new
						{
							StatusID = group.Key,
							Count = group.Count()
						}).OrderBy(x => x.StatusID).ToListAsync();

			response.AllCustomer = statusTotal.Sum(x => x.Count);
			//int waitContact = 0;
			//int contact = 0;
			//int waitMeet = 0;
			//int meet = 0;
			//int submitDocument = 0;
			//int results = 0;
			//int closeSale = 0;

			foreach (var item in statusTotal)
			{
				if (item.StatusID == (int)StatusSaleModel.WaitContact) response.WaitContact = item.Count;
				if (item.StatusID == (int)StatusSaleModel.Contact) response.Contact = item.Count;
				if (item.StatusID == (int)StatusSaleModel.WaitMeet) response.WaitMeet = item.Count;
				if (item.StatusID == (int)StatusSaleModel.Meet) response.Meet = item.Count;
				if (item.StatusID == (int)StatusSaleModel.SubmitDocument) response.SubmitDocument = item.Count;
				if (item.StatusID == (int)StatusSaleModel.Results) response.Results = item.Count;
				if (item.StatusID == (int)StatusSaleModel.CloseSale) response.CloseSale = item.Count;
			}

			return response;
		}

		public async Task<Sale_Contact_InfoCustom> CreateInfo(Sale_Contact_InfoCustom model)
		{
			DateTime _dateNow = DateTime.Now;

			var sale_Contact_Info = new Data.Entity.Sale_Contact_Info();
			sale_Contact_Info.Status = StatusModel.Active;
			sale_Contact_Info.CreateDate = _dateNow;
			sale_Contact_Info.SaleId = model.SaleId;
			sale_Contact_Info.FullName = model.FullName;
			sale_Contact_Info.Position = model.Position;
			sale_Contact_Info.Tel = model.Tel;
			sale_Contact_Info.Email = model.Email;

			await _db.InsterAsync(sale_Contact_Info);
			await _db.SaveAsync();

			return _mapper.Map<Sale_Contact_InfoCustom>(sale_Contact_Info);
		}

		public async Task<Sale_Contact_InfoCustom> UpdateInfo(Sale_Contact_InfoCustom model)
		{
			DateTime _dateNow = DateTime.Now;

			var sale_Contact_Info = await _repo.Context.Sale_Contact_Infos
				.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
			if (sale_Contact_Info != null)
			{
				sale_Contact_Info.FullName = model.FullName;
				sale_Contact_Info.Position = model.Position;
				sale_Contact_Info.Tel = model.Tel;
				sale_Contact_Info.Email = model.Email;

				_db.Update(sale_Contact_Info);
				await _db.SaveAsync();
			}
			return _mapper.Map<Sale_Contact_InfoCustom>(sale_Contact_Info);
		}

		public async Task<Sale_Contact_InfoCustom> GetInfoById(Guid id)
		{
			var query = await _repo.Context.Sale_Contact_Infos
				.Where(x => x.Id == id).FirstOrDefaultAsync();
			return _mapper.Map<Sale_Contact_InfoCustom>(query);
		}

		public async Task<PaginationView<List<Sale_Contact_InfoCustom>>> GetListInfo(allFilter model)
		{
			var query = _repo.Context.Sale_Contact_Infos
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderBy(x => x.CreateDate)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Sale_Contact_InfoCustom>>()
			{
				Items = _mapper.Map<List<Sale_Contact_InfoCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task<Sale_PartnerCustom> CreatePartner(Sale_PartnerCustom model)
		{
			DateTime _dateNow = DateTime.Now;

			string? master_BusinessTypeName = null;
			string? master_YieldName = null;
			string? master_ChainName = null;
			string? master_BusinessSizeName = null;

			if (model.Master_BusinessTypeId.HasValue)
			{
				master_BusinessTypeName = await _repo.MasterBusinessType.GetNameById(model.Master_BusinessTypeId.Value);
			}
			if (model.Master_YieldId.HasValue)
			{
				master_YieldName = await _repo.MasterYield.GetNameById(model.Master_YieldId.Value);
			}
			if (model.Master_ChainId.HasValue)
			{
				master_ChainName = await _repo.MasterChain.GetNameById(model.Master_ChainId.Value);
			}
			if (model.Master_BusinessSizeId.HasValue)
			{
				master_BusinessSizeName = await _repo.MasterBusinessSize.GetNameById(model.Master_BusinessSizeId.Value);
			}

			var sale_Partner = new Data.Entity.Sale_Partner();
			sale_Partner.Status = StatusModel.Active;
			sale_Partner.CreateDate = _dateNow;
			sale_Partner.SaleId = model.SaleId;
			sale_Partner.FullName = model.FullName;
			sale_Partner.Master_BusinessTypeId = model.Master_BusinessTypeId;
			sale_Partner.Master_BusinessTypeName = master_BusinessTypeName;
			sale_Partner.Master_YieldId = model.Master_YieldId;
			sale_Partner.Master_YieldName = master_YieldName;
			sale_Partner.Master_ChainId = model.Master_ChainId;
			sale_Partner.Master_ChainName = master_ChainName;
			sale_Partner.Master_BusinessSizeId = model.Master_BusinessSizeId;
			sale_Partner.Master_BusinessSizeName = master_BusinessSizeName;
			sale_Partner.Tel = model.Tel;

			await _db.InsterAsync(sale_Partner);
			await _db.SaveAsync();

			return _mapper.Map<Sale_PartnerCustom>(sale_Partner);
		}

		public async Task<Sale_PartnerCustom> UpdatePartner(Sale_PartnerCustom model)
		{
			DateTime _dateNow = DateTime.Now;

			var sale_Partner = await _repo.Context.Sale_Partners
				.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
			if (sale_Partner != null)
			{
				string? master_BusinessTypeName = null;
				string? master_YieldName = null;
				string? master_ChainName = null;
				string? master_BusinessSizeName = null;

				if (model.Master_BusinessTypeId.HasValue)
				{
					master_BusinessTypeName = await _repo.MasterBusinessType.GetNameById(model.Master_BusinessTypeId.Value);
				}
				if (model.Master_YieldId.HasValue)
				{
					master_YieldName = await _repo.MasterYield.GetNameById(model.Master_YieldId.Value);
				}
				if (model.Master_ChainId.HasValue)
				{
					master_ChainName = await _repo.MasterChain.GetNameById(model.Master_ChainId.Value);
				}
				if (model.Master_BusinessSizeId.HasValue)
				{
					master_BusinessSizeName = await _repo.MasterBusinessSize.GetNameById(model.Master_BusinessSizeId.Value);
				}

				sale_Partner.FullName = model.FullName;
				sale_Partner.Master_BusinessTypeId = model.Master_BusinessTypeId;
				sale_Partner.Master_BusinessTypeName = master_BusinessTypeName;
				sale_Partner.Master_YieldId = model.Master_YieldId;
				sale_Partner.Master_YieldName = master_YieldName;
				sale_Partner.Master_ChainId = model.Master_ChainId;
				sale_Partner.Master_ChainName = master_ChainName;
				sale_Partner.Master_BusinessSizeId = model.Master_BusinessSizeId;
				sale_Partner.Master_BusinessSizeName = master_BusinessSizeName;
				sale_Partner.Tel = model.Tel;

				_db.Update(sale_Partner);
				await _db.SaveAsync();
			}
			return _mapper.Map<Sale_PartnerCustom>(sale_Partner);
		}

		public async Task<Sale_PartnerCustom> GetPartnerById(Guid id)
		{
			var query = await _repo.Context.Sale_Partners
				.Where(x => x.Id == id).FirstOrDefaultAsync();
			return _mapper.Map<Sale_PartnerCustom>(query);
		}

		public async Task<PaginationView<List<Sale_PartnerCustom>>> GetListPartner(allFilter model)
		{
			var query = _repo.Context.Sale_Partners
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderBy(x => x.CreateDate)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Sale_PartnerCustom>>()
			{
				Items = _mapper.Map<List<Sale_PartnerCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

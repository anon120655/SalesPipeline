using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using System.Linq.Expressions;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Utils.Resources.Customers;

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

		public async Task<IQueryable<Sale>> QueryArea(IQueryable<Sale> query, UserCustom user)
		{
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			if (user.Role.Code != null && user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				query = query.Where(x => x.AssUserId == user.Id);
			}
			else
			{
				//99999999-9999-9999-9999-999999999999 เห็นทั้งประเทศ
				if (!user.Role.IsAssignCenter && user.Master_Branch_RegionId != Guid.Parse("99999999-9999-9999-9999-999999999999"))
				{
					Expression<Func<Sale, bool>> orExpression = x => false;
					//9999 เห็นทุกจังหวัดในภาค
					if (user.Master_Branch_RegionId.HasValue && user_Areas.Any(x => x == 9999))
					{
						var provinces = await _repo.Thailand.GetProvince(user.Master_Branch_RegionId);
						if (provinces?.Count > 0)
						{
							user_Areas = provinces.Select(x => x.ProvinceID).ToList();
						}
					}

					//พนักงาน RM ภายใต้พื้นที่การดูแล
					//ในพื้นที่จังหวัด หรือ ดูแลทุกจังหวัดในภาค
					foreach (var provinceId in user_Areas)
					{
						var tempProvinceId = provinceId;
						orExpression = orExpression.Or(x =>
						(x.AssUser != null && x.AssUser.User_Areas.Any(s => s.ProvinceId == tempProvinceId))
						|| (x.AssUser != null && x.AssUser.User_Areas.Any(s => s.User.Master_Branch_RegionId == user.Master_Branch_RegionId && s.ProvinceId == 9999))
						);
					}

					//งานที่สร้างเอง หรือถูกมอบหมายมาจาก ธญ
					orExpression = orExpression.Or(x => x.AssCenterUserId == user.Id);
					query = query.Where(orExpression);

					query = query.Where(x => x.StatusSaleId != StatusSaleModel.MCenterReturnLoan);
				}
			}

			return query;
		}

		public async Task<SaleCustom> Create(SaleCustom model)
		{
			DateTime _dateNow = DateTime.Now;

			var companyName = await _repo.Customer.GetCompanyNameById(model.CustomerId);
			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);
			var master_Branch_RegionName = await _repo.MasterBranchReg.GetNameById(model.Master_Branch_RegionId ?? Guid.Empty);
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
			sale.CIF = model.CIF;
			sale.CompanyName = companyName;
			sale.StatusSaleId = model.StatusSaleId;
			sale.DateAppointment = model.DateAppointment;
			sale.PercentChanceLoanPass = model.PercentChanceLoanPass;
			//sale.Master_Branch_RegionId = model.Master_Branch_RegionId;
			//sale.Master_Branch_RegionName = master_Branch_RegionName;
			sale.ProvinceId = model.ProvinceId;
			sale.ProvinceName = provinceName;
			sale.BranchId = model.BranchId;
			sale.BranchName = branchName;

			if (model.AssCenterUserId.HasValue)
			{
				sale.AssCenterAlready = true;
				sale.AssCenterUserId = model.AssCenterUserId;
				sale.AssCenterUserName = model.AssCenterUserName;
				sale.AssCenterCreateBy = model.CurrentUserId;
				sale.AssCenterDate = _dateNow;
			}
			if (model.AssUserId.HasValue)
			{
				sale.AssUserAlready = true;
				sale.AssUserId = model.AssUserId;
				sale.AssUserName = model.AssUserName;
			}

			sale.IsRePurpose = model.IsRePurpose;

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
			var provinceName = await _repo.Thailand.GetProvinceNameByid(model.ProvinceId ?? 0);
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
				sale.CIF = model.CIF;
				sale.CompanyName = companyName;
				sale.ProvinceId = model.ProvinceId;
				sale.ProvinceName = provinceName;
				sale.BranchId = model.BranchId;
				sale.BranchName = branchName;
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
							//if (assignment.CurrentNumber >= 100)
							//{
							//	throw new ExceptionCustom("ลูกค้าที่ดูแลปัจจุบันเกินจำนวนที่กำหนด");
							//}

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

							await _repo.AssignmentRM.UpdateCurrentNumber(assignment.UserId);
						}

						//Noti
						await _repo.Notifys.Create(new()
						{
							EventId = NotifyEventIdModel.ApproveTarget,
							SaleId = model.SaleId,
							FromUserId = model.CreateBy,
							ToUserId = sales.AssUserId.Value,
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
				else if (model.StatusId == StatusSaleModel.NotApprove)
				{
					if (sales.AssUserId.HasValue)
					{
						//Noti
						await _repo.Notifys.Create(new()
						{
							EventId = NotifyEventIdModel.NotApproveTarget,
							SaleId = model.SaleId,
							FromUserId = model.CreateBy,
							ToUserId = sales.AssUserId.Value,
							ActionName1 = sales.CompanyName
						});
					}
				}
				else if (model.StatusId == StatusSaleModel.WaitAPIPHOENIX)
				{
					string? topicName = "ผจศ. อนุมัติคำขอสินเชื่อ";
					string? noteSystem = "รอวิเคราะห์สินเชื่อ(PHOENIX)";
					if (_appSet.SystemType == SystemTypeModel.FCC)
					{
						topicName = "ผจก. อนุมัติคำขอสินเชื่อ";
						noteSystem = "รอวิเคราะห์เอกสาร";
					}
					//กรณี ผจศ. ถูกมอบหมายแล้ว
					if (sales.AssCenterUserId.HasValue && sales.AssCenterUserId.Value != model.CreateBy)
					{
						throw new ExceptionCustom("ไม่สามารถอนุมัติคำขอได้ เนื่องจากไม่อยู่ในความรับผิดชอบของท่าน");
					}
					else
					{
						//กรณี ผจศ. ยังไม่ถูกมอบหมายและ อยู่ในพื้นที่ดูแล
						sales.AssCenterAlready = true;
						sales.AssCenterUserId = model.CreateBy;
						sales.AssCenterUserName = currentUserName;
						sales.AssCenterDate = _dateNow;
						_db.Update(sales);
						await _db.SaveAsync();
					}

					await _repo.ProcessSale.CreateContactHistory(new()
					{
						CurrentUserId = model.CreateBy,
						SaleId = model.SaleId,
						ProcessSaleCode = ProcessSaleCodeModel.Document,
						StatusSaleId = model.StatusId,
						TopicName = topicName,
						NoteSystem = noteSystem
					});

					if (sales.AssUserId.HasValue)
					{
						//Noti
						await _repo.Notifys.Create(new()
						{
							EventId = NotifyEventIdModel.ApproveLoan,
							SaleId = model.SaleId,
							FromUserId = model.CreateBy,
							ToUserId = sales.AssUserId.Value,
							ActionName1 = sales.CompanyName
						});
					}
				}
				else if (model.StatusId == StatusSaleModel.RMReturnMCenter)
				{
					if (sales.AssUserId.HasValue)
					{
						//Noti
						await _repo.Notifys.Create(new()
						{
							EventId = NotifyEventIdModel.Return,
							SaleId = model.SaleId,
							FromUserId = model.CreateBy,
							ToUserId = sales.AssUserId.Value,
							ActionName1 = sales.CompanyName
						});
					}
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

				//SetNoti
				await _repo.Notifys.SetScheduleNoti();
			}
		}

		public async Task<bool> CheckStatusById(Guid id, int statusid)
		{
			return await _repo.Context.Sale_Statuses.AnyAsync(x => x.Status == StatusModel.Active && x.SaleId == id && x.StatusId == statusid);
		}

		public async Task<SaleCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Sales
				.Include(x => x.Customer).ThenInclude(x => x.Customer_Committees)
				.Include(x => x.Customer).ThenInclude(x => x.Customer_Shareholders)
				.Include(x => x.StatusSale)
				.Where(x => x.Id == id).FirstOrDefaultAsync();

			var dataMap = _mapper.Map<SaleCustom>(query);

			if (dataMap.StatusSaleId == StatusSaleModel.NotApprove
				|| dataMap.StatusSaleId == StatusSaleModel.NotApproveLoanRequest
				|| dataMap.StatusSaleId == StatusSaleModel.ResultsNotConsidered
				|| dataMap.StatusSaleId == StatusSaleModel.CloseSaleNotLoan
				|| dataMap.StatusSaleId == StatusSaleModel.CloseSale
				|| dataMap.StatusSaleId == StatusSaleModel.CloseSaleFail)
			{
				dataMap.IsShowRePurpose = true;
			}
			return dataMap;
		}

		public async Task<SaleCustom> GetByCustomerId(Guid id)
		{
			var query = await _repo.Context.Sales
				.Include(x => x.Customer).ThenInclude(x => x.Customer_Committees)
				.Include(x => x.Customer).ThenInclude(x => x.Customer_Shareholders)
				.Include(x => x.StatusSale)
				.Where(x => x.CustomerId == id).FirstOrDefaultAsync();

			var dataMap = _mapper.Map<SaleCustom>(query);

			if (dataMap.StatusSaleId == StatusSaleModel.NotApprove
				|| dataMap.StatusSaleId == StatusSaleModel.NotApproveLoanRequest
				|| dataMap.StatusSaleId == StatusSaleModel.ResultsNotConsidered
				|| dataMap.StatusSaleId == StatusSaleModel.CloseSaleNotLoan
				|| dataMap.StatusSaleId == StatusSaleModel.CloseSale
				|| dataMap.StatusSaleId == StatusSaleModel.CloseSaleFail)
			{
				dataMap.IsShowRePurpose = true;
			}
			return dataMap;
		}

		public async Task<SaleCustom> GetStatusById(Guid id)
		{
			var query = await _repo.Context.Sales
				.Include(x => x.StatusSale)
				.Where(x => x.Id == id).FirstOrDefaultAsync();
			return _mapper.Map<SaleCustom>(query);
		}

		public async Task<bool> IsViewSales(Guid id, int userid)
		{
			var user = await _repo.User.GetById(userid);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");

			if (user.Role.IsAssignRM)
			{
				var areaCenter = user.User_Areas?.Select(x => x.ProvinceId).ToList();

				var sale = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.Id == id);
				if (sale == null) throw new ExceptionCustom("sale not found.");
				//เช็คว่าถูกมอบหมายให้ ผจศ หรือไม่
				if (sale.AssCenterUserId != userid)
				{
					//99999999-9999-9999-9999-999999999999 ผจศ เห็นทั้งประเทศ
					if (user.Master_Branch_RegionId != Guid.Parse("99999999-9999-9999-9999-999999999999"))
					{
						if (!sale.AssUserId.HasValue) return false; //ยังไม่ถูกมอบหมายให้ RM

						var userRM = await _repo.Context.Users.Include(x => x.User_Areas).FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.Id == sale.AssUserId);
						if (userRM == null) throw new ExceptionCustom("users rm not found.");
						var areaRM = user.User_Areas?.Select(x => x.ProvinceId).ToList();
						if (areaRM == null || areaRM.Count == 0) return false;

						//99999999-9999-9999-9999-999999999999 RM เห็นทั้งประเทศ
						if (userRM.Master_Branch_RegionId != Guid.Parse("99999999-9999-9999-9999-999999999999"))
						{
							if (user.Master_Branch_RegionId != userRM.Master_Branch_RegionId) return false; //กิจการสาขาไม่ตรงกัน

							//9999 ผจศ และ RM เห็นทุกจังหวัดในภาค
							if (areaCenter == null || areaCenter.Count == 0) return false;
							if (areaCenter.Any(x => x == 9999) && areaRM.Any(x => x == 9999)) return true;

							//เช็คพนักงาน RM อยู่ภายใต้พื้นที่การดูแลหรือไม่ (เช็คพื้นที่ดูแล ผจศ และ พนักงาน RM ว่าตรงกันหรือไม่)
							var check = GeneralUtils.HasAtLeastOneCommonElement(areaRM, areaCenter);

							return check;
						}
					}
				}
			}

			return true;
		}

		public async Task<PaginationView<List<SaleCustom>>> GetList(allFilter model)
		{
			if (!model.userid.HasValue) throw new ExceptionCustom("userid require.");

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			//var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			if (model.isassigncenter == 1)
			{
				if (!user.Role.IsAssignCenter)
				{
					return new() { Items = new() };
				}
			}

			if (model.isassignrm == 1)
			{
				if (!user.Role.IsAssignRM)
				{
					return new() { Items = new() };
				}
			}

			IQueryable<Sale> query;

			if (user.Role.Code != null && user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
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
													.Include(x => x.AssUser).ThenInclude(t => t.User_Areas)
													.Include(x => x.AssCenterUser).ThenInclude(s => s.Master_Branch_Region)
													.Include(x => x.Sale_Statuses)
													.OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
													.AsQueryable();
			}

			query = await QueryArea(query, user);

			if (model.isoverdue == 1)
			{
				var dataSLA = await _repo.System.GetListSLA(model);
				if (dataSLA != null && dataSLA.Items != null)
				{
					var _date_Contact = dataSLA.Items.FirstOrDefault(x => x.Code == nameof(StatusSaleMainModel.Contact))?.Number ?? 0;
					var _date_Meet = dataSLA.Items.FirstOrDefault(x => x.Code == nameof(StatusSaleMainModel.Meet))?.Number ?? 0;
					var _date_Document = dataSLA.Items.FirstOrDefault(x => x.Code == nameof(StatusSaleMainModel.Document))?.Number ?? 0;
					var _date_Result = dataSLA.Items.FirstOrDefault(x => x.Code == nameof(StatusSaleMainModel.Result))?.Number ?? 0;

					var _dnow = DateTime.Now.Date;

					query = query.Where(x => x.StatusSaleMainId != StatusSaleMainModel.CloseSale
								  && x.StatusSaleId != StatusSaleModel.CloseSaleNotLoan
								  && x.StatusSaleId != StatusSaleModel.ResultsNotConsidered
								  && x.StatusSaleId != StatusSaleModel.NotApproveLoanRequest
								  && x.StatusSaleId != StatusSaleModel.MCenterReturnLoan
								  && x.StatusSaleId != StatusSaleModel.RMReturnMCenter
								  && (x.StatusSaleMainId == StatusSaleMainModel.Contact && x.StatusSaleId == StatusSaleModel.WaitContact
								 || x.StatusSaleMainId == StatusSaleMainModel.Contact && x.Sale_Contacts.Any(a => a.CreateDate.Date < _dnow.AddDays(_date_Contact))
								 || x.StatusSaleMainId == StatusSaleMainModel.Meet && x.Sale_Meets.Any(a => a.CreateDate.Date < _dnow.AddDays(_date_Meet))
								 || x.StatusSaleMainId == StatusSaleMainModel.Document && x.Sale_Documents.Any(a => a.CreateDate.Date < _dnow.AddDays(_date_Document))
								 || x.StatusSaleMainId == StatusSaleMainModel.Result && x.Sale_Results.Any(a => a.CreateDate.Date < _dnow.AddDays(_date_Result))
								 ));
				}

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

			if (model.StatusSales?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.StatusSales);
				if (idList.Count > 0)
				{
					query = query.Where(x => idList.Contains(x.StatusSaleId));
				}
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
				query = query.Where(x => x.Sale_Statuses.Any(w => w.StatusId == StatusSaleModel.MCenterReturnLoan && w.CreateDate.Date == model.returndate.Value.Date));
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

			//ประเภทกิจการ
			if (!String.IsNullOrEmpty(model.businesstype))
			{
				if (Guid.TryParse(model.businesstype, out Guid id) && id != Guid.Empty)
				{
					query = query.Where(x => x.Customer != null && x.Customer.Master_BusinessTypeId == id);
				}
			}

			//ประเภทสินเชื่อ
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
				//query = query.Where(x => x.Customer != null && x.Customer.ProvinceId == model.provinceid);
				query = query.Where(x => x.ProvinceId == model.provinceid);
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
					query = query.Where(x => x.Master_Branch_RegionId.HasValue && idList.Contains(x.Master_Branch_RegionId));
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

			if (!String.IsNullOrEmpty(model.cif))
			{
				query = query.Where(x => x.CIF != null && x.CIF.Contains(model.cif));
			}

			if (!String.IsNullOrEmpty(model.searchtxt))
			{
				model.searchtxt = model.searchtxt.Trim();
				query = query.Where(x => x.CompanyName != null && x.CompanyName.Contains(model.searchtxt)
				|| x.Customer != null && x.Customer.JuristicPersonRegNumber != null && x.Customer.JuristicPersonRegNumber.Contains(model.searchtxt));
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<SaleCustom>>()
			{
				Items = _mapper.Map<List<SaleCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task<List<Sale_StatusCustom>> GetListStatusById(Guid id)
		{
			var query = await _repo.Context.Sale_Statuses
				.Where(x => x.SaleId == id).ToListAsync();
			return _mapper.Map<List<Sale_StatusCustom>>(query);
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

			//ประเภทกิจการ
			if (!String.IsNullOrEmpty(model.businesstype))
			{
				if (Guid.TryParse(model.businesstype, out Guid id) && id != Guid.Empty)
				{
					query = query.Where(x => x.Master_BusinessTypeId != null && x.Master_BusinessTypeId == id);
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

		public async Task<int> GetOverdueCount(allFilter model)
		{
			if (!model.userid.HasValue) throw new ExceptionCustom("userid require.");

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			IQueryable<Sale> query = _repo.Context.Sales.Where(x => x.Status != StatusModel.Delete)
													//.Include(x => x.Customer)
													//.Include(x => x.Sale_Statuses)
													.OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
													.AsQueryable();

			if (user.Role.Code != null && user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				query = query.Where(x => x.AssUserId == user.Id);
			}
			else
			{
				if (user_Areas.Count > 0)
				{
					query = query.Where(x => x.ProvinceId.HasValue && user_Areas.Contains(x.ProvinceId.Value));
				}
			}

			var dataSLA = await _repo.System.GetListSLA(model);
			if (dataSLA != null && dataSLA.Items != null)
			{
				var _date_Contact = dataSLA.Items.FirstOrDefault(x => x.Code == nameof(StatusSaleMainModel.Contact))?.Number ?? 0;
				var _date_Meet = dataSLA.Items.FirstOrDefault(x => x.Code == nameof(StatusSaleMainModel.Meet))?.Number ?? 0;
				var _date_Document = dataSLA.Items.FirstOrDefault(x => x.Code == nameof(StatusSaleMainModel.Document))?.Number ?? 0;
				var _date_Result = dataSLA.Items.FirstOrDefault(x => x.Code == nameof(StatusSaleMainModel.Result))?.Number ?? 0;

				var _dnow = DateTime.Now.Date;

				query = query.Where(x => x.StatusSaleMainId != StatusSaleMainModel.CloseSale &&
							   (x.StatusSaleMainId == StatusSaleMainModel.Contact && x.StatusSaleId == StatusSaleModel.WaitContact
							 || x.StatusSaleMainId == StatusSaleMainModel.Contact && x.Sale_Contacts.Any(a => a.CreateDate.Date < _dnow.AddDays(_date_Contact))
							 || x.StatusSaleMainId == StatusSaleMainModel.Meet && x.Sale_Meets.Any(a => a.CreateDate.Date < _dnow.AddDays(_date_Meet))
							 || x.StatusSaleMainId == StatusSaleMainModel.Document && x.Sale_Documents.Any(a => a.CreateDate.Date < _dnow.AddDays(_date_Document))
							 || x.StatusSaleMainId == StatusSaleMainModel.Result && x.Sale_Results.Any(a => a.CreateDate.Date < _dnow.AddDays(_date_Result))
							 ));
			}

			return query.Count();
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
			sale_Contact_Info.Createdfrom = model.Createdfrom;

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
				sale_Contact_Info.Createdfrom = model.Createdfrom;

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
												 //.OrderBy(x => x.CreateDate)
												 .OrderBy(x => x.FullName)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (model.saleid.HasValue)
			{
				query = query.Where(x => x.SaleId == model.saleid);
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

			if (model.saleid.HasValue)
			{
				query = query.Where(x => x.SaleId == model.saleid);
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Sale_PartnerCustom>>()
			{
				Items = _mapper.Map<List<Sale_PartnerCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task<List<HistoryLoanModel>> GetListHistoryLoan(allFilter model)
		{
			var historyLoan = await _repo.Context.Sales
										.Include(x => x.Sale_Statuses)
										.Where(x => x.Customer.JuristicPersonRegNumber == model.juristicnumber)
										.Select(x => new
										{
											Loan = x,
											Status = x.Sale_Statuses.FirstOrDefault(s => s.StatusId == StatusSaleModel.WaitAPIPHOENIX)
										})
										.Select(x => new HistoryLoanModel()
										{
											DateApprove = x.Status != null ? x.Status.CreateDate : (DateTime?)null,
											LoanAmount = x.Loan.LoanAmount,
											ProjectLoan = x.Loan.ProjectLoanName,
											LoanPeriod = x.Loan.LoanPeriod,
										})
										.ToListAsync();

			return historyLoan;
		}

		public async Task<CustomerCustom> RePurpose(RePurposeModel model)
		{
			if (model.CurrentUserId == 0) throw new ExceptionCustom("userId not found.");
			var user = await _repo.User.GetById(model.CurrentUserId);
			if (user == null || user.Role == null) throw new ExceptionCustom("User not found.");

			var sales = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Id == model.SaleId);
			if (sales == null) throw new ExceptionCustom("sales not found.");

			var customer = await _repo.Customer.GetById(sales.CustomerId);
			if (customer == null) throw new ExceptionCustom("customer not found.");

			var formModel = GeneralUtils.DeepCopyJson(customer);

			formModel.IsRePurpose = true;
			formModel.CurrentUserId = model.CurrentUserId;
			formModel.CIF = null;

			var data = await _repo.Customer.Create(formModel);

			return data;
		}

	}
}

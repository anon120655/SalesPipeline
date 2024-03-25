using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.GeometriesGraph;
using NetTopologySuite.Index.HPRtree;
using NPOI.SS.Formula.Functions;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using System.Text.Json;

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

		public async Task Validate(CustomerCustom model, bool isThrow = false)
		{
			string? juristicPersonRegNumber = model.JuristicPersonRegNumber?.Trim();
			var customer = await _repo.Context.Customers.Where(x => x.JuristicPersonRegNumber == juristicPersonRegNumber).FirstOrDefaultAsync();
			if (customer != null)
			{
				throw new ExceptionCustom($"มีเลขทะเบียนนิติบุคคล {customer.JuristicPersonRegNumber} ในระบบแล้ว");
			}
		}

		public Task<List<CustomerCustom>> ValidateUpload(List<CustomerCustom> model)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseDefaultModel> VerifyByNumber(string juristicNumber)
		{
			string code = "pass";
			string message = "ผ่าน";
			string id = "";

			juristicNumber = juristicNumber.Trim();

			var customers = await _repo.Context.Customers.Include(x => x.Sales).FirstOrDefaultAsync(x => x.JuristicPersonRegNumber == juristicNumber);
			if (customers != null)
			{
				bool isProceed = false;

				if (customers.Sales != null)
				{
					foreach (var item_sale in customers.Sales)
					{
						if (item_sale.StatusSaleId >= StatusSaleModel.WaitContact)
						{
							if (!isProceed)
							{
								code = "proceed";
								message = "ลูกค้าท่านนี้อยู่ระหว่างการดำเนินการ <br/>ไม่สามารถดำเนินการต่อได้";
								isProceed = true;
								break;
							}
						}
					}
				}

				if (!isProceed)
				{
					code = "duplicate";
					message = "ลูกค้าท่านนี้อยู่ในระบบแล้ว <br/>ต้องการดำเนินการต่อ?";
					id = customers.Id.ToString();
				}
			}

			return new()
			{
				Code = code,
				Message = message,
				ID = id
			};
		}

		public async Task<CustomerCustom> Create(CustomerCustom model)
		{
			//string strJson = JsonSerializer.Serialize<CustomerCustom>(model);

			using (var _transaction = _repo.BeginTransaction())
			{
				await Validate(model);

				string? master_ContactChannelName = null;
				string? master_BusinessTypeName = null;
				string? master_BusinessSizeName = null;
				string? master_ISICCodeName = null;
				string? master_YieldName = null;
				string? master_ChainName = null;
				string? master_LoanTypeName = null;
				string? provinceName = null;
				string? amphurName = null;
				string? tambolName = null;

				if (model.Master_ContactChannelId.HasValue)
				{
					master_ContactChannelName = await _repo.MasterContactChannel.GetNameById(model.Master_ContactChannelId.Value);
				}
				if (model.Master_BusinessTypeId.HasValue)
				{
					master_BusinessTypeName = await _repo.MasterBusinessType.GetNameById(model.Master_BusinessTypeId.Value);
				}
				if (model.Master_BusinessSizeId.HasValue)
				{
					master_BusinessSizeName = await _repo.MasterBusinessSize.GetNameById(model.Master_BusinessSizeId.Value);
				}
				if (model.Master_ISICCodeId.HasValue)
				{
					master_ISICCodeName = await _repo.MasterISICCode.GetNameById(model.Master_ISICCodeId.Value);
				}
				if (model.Master_YieldId.HasValue)
				{
					master_YieldName = await _repo.MasterYield.GetNameById(model.Master_YieldId.Value);
				}
				if (model.Master_ChainId.HasValue)
				{
					master_ChainName = await _repo.MasterChain.GetNameById(model.Master_ChainId.Value);
				}
				if (model.Master_LoanTypeId.HasValue)
				{
					master_LoanTypeName = await _repo.MasterLoanTypes.GetNameById(model.Master_LoanTypeId.Value);
				}
				if (model.ProvinceId.HasValue)
				{
					provinceName = await _repo.Thailand.GetProvinceNameByid(model.ProvinceId.Value);
				}
				if (model.AmphurId.HasValue)
				{
					amphurName = await _repo.Thailand.GetAmphurNameByid(model.AmphurId.Value);
				}
				if (model.TambolId.HasValue)
				{
					tambolName = await _repo.Thailand.GetTambolNameByid(model.TambolId.Value);
				}

				var userRole = await _repo.User.GetRoleByUserId(model.CurrentUserId);
				if (userRole == null) throw new ExceptionCustom("currentUserId not map role.");

				DateTime _dateNow = DateTime.Now;

				var customer = new Data.Entity.Customer();
				customer.Status = StatusModel.Active;
				customer.CreateDate = _dateNow;
				customer.CreateBy = model.CurrentUserId;
				customer.UpdateDate = _dateNow;
				customer.UpdateBy = model.CurrentUserId;
				customer.InsertRoleCode = userRole.Code;
				customer.DateContact = model.DateContact;
				customer.Master_ContactChannelId = model.Master_ContactChannelId;
				customer.Master_ContactChannelName = master_ContactChannelName;
				customer.BranchName = model.BranchName;
				customer.ProvincialOffice = model.ProvincialOffice;
				customer.EmployeeName = model.EmployeeName;
				customer.EmployeeId = model.EmployeeId;
				customer.ContactName = model.ContactName;
				customer.ContactTel = model.ContactTel;
				customer.CompanyName = model.CompanyName;
				customer.JuristicPersonRegNumber = model.JuristicPersonRegNumber?.Trim();
				customer.Master_BusinessTypeId = model.Master_BusinessTypeId;
				customer.Master_BusinessTypeName = master_BusinessTypeName;
				customer.Master_BusinessSizeId = model.Master_BusinessSizeId;
				customer.Master_BusinessSizeName = master_BusinessSizeName;
				customer.Master_ISICCodeId = model.Master_ISICCodeId;
				customer.Master_ISICCodeName = master_ISICCodeName;
				customer.Master_YieldId = model.Master_YieldId;
				customer.Master_YieldName = master_YieldName;
				customer.Master_ChainId = model.Master_ChainId;
				customer.Master_LoanTypeId = model.Master_LoanTypeId;
				customer.Master_LoanTypeName = master_LoanTypeName;
				customer.Master_ChainName = master_ChainName;
				customer.CompanyEmail = model.CompanyEmail;
				customer.CompanyTel = model.CompanyTel;
				customer.ParentCompanyGroup = model.ParentCompanyGroup;
				customer.HouseNo = model.HouseNo;
				customer.VillageNo = model.VillageNo;
				customer.ProvinceId = model.ProvinceId;
				customer.ProvinceName = provinceName;
				customer.AmphurId = model.AmphurId;
				customer.AmphurName = amphurName;
				customer.TambolId = model.TambolId;
				customer.TambolName = tambolName;
				customer.ZipCode = model.ZipCode;
				customer.ShareholderMeetDay = model.ShareholderMeetDay;
				customer.RegisteredCapital = model.RegisteredCapital;
				customer.CreditScore = model.CreditScore;
				customer.FiscalYear = model.FiscalYear;
				customer.StatementDate = model.StatementDate;
				customer.TradeAccReceivable = model.TradeAccReceivable;
				customer.TradeAccRecProceedsNet = model.TradeAccRecProceedsNet;
				customer.Inventories = model.Inventories;
				customer.LoansShort = model.LoansShort;
				customer.TotalCurrentAssets = model.TotalCurrentAssets;
				customer.LoansLong = model.LoansLong;
				customer.LandBuildingEquipment = model.LandBuildingEquipment;
				customer.TotalNotCurrentAssets = model.TotalNotCurrentAssets;
				customer.AssetsTotal = model.AssetsTotal;
				customer.TradeAccPay = model.TradeAccPay;
				customer.TradeAccPayLoansShot = model.TradeAccPayLoansShot;
				customer.TradeAccPayTotalCurrentLia = model.TradeAccPayTotalCurrentLia;
				customer.TradeAccPayLoansLong = model.TradeAccPayLoansLong;
				customer.TradeAccPayTotalNotCurrentLia = model.TradeAccPayTotalNotCurrentLia;
				customer.TradeAccPayForLoansShot = model.TradeAccPayForLoansShot;
				customer.TradeAccPayTotalLiabilitie = model.TradeAccPayTotalLiabilitie;
				customer.RegisterCapitalOrdinary = model.RegisterCapitalOrdinary;
				customer.RegisterCapitalPaid = model.RegisterCapitalPaid;
				customer.ProfitLossAccumulate = model.ProfitLossAccumulate;
				customer.TotalShareholders = model.TotalShareholders;
				customer.TotalLiabilitieShareholders = model.TotalLiabilitieShareholders;
				customer.TotalIncome = model.TotalIncome;
				customer.CostSales = model.CostSales;
				customer.GrossProfit = model.GrossProfit;
				customer.OperatingExpenses = model.OperatingExpenses;
				customer.ProfitLossBeforeDepExp = model.ProfitLossBeforeDepExp;
				customer.ProfitLossBeforeInterestTax = model.ProfitLossBeforeInterestTax;
				customer.NetProfitLoss = model.NetProfitLoss;
				customer.InterestLoan = model.InterestLoan;
				customer.InterestLoanSpecify = model.InterestLoanSpecify;
				customer.InterestObjectiveLoan = model.InterestObjectiveLoan;
				customer.InterestCreditLimit = model.InterestCreditLimit;
				customer.InterestNote = model.InterestNote;
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

				int statusSaleId = StatusSaleModel.WaitApprove;
				Guid? master_Department_BranchId = null;
				int? provinceId = null;
				int? branchId = null;
				int? assCenterUserId = null;
				string? assCenterUserName = null;
				int? assUserId = null;
				string? assUserName = null;

				var user = await _repo.User.GetById(model.CurrentUserId);
				if (user == null) throw new ExceptionCustom("currentUserId required!");

				if (userRole.Code.ToUpper().StartsWith(RoleCodes.RM))
				{
					if (user.BranchId.HasValue)
					{
						var userMcencer = await _repo.User.GetMcencerByBranchId(user.BranchId.Value);
						if (userMcencer == null) throw new ExceptionCustom("mcencer branchId not found!");

						assCenterUserId = userMcencer.Id;
						assCenterUserName = userMcencer.FullName;
					}

					statusSaleId = StatusSaleModel.WaitApprove;
					assUserId = model.CurrentUserId;
					assUserName = user.FullName;
					master_Department_BranchId = user.Master_Department_BranchId;
					provinceId = user.ProvinceId;
					branchId = user.BranchId;
				}

				if (userRole.Code.ToUpper().StartsWith(RoleCodes.MCENTER))
				{
					var userCenter = await _repo.User.GetById(user.Id);
					if (userCenter == null) throw new ExceptionCustom("AssignedCenter not found!");
					//if (!userCenter.Master_Department_CenterId.HasValue) throw new ExceptionCustom("AssignedCenter CenterId not found!");

					//var masterDepCenter = await _repo.MasterDepCenter.GetById(userCenter.Master_Department_CenterId.Value);
					//if (masterDepCenter == null) throw new ExceptionCustom("MasterDepCenter not found!");

					//พนักงาน RM สร้าง ต้องเช็คกิจการสาขาภาคว่าตรงกับผู้จัดการศูนย์ที่ดูแลอยู่หรือป่าว
					if (userRole.Code.ToUpper().StartsWith(RoleCodes.RM))
					{
						var assignmentRM = await _repo.AssignmentRM.GetByUserId(model.CurrentUserId);
						if (assignmentRM == null) throw new ExceptionCustom($"ไม่พบข้อมูล AssignmentRM!");
						//if (assignmentRM.Assignment.User?.Master_Department_Center != null)
						//{
						//	if (assignmentRM.Assignment.User.Master_Department_Center.Master_Department_BranchId != masterDepCenter.Master_Department_BranchId)
						//	{
						//		throw new ExceptionCustom("assignedCenter not correct!");
						//	}
						//}
					}

					statusSaleId = StatusSaleModel.WaitAssign;
					assCenterUserId = user.Id;
					assCenterUserName = userCenter.FullName;
					master_Department_BranchId = userCenter.Master_Department_BranchId;
					provinceId = userCenter.ProvinceId;
					branchId = userCenter.BranchId;
				}

				if (userRole.Code.ToUpper().StartsWith(RoleCodes.BRANCH))
				{
					statusSaleId = StatusSaleModel.WaitAssignCenter;
					master_Department_BranchId = user.Master_Department_BranchId;
					provinceId = user.ProvinceId;
					branchId = user.BranchId;
				}

				if (userRole.Code.ToUpper().StartsWith(RoleCodes.LOAN))
				{
					statusSaleId = StatusSaleModel.WaitVerifyBranch;
				}

				var saleData = new SaleCustom()
				{
					CurrentUserId = model.CurrentUserId,
					CreateBy = model.CurrentUserId,
					CreateDate = _dateNow,
					UpdateBy = model.CurrentUserId,
					UpdateDate = _dateNow,
					CustomerId = customer.Id,
					StatusSaleId = statusSaleId,
					Master_Department_BranchId = master_Department_BranchId,
					ProvinceId = provinceId,
					BranchId = branchId,
					AssCenterUserId = assCenterUserId,
					AssCenterUserName = assCenterUserName,
					AssUserId = assUserId,
					AssUserName = assUserName
				};
				var sale = await _repo.Sales.Create(saleData);

				if (branchId.HasValue)
				{
					await _repo.AssignmentCenter.UpdateCurrentNumber(branchId.Value);
				}

				//**************** Create AssignmentSale ตอน อนุมัติ RM หรือตอน ผู้จัดการศูนย์ Assign ****************

				_transaction.Commit();

				return _mapper.Map<CustomerCustom>(customer);
			}
		}

		public async Task<CustomerCustom> Update(CustomerCustom model)
		{
			string strJson = JsonSerializer.Serialize<CustomerCustom>(model);

			using (var _transaction = _repo.BeginTransaction())
			{
				var _dateNow = DateTime.Now;

				var customer = await _repo.Context.Customers.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (customer != null)
				{
					string? master_ContactChannelName = null;
					string? master_BusinessTypeName = null;
					string? master_BusinessSizeName = null;
					string? master_ISICCodeName = null;
					string? master_YieldName = null;
					string? master_ChainName = null;
					string? master_LoanTypeName = null;
					string? provinceName = null;
					string? amphurName = null;
					string? tambolName = null;

					if (model.Master_ContactChannelId.HasValue)
					{
						master_ContactChannelName = await _repo.MasterContactChannel.GetNameById(model.Master_ContactChannelId.Value);
					}
					if (model.Master_BusinessTypeId.HasValue)
					{
						master_BusinessTypeName = await _repo.MasterBusinessType.GetNameById(model.Master_BusinessTypeId.Value);
					}
					if (model.Master_BusinessSizeId.HasValue)
					{
						master_BusinessSizeName = await _repo.MasterBusinessSize.GetNameById(model.Master_BusinessSizeId.Value);
					}
					if (model.Master_ISICCodeId.HasValue)
					{
						master_ISICCodeName = await _repo.MasterISICCode.GetNameById(model.Master_ISICCodeId.Value);
					}
					if (model.Master_YieldId.HasValue)
					{
						master_YieldName = await _repo.MasterYield.GetNameById(model.Master_YieldId.Value);
					}
					if (model.Master_ChainId.HasValue)
					{
						master_ChainName = await _repo.MasterChain.GetNameById(model.Master_ChainId.Value);
					}
					if (model.Master_LoanTypeId.HasValue)
					{
						master_LoanTypeName = await _repo.MasterLoanTypes.GetNameById(model.Master_LoanTypeId.Value);
					}
					if (model.ProvinceId.HasValue)
					{
						provinceName = await _repo.Thailand.GetProvinceNameByid(model.ProvinceId.Value);
					}
					if (model.AmphurId.HasValue)
					{
						amphurName = await _repo.Thailand.GetAmphurNameByid(model.AmphurId.Value);
					}
					if (model.TambolId.HasValue)
					{
						tambolName = await _repo.Thailand.GetTambolNameByid(model.TambolId.Value);
					}

					var userRole = await _repo.User.GetRoleByUserId(model.CurrentUserId);
					if (userRole == null) throw new ExceptionCustom("currentUserId not map role.");

					customer.UpdateDate = _dateNow;
					customer.UpdateBy = model.CurrentUserId;
					customer.InsertRoleCode = userRole.Code;
					customer.DateContact = model.DateContact;
					customer.Master_ContactChannelId = model.Master_ContactChannelId;
					customer.Master_ContactChannelName = master_ContactChannelName;
					customer.BranchName = model.BranchName;
					customer.ProvincialOffice = model.ProvincialOffice;
					customer.EmployeeName = model.EmployeeName;
					customer.EmployeeId = model.EmployeeId;
					customer.ContactName = model.ContactName;
					customer.ContactTel = model.ContactTel;
					customer.CompanyName = model.CompanyName;
					//customer.JuristicPersonRegNumber = model.JuristicPersonRegNumber; //ไม่ต้อง update กรณีแก้ไข
					customer.Master_BusinessTypeId = model.Master_BusinessTypeId;
					customer.Master_BusinessTypeName = master_BusinessTypeName;
					customer.Master_BusinessSizeId = model.Master_BusinessSizeId;
					customer.Master_BusinessSizeName = master_BusinessSizeName;
					customer.Master_ISICCodeId = model.Master_ISICCodeId;
					customer.Master_ISICCodeName = master_ISICCodeName;
					customer.Master_YieldId = model.Master_YieldId;
					customer.Master_YieldName = master_YieldName;
					customer.Master_ChainId = model.Master_ChainId;
					customer.Master_ChainName = master_ChainName;
					customer.Master_LoanTypeId = model.Master_LoanTypeId;
					customer.Master_LoanTypeName = master_LoanTypeName;
					customer.CompanyEmail = model.CompanyEmail;
					customer.CompanyTel = model.CompanyTel;
					customer.ParentCompanyGroup = model.ParentCompanyGroup;
					customer.HouseNo = model.HouseNo;
					customer.VillageNo = model.VillageNo;
					customer.ProvinceId = model.ProvinceId;
					customer.ProvinceName = provinceName;
					customer.AmphurId = model.AmphurId;
					customer.AmphurName = amphurName;
					customer.TambolId = model.TambolId;
					customer.TambolName = tambolName;
					customer.ZipCode = model.ZipCode;
					customer.ShareholderMeetDay = model.ShareholderMeetDay;
					customer.RegisteredCapital = model.RegisteredCapital;
					customer.CreditScore = model.CreditScore;
					customer.FiscalYear = model.FiscalYear;
					customer.StatementDate = model.StatementDate;
					customer.TradeAccReceivable = model.TradeAccReceivable;
					customer.TradeAccRecProceedsNet = model.TradeAccRecProceedsNet;
					customer.Inventories = model.Inventories;
					customer.LoansShort = model.LoansShort;
					customer.TotalCurrentAssets = model.TotalCurrentAssets;
					customer.LoansLong = model.LoansLong;
					customer.LandBuildingEquipment = model.LandBuildingEquipment;
					customer.TotalNotCurrentAssets = model.TotalNotCurrentAssets;
					customer.AssetsTotal = model.AssetsTotal;
					customer.TradeAccPay = model.TradeAccPay;
					customer.TradeAccPayLoansShot = model.TradeAccPayLoansShot;
					customer.TradeAccPayTotalCurrentLia = model.TradeAccPayTotalCurrentLia;
					customer.TradeAccPayLoansLong = model.TradeAccPayLoansLong;
					customer.TradeAccPayTotalNotCurrentLia = model.TradeAccPayTotalNotCurrentLia;
					customer.TradeAccPayForLoansShot = model.TradeAccPayForLoansShot;
					customer.TradeAccPayTotalLiabilitie = model.TradeAccPayTotalLiabilitie;
					customer.RegisterCapitalOrdinary = model.RegisterCapitalOrdinary;
					customer.RegisterCapitalPaid = model.RegisterCapitalPaid;
					customer.ProfitLossAccumulate = model.ProfitLossAccumulate;
					customer.TotalShareholders = model.TotalShareholders;
					customer.TotalLiabilitieShareholders = model.TotalLiabilitieShareholders;
					customer.TotalIncome = model.TotalIncome;
					customer.CostSales = model.CostSales;
					customer.GrossProfit = model.GrossProfit;
					customer.OperatingExpenses = model.OperatingExpenses;
					customer.ProfitLossBeforeDepExp = model.ProfitLossBeforeDepExp;
					customer.ProfitLossBeforeInterestTax = model.ProfitLossBeforeInterestTax;
					customer.NetProfitLoss = model.NetProfitLoss;
					customer.InterestLoan = model.InterestLoan;
					customer.InterestLoanSpecify = model.InterestLoanSpecify;
					customer.InterestObjectiveLoan = model.InterestObjectiveLoan;
					customer.InterestCreditLimit = model.InterestCreditLimit;
					customer.InterestNote = model.InterestNote;
					_db.Update(customer);
					await _db.SaveAsync();

					//Update Status To Delete All
					var customer_Committees = _repo.Context.Customer_Committees.Where(x => x.CustomerId == model.Id).ToList();
					if (customer_Committees.Count > 0)
					{
						foreach (var committ_item in customer_Committees)
						{
							committ_item.Status = StatusModel.Delete;
						}
						await _db.SaveAsync();
					}
					var customer_Shareholders = _repo.Context.Customer_Shareholders.Where(x => x.CustomerId == model.Id).ToList();
					if (customer_Shareholders.Count > 0)
					{
						foreach (var sharehold_item in customer_Shareholders)
						{
							sharehold_item.Status = StatusModel.Delete;
						}
						await _db.SaveAsync();
					}

					int indexCommittee = 1;
					foreach (var item in model.Customer_Committees ?? new())
					{
						var customerCommittee = await _repo.Context.Customer_Committees
															   .FirstOrDefaultAsync(x => x.CustomerId == model.Id && x.Id == item.Id);
						if (item.Status == StatusModel.Active)
						{
							int CRUD = CRUDModel.Update;

							if (customerCommittee == null)
							{
								CRUD = CRUDModel.Create;
								customerCommittee = new();
							}
							customerCommittee.Status = item.Status;
							customerCommittee.SequenceNo = indexCommittee++;
							customerCommittee.CustomerId = customer.Id;
							customerCommittee.Name = item.Name;

							if (CRUD == CRUDModel.Create)
							{
								await _db.InsterAsync(customerCommittee);
							}
							else
							{
								_db.Update(customerCommittee);
							}
							await _db.SaveAsync();
						}
					}

					int indexShareholder = 1;
					foreach (var item in model.Customer_Shareholders ?? new())
					{
						var customerShareholder = await _repo.Context.Customer_Shareholders
															   .FirstOrDefaultAsync(x => x.CustomerId == model.Id && x.Id == item.Id);

						if (item.Status == StatusModel.Active)
						{
							int CRUD = CRUDModel.Update;

							if (customerShareholder == null)
							{
								CRUD = CRUDModel.Create;
								customerShareholder = new();
							}

							customerShareholder.Status = item.Status;
							customerShareholder.SequenceNo = indexShareholder++;
							customerShareholder.CustomerId = customer.Id;
							customerShareholder.Name = item.Name;
							customerShareholder.Nationality = item.Nationality;
							customerShareholder.Proportion = item.Proportion;
							customerShareholder.NumberShareholder = item.NumberShareholder;
							customerShareholder.TotalShareValue = item.TotalShareValue;

							if (CRUD == CRUDModel.Create)
							{
								await _db.InsterAsync(customerShareholder);
							}
							else
							{
								_db.Update(customerShareholder);
							}
							await _db.SaveAsync();
						}
					}

					var sale = new SaleCustom()
					{
						CurrentUserId = model.CurrentUserId,
						CreateBy = model.CurrentUserId,
						CreateDate = _dateNow,
						UpdateBy = model.CurrentUserId,
						UpdateDate = _dateNow,
						CustomerId = customer.Id,
						StatusSaleId = StatusSaleModel.NotStatus
					};

					var sales = await _repo.Context.Sales.Where(x => x.CustomerId == customer.Id).FirstOrDefaultAsync();
					if (sales == null)
					{
						await _repo.Sales.Create(sale);
					}
					else
					{
						sale.Id = sales.Id;
						await _repo.Sales.Update(sale);
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

			var querySales = await _repo.Context.Sales.Where(x => x.Status != StatusModel.Delete && x.CustomerId == id).FirstOrDefaultAsync();
			if (querySales != null)
			{
				querySales.UpdateDate = DateTime.Now;
				querySales.UpdateBy = model.userid;
				querySales.Status = StatusModel.Delete;
				_db.Update(querySales);
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
				.Include(x => x.Customer_Committees.Where(s => s.Status == StatusModel.Active).OrderBy(o => o.SequenceNo))
				.Include(x => x.Customer_Shareholders.Where(s => s.Status == StatusModel.Active).OrderBy(o => o.SequenceNo))
				.Include(x => x.Sales.Where(s => s.Status != StatusModel.Delete).OrderBy(o => o.CreateDate))
				.Where(x => x.Id == id).FirstOrDefaultAsync();
			return _mapper.Map<CustomerCustom>(query);
		}

		public async Task<PaginationView<List<CustomerCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Customers.Where(x => x.Status != StatusModel.Delete)
												 .OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (!String.IsNullOrEmpty(model.cif))
				query = query.Where(x => x.CIF != null && x.CIF.Contains(model.cif));

			if (!String.IsNullOrEmpty(model.searchtxt))
				query = query.Where(x => x.CompanyName != null && x.CompanyName.Contains(model.searchtxt)
				|| x.JuristicPersonRegNumber != null && x.JuristicPersonRegNumber.Contains(model.searchtxt));

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<CustomerCustom>>()
			{
				Items = _mapper.Map<List<CustomerCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task<string?> GetCompanyNameById(Guid id)
		{
			var companyName = await _repo.Context.Customers.Where(x => x.Id == id).Select(x => x.CompanyName).FirstOrDefaultAsync();
			return companyName;
		}

	}
}

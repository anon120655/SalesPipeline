using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
//using System.Text.Json;

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

		public async Task<CustomerCustom> Validate(CustomerCustom model, bool isThrow = true, bool? isSetMaster = false, bool? isFileUpload = false)
		{
			string errorMessage = string.Empty;
			model.IsValidate = true;
			model.IsSelectVersion = true;
			model.IsKeep = false;
			if (model.ValidateError == null) model.ValidateError = new();

			string? juristicPersonRegNumber = model.JuristicPersonRegNumber?.Trim();

			if (juristicPersonRegNumber == null || juristicPersonRegNumber.Length != 13)
			{
				errorMessage = $"เลขทะเบียนนิติบุคคลไม่ถูกต้อง";
				model.IsValidate = false;
				model.IsSelectVersion = false;
				model.ValidateError.Add(errorMessage);
				if (isThrow) throw new ExceptionCustom(errorMessage);
			}

			if (model.IsRePurpose != true)
			{
                //ปิดไว้ชั่วคราวทดสอบ performance test
                //var customer = await _repo.Context.Customers.Where(x => x.JuristicPersonRegNumber == juristicPersonRegNumber).FirstOrDefaultAsync();
                //if (customer != null)
                //{
                //	model.Id = customer.Id;
                //	errorMessage = $"มีเลขทะเบียนนิติบุคคล {customer.JuristicPersonRegNumber} ในระบบแล้ว";
                //	model.IsValidate = false;
                //	model.ValidateError.Add(errorMessage);
                //	if (isThrow) throw new ExceptionCustom(errorMessage);

                //	if (juristicPersonRegNumber != null)
                //	{
                //		var juristicNumber = await VerifyByNumber(juristicPersonRegNumber, model.CurrentUserId);
                //		if (juristicNumber.Code == "proceed")
                //		{
                //			errorMessage = juristicNumber.Message ?? string.Empty;
                //			model.IsValidate = false;
                //			model.IsSelectVersion = false;
                //			model.ValidateError.Add(errorMessage);
                //			if (isThrow) throw new ExceptionCustom(errorMessage);
                //		}
                //	}
                //}
            }

            if (model.DateContact.HasValue)
			{
				if (model.DateContact.Value.Date > DateTime.Now.Date)
				{
					errorMessage = $"วันที่เข้ามาติดต่อ ต้องไม่มากกว่าวันที่ปัจจุบัน";
					model.IsValidate = false;
					model.IsSelectVersion = false;
					model.ValidateError.Add(errorMessage);
				}
			}

			if (string.IsNullOrEmpty(model.CompanyName))
			{
				errorMessage = $"ระบุ ชื่อบริษัท";
				model.IsValidate = false;
				model.IsSelectVersion = false;
				model.ValidateError.Add(errorMessage);
				if (isThrow) throw new ExceptionCustom(errorMessage);
			}

			if (string.IsNullOrEmpty(model.HouseNo))
			{
				errorMessage = $"ระบุ บ้านเลขที่";
				model.IsValidate = false;
				model.IsSelectVersion = false;
				model.ValidateError.Add(errorMessage);
				if (isThrow) throw new ExceptionCustom(errorMessage);
			}

			string[] prefixAmphurToRemove = { "กิ่ง", "เขต" };

			if (isSetMaster == true)
			{
				if (model.Branch_RegionId.HasValue)
				{
					model.Branch_RegionName = await _repo.MasterBranchReg.GetNameById(model.Branch_RegionId.Value);
				}
				if (model.Master_ContactChannelId.HasValue)
				{
					model.Master_ContactChannelName = await _repo.MasterContactChannel.GetNameById(model.Master_ContactChannelId.Value);
				}
				if (model.Master_BusinessTypeId.HasValue)
				{
					model.Master_BusinessTypeName = await _repo.MasterBusinessType.GetNameById(model.Master_BusinessTypeId.Value);
				}
				else if (!model.Master_BusinessTypeId.HasValue && !string.IsNullOrEmpty(model.Master_BusinessTypeName))
				{
					model.Master_BusinessTypeId = await _repo.MasterBusinessType.GetIdByName(model.Master_BusinessTypeName);
				}

				if (model.Master_BusinessSizeId.HasValue)
				{
					model.Master_BusinessSizeName = await _repo.MasterBusinessSize.GetNameById(model.Master_BusinessSizeId.Value);
				}
				else if (!model.Master_BusinessSizeId.HasValue && !string.IsNullOrEmpty(model.Master_BusinessSizeName))
				{
					model.Master_BusinessSizeId = await _repo.MasterBusinessSize.GetIdByName(model.Master_BusinessSizeName);
				}

				if (model.Master_ISICCodeId.HasValue)
				{
					model.Master_ISICCodeName = await _repo.MasterISICCode.GetNameById(model.Master_ISICCodeId.Value);
				}

				if (model.Master_TSICId.HasValue)
				{
					model.Master_TSICName = await _repo.MasterTSIC.GetNameById(model.Master_TSICId.Value);
				}
				else if (!model.Master_TSICId.HasValue && !string.IsNullOrEmpty(model.Master_TSICName))
				{
					model.Master_TSICId = await _repo.MasterTSIC.GetIdByName(model.Master_TSICName);
				}

				if (model.Master_YieldId.HasValue)
				{
					model.Master_YieldName = await _repo.MasterYield.GetNameById(model.Master_YieldId.Value);
				}
				if (model.Master_ChainId.HasValue)
				{
					model.Master_ChainName = await _repo.MasterChain.GetNameById(model.Master_ChainId.Value);
				}
				if (model.Master_LoanTypeId.HasValue)
				{
					model.Master_LoanTypeName = await _repo.MasterLoanTypes.GetNameById(model.Master_LoanTypeId.Value);
				}

				if (model.ProvinceId.HasValue)
				{
					model.ProvinceName = await _repo.Thailand.GetProvinceNameByid(model.ProvinceId.Value);
				}
				else if (!model.ProvinceId.HasValue && !string.IsNullOrEmpty(model.ProvinceName))
				{
					model.ProvinceId = await _repo.Thailand.GetProvinceIdByName(model.ProvinceName);
				}

				if (model.AmphurId.HasValue)
				{
					model.AmphurName = await _repo.Thailand.GetAmphurNameByid(model.AmphurId.Value);
				}
				else if (!model.AmphurId.HasValue && !string.IsNullOrEmpty(model.AmphurName))
				{
					if (GeneralUtils.HasPrefix(model.AmphurName, prefixAmphurToRemove))
					{
						model.AmphurName = GeneralUtils.RemovePrefixes(model.AmphurName, prefixAmphurToRemove);
					}
					model.AmphurId = await _repo.Thailand.GetAmphurIdByName(model.AmphurName);
				}

				if (model.TambolId.HasValue)
				{
					model.TambolName = await _repo.Thailand.GetTambolNameByid(model.TambolId.Value);
				}
				else if (!model.TambolId.HasValue && !string.IsNullOrEmpty(model.TambolName))
				{
					model.TambolId = await _repo.Thailand.GetTambolIdByName(model.TambolName);
				}

			}

			if (!model.ProvinceId.HasValue)
			{
				errorMessage = $"ระบุ จังหวัด";
				model.IsValidate = false;
				model.IsSelectVersion = false;
				model.ValidateError.Add(errorMessage);
				if (isThrow) throw new ExceptionCustom(errorMessage);
			}

			if (!model.AmphurId.HasValue)
			{
				errorMessage = $"ระบุ เขต/อำเภอ";
				model.IsValidate = false;
				model.IsSelectVersion = false;
				model.ValidateError.Add(errorMessage);
				if (isThrow) throw new ExceptionCustom(errorMessage);
			}

			if (!model.TambolId.HasValue)
			{
				errorMessage = $"ระบุ แขวง/ตำบล";
				model.IsValidate = false;
				model.IsSelectVersion = false;
				model.ValidateError.Add(errorMessage);
				if (isThrow) throw new ExceptionCustom(errorMessage);
			}

			return model;
		}

		public async Task<List<CustomerCustom>> ValidateUpload(List<CustomerCustom> model)
		{
			for (int i = 0; i < model.Count; i++)
			{
				model[i] = await Validate(model[i], false, true, true);
			}
			return model;
		}

		public async Task<ResponseDefaultModel> VerifyByNumber(string juristicNumber, int? userid = null)
		{
			string code = "pass";
			string message = "ผ่าน";
			string id = "";

			juristicNumber = juristicNumber.Trim();

			UserCustom? user = null;
			if (userid.HasValue)
			{
				user = await _repo.User.GetById(userid.Value);
			}

			if (juristicNumber == null || juristicNumber.Length != 13)
			{
				code = "error";
				message = "เลขทะเบียนนิติบุคคลไม่ถูกต้อง";
				return new()
				{
					Code = code,
					Message = message,
					ID = id
				};
			}

			var customers = await _repo.Context.Customers.Include(x => x.Sales).FirstOrDefaultAsync(x => x.JuristicPersonRegNumber == juristicNumber);
			if (customers != null)
			{
				bool isProceed = false;

				if (customers.Sales != null)
				{
					foreach (var item_sale in customers.Sales)
					{
						if (user != null && user.Role != null && !user.Role.IsAssignCenter)
						{
							if (user.Role.Code == RoleCodes.RM && item_sale.AssUserId != user.Id)
							{
								code = "proceed";
								message = "ลูกค้าท่านนี้อยู่ในระบบแล้ว <br/>ท่านไม่มีสิทธิ์ดำเนินการเนื่องจากอยู่ในเขตรับผิดชอบอื่น";
								isProceed = true;
								break;
							}
						}
						if (item_sale.StatusSaleId >= StatusSaleModel.WaitContact)
						{
							//if (!isProceed)
							//{
							//	code = "proceed";
							//	message = "ลูกค้าท่านนี้อยู่ระหว่างการดำเนินการ <br/>ไม่สามารถดำเนินการต่อได้";
							//	isProceed = true;
							//	break;
							//}
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
			using (var _transaction = _repo.BeginTransaction())
			{
                //นำออกชั่วคราวเพื่อทดสอบ JMeter
                //await Validate(model);

                string? branchName = null;
				string? master_ContactChannelName = null;
				string? master_BusinessTypeName = null;
				string? master_BusinessSizeName = null;
				string? master_ISICCodeName = null;
				string? master_TSICName = null;
				string? master_YieldName = null;
				string? master_ChainName = null;
				string? master_LoanTypeName = null;
                //string? provinceName = null;
                //string? amphurName = null;
                //string? tambolName = null;


                //นำออกชั่วคราวเพื่อทดสอบ JMeter
                //if (model.ContactProvinceId.HasValue)
                //{
                //	model.ContactProvinceName = await _repo.Thailand.GetProvinceNameByid(model.ContactProvinceId.Value);
                //}

                //if (model.BranchId.HasValue && model.BranchId > 0)
                //{
                //	branchName = await _repo.MasterBranch.GetNameById(model.BranchId.Value);
                //}

                //if (model.Master_ContactChannelId.HasValue)
                //{
                //	master_ContactChannelName = await _repo.MasterContactChannel.GetNameById(model.Master_ContactChannelId.Value);
                //}

                //if (model.Master_BusinessTypeId.HasValue)
                //{
                //	master_BusinessTypeName = await _repo.MasterBusinessType.GetNameById(model.Master_BusinessTypeId.Value);
                //}
                //else if (!model.Master_BusinessTypeId.HasValue && !string.IsNullOrEmpty(model.Master_BusinessTypeName))
                //{
                //	model.Master_BusinessTypeId = await _repo.MasterBusinessType.GetIdByName(model.Master_BusinessTypeName);
                //}

                //if (model.Master_BusinessSizeId.HasValue)
                //{
                //	master_BusinessSizeName = await _repo.MasterBusinessSize.GetNameById(model.Master_BusinessSizeId.Value);
                //}
                //else if (!model.Master_BusinessSizeId.HasValue && !string.IsNullOrEmpty(model.Master_BusinessSizeName))
                //{
                //	model.Master_BusinessSizeId = await _repo.MasterBusinessSize.GetIdByName(model.Master_BusinessSizeName);
                //}

                //if (model.Master_ISICCodeId.HasValue)
                //{
                //	master_ISICCodeName = await _repo.MasterISICCode.GetNameById(model.Master_ISICCodeId.Value);
                //}

                //if (model.Master_TSICId.HasValue)
                //{
                //	master_TSICName = await _repo.MasterTSIC.GetNameById(model.Master_TSICId.Value);
                //}
                //else if (!model.Master_TSICId.HasValue && !string.IsNullOrEmpty(model.Master_TSICName))
                //{
                //	model.Master_TSICId = await _repo.MasterBusinessSize.GetIdByName(model.Master_TSICName);
                //}

                //if (model.Master_YieldId.HasValue)
                //{
                //	master_YieldName = await _repo.MasterYield.GetNameById(model.Master_YieldId.Value);
                //}
                //if (model.Master_ChainId.HasValue)
                //{
                //	master_ChainName = await _repo.MasterChain.GetNameById(model.Master_ChainId.Value);
                //}
                //if (model.Master_LoanTypeId.HasValue)
                //{
                //	master_LoanTypeName = await _repo.MasterLoanTypes.GetNameById(model.Master_LoanTypeId.Value);
                //}

                //if (model.ProvinceId.HasValue)
                //{
                //	model.ProvinceName = await _repo.Thailand.GetProvinceNameByid(model.ProvinceId.Value);
                //}
                //else if (!model.ProvinceId.HasValue && !string.IsNullOrEmpty(model.ProvinceName))
                //{
                //	model.ProvinceId = await _repo.Thailand.GetProvinceIdByName(model.ProvinceName);
                //}

                //if (model.AmphurId.HasValue)
                //{
                //	model.AmphurName = await _repo.Thailand.GetAmphurNameByid(model.AmphurId.Value);
                //}
                //else if (!model.AmphurId.HasValue && !string.IsNullOrEmpty(model.AmphurName))
                //{
                //	model.AmphurId = await _repo.Thailand.GetAmphurIdByName(model.AmphurName);
                //}

                //if (model.TambolId.HasValue)
                //{
                //	model.TambolName = await _repo.Thailand.GetTambolNameByid(model.TambolId.Value);
                //}
                //else if (!model.TambolId.HasValue && !string.IsNullOrEmpty(model.TambolName))
                //{
                //	model.TambolId = await _repo.Thailand.GetTambolIdByName(model.TambolName);
                //}


                var user = await _repo.User.GetById(model.CurrentUserId);
				if (user == null || user.Role == null) throw new ExceptionCustom("currentUserId not found!");

				DateTime _dateNow = DateTime.Now;

				var customer = new Data.Entity.Customer();
				customer.Status = StatusModel.Active;
				customer.CreateDate = _dateNow;
				customer.CreateBy = model.CurrentUserId;
				customer.UpdateDate = _dateNow;
				customer.UpdateBy = model.CurrentUserId;
				customer.InsertRoleCode = user.Role.Code;
				customer.DateContact = model.DateContact;
				customer.Master_ContactChannelId = model.Master_ContactChannelId;
				customer.Master_ContactChannelName = master_ContactChannelName;
				customer.ContactProvinceId = model.ContactProvinceId > 0 ? model.ContactProvinceId.Value : null;
				customer.ContactProvinceName = model.ContactProvinceName;
				customer.BranchId = model.BranchId > 0 ? model.BranchId.Value : null;
				customer.BranchName = branchName;
				customer.ProvincialOffice = model.ProvincialOffice;
				customer.EmployeeName = model.EmployeeName;
				customer.EmployeeId = model.EmployeeId;
				customer.CIF = model.CIF;
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
				customer.Master_TSICId = model.Master_TSICId;
				customer.Master_TSICName = master_TSICName;
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
				customer.Road_Soi_Village = model.Road_Soi_Village;
				customer.ProvinceId = model.ProvinceId > 0 ? model.ProvinceId.Value : null;
				customer.ProvinceName = model.ProvinceName;
				customer.AmphurId = model.AmphurId > 0 ? model.AmphurId.Value : null;
				customer.AmphurName = model.AmphurName;
				customer.TambolId = model.TambolId > 0 ? model.TambolId.Value : null;
				customer.TambolName = model.TambolName;
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
				customer.ProfitLossBeforeIncomeTaxExpense = model.ProfitLossBeforeIncomeTaxExpense;
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
					if (!string.IsNullOrEmpty(item.Name))
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
				}

				int indexShareholder = 1;
				foreach (var item in model.Customer_Shareholders ?? new())
				{
					if (!string.IsNullOrEmpty(item.Name))
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
				}

				int statusSaleId = StatusSaleModel.WaitApprove;
				Guid? master_Branch_RegionId = null;
				int? provinceId = null;
				int? assCenterUserId = null;
				string? assCenterUserName = null;
				int? assUserId = null;
				string? assUserName = null;

				if (user.Role.Code != null && user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
				{
					statusSaleId = StatusSaleModel.WaitApprove;
					assUserId = model.CurrentUserId;
					assUserName = user.FullName;
					master_Branch_RegionId = user.Master_Branch_RegionId;
					provinceId = user.ProvinceId;
				}
				else if (user.Role.IsAssignRM)
				{
					statusSaleId = StatusSaleModel.WaitAssign;
					assCenterUserId = user.Id;
					assCenterUserName = user.FullName;
					master_Branch_RegionId = user.Master_Branch_RegionId;
					provinceId = user.ProvinceId;
				}
				else if (user.Role.IsAssignCenter)
				{
					statusSaleId = StatusSaleModel.WaitAssignCenter;
				}

				var saleData = new SaleCustom()
				{
					CurrentUserId = model.CurrentUserId,
					CreateBy = model.CurrentUserId,
					CreateDate = _dateNow,
					UpdateBy = model.CurrentUserId,
					UpdateDate = _dateNow,
					CustomerId = customer.Id,
					CIF = customer.CIF,
					StatusSaleId = statusSaleId,
					Master_Branch_RegionId = master_Branch_RegionId,
					ProvinceId = model.ProvinceId,
					BranchId = model.BranchId, //สาขาจะใช้เฉพาะลูกค้า พนักงานจะไม่มีสาขา เลือกสาขาจากหน้าฟอร์มลูกค้า
					AssCenterUserId = assCenterUserId,
					AssCenterUserName = assCenterUserName,
					AssUserId = assUserId,
					AssUserName = assUserName,
					IsRePurpose = model.IsRePurpose
				};
				var sale = await _repo.Sales.Create(saleData);

				await _repo.Sales.CreateInfo(new()
				{
					SaleId = sale.Id,
					FullName = model.ContactName,
					Tel = model.ContactTel,
					Createdfrom = 1
				});

				if (assCenterUserId.HasValue)
				{
					await _repo.AssignmentCenter.UpdateCurrentNumber(assCenterUserId.Value);
				}

				//**************** Create AssignmentSale ตอน อนุมัติ RM หรือตอน ผู้จัดการศูนย์ Assign ****************

				_transaction.Commit();

				return _mapper.Map<CustomerCustom>(customer);
			}
		}

		public async Task<CustomerCustom> Update(CustomerCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				await CreateHistory(model);

				var _dateNow = DateTime.Now;

				var customer = await _repo.Context.Customers.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (customer != null)
				{
					string? branchName = null;
					string? master_ContactChannelName = null;
					string? master_BusinessTypeName = null;
					string? master_BusinessSizeName = null;
					string? master_ISICCodeName = null;
					string? master_TSICName = null;
					string? master_YieldName = null;
					string? master_ChainName = null;
					string? master_LoanTypeName = null;
					//string? provinceName = null;
					//string? amphurName = null;
					//string? tambolName = null;

					if (model.ContactProvinceId.HasValue)
					{
						model.ContactProvinceName = await _repo.Thailand.GetProvinceNameByid(model.ContactProvinceId.Value);
					}

					if (model.BranchId.HasValue && model.BranchId.Value > 0)
					{
						branchName = await _repo.MasterBranch.GetNameById(model.BranchId.Value);
					}

					if (model.Master_ContactChannelId.HasValue)
					{
						master_ContactChannelName = await _repo.MasterContactChannel.GetNameById(model.Master_ContactChannelId.Value);
					}
					if (model.Master_BusinessTypeId.HasValue)
					{
						master_BusinessTypeName = await _repo.MasterBusinessType.GetNameById(model.Master_BusinessTypeId.Value);
					}
					else if (!model.Master_BusinessTypeId.HasValue && !string.IsNullOrEmpty(model.Master_BusinessTypeName))
					{
						model.Master_BusinessTypeId = await _repo.MasterBusinessType.GetIdByName(model.Master_BusinessTypeName);
					}

					if (model.Master_BusinessSizeId.HasValue)
					{
						master_BusinessSizeName = await _repo.MasterBusinessSize.GetNameById(model.Master_BusinessSizeId.Value);
					}
					else if (!model.Master_BusinessSizeId.HasValue && !string.IsNullOrEmpty(model.Master_BusinessSizeName))
					{
						model.Master_BusinessSizeId = await _repo.MasterBusinessSize.GetIdByName(model.Master_BusinessSizeName);
					}

					if (model.Master_ISICCodeId.HasValue)
					{
						master_ISICCodeName = await _repo.MasterISICCode.GetNameById(model.Master_ISICCodeId.Value);
					}
					if (model.Master_TSICId.HasValue)
					{
						master_TSICName = await _repo.MasterTSIC.GetNameById(model.Master_TSICId.Value);
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
						model.ProvinceName = await _repo.Thailand.GetProvinceNameByid(model.ProvinceId.Value);
					}
					else if (!model.ProvinceId.HasValue && !string.IsNullOrEmpty(model.ProvinceName))
					{
						model.ProvinceId = await _repo.Thailand.GetProvinceIdByName(model.ProvinceName);
					}

					if (model.AmphurId.HasValue)
					{
						model.AmphurName = await _repo.Thailand.GetAmphurNameByid(model.AmphurId.Value);
					}
					else if (!model.AmphurId.HasValue && !string.IsNullOrEmpty(model.AmphurName))
					{
						model.AmphurId = await _repo.Thailand.GetAmphurIdByName(model.AmphurName);
					}

					if (model.TambolId.HasValue)
					{
						model.TambolName = await _repo.Thailand.GetTambolNameByid(model.TambolId.Value);
					}
					else if (!model.TambolId.HasValue && !string.IsNullOrEmpty(model.TambolName))
					{
						model.TambolId = await _repo.Thailand.GetTambolIdByName(model.TambolName);
					}

					var user = await _repo.User.GetById(model.CurrentUserId);
					if (user == null) throw new ExceptionCustom("currentUserId not found.");
					//var userRole = await _repo.User.GetRoleByUserId(model.CurrentUserId);
					//if (userRole == null) throw new ExceptionCustom("currentUserId not map role.");

					customer.UpdateDate = _dateNow;
					customer.UpdateBy = model.CurrentUserId;
					//customer.InsertRoleCode = userRole.Code;
					customer.DateContact = model.DateContact;
					customer.Master_ContactChannelId = model.Master_ContactChannelId;
					customer.Master_ContactChannelName = master_ContactChannelName;
					customer.ContactProvinceId = model.ContactProvinceId > 0 ? model.ContactProvinceId.Value : null;
					customer.ContactProvinceName = model.ContactProvinceName;
					customer.BranchId = model.BranchId > 0 ? model.BranchId.Value : null;
					customer.BranchName = branchName;
					customer.ProvincialOffice = model.ProvincialOffice;
					customer.EmployeeName = model.EmployeeName;
					customer.EmployeeId = model.EmployeeId;
					customer.ContactName = model.ContactName;
					customer.ContactTel = model.ContactTel;
					customer.CIF = model.CIF;
					customer.CompanyName = model.CompanyName;

					//แก้ไขเลขนิติบุคคลได้ เฉพาะระดับ 10,11,12
					if (user.LevelId >= 10 && user.LevelId <= 12)
					{
						if (customer.JuristicPersonRegNumber != model.JuristicPersonRegNumber)
						{
							if (_repo.Context.Customers.Any(x => x.Id != model.Id && x.JuristicPersonRegNumber == model.JuristicPersonRegNumber))
								throw new ExceptionCustom($"มีเลขทะเบียนนิติบุคคล {customer.JuristicPersonRegNumber} ในระบบแล้ว");

							customer.JuristicPersonRegNumber = model.JuristicPersonRegNumber;
						}
					}

					customer.Master_BusinessTypeId = model.Master_BusinessTypeId;
					customer.Master_BusinessTypeName = master_BusinessTypeName;
					customer.Master_BusinessSizeId = model.Master_BusinessSizeId;
					customer.Master_BusinessSizeName = master_BusinessSizeName;
					customer.Master_ISICCodeId = model.Master_ISICCodeId;
					customer.Master_ISICCodeName = master_ISICCodeName;
					customer.Master_TSICId = model.Master_TSICId;
					customer.Master_TSICName = master_TSICName;
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
					customer.ProvinceId = model.ProvinceId > 0 ? model.ProvinceId.Value : null;
					customer.ProvinceName = model.ProvinceName;
					customer.AmphurId = model.AmphurId > 0 ? model.AmphurId.Value : null;
					customer.AmphurName = model.AmphurName;
					customer.TambolId = model.TambolId > 0 ? model.TambolId.Value : null;
					customer.TambolName = model.TambolName;
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
					customer.ProfitLossBeforeIncomeTaxExpense = model.ProfitLossBeforeIncomeTaxExpense;
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
						//foreach (var committ_item in customer_Committees)
						//{
						//	committ_item.Status = StatusModel.Delete;
						//}
						_db.DeleteRange(customer_Committees);
						await _db.SaveAsync();
					}
					var customer_Shareholders = _repo.Context.Customer_Shareholders.Where(x => x.CustomerId == model.Id).ToList();
					if (customer_Shareholders.Count > 0)
					{
						//foreach (var sharehold_item in customer_Shareholders)
						//{
						//	sharehold_item.Status = StatusModel.Delete;
						//}
						_db.DeleteRange(customer_Shareholders);
						await _db.SaveAsync();
					}

					int indexCommittee = 1;
					foreach (var item in model.Customer_Committees ?? new())
					{
						if (!string.IsNullOrEmpty(item.Name))
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
					}

					int indexShareholder = 1;
					foreach (var item in model.Customer_Shareholders ?? new())
					{
						if (!string.IsNullOrEmpty(item.Name))
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
					}

					var sale = new SaleCustom()
					{
						CurrentUserId = model.CurrentUserId,
						CreateBy = model.CurrentUserId,
						CreateDate = _dateNow,
						UpdateBy = model.CurrentUserId,
						UpdateDate = _dateNow,
						CustomerId = customer.Id,
						ProvinceId = model.ProvinceId,
						BranchId = model.BranchId,
						CIF = model.CIF,
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

					var sale_Contact_Info = await _repo.Context.Sale_Contact_Infos.Where(x => x.SaleId == sale.Id && x.Createdfrom == 1)
						.FirstOrDefaultAsync();
					if (sale_Contact_Info != null)
					{
						await _repo.Sales.UpdateInfo(new()
						{
							Id = sale_Contact_Info.Id,
							FullName = model.ContactName,
							Tel = model.ContactTel,
							Createdfrom = 1
						});
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

			//if (!String.IsNullOrEmpty(model.cif))
			//	query = query.Where(x => x.CIF != null && x.CIF.Contains(model.cif));

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
			var companyName = await _repo.Context.Customers.AsNoTracking().Where(x => x.Id == id).Select(x => x.CompanyName).FirstOrDefaultAsync();
			return companyName;
		}

		public async Task CreateHistory(CustomerCustom model)
		{
			DateTime _dateNow = DateTime.Now;

			string? fullNameUser = await _repo.User.GetFullNameById(model.CurrentUserId);
			string? originalJson = null;
			string? changeJson = null;

			var customer = await _repo.Context.Customers
				.Include(x => x.Customer_Committees)
				.Include(x => x.Customer_Shareholders)
				.FirstOrDefaultAsync(x => x.Id == model.Id);
			if (customer == null) throw new ExceptionCustom("customer not found.");

			var customerMap = _mapper.Map<CustomerCustom>(customer);
			customerMap.Sales = null;
			originalJson = JsonConvert.SerializeObject(customerMap);

			model.Sales = null;
			changeJson = JsonConvert.SerializeObject(model);

			//originalJson = JsonConvert.SerializeObject(customer, new JsonSerializerSettings
			//{
			//	ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			//});

			var customer_History = new Data.Entity.Customer_History();
			customer_History.Status = StatusModel.Active;
			customer_History.CreateDate = _dateNow;
			customer_History.CreateBy = model.CurrentUserId;
			customer_History.CreateByFullName = fullNameUser;
			customer_History.CustomerId = model.Id;
			customer_History.OriginalJson = originalJson;
			customer_History.ChangeJson = changeJson;
			await _db.InsterAsync(customer_History);
			await _db.SaveAsync();
		}

		public async Task<PaginationView<List<Customer_HistoryCustom>>> GetListHistory(allFilter model)
		{
			var query = _repo.Context.Customer_Histories.Where(x => x.Status != StatusModel.Delete)
												 .OrderByDescending(x => x.CreateDate)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (model.customerid.HasValue)
			{
				query = query.Where(x => x.CustomerId == model.customerid);
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Customer_HistoryCustom>>()
			{
				Items = _mapper.Map<List<Customer_HistoryCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetTopologySuite.Algorithm;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Loggers;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;
using System.Collections.Generic;
using System;
using System.Net.NetworkInformation;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class CustomerController : ControllerBase
	{
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;

		public CustomerController(IRepositoryWrapper repo, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_appSet = appSet.Value;
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[AllowAnonymous]
		[HttpPost("SaveLog")]
		public async Task<IActionResult> SaveLog(RequestResponseLogModel model)
		{
			try
			{
				await _repo.Logger.SaveLog(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ตรวจสอบข้อมูลลูกค้า
		/// </summary>
		//[AllowAnonymous]
		[HttpGet("VerifyByNumber")]
		public async Task<IActionResult> VerifyByNumber([FromQuery] string juristicNumber)
		{
			try
			{
				if (juristicNumber.Length != 13) throw new ExceptionCustom("ระบุข้อมูลไม่ถูกต้อง");

				var data = await _repo.Customer.VerifyByNumber(juristicNumber);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// เพิ่มข้อมูลลูกค้า
		/// </summary>
		[HttpPost("Create")]
		public async Task<IActionResult> Create(CustomerCustom model)
		{
			try
			{
				var data = await _repo.Customer.Create(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// แก้ไขข้อมูลลูกค้า
		/// </summary>
		[HttpPut("Update")]
		public async Task<IActionResult> Update(CustomerCustom model)
		{
			try
			{
				var data = await _repo.Customer.Update(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลลูกค้า ById
		/// </summary>
		[HttpDelete("DeleteById")]
		public async Task<IActionResult> DeleteById([FromQuery] UpdateModel model)
		{
			try
			{
				await _repo.Customer.DeleteById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// แก้ไข Status Only
		/// </summary>
		[HttpPut("UpdateStatusById")]
		public async Task<IActionResult> UpdateStatusById(UpdateModel model)
		{
			try
			{
				await _repo.Customer.UpdateStatusById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลลูกค้า ById
		/// </summary>
		[HttpGet("GetById")]
		public async Task<IActionResult> GetById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.Customer.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลลูกค้าทั้งหมด
		/// </summary>
		//[AllowAnonymous]
		[HttpGet("GetList")]
		public async Task<IActionResult> GetList([FromQuery] CustomerFilter model)
		{
			try
			{
				var response = await _repo.Customer.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpGet("CreateTestData")]
		public async Task<IActionResult> CreateTestData([FromQuery] string check, int number, int provinceId, int amphurId, int tambolId, string rolecode)
		{
			try
			{
				if (check != "$2a$11$GSuWBf34jTzwRsn3pvQnh.BSk6TGAfuYsRSzOn1ZE7CkZrF5EeNKq") throw new ExceptionCustom("not permission");

				var contactChannel = new List<Guid>();
				var businessType = new List<Guid>();
				var businessSize = new List<Guid>();
				var ISICCode = new List<Guid>();
				var Yield = new List<Guid>();
				var Chain = new List<Guid>();

				var MasterContactChannel = await _repo.MasterContactChannel.GetList(new() { pagesize = 100 });
				if (MasterContactChannel != null && MasterContactChannel.Items.Count > 0)
				{
					contactChannel = MasterContactChannel.Items.Select(x => x.Id).ToList();
				}

				var MasterBusinessType = await _repo.MasterBusinessType.GetList(new() { pagesize = 100 });
				if (MasterBusinessType != null && MasterBusinessType.Items.Count > 0)
				{
					businessType = MasterBusinessType.Items.Select(x => x.Id).ToList();
				}

				var MasterBusinessSize = await _repo.MasterBusinessSize.GetList(new() { pagesize = 100 });
				if (MasterBusinessSize != null && MasterBusinessSize.Items.Count > 0)
				{
					businessSize = MasterBusinessSize.Items.Select(x => x.Id).ToList();
				}

				var MasterISICCode = await _repo.MasterISICCode.GetList(new() { pagesize = 100 });
				if (MasterISICCode != null && MasterISICCode.Items.Count > 0)
				{
					ISICCode = MasterISICCode.Items.Select(x => x.Id).ToList();
				}

				var MasterYield = await _repo.MasterYield.GetList(new() { pagesize = 100 });
				if (MasterYield != null && MasterYield.Items.Count > 0)
				{
					Yield = MasterYield.Items.Select(x => x.Id).ToList();
				}

				var MasterChain = await _repo.MasterChain.GetList(new() { pagesize = 100 });
				if (MasterChain != null && MasterChain.Items.Count > 0)
				{
					Chain = MasterChain.Items.Select(x => x.Id).ToList();
				}

				string? employeeName = null;
				int currentUserId = 0;
				int statusSaleId = StatusSaleModel.NotStatus;

				for (int i = 1; i <= number; i++)
				{
					if (rolecode == RoleCodes.RM)
					{
						currentUserId = 13;
						employeeName = $"RM_{provinceId}_{i} ทดสอบ";
					}
					else if (rolecode == RoleCodes.MANAGERCENTER)
					{
						currentUserId = 11;
						employeeName = $"MCenter_{provinceId}_{i} ทดสอบ";
						statusSaleId = StatusSaleModel.WaitAssign;
					}
					else if (rolecode == RoleCodes.LOAN)
					{
						currentUserId = 5;
						employeeName = $"LOAN_{provinceId}_{i} ทดสอบ";
						statusSaleId = StatusSaleModel.WaitAssignCenter;
					}
					else if (rolecode == RoleCodes.BRANCH)
					{
						currentUserId = 9;
						employeeName = $"BRANCH_{provinceId}_{i} ทดสอบ";
						statusSaleId = StatusSaleModel.WaitAssignCenter;
					}

					var random = new Random();
					String juristicPersonRegNumber = random.Next(000000000, 999999999).ToString("D12");
					int contactChannelRandom = random.Next(contactChannel.Count);
					int businessTypeRandom = random.Next(businessType.Count);
					int businessSizeRandom = random.Next(businessSize.Count);
					int ISICCodeRandom = random.Next(ISICCode.Count);
					int YieldRandom = random.Next(Yield.Count);
					int ChainRandom = random.Next(Chain.Count);

					var data = await _repo.Customer.Create(new()
					{
						StatusSaleId = statusSaleId,
						CurrentUserId = currentUserId, //RM
						Status = StatusModel.Active,
						CreateBy = currentUserId,
						UpdateBy = currentUserId,
						DateContact = DateTime.Now.AddDays(i),
						Master_ContactChannelId = contactChannel[contactChannelRandom],
						BranchName = $"สาขา_{i}",
						ProvincialOffice = $"สนจ_{i}",
						EmployeeId = int.Parse($"50000{i}"),
						EmployeeName = employeeName,
						ContactName = $"ผู้ติดต่อ_{i} ทดสอบ",
						ContactTel = $"08{i.ToString("00000000")}",
						CompanyName = $"บริษัท ทดสอบ_{juristicPersonRegNumber.Substring(8)} จำกัด",
						JuristicPersonRegNumber = $"1{juristicPersonRegNumber}",
						Master_BusinessTypeId = businessType[businessTypeRandom],
						Master_BusinessSizeId = businessSize[businessSizeRandom],
						Master_ISICCodeId = ISICCode[ISICCodeRandom],
						Master_YieldId = Yield[YieldRandom],
						Master_ChainId = Chain[ChainRandom],
						CompanyEmail = $"company_{i}@email.com",
						CompanyTel = $"02{i.ToString("0000000")}",
						ParentCompanyGroup = $"บริษัทแม่_{i}",
						HouseNo = $"10/{i}",
						VillageNo = int.Parse(i.ToString()),
						ProvinceId = provinceId,
						AmphurId = amphurId,
						TambolId = tambolId,
						ZipCode = $"3{i.ToString("0000")}",
						ShareholderMeetDay = DateTime.Now,
						RegisteredCapital = "1000000",
						CreditScore = $"5{i.ToString("000")}",
						FiscalYear = $"2567",
						StatementDate = DateTime.Now,
						TradeAccReceivable = $"ลูกหนี้การค้า_{i}",
						TradeAccRecProceedsNet = $"ตั่วเงินรับ-สุทธิ_{i}",
						Inventories = $"1{i.ToString("0000")}",
						LoansShort = 1000000,
						TotalCurrentAssets = 2000000,
						LoansLong = 3000000,
						LandBuildingEquipment = 4000000,
						TotalNotCurrentAssets = 5000000,
						AssetsTotal = 6000000,
						TradeAccPay = $"เจ้าหนี้การค้า_{i}",
						TradeAccPayLoansShot = 10000000,
						TradeAccPayTotalCurrentLia = 20000000,
						TradeAccPayLoansLong = 30000000,
						TradeAccPayTotalNotCurrentLia = 40000000,
						TradeAccPayForLoansShot = 50000000,
						TradeAccPayTotalLiabilitie = 60000000,
						RegisterCapitalOrdinary = 70000000,
						RegisterCapitalPaid = 180000000,
						ProfitLossAccumulate = 90000000,
						TotalShareholders = 10000000,
						TotalLiabilitieShareholders = 20000000,
						TotalIncome = 30000000,
						CostSales = 40000000,
						GrossProfit = 150000000,
						OperatingExpenses = 160000000,
						ProfitLossBeforeDepExp = 70000000,
						ProfitLossBeforeInterestTax = 80000000,
						NetProfitLoss = 90000000,
						InterestLoan = $"สินเชื่อที่สนใจ_{i}",
						InterestLoanSpecify = $"ระบุ_{i}",
						InterestObjectiveLoan = $"จุดประสงค์การกู้_{i}",
						InterestCreditLimit = 1000000000,
						InterestNote = $"หมายเหตุ_{i}",
						Customer_Committees = new() {
							new() { Name = $"คณะกรรมการ_1" },
							new() { Name = $"คณะกรรมการ_2" }
						},
						Customer_Shareholders = new() {
							new() { Name = $"ผู้ถือหุ้น_1" , Nationality="ไทย", Proportion = "40%", NumberShareholder = 40 ,TotalShareValue = 400000 },
							new() { Name = $"ผู้ถือหุ้น_2" , Nationality="ไทย", Proportion = "60%", NumberShareholder = 60 ,TotalShareValue = 600000 }
						}
					});
				}

				await _repo.User.CreateAssignmentRMAll(new());

				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}


	}
}

using Asp.Versioning;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using NetTopologySuite.Index.HPRtree;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Formula.Functions;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class Dashboard : IDashboard
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public Dashboard(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Dash_Status_TotalCustom> GetStatus_TotalById(int userid)
		{
			//var dash_Status_Total = await _repo.Context.Dash_Status_Totals.FirstOrDefaultAsync(x => x.UserId == userid);

			//if (dash_Status_Total == null || (dash_Status_Total != null && dash_Status_Total.IsUpdate))
			//{
			//	await UpdateStatus_TotalById(userid);
			//	dash_Status_Total = await _repo.Context.Dash_Status_Totals.FirstOrDefaultAsync(x => x.UserId == userid);
			//}

			//return _mapper.Map<Dash_Status_TotalCustom>(dash_Status_Total);

			var dash_Status_Total = new Dash_Status_TotalCustom();
			var user = await _repo.User.GetById(userid);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");

			if (!user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				var statusTotal = new List<SaleStatusGroupByModel>();

				if (user.Role.Code.ToUpper().StartsWith(RoleCodes.MCENTER))
				{
					statusTotal = await _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.AssCenterUserId == userid).GroupBy(info => info.StatusSaleId)
							   .Select(group => new SaleStatusGroupByModel()
							   {
								   StatusID = group.Key,
								   Count = group.Count()
							   }).OrderBy(x => x.StatusID).ToListAsync();
				}
				else if (user.Role.Code.ToUpper().StartsWith(RoleCodes.BRANCH))
				{
					statusTotal = await _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.Master_Department_BranchId == user.Master_Department_BranchId).GroupBy(info => info.StatusSaleId)
							   .Select(group => new SaleStatusGroupByModel()
							   {
								   StatusID = group.Key,
								   Count = group.Count()
							   }).OrderBy(x => x.StatusID).ToListAsync();
				}
				else if (user.Role.Code.ToUpper().StartsWith(RoleCodes.LOAN) || user.Role.Code.ToUpper().Contains(RoleCodes.ADMIN))
				{
					statusTotal = await _repo.Context.Sales.Where(x => x.Status == StatusModel.Active).GroupBy(info => info.StatusSaleId)
							   .Select(group => new SaleStatusGroupByModel()
							   {
								   StatusID = group.Key,
								   Count = group.Count()
							   }).OrderBy(x => x.StatusID).ToListAsync();
				}

				if (statusTotal != null)
				{
					dash_Status_Total.NumCusAll = statusTotal.Sum(x => x.Count);
					dash_Status_Total.NumCusWaitMCenterAssign = 0;
					dash_Status_Total.NumCusMCenterAssign = 0;
					dash_Status_Total.NumCusInProcess = 0;
					dash_Status_Total.NumCusReturn = 0;
					dash_Status_Total.NumCusTargeNotSuccess = 0;

					foreach (var item in statusTotal)
					{
						if (item.StatusID == (int)StatusSaleModel.WaitAssign) dash_Status_Total.NumCusWaitMCenterAssign = item.Count;
						if (item.StatusID == (int)StatusSaleModel.WaitContact) dash_Status_Total.NumCusMCenterAssign = item.Count;

						if (item.StatusID == (int)StatusSaleModel.Contact
							|| item.StatusID == (int)StatusSaleModel.WaitMeet
							|| item.StatusID == (int)StatusSaleModel.Meet
							|| item.StatusID == (int)StatusSaleModel.WaitSubmitDocument
							|| item.StatusID == (int)StatusSaleModel.SubmitDocument
							|| item.StatusID == (int)StatusSaleModel.WaitApproveLoanRequest
							|| item.StatusID == (int)StatusSaleModel.WaitAPIPHOENIX
							|| item.StatusID == (int)StatusSaleModel.WaitResults
							|| item.StatusID == (int)StatusSaleModel.Results)
						{
							dash_Status_Total.NumCusInProcess = dash_Status_Total.NumCusInProcess + item.Count;
						}

						if (item.StatusID == (int)StatusSaleModel.RMReturnMCenter) dash_Status_Total.NumCusReturn = item.Count;

						if (item.StatusID == (int)StatusSaleModel.ResultsNotConsidered
							|| item.StatusID == (int)StatusSaleModel.ResultsNotLoan
							|| item.StatusID == (int)StatusSaleModel.CloseSaleFail)
						{
							dash_Status_Total.NumCusTargeNotSuccess = dash_Status_Total.NumCusTargeNotSuccess + item.Count;
						}
					}

				}

			}

			return dash_Status_Total;
		}

		public async Task UpdateStatus_TotalById(int userid)
		{
			var user = await _repo.User.GetById(userid);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");

			if (!user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				var dash_Status_Total = await _repo.Context.Dash_Status_Totals
																   .FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.UserId == userid);

				int CRUD = CRUDModel.Update;

				if (dash_Status_Total == null)
				{
					CRUD = CRUDModel.Create;
					dash_Status_Total = new();
					dash_Status_Total.Status = StatusModel.Active;
					dash_Status_Total.UserId = userid;
				}

				var statusTotal = new List<SaleStatusGroupByModel>();

				if (user.Role.Code.ToUpper().StartsWith(RoleCodes.MCENTER))
				{
					statusTotal = await _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.AssCenterUserId == userid).GroupBy(info => info.StatusSaleId)
							   .Select(group => new SaleStatusGroupByModel()
							   {
								   StatusID = group.Key,
								   Count = group.Count()
							   }).OrderBy(x => x.StatusID).ToListAsync();
				}
				else if (user.Role.Code.ToUpper().StartsWith(RoleCodes.BRANCH))
				{
					statusTotal = await _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.Master_Department_BranchId == user.Master_Department_BranchId).GroupBy(info => info.StatusSaleId)
							   .Select(group => new SaleStatusGroupByModel()
							   {
								   StatusID = group.Key,
								   Count = group.Count()
							   }).OrderBy(x => x.StatusID).ToListAsync();
				}
				else if (user.Role.Code.ToUpper().StartsWith(RoleCodes.LOAN) || user.Role.Code.ToUpper().Contains(RoleCodes.ADMIN))
				{
					statusTotal = await _repo.Context.Sales.Where(x => x.Status == StatusModel.Active).GroupBy(info => info.StatusSaleId)
							   .Select(group => new SaleStatusGroupByModel()
							   {
								   StatusID = group.Key,
								   Count = group.Count()
							   }).OrderBy(x => x.StatusID).ToListAsync();
				}

				if (statusTotal != null)
				{
					dash_Status_Total.NumCusAll = statusTotal.Sum(x => x.Count);
					dash_Status_Total.NumCusWaitMCenterAssign = 0;
					dash_Status_Total.NumCusMCenterAssign = 0;
					dash_Status_Total.NumCusInProcess = 0;
					dash_Status_Total.NumCusReturn = 0;
					dash_Status_Total.NumCusTargeNotSuccess = 0;

					foreach (var item in statusTotal)
					{
						if (item.StatusID == (int)StatusSaleModel.WaitAssign) dash_Status_Total.NumCusWaitMCenterAssign = item.Count;
						if (item.StatusID == (int)StatusSaleModel.WaitContact) dash_Status_Total.NumCusMCenterAssign = item.Count;

						if (item.StatusID == (int)StatusSaleModel.Contact
							|| item.StatusID == (int)StatusSaleModel.WaitMeet
							|| item.StatusID == (int)StatusSaleModel.Meet
							|| item.StatusID == (int)StatusSaleModel.WaitSubmitDocument
							|| item.StatusID == (int)StatusSaleModel.SubmitDocument
							|| item.StatusID == (int)StatusSaleModel.WaitApproveLoanRequest
							|| item.StatusID == (int)StatusSaleModel.WaitAPIPHOENIX
							|| item.StatusID == (int)StatusSaleModel.WaitResults
							|| item.StatusID == (int)StatusSaleModel.Results)
						{
							dash_Status_Total.NumCusInProcess = dash_Status_Total.NumCusInProcess + item.Count;
						}

						if (item.StatusID == (int)StatusSaleModel.RMReturnMCenter) dash_Status_Total.NumCusReturn = item.Count;

						if (item.StatusID == (int)StatusSaleModel.ResultsNotConsidered
							|| item.StatusID == (int)StatusSaleModel.ResultsNotLoan
							|| item.StatusID == (int)StatusSaleModel.CloseSaleFail)
						{
							dash_Status_Total.NumCusTargeNotSuccess = dash_Status_Total.NumCusTargeNotSuccess + item.Count;
						}
					}

				}

				dash_Status_Total.IsUpdate = false;
				if (CRUD == CRUDModel.Create)
				{
					await _db.InsterAsync(dash_Status_Total);
				}
				else
				{
					_db.Update(dash_Status_Total);
				}
				await _db.SaveAsync();
			}
		}

		public async Task<Dash_Avg_NumberCustom> GetAvgTop_NumberById(int userid)
		{
			//var dash_Avg_Number = await _repo.Context.Dash_Avg_Numbers.FirstOrDefaultAsync(x => x.UserId == userid);

			//if (dash_Avg_Number == null || (dash_Avg_Number != null && dash_Avg_Number.IsUpdate))
			//{
			//	await UpdateAvg_NumberById(userid);
			//	dash_Avg_Number = await _repo.Context.Dash_Avg_Numbers.FirstOrDefaultAsync(x => x.UserId == userid);
			//}

			//return _mapper.Map<Dash_Avg_NumberCustom>(dash_Avg_Number);

			var dash_Avg_Number = new Dash_Avg_NumberCustom();

			var user = await _repo.User.GetById(userid);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");


			var sale_Durations = _repo.Context.Sale_Durations.Include(x => x.Sale)
												.Where(x => x.Status == StatusModel.Active)
												.OrderByDescending(x => x.CreateDate)
												.AsQueryable();

			if (!user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				var ratingAverage = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active);

				if (user.Role.Code.ToUpper().StartsWith(RoleCodes.MCENTER))
				{
					var avgPerDeal = ratingAverage.Where(x => x.AssCenterUserId == userid).Select(s => s.LoanAmount).DefaultIfEmpty().Average();

					dash_Avg_Number.AvgPerDeal = avgPerDeal ?? 0;
				}
				else if (user.Role.Code.ToUpper().StartsWith(RoleCodes.BRANCH))
				{
					var avgPerDeal = ratingAverage.Where(x => x.BranchId == user.BranchId).Select(s => s.LoanAmount).DefaultIfEmpty().Average();

					dash_Avg_Number.AvgPerDeal = avgPerDeal ?? 0;
				}
				else if (user.Role.Code.ToUpper().StartsWith(RoleCodes.LOAN) || user.Role.Code.ToUpper().Contains(RoleCodes.ADMIN))
				{
					var avgPerDeal = ratingAverage.Select(s => s.LoanAmount).DefaultIfEmpty().Average();
					dash_Avg_Number.AvgPerDeal = avgPerDeal ?? 0;

					var avgTimeCloseSale = sale_Durations.Where(x => x.Sale.StatusSaleId == StatusSaleModel.CloseSale)
															 .Select(x => (x.WaitContact + x.Contact + x.Meet + x.Document + x.Result + x.CloseSale))
															 .DefaultIfEmpty()
															 .Average();
					dash_Avg_Number.AvgDurationCloseSale = (int)(avgTimeCloseSale);

					var avgTimeLostSale = sale_Durations.Where(x => x.Sale.StatusSaleId == StatusSaleModel.RMReturnMCenter
															 || x.Sale.StatusSaleId == StatusSaleModel.ResultsNotConsidered
															 || x.Sale.StatusSaleId == StatusSaleModel.ResultsNotLoan)
															 .Select(x => (x.WaitContact + x.Contact + x.Meet + x.Document + x.Result + x.CloseSale))
															 .DefaultIfEmpty()
															 .Average();
					dash_Avg_Number.AvgDurationLostSale = (int)(avgTimeLostSale);
				}

			}

			return dash_Avg_Number;
		}

		public async Task UpdateAvg_NumberById(int userid)
		{
			var user = await _repo.User.GetById(userid);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");

			if (!user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				var dash_Avg_Number = await _repo.Context.Dash_Avg_Numbers
																   .FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.UserId == userid);

				int CRUD = CRUDModel.Update;

				if (dash_Avg_Number == null)
				{
					CRUD = CRUDModel.Create;
					dash_Avg_Number = new();
					dash_Avg_Number.Status = StatusModel.Active;
					dash_Avg_Number.UserId = userid;
				}

				var statusTotal = new List<SaleStatusGroupByModel>();

				if (user.Role.Code.ToUpper().StartsWith(RoleCodes.MCENTER))
				{
					decimal? RatingAverage = await _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.AssCenterUserId == userid)
																	  .AverageAsync(r => r.LoanAmount);

					dash_Avg_Number.AvgPerDeal = RatingAverage ?? 0;
				}
				else if (user.Role.Code.ToUpper().StartsWith(RoleCodes.BRANCH))
				{

				}
				else if (user.Role.Code.ToUpper().StartsWith(RoleCodes.LOAN) || user.Role.Code.ToUpper().Contains(RoleCodes.ADMIN))
				{
				}

				if (statusTotal != null)
				{

				}

				dash_Avg_Number.IsUpdate = false;
				if (CRUD == CRUDModel.Create)
				{
					await _db.InsterAsync(dash_Avg_Number);
				}
				else
				{
					_db.Update(dash_Avg_Number);
				}
				await _db.SaveAsync();
			}
		}

		public async Task<List<Dash_Map_ThailandCustom>> GetMap_ThailandById(int userid)
		{
			var dash_Map_Thailands = await _repo.Context.Dash_Map_Thailands.Where(x => x.UserId == userid).ToListAsync();

			if (dash_Map_Thailands.Count == 0 || (dash_Map_Thailands.Count > 0 && dash_Map_Thailands.First().IsUpdate))
			{
				await UpdateMap_ThailandById(userid);
				dash_Map_Thailands = await _repo.Context.Dash_Map_Thailands.Where(x => x.UserId == userid).ToListAsync();
			}

			return _mapper.Map<List<Dash_Map_ThailandCustom>>(dash_Map_Thailands);
		}

		public async Task UpdateMap_ThailandById(int userid)
		{
			var user = await _repo.User.GetById(userid);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");

			if (!user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				var dash_Map_Thailands = _repo.Context.Dash_Map_Thailands.Where(x => x.Status == StatusModel.Active && x.UserId == userid).ToList();
				if (dash_Map_Thailands.Count > 0)
				{
					_db.DeleteRange(dash_Map_Thailands);
					await _db.SaveAsync();
				}

				if (user.Role.Code.ToUpper().StartsWith(RoleCodes.LOAN) || user.Role.Code.ToUpper().Contains(RoleCodes.ADMIN))
				{
					Random rnd = new Random();
					//1=ยอดขายสูงสุด
					for (int i = 1; i <= 10; i++)
					{
						int month = rnd.Next(1, 77);
						var province = await _repo.Thailand.GetProvinceByid(month);

						var dash_Map_Thailand = new Data.Entity.Dash_Map_Thailand();
						dash_Map_Thailand.Status = StatusModel.Active;
						dash_Map_Thailand.CreateDate = DateTime.Now;
						dash_Map_Thailand.IsUpdate = false;
						dash_Map_Thailand.UserId = userid;
						dash_Map_Thailand.Type = 1;
						if (province != null)
						{
							dash_Map_Thailand.ProvinceId = province.ProvinceID;
							dash_Map_Thailand.ProvinceName = province.ProvinceName;
						}
						dash_Map_Thailand.SalesAmount = 1000000 * i;
						await _db.InsterAsync(dash_Map_Thailand);
						await _db.SaveAsync();
					}

					//2=แพ้ให้กับคู่แข่งสูงสุด
					for (int i = 1; i <= 10; i++)
					{
						int month = rnd.Next(1, 77);
						var province = await _repo.Thailand.GetProvinceByid(month);

						var dash_Map_Thailand = new Data.Entity.Dash_Map_Thailand();
						dash_Map_Thailand.Status = StatusModel.Active;
						dash_Map_Thailand.CreateDate = DateTime.Now;
						dash_Map_Thailand.IsUpdate = false;
						dash_Map_Thailand.UserId = userid;
						dash_Map_Thailand.Type = 2;
						if (province != null)
						{
							dash_Map_Thailand.ProvinceId = province.ProvinceID;
							dash_Map_Thailand.ProvinceName = province.ProvinceName;
						}
						dash_Map_Thailand.SalesAmount = 10000 * i;
						await _db.InsterAsync(dash_Map_Thailand);
						await _db.SaveAsync();
					}
				}
			}

		}

		public async Task<List<Dash_PieCustom>> GetPieCloseSaleReason(int userid)
		{
			var user = await _repo.User.GetById(userid);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");

			var response = new List<Dash_PieCustom>();

			if (!user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				var salesAllCount = await _repo.Context.Sales.CountAsync(x => x.Status == StatusModel.Active);

				//var salesAllCount = await _repo.Context.Sales.CountAsync(x => x.StatusSaleId == StatusSaleModel.RMReturnMCenter
				//											 || x.StatusSaleId == StatusSaleModel.ResultsNotConsidered
				//											 || x.StatusSaleId == StatusSaleModel.ResultsNotLoan);

				var salesCloseSaleCount = await _repo.Context.Sales.CountAsync(x => x.Status == StatusModel.Active && x.StatusSaleId == StatusSaleModel.CloseSale);

				//salesAllCount = 50;
				//salesCloseSaleCount = 47;

				if (salesAllCount > 0 || salesCloseSaleCount > 0)
				{

					var perSuccess = ((decimal)salesCloseSaleCount / salesAllCount) * 100;
					var perFail = 100 - perSuccess;

					response.Add(new()
					{
						Status = StatusModel.Active,
						Code = Dash_PieCodeModel.ClosingSale,
						TitleName = "การปิดการขาย",
						Name = "สำเร็จ",
						Value = perSuccess
					});
					response.Add(new()
					{
						Status = StatusModel.Active,
						Code = Dash_PieCodeModel.ClosingSale,
						TitleName = "การปิดการขาย",
						Name = "ไม่สำเร็จ",
						Value = perFail
					});

				}
				//เหตุผลไม่ประสงค์ขอสินเชื่อ

				var salesResultsNotLoan = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.StatusSaleId == StatusSaleModel.ResultsNotLoan && x.Master_Reason_CloseSaleId.HasValue)
											 .GroupBy(m => m.Master_Reason_CloseSaleId)
											 .Select(group => new { Label = group.Key, Sales = group.ToList() })
											 .ToList();

				if (salesResultsNotLoan.Count > 0)
				{
					foreach (var item in salesResultsNotLoan)
					{
						response.Add(new()
						{
							Status = StatusModel.Active,
							Code = Dash_PieCodeModel.ReasonNotLoan,
							TitleName = "เหตุผลไม่ประสงค์ขอสินเชื่อ",
							Name = $"{item.Sales.Select(x => x.StatusDescription).FirstOrDefault()} ",
							Value = item.Sales.Count
						});
					}
				}
				else
				{
					//response.Add(new()
					//{
					//	Status = StatusModel.Active,
					//	Code = Dash_PieCodeModel.ReasonNotLoan,
					//	TitleName = "เหตุผลไม่ประสงค์ขอสินเชื่อ",
					//	Name = "ไม่พบข้อมูล ",
					//	Value = 100
					//});
				}
			}

			return response;
		}

		public async Task<List<Dash_PieCustom>> GetPieNumberCustomer(int userid)
		{
			var user = await _repo.User.GetById(userid);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");

			var response = new List<Dash_PieCustom>();

			if (!user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				if (user.Role.Code.ToUpper().StartsWith(RoleCodes.LOAN) || user.Role.Code.ToUpper().Contains(RoleCodes.ADMIN))
				{
					var salesBusinessSize = _repo.Context.Customers.Where(x => x.Status == StatusModel.Active)
												 .GroupBy(m => m.Master_BusinessSizeId)
												 .Select(group => new { GroupID = group.Key, Customers = group.ToList() })
												 .ToList();

					if (salesBusinessSize.Count > 0)
					{
						foreach (var item in salesBusinessSize)
						{
							response.Add(new()
							{
								Status = StatusModel.Active,
								Code = Dash_PieCodeModel.NumCusSizeBusiness,
								TitleName = "จำนวนลูกค้าตามขนาดธุรกิจ",
								Name = $"{item.Customers.Select(x => x.Master_BusinessSizeName).FirstOrDefault()} ",
								Value = item.Customers.Count
							});
						}
					}

					var salesBusinessType = _repo.Context.Customers.Where(x => x.Status == StatusModel.Active)
												 .GroupBy(m => m.Master_BusinessTypeId)
												 .Select(group => new { GroupID = group.Key, Customers = group.ToList() })
												 .ToList();
					if (salesBusinessType.Count > 0)
					{
						foreach (var item in salesBusinessType)
						{
							response.Add(new()
							{
								Status = StatusModel.Active,
								Code = Dash_PieCodeModel.NumCusTypeBusiness,
								TitleName = "จำนวนลูกค้าตามประเภทธุรกิจ",
								Name = $"{item.Customers.Select(x => x.Master_BusinessTypeName).FirstOrDefault()} ",
								Value = item.Customers.Count
							});
						}
					}

					response.Add(new()
					{
						Status = StatusModel.Active,
						Code = Dash_PieCodeModel.NumCusISICCode,
						TitleName = "จำนวนลูกค้าตาม ISIC Code",
						Name = "ไม่พบข้อมูล ",
						Value = 100
					});

					var salesLoanType = _repo.Context.Customers.Where(x => x.Status == StatusModel.Active)
												 .GroupBy(m => m.Master_LoanTypeId)
												 .Select(group => new { GroupID = group.Key, Customers = group.ToList() })
												 .ToList();
					if (salesLoanType.Count > 0)
					{
						foreach (var item in salesBusinessType)
						{
							response.Add(new()
							{
								Status = StatusModel.Active,
								Code = Dash_PieCodeModel.NumCusLoanType,
								TitleName = "จำนวนลูกค้าตามประเภทสินเชื่อ",
								Name = $"{item.Customers.Select(x => x.Master_LoanTypeName).FirstOrDefault()} ",
								Value = item.Customers.Count
							});
						}
					}

				}
			}

			return response;
		}

		public async Task<List<Dash_PieCustom>> GetPieLoanValue(int userid)
		{
			var user = await _repo.User.GetById(userid);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");

			var response = new List<Dash_PieCustom>();

			if (!user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				if (user.Role.Code.ToUpper().StartsWith(RoleCodes.LOAN) || user.Role.Code.ToUpper().Contains(RoleCodes.ADMIN))
				{
					var salesBusinessSize = _repo.Context.Sales.Include(x => x.Customer).Where(x => x.Status == StatusModel.Active)
												 .GroupBy(m => m.Customer.Master_BusinessSizeId)
												 .Select(group => new { GroupID = group.Key, Sales = group.ToList() })
												 .ToList();

					if (salesBusinessSize.Count > 0)
					{
						foreach (var item in salesBusinessSize)
						{
							response.Add(new()
							{
								Status = StatusModel.Active,
								Code = Dash_PieCodeModel.ValueSizeBusiness,
								TitleName = "มูลค่าสินเชื่อตามขนาดธุรกิจ",
								Name = $"{item.Sales.Select(x => x.Customer.Master_BusinessSizeName).FirstOrDefault()} ",
								Value = item.Sales.Sum(s => s.LoanAmount)
							});
						}
					}

					var salesBusinessType = _repo.Context.Sales.Include(x => x.Customer).Where(x => x.Status == StatusModel.Active)
												 .GroupBy(m => m.Customer.Master_BusinessTypeId)
												 .Select(group => new { GroupID = group.Key, Sales = group.ToList() })
												 .ToList();
					if (salesBusinessType.Count > 0)
					{
						foreach (var item in salesBusinessType)
						{
							response.Add(new()
							{
								Status = StatusModel.Active,
								Code = Dash_PieCodeModel.ValueTypeBusiness,
								TitleName = "มูลค่าสินเชื่อตามประเภทธุรกิจ",
								Name = $"{item.Sales.Select(x => x.Customer.Master_BusinessTypeName).FirstOrDefault()} ",
								Value = item.Sales.Sum(s => s.LoanAmount)
							});
						}
					}

					response.Add(new()
					{
						Status = StatusModel.Active,
						Code = Dash_PieCodeModel.ValueISICCode,
						TitleName = "มูลค่าสินเชื่อตาม ISIC Code",
						Name = "ไม่พบข้อมูล ",
						Value = 100
					});

					var salesLoanType = _repo.Context.Sales.Include(x => x.Customer).Where(x => x.Status == StatusModel.Active)
												 .GroupBy(m => m.Customer.Master_LoanTypeId)
												 .Select(group => new { GroupID = group.Key, Sales = group.ToList() })
												 .ToList();
					if (salesLoanType.Count > 0)
					{
						foreach (var item in salesBusinessType)
						{
							response.Add(new()
							{
								Status = StatusModel.Active,
								Code = Dash_PieCodeModel.ValueLoanType,
								TitleName = "มูลค่าสินเชื่อตามประเภทสินเชื่อ",
								Name = $"{item.Sales.Select(x => x.Customer.Master_BusinessSizeName).FirstOrDefault()} ",
								Value = item.Sales.Sum(s => s.LoanAmount)
							});
						}
					}

				}
			}

			return response;
		}

		public async Task<PaginationView<List<Sale_DurationCustom>>> GetDuration(allFilter model)
		{
			IQueryable<Sale_Duration> query;
			string? roleCode = null;

			if (model.userid.HasValue)
			{
				var roleList = await _repo.User.GetRoleByUserId(model.userid.Value);
				if (roleList != null)
				{
					roleCode = roleList.Code;
				}
			}

			query = _repo.Context.Sale_Durations.Include(x => x.Sale)
												.Where(x => x.Status != StatusModel.Delete)
												.OrderByDescending(x => x.CreateDate)
												.AsQueryable();

			if (!String.IsNullOrEmpty(model.type))
			{
				if (model.type.ToLower() == "avgdurationclosesale")
				{
					query = query.Where(x => x.Sale.StatusSaleId == StatusSaleModel.CloseSale);
				}
				else if (model.type.ToLower() == "avgdurationlostsale")
				{
					query = query.Where(x => x.Sale.StatusSaleId == StatusSaleModel.RMReturnMCenter
															 || x.Sale.StatusSaleId == StatusSaleModel.ResultsNotConsidered
															 || x.Sale.StatusSaleId == StatusSaleModel.ResultsNotLoan);
				}
			}


			if (!String.IsNullOrEmpty(model.searchtxt))
				query = query.Where(x => x.Sale.CompanyName != null && x.Sale.CompanyName.Contains(model.searchtxt));

			if (!String.IsNullOrEmpty(model.contact_name))
				query = query.Where(x => x.ContactName != null && x.ContactName.Contains(model.contact_name));

			if (!String.IsNullOrEmpty(model.assignrm_name))
				query = query.Where(x => x.Sale.AssUserName != null && x.Sale.AssUserName.Contains(model.assignrm_name));

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Sale_DurationCustom>>()
			{
				Items = _mapper.Map<List<Sale_DurationCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task UpdateDurationById(Guid saleid)
		{
			var sale_Durations = await _repo.Context.Sale_Durations.FirstOrDefaultAsync(x => x.SaleId == saleid);

			int CRUD = CRUDModel.Update;

			if (sale_Durations == null)
			{
				CRUD = CRUDModel.Create;
				sale_Durations = new();
				sale_Durations.Status = StatusModel.Active;
				sale_Durations.CreateDate = DateTime.Now;
				sale_Durations.SaleId = saleid;
			}

			DateTime contactFirst = DateTime.MinValue;
			var sales = await _repo.Context.Sales.Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == saleid);
			if (sales != null && sales.Customer != null)
			{
				sale_Durations.ContactName = sales.Customer.ContactName;
				if (sales.ContactStartDate.HasValue)
				{
					contactFirst = sales.ContactStartDate.Value.Date;
				}
			}

			sale_Durations.WaitContact = 0;
			sale_Durations.Contact = 0;
			sale_Durations.Meet = 0;
			sale_Durations.Document = 0;
			sale_Durations.Result = 0;
			sale_Durations.CloseSale = 0;

			var sale_Status = await _repo.Context.Sale_Statuses.Where(x => x.SaleId == saleid).ToListAsync();
			if (sale_Status.Count > 0)
			{
				var waitContactLast = sale_Status.Where(x => x.StatusId == StatusSaleModel.WaitContact).OrderByDescending(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
				if (contactFirst != DateTime.MinValue && waitContactLast != DateTime.MinValue)
					sale_Durations.WaitContact = (int)(contactFirst - waitContactLast).TotalDays;

				var contactLast = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Contact).OrderByDescending(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
				var meetFirst = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Meet).OrderBy(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
				if (meetFirst != DateTime.MinValue && contactLast != DateTime.MinValue)
					sale_Durations.Contact = (int)(meetFirst - contactLast).TotalDays;

				var meetLast = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Meet).OrderByDescending(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
				var documentFirst = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Document).OrderBy(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
				if (documentFirst != DateTime.MinValue && meetLast != DateTime.MinValue)
					sale_Durations.Meet = (int)(documentFirst - meetLast).TotalDays;

				var documentLast = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Document).OrderByDescending(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
				var resultFirst = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Result).OrderBy(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
				if (resultFirst != DateTime.MinValue && documentLast != DateTime.MinValue)
					sale_Durations.Document = (int)(resultFirst - documentLast).TotalDays;

				var resultLast = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Result).OrderByDescending(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
				var closeSaleFirst = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.CloseSale).OrderBy(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
				if (closeSaleFirst != DateTime.MinValue && resultLast != DateTime.MinValue)
					sale_Durations.Result = (int)(closeSaleFirst - resultLast).TotalDays;

				sale_Durations.ContactStartDate = contactFirst;
			}

			if (CRUD == CRUDModel.Create)
			{
				await _db.InsterAsync(sale_Durations);
			}
			else
			{
				_db.Update(sale_Durations);
			}
			await _db.SaveAsync();
		}

		public async Task UpdateActivityById(Guid saleid)
		{
			var sales_Activities = await _repo.Context.Sales_Activities.FirstOrDefaultAsync(x => x.SaleId == saleid);

			int CRUD = CRUDModel.Update;

			if (sales_Activities == null)
			{
				CRUD = CRUDModel.Create;
				sales_Activities = new();
				sales_Activities.Status = StatusModel.Active;
				sales_Activities.CreateDate = DateTime.Now;
				sales_Activities.SaleId = saleid;
			}

			var sales = await _repo.Context.Sales.Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == saleid);
			if (sales != null && sales.Customer != null)
			{
				sales_Activities.ContactName = sales.Customer.ContactName;
			}

			var sale_Contacts = await _repo.Context.Sale_Contacts.Where(x => x.SaleId == saleid).ToListAsync();
			if (sale_Contacts.Count > 0)
			{
				sales_Activities.Contact = sale_Contacts.Count;
			}

			var sale_Meets = await _repo.Context.Sale_Meets.Where(x => x.SaleId == saleid).ToListAsync();
			if (sale_Meets.Count > 0)
			{
				sales_Activities.Meet = sale_Meets.Count;
			}

			var sale_Documents = await _repo.Context.Sale_Documents.Where(x => x.SaleId == saleid).ToListAsync();
			if (sale_Documents.Count > 0)
			{
				sales_Activities.Document = sale_Documents.Count;
			}

			var sale_Results = await _repo.Context.Sale_Results.Where(x => x.SaleId == saleid).ToListAsync();
			if (sale_Results.Count > 0)
			{
				sales_Activities.Result = sale_Results.Count;
			}

			var sale_Close_Sales = await _repo.Context.Sale_Close_Sales.Where(x => x.SaleId == saleid).ToListAsync();
			if (sale_Close_Sales.Count > 0)
			{
				sales_Activities.CloseSale = sale_Close_Sales.Count;
			}

			if (CRUD == CRUDModel.Create)
			{
				await _db.InsterAsync(sales_Activities);
			}
			else
			{
				_db.Update(sales_Activities);
			}
			await _db.SaveAsync();
		}

		public async Task<List<Dash_PieCustom>> GetGroupReasonNotLoan(int userid)
		{
			var user = await _repo.User.GetById(userid);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");

			var response = new List<Dash_PieCustom>();

			var sales = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.StatusSaleId == StatusSaleModel.ResultsNotLoan && x.Master_Reason_CloseSaleId.HasValue)
										 .GroupBy(m => m.Master_Reason_CloseSaleId)
										 .Select(group => new { GroupID = group.Key, Sales = group.ToList() })
										 .ToList();

			if (sales.Count > 0)
			{
				foreach (var item in sales)
				{
					response.Add(new()
					{
						Status = StatusModel.Active,
						Code = Dash_PieCodeModel.ReasonNotLoan,
						TitleName = "เหตุผลไม่ประสงค์ขอสินเชื่อ",
						Name = $"{item.Sales.Select(x => x.StatusDescription).FirstOrDefault()} ",
						Value = item.Sales.Count
					});

				}
			}

			return response;
		}

	}
}

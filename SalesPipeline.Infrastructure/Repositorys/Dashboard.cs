using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Index.HPRtree;
using NPOI.OpenXmlFormats.Spreadsheet;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			var dash_Status_Total = await _repo.Context.Dash_Status_Totals.FirstOrDefaultAsync(x => x.UserId == userid);

			if (dash_Status_Total == null || (dash_Status_Total != null && dash_Status_Total.IsUpdate))
			{
				await UpdateStatus_TotalById(userid);
				dash_Status_Total = await _repo.Context.Dash_Status_Totals.FirstOrDefaultAsync(x => x.UserId == userid);
			}

			return _mapper.Map<Dash_Status_TotalCustom>(dash_Status_Total);
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
							|| item.StatusID == (int)StatusSaleModel.WaitApproveDocument
							|| item.StatusID == (int)StatusSaleModel.WaitAPIPHOENIXLPS
							|| item.StatusID == (int)StatusSaleModel.WaitResults
							|| item.StatusID == (int)StatusSaleModel.Results
							|| item.StatusID == (int)StatusSaleModel.WaitAPIPHOENIXLPS)
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

		public async Task<Dash_Avg_NumberCustom> GetAvg_NumberById(int userid)
		{
			var dash_Avg_Number = await _repo.Context.Dash_Avg_Numbers.FirstOrDefaultAsync(x => x.UserId == userid);

			if (dash_Avg_Number == null || (dash_Avg_Number != null && dash_Avg_Number.IsUpdate))
			{
				await UpdateAvg_NumberById(userid);
				dash_Avg_Number = await _repo.Context.Dash_Avg_Numbers.FirstOrDefaultAsync(x => x.UserId == userid);
			}

			return _mapper.Map<Dash_Avg_NumberCustom>(dash_Avg_Number);
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

				else if (user.Role.Code.ToUpper().StartsWith(RoleCodes.LOAN) || user.Role.Code.ToUpper().Contains(RoleCodes.ADMIN))
				{
					//1=ยอดขายสูงสุด
					for (int i = 1; i <= 10; i++)
					{
						var province = await _repo.Thailand.GetProvinceByid(i);

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
						var province = await _repo.Thailand.GetProvinceByid(i);

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
						dash_Map_Thailand.SalesAmount = 1000000 * i;
						await _db.InsterAsync(dash_Map_Thailand);
						await _db.SaveAsync();
					}
				}
			}

		}



	}
}

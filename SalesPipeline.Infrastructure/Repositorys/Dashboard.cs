using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Index.HPRtree;
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

			if (dash_Status_Total == null)
			{
				await UpdateStatus_TotalById(userid);
			}

			return _mapper.Map<Dash_Status_TotalCustom>(dash_Status_Total);
		}

		public async Task UpdateStatus_TotalById(int userid)
		{
			var userRole = await _repo.User.GetRoleByUserId(userid);
			if (userRole == null) throw new ExceptionCustom("userid not map role.");

			if (!userRole.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				var dash_Status_Total = await _repo.Context.Dash_Status_Totals
																   .FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.UserId == userid);

				int CRUD = CRUDModel.Update;

				if (dash_Status_Total == null)
				{
					CRUD = CRUDModel.Create;
					dash_Status_Total = new();
				}

				if (userRole.Code.ToUpper().StartsWith(RoleCodes.MCENTER))
				{
					var statusTotal = await _repo.Context.Sales.Where(x => x.Status != StatusModel.Delete && x.AssCenterUserId == userid).GroupBy(info => info.StatusSaleId)
								.Select(group => new
								{
									StatusID = group.Key,
									Count = group.Count()
								}).OrderBy(x => x.StatusID).ToListAsync();

					dash_Status_Total.NumCusAll = statusTotal.Sum(x => x.Count);

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
							dash_Status_Total.NumCusInProcess = dash_Status_Total.NumCusTargeNotSuccess + item.Count;
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
			var query = await _repo.Context.Dash_Avg_Numbers.FirstOrDefaultAsync(x => x.UserId == userid);

			return _mapper.Map<Dash_Avg_NumberCustom>(query);
		}

	}
}

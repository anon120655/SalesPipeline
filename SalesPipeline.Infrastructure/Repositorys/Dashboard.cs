using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using System.Linq.Expressions;
using SalesPipeline.Infrastructure.Helpers;
using System.Linq;

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

		private async Task<IQueryable<Sale>> QueryArea(IQueryable<Sale> query, UserCustom user)
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

					//ผจศ. เห็นเฉพาะพนักงาน RM ภายใต้พื้นที่การดูแล
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

		private async Task<IQueryable<Sale_Duration>> QueryAreaDuration(IQueryable<Sale_Duration> query, UserCustom user)
		{
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			if (user.Role.Code != null && user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				query = query.Where(x => x.Sale.AssUserId == user.Id);
			}
			else
			{
				//99999999-9999-9999-9999-999999999999 เห็นทั้งประเทศ
				if (!user.Role.IsAssignCenter && user.Master_Branch_RegionId != Guid.Parse("99999999-9999-9999-9999-999999999999"))
				{
					Expression<Func<Sale_Duration, bool>> orExpression = x => false;
					//9999 เห็นทุกจังหวัดในภาค
					if (user.Master_Branch_RegionId.HasValue && user_Areas.Any(x => x == 9999))
					{
						var provinces = await _repo.Thailand.GetProvince(user.Master_Branch_RegionId);
						if (provinces?.Count > 0)
						{
							user_Areas = provinces.Select(x => x.ProvinceID).ToList();
						}
					}

					//ผจศ. เห็นเฉพาะพนักงาน RM ภายใต้พื้นที่การดูแล
					//ในพื้นที่จังหวัด หรือ ดูแลทุกจังหวัดในภาค
					foreach (var provinceId in user_Areas)
					{
						var tempProvinceId = provinceId;
						orExpression = orExpression.Or(x =>
						(x.Sale.AssUser != null && x.Sale.AssUser.User_Areas.Any(s => s.ProvinceId == tempProvinceId))
						|| (x.Sale.AssUser != null && x.Sale.AssUser.User_Areas.Any(s => s.User.Master_Branch_RegionId == user.Master_Branch_RegionId && s.ProvinceId == 9999))
						);
					}

					//งานที่สร้างเอง หรือถูกมอบหมายมาจาก ธญ
					orExpression = orExpression.Or(x => x.Sale.AssCenterUserId == user.Id);
					query = query.Where(orExpression);

					query = query.Where(x => x.Sale.StatusSaleId != StatusSaleModel.MCenterReturnLoan);
				}
			}

			return query;
		}

		private async Task<IQueryable<Sale_Activity>> QueryAreaActivity(IQueryable<Sale_Activity> query, UserCustom user)
		{
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			if (user.Role.Code != null && user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				query = query.Where(x => x.Sale.AssUserId == user.Id);
			}
			else
			{
				//99999999-9999-9999-9999-999999999999 เห็นทั้งประเทศ
				if (!user.Role.IsAssignCenter && user.Master_Branch_RegionId != Guid.Parse("99999999-9999-9999-9999-999999999999"))
				{
					Expression<Func<Sale_Activity, bool>> orExpression = x => false;
					//9999 เห็นทุกจังหวัดในภาค
					if (user.Master_Branch_RegionId.HasValue && user_Areas.Any(x => x == 9999))
					{
						var provinces = await _repo.Thailand.GetProvince(user.Master_Branch_RegionId);
						if (provinces?.Count > 0)
						{
							user_Areas = provinces.Select(x => x.ProvinceID).ToList();
						}
					}

					//ผจศ. เห็นเฉพาะพนักงาน RM ภายใต้พื้นที่การดูแล
					//ในพื้นที่จังหวัด หรือ ดูแลทุกจังหวัดในภาค
					foreach (var provinceId in user_Areas)
					{
						var tempProvinceId = provinceId; 
						orExpression = orExpression.Or(x =>
						(x.Sale.AssUser != null && x.Sale.AssUser.User_Areas.Any(s => s.ProvinceId == tempProvinceId))
						|| (x.Sale.AssUser != null && x.Sale.AssUser.User_Areas.Any(s => s.User.Master_Branch_RegionId == user.Master_Branch_RegionId && s.ProvinceId == 9999))
						);
					}

					//งานที่สร้างเอง หรือถูกมอบหมายมาจาก ธญ
					orExpression = orExpression.Or(x => x.Sale.AssCenterUserId == user.Id);
					query = query.Where(orExpression);

					query = query.Where(x => x.Sale.StatusSaleId != StatusSaleModel.MCenterReturnLoan);
				}
			}

			return query;
		}

		private async Task<IQueryable<Sale_Deliver>> QueryAreaDeliver(IQueryable<Sale_Deliver> query, UserCustom user)
		{
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			if (user.Role.Code != null && user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				query = query.Where(x => x.Sale.AssUserId == user.Id);
			}
			else
			{
				//99999999-9999-9999-9999-999999999999 เห็นทั้งประเทศ
				if (!user.Role.IsAssignCenter && user.Master_Branch_RegionId != Guid.Parse("99999999-9999-9999-9999-999999999999"))
				{
					Expression<Func<Sale_Deliver, bool>> orExpression = x => false;
					//9999 เห็นทุกจังหวัดในภาค
					if (user.Master_Branch_RegionId.HasValue && user_Areas.Any(x => x == 9999))
					{
						var provinces = await _repo.Thailand.GetProvince(user.Master_Branch_RegionId);
						if (provinces?.Count > 0)
						{
							user_Areas = provinces.Select(x => x.ProvinceID).ToList();
						}
					}

					//ผจศ. เห็นเฉพาะพนักงาน RM ภายใต้พื้นที่การดูแล
					//ในพื้นที่จังหวัด หรือ ดูแลทุกจังหวัดในภาค
					foreach (var provinceId in user_Areas)
					{
						var tempProvinceId = provinceId;
						orExpression = orExpression.Or(x =>
						(x.Sale.AssUser != null && x.Sale.AssUser.User_Areas.Any(s => s.ProvinceId == tempProvinceId))
						|| (x.Sale.AssUser != null && x.Sale.AssUser.User_Areas.Any(s => s.User.Master_Branch_RegionId == user.Master_Branch_RegionId && s.ProvinceId == 9999))
						);
					}

					//งานที่สร้างเอง หรือถูกมอบหมายมาจาก ธญ
					orExpression = orExpression.Or(x => x.Sale.AssCenterUserId == user.Id);
					query = query.Where(orExpression);

					query = query.Where(x => x.Sale.StatusSaleId != StatusSaleModel.MCenterReturnLoan);
				}
			}

			return query;
		}

		public async Task<Dash_Status_TotalCustom> GetStatus_TotalById(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var dash_Status_Total = new Dash_Status_TotalCustom();
			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var query = _repo.Context.Sales.Include(x => x.AssUser).Where(x => x.Status == StatusModel.Active);

			query = await QueryArea(query, user);

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

			//กิจการสาขาภาค[]
			if (model.DepBranchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToGuid(model.DepBranchs);
				if (idList.Count > 0)
				{
					foreach (var branchRegId in idList)
					{
						var provinces = await _repo.Thailand.GetProvince(branchRegId);
						if (provinces?.Count > 0)
						{
							var provincesIdlist = provinces.Select(x => x.ProvinceID).ToList();
							query = query.Where(x => x.ProvinceId.HasValue && provincesIdlist.Contains(x.ProvinceId.Value));
						}
					}
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

			if (user.Role.Code != null && !user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				var statusTotal = new List<SaleStatusGroupByModel>();

				statusTotal = await query.GroupBy(info => info.StatusSaleId)
						   .Select(group => new SaleStatusGroupByModel()
						   {
							   StatusID = group.Key,
							   Count = group.Count()
						   }).OrderBy(x => x.StatusID).ToListAsync();

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

						if (user.Role.IsAssignRM && item.StatusID == (int)StatusSaleModel.RMReturnMCenter)
						{
							dash_Status_Total.NumCusReturn = item.Count;
						}
						else if (user.Role.IsAssignCenter && item.StatusID == (int)StatusSaleModel.MCenterReturnLoan)
						{
							dash_Status_Total.NumCusReturn = item.Count;
						}
					}

					int _year = DateTime.Now.Year;
					if (model.startdate.HasValue)
					{
						_year = model.startdate.Value.Year;
					}

					var user_Target_Sales = _repo.Context.User_Target_Sales.Include(x => x.User)
						.Where(x => x.Status == StatusModel.Active && x.Year == _year && x.AmountActual < x.AmountTarget);

					//99999999-9999-9999-9999-999999999999 เห็นทั้งประเทศ
					if (!user.Role.IsAssignCenter && user.Master_Branch_RegionId != Guid.Parse("99999999-9999-9999-9999-999999999999"))
					{
						//9999 เห็นทุกจังหวัดในภาค
						if (user.Master_Branch_RegionId.HasValue && user_Areas.Any(x => x == 9999))
						{
							user_Target_Sales = user_Target_Sales.Where(x => x.User.Master_Branch_RegionId == user.Master_Branch_RegionId);
						}
						else
						{
							//เห็นเฉพาะจังหวัดที่ดูแล
							user_Target_Sales = user_Target_Sales.Where(x => x.User.ProvinceId.HasValue && user_Areas.Contains(x.User.ProvinceId.Value));
						}
					}

					dash_Status_Total.NumCusTargeNotSuccess = user_Target_Sales.Count();
				}

			}

			return dash_Status_Total;
		}

		public async Task UpdateStatus_TotalById(allFilter model)
		{
			if (model.userid.HasValue)
			{
				var user = await _repo.User.GetById(model.userid.Value);
				if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
				var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

				if (user.Role.Code != null && !user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
				{
					var dash_Status_Total = await _repo.Context.Dash_Status_Totals
																	   .FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.UserId == model.userid.Value);

					int CRUD = CRUDModel.Update;

					if (dash_Status_Total == null)
					{
						CRUD = CRUDModel.Create;
						dash_Status_Total = new();
						dash_Status_Total.Status = StatusModel.Active;
						dash_Status_Total.UserId = model.userid.Value;
					}
					var statusTotal = new List<SaleStatusGroupByModel>();


					IQueryable<Sale> query;
					query = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active).AsQueryable();

					query = await QueryArea(query, user);

					statusTotal = await query.GroupBy(info => info.StatusSaleId)
							   .Select(group => new SaleStatusGroupByModel()
							   {
								   StatusID = group.Key,
								   Count = group.Count()
							   }).OrderBy(x => x.StatusID).ToListAsync();

					//if (user.Role.Code != null && user.Role.Code.ToUpper().StartsWith(RoleCodes.CEN_BRANCH))
					//{
					//	statusTotal = await _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.AssCenterUserId == model.userid.Value).GroupBy(info => info.StatusSaleId)
					//			   .Select(group => new SaleStatusGroupByModel()
					//			   {
					//				   StatusID = group.Key,
					//				   Count = group.Count()
					//			   }).OrderBy(x => x.StatusID).ToListAsync();
					//}
					//else if (user.Role.Code != null && user.Role.Code.ToUpper().StartsWith(RoleCodes.BRANCH_REG))
					//{
					//	statusTotal = await _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.Master_Branch_RegionId == user.Master_Branch_RegionId).GroupBy(info => info.StatusSaleId)
					//			   .Select(group => new SaleStatusGroupByModel()
					//			   {
					//				   StatusID = group.Key,
					//				   Count = group.Count()
					//			   }).OrderBy(x => x.StatusID).ToListAsync();
					//}
					//else if (user.Role.Code != null && user.Role.Code.ToUpper().StartsWith(RoleCodes.LOAN) || user.Role.Code.ToUpper().Contains(RoleCodes.ADMIN))
					//{
					//	statusTotal = await _repo.Context.Sales.Where(x => x.Status == StatusModel.Active).GroupBy(info => info.StatusSaleId)
					//			   .Select(group => new SaleStatusGroupByModel()
					//			   {
					//				   StatusID = group.Key,
					//				   Count = group.Count()
					//			   }).OrderBy(x => x.StatusID).ToListAsync();
					//}

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
								|| item.StatusID == (int)StatusSaleModel.CloseSaleNotLoan
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
		}

		public async Task<PaginationView<List<User_Target_SaleCustom>>> GetListTarget_SaleById(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var dash_SalesPipeline = new Dash_SalesPipelineModel();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			int _year = DateTime.Now.Year;
			if (!string.IsNullOrEmpty(model.year))
			{
				if (int.TryParse(model.year, out int year))
				{
					_year = year;
				}
			}

			var query = _repo.Context.User_Target_Sales
				.Include(x => x.User)
				.Where(x => x.Status == StatusModel.Active && x.Year == _year)
				.OrderByDescending(x => x.AmountTarget).AsQueryable();

			if (!String.IsNullOrEmpty(model.emp_id))
			{
				query = query.Where(x => x.User.EmployeeId != null && x.User.EmployeeId.Contains(model.emp_id));
			}

			if (!String.IsNullOrEmpty(model.emp_name))
			{
				query = query.Where(x => x.User.FullName != null && x.User.FullName.Contains(model.emp_name));
			}

			if (model.amounttarget > 0)
			{
				query = query.Where(x => x.AmountTarget == model.amounttarget.Value);
			}

			if (model.achieve_goal > 0)
			{
				if (model.achieve_goal == 1)
				{
					query = query.Where(x => x.AmountActual >= x.AmountTarget);
				}
				else if (model.achieve_goal == 2)
				{
					query = query.Where(x => x.AmountActual < x.AmountTarget);
				}
			}

			//99999999-9999-9999-9999-999999999999 เห็นทั้งประเทศ
			if (!user.Role.IsAssignCenter && user.Master_Branch_RegionId != Guid.Parse("99999999-9999-9999-9999-999999999999"))
			{
				//9999 เห็นทุกจังหวัดในภาค
				if (user.Master_Branch_RegionId.HasValue && user_Areas.Any(x => x == 9999))
				{
					query = query.Where(x => x.User.Master_Branch_RegionId == user.Master_Branch_RegionId);
				}
				else
				{
					//เห็นเฉพาะจังหวัดที่ดูแล
					query = query.Where(x => x.User.ProvinceId.HasValue && user_Areas.Contains(x.User.ProvinceId.Value));
				}
			}

			//กิจการสาขาภาค[]
			if (model.DepBranchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToGuid(model.DepBranchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.User.Master_Branch_RegionId.HasValue && idList.Contains(x.User.Master_Branch_RegionId));
				}
			}

			//จังหวัด[]
			if (model.Provinces?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Provinces);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.User.ProvinceId.HasValue && idList.Contains(x.User.ProvinceId));
				}
			}

			//สาขา[]
			if (model.Branchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Branchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.User.BranchId.HasValue && idList.Contains(x.User.BranchId));
				}
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<User_Target_SaleCustom>>()
			{
				Items = _mapper.Map<List<User_Target_SaleCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task UpdateTarget_SaleById(allFilter model)
		{
			if (!model.userid.HasValue) throw new ExceptionCustom("userid required");
			if (string.IsNullOrEmpty(model.year)) throw new ExceptionCustom("year required");

			if (int.TryParse(model.year, out int _year))
			{
				DateTime _dateNow = DateTime.Now;

				var user_Target_Sales = await _repo.Context.User_Target_Sales.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.UserId == model.userid && x.Year == _year);

				var amountTarget = await _repo.Context.Sales
					.Where(x => x.Status == StatusModel.Active && x.AssUserId == model.userid && x.LoanAmount > 0 && x.StatusSaleId == StatusSaleModel.CloseSale && x.CreateDate.Year == _year)
					.SumAsync(x => x.LoanAmount);

				int CRUD = CRUDModel.Update;
				if (user_Target_Sales == null)
				{
					CRUD = CRUDModel.Create;
					user_Target_Sales = new();
					user_Target_Sales.CreateDate = _dateNow;
					user_Target_Sales.CreateBy = model.CurrentUserId;
				}
				user_Target_Sales.UpdateDate = _dateNow;
				user_Target_Sales.UpdateBy = model.CurrentUserId;

				user_Target_Sales.UserId = model.userid.Value;
				user_Target_Sales.Year = _year;
				user_Target_Sales.AmountActual = amountTarget ?? 0;

				if (CRUD == CRUDModel.Create)
				{
					await _db.InsterAsync(user_Target_Sales);
				}
				else
				{
					_db.Update(user_Target_Sales);
				}
				await _db.SaveAsync();

			}

		}

		public async Task UpdateTarget_SaleAll(string year)
		{
			if (string.IsNullOrEmpty(year)) throw new ExceptionCustom("year required");

			var user_rm = await _repo.Context.Users.Where(x => x.Status == StatusModel.Active && x.RoleId == 8).ToListAsync();
			foreach (var user in user_rm)
			{
				await UpdateTarget_SaleById(new() { userid = user.Id, year = year });
			}
		}

		public async Task<Dash_SalesPipelineModel> Get_SalesPipelineById(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var dash_SalesPipeline = new Dash_SalesPipelineModel();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var query = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active);

			var statusTotal = new List<SaleStatusGroupByModel>();

			query = await QueryArea(query, user);

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

			statusTotal = await query.GroupBy(info => info.StatusSaleId)
					   .Select(group => new SaleStatusGroupByModel()
					   {
						   StatusID = group.Key,
						   Count = group.Count()
					   }).OrderBy(x => x.StatusID).ToListAsync();

			if (statusTotal != null)
			{
				dash_SalesPipeline.CusAll = statusTotal.Sum(x => x.Count);
				dash_SalesPipeline.Interested = 0;
				dash_SalesPipeline.SubmitApproval = 0;
				dash_SalesPipeline.Approval = 0;
				dash_SalesPipeline.CloseSale = 0;

				foreach (var item in statusTotal)
				{
					if (item.StatusID == (int)StatusSaleModel.WaitMeet
						|| item.StatusID == (int)StatusSaleModel.Meet)
					{
						dash_SalesPipeline.Interested = dash_SalesPipeline.Interested + item.Count;
					}
					if (item.StatusID == (int)StatusSaleModel.WaitApproveLoanRequest) dash_SalesPipeline.SubmitApproval = item.Count;

					if (item.StatusID == (int)StatusSaleModel.WaitAPIPHOENIX
						|| item.StatusID == (int)StatusSaleModel.WaitCIF
						|| item.StatusID == (int)StatusSaleModel.WaitResults)
					{
						dash_SalesPipeline.Approval = dash_SalesPipeline.Approval + item.Count;
					}

					if (item.StatusID == (int)StatusSaleModel.CloseSale) dash_SalesPipeline.CloseSale = item.Count;

				}
			}

			return dash_SalesPipeline;
		}

		public async Task<Dash_AvgTop_NumberCustom> GetAvgTop_NumberById(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var dash_Avg_Number = new Dash_AvgTop_NumberCustom();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var saleQuery = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active);

			var sale_DurationsQuery = _repo.Context.Sale_Durations.Include(x => x.Sale)
												.Where(x => x.Status == StatusModel.Active)
												.OrderByDescending(x => x.CreateDate)
												.AsQueryable();

			saleQuery = await QueryArea(saleQuery, user);
			sale_DurationsQuery = await QueryAreaDuration(sale_DurationsQuery, user);

			if (model.startdate.HasValue && !model.enddate.HasValue)
			{
				saleQuery = saleQuery.Where(x => x.CreateDate.Date >= model.startdate.Value.Date).OrderByDescending(x => x.CreateDate);
				sale_DurationsQuery = sale_DurationsQuery.Where(x => x.Sale.CreateDate.Date >= model.startdate.Value.Date).OrderByDescending(x => x.CreateDate);
			}
			if (!model.startdate.HasValue && model.enddate.HasValue)
			{
				saleQuery = saleQuery.Where(x => x.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
				sale_DurationsQuery = sale_DurationsQuery.Where(x => x.Sale.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
			}
			if (model.startdate.HasValue && model.enddate.HasValue)
			{
				saleQuery = saleQuery.Where(x => x.CreateDate.Date >= model.startdate.Value.Date && x.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
				sale_DurationsQuery = sale_DurationsQuery.Where(x => x.Sale.CreateDate.Date >= model.startdate.Value.Date && x.Sale.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
			}

			//กิจการสาขาภาค[]
			if (model.DepBranchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToGuid(model.DepBranchs);
				if (idList.Count > 0)
				{
					saleQuery = saleQuery.Where(x => x.Master_Branch_RegionId.HasValue && idList.Contains(x.Master_Branch_RegionId));
					sale_DurationsQuery = sale_DurationsQuery.Where(x => x.Sale.Master_Branch_RegionId.HasValue && idList.Contains(x.Sale.Master_Branch_RegionId));
				}
			}

			//จังหวัด[]
			if (model.Provinces?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Provinces);
				if (idList.Count > 0)
				{
					saleQuery = saleQuery.Where(x => x.ProvinceId.HasValue && idList.Contains(x.ProvinceId));
					sale_DurationsQuery = sale_DurationsQuery.Where(x => x.Sale.ProvinceId.HasValue && idList.Contains(x.Sale.ProvinceId));
				}
			}

			//สาขา[]
			if (model.Branchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Branchs);
				if (idList.Count > 0)
				{
					saleQuery = saleQuery.Where(x => x.BranchId.HasValue && idList.Contains(x.BranchId));
					sale_DurationsQuery = sale_DurationsQuery.Where(x => x.Sale.BranchId.HasValue && idList.Contains(x.Sale.BranchId));
				}
			}

			var avgPerDeal = saleQuery.Select(s => s.LoanAmount).DefaultIfEmpty().Average();
			dash_Avg_Number.AvgPerDeal = avgPerDeal ?? 0;

			var avgTimeCloseSale = sale_DurationsQuery.Where(x => x.Sale.StatusSaleId == StatusSaleModel.CloseSale)
													 .Select(x => (x.WaitContact + x.Contact + x.Meet + x.Document + x.Result + x.CloseSale))
													 .DefaultIfEmpty()
													 .Average();
			dash_Avg_Number.AvgDurationCloseSale = (int)(avgTimeCloseSale);

			var avgTimeLostSale = sale_DurationsQuery.Where(x => x.Sale.StatusSaleId == StatusSaleModel.RMReturnMCenter
													 || x.Sale.StatusSaleId == StatusSaleModel.ResultsNotConsidered
													 || x.Sale.StatusSaleId == StatusSaleModel.CloseSaleNotLoan)
													 .Select(x => (x.WaitContact + x.Contact + x.Meet + x.Document + x.Result + x.CloseSale))
													 .DefaultIfEmpty()
													 .Average();
			dash_Avg_Number.AvgDurationLostSale = (int)(avgTimeLostSale);

			return dash_Avg_Number;
		}

		public async Task UpdateAvg_NumberById(allFilter model)
		{
			if (model.userid.HasValue)
			{
				var user = await _repo.User.GetById(model.userid.Value);
				if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
				var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

				if (user.Role.Code != null && !user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
				{
					var dash_Avg_Number = await _repo.Context.Dash_Avg_Numbers
																	   .FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.UserId == model.userid.Value);

					int CRUD = CRUDModel.Update;

					if (dash_Avg_Number == null)
					{
						CRUD = CRUDModel.Create;
						dash_Avg_Number = new();
						dash_Avg_Number.Status = StatusModel.Active;
						dash_Avg_Number.UserId = model.userid.Value;
					}

					var statusTotal = new List<SaleStatusGroupByModel>();

					if (user.Role.IsAssignRM)
					{
						var query = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active);

						query = await QueryArea(query, user);

						decimal? RatingAverage = await query.AverageAsync(r => r.LoanAmount);

						dash_Avg_Number.AvgPerDeal = RatingAverage ?? 0;
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
		}

		public async Task<Dash_AvgBottom_NumberCustom> GetAvgBottom_NumberById(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var dash_Avg_Number = new Dash_AvgBottom_NumberCustom();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var query = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active);
			var queryActcloseDeal = _repo.Context.Sale_Activities.Include(x => x.Sale).Where(x => x.Status == StatusModel.Active && x.Sale.StatusSaleId == StatusSaleModel.CloseSale);
			var queryDeliver = _repo.Context.Sale_Delivers.Include(x => x.Sale).Where(x => x.Status == StatusModel.Active);

			query = await QueryArea(query, user);
			queryActcloseDeal = await QueryAreaActivity(queryActcloseDeal, user);
			queryDeliver = await QueryAreaDeliver(queryDeliver, user);

			if (model.startdate.HasValue && !model.enddate.HasValue)
			{
				query = query.Where(x => x.CreateDate.Date >= model.startdate.Value.Date).OrderByDescending(x => x.CreateDate);
				queryActcloseDeal = queryActcloseDeal.Where(x => x.Sale.CreateDate.Date >= model.startdate.Value.Date).OrderByDescending(x => x.CreateDate);
				queryDeliver = queryDeliver.Where(x => x.Sale.CreateDate.Date >= model.startdate.Value.Date).OrderByDescending(x => x.CreateDate);
			}
			if (!model.startdate.HasValue && model.enddate.HasValue)
			{
				query = query.Where(x => x.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
				queryActcloseDeal = queryActcloseDeal.Where(x => x.Sale.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
				queryDeliver = queryDeliver.Where(x => x.Sale.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
			}
			if (model.startdate.HasValue && model.enddate.HasValue)
			{
				query = query.Where(x => x.CreateDate.Date >= model.startdate.Value.Date && x.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
				queryActcloseDeal = queryActcloseDeal.Where(x => x.Sale.CreateDate.Date >= model.startdate.Value.Date && x.Sale.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
				queryDeliver = queryDeliver.Where(x => x.Sale.CreateDate.Date >= model.startdate.Value.Date && x.Sale.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
			}

			//กิจการสาขาภาค[]
			if (model.DepBranchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToGuid(model.DepBranchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Master_Branch_RegionId.HasValue && idList.Contains(x.Master_Branch_RegionId));
					queryActcloseDeal = queryActcloseDeal.Where(x => x.Sale.Master_Branch_RegionId.HasValue && idList.Contains(x.Sale.Master_Branch_RegionId));
					queryDeliver = queryDeliver.Where(x => x.Sale.Master_Branch_RegionId.HasValue && idList.Contains(x.Sale.Master_Branch_RegionId));
				}
			}

			//จังหวัด[]
			if (model.Provinces?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Provinces);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.ProvinceId.HasValue && idList.Contains(x.ProvinceId));
					queryActcloseDeal = queryActcloseDeal.Where(x => x.Sale.ProvinceId.HasValue && idList.Contains(x.Sale.ProvinceId));
					queryDeliver = queryDeliver.Where(x => x.Sale.ProvinceId.HasValue && idList.Contains(x.Sale.ProvinceId));
				}
			}

			//สาขา[]
			if (model.Branchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Branchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.BranchId.HasValue && idList.Contains(x.BranchId));
					queryActcloseDeal = queryActcloseDeal.Where(x => x.Sale.BranchId.HasValue && idList.Contains(x.Sale.BranchId));
					queryDeliver = queryDeliver.Where(x => x.Sale.BranchId.HasValue && idList.Contains(x.Sale.BranchId));
				}
			}

			try
			{
				var avgDealBranch = query.Where(x => x.BranchId.HasValue).GroupBy(m => m.BranchId)
											.Select(group => new
											{
												GroupID = group.Key,
												Count = group.Count(),
											})
											.DefaultIfEmpty()
											.Average(a => a.Count);

				dash_Avg_Number.AvgDealBranch = (int)avgDealBranch;

			}
			catch { }

			try
			{
				var avgSaleActcloseDeal = queryActcloseDeal.Select(x => (x.Contact + x.Meet + x.Document))
													 .DefaultIfEmpty()
													 .Average();

				dash_Avg_Number.AvgSaleActcloseDeal = (int)avgSaleActcloseDeal;
			}
			catch { }

			try
			{
				var avgDeliveryDuration = queryDeliver.Select(x => (x.LoanToBranchReg + x.BranchRegToCenBranch + x.CenBranchToRM + x.CloseSale))
													 .DefaultIfEmpty()
													 .Average();

				dash_Avg_Number.AvgDeliveryDuration = (int)avgDeliveryDuration;
			}
			catch { }

			try
			{
				double avgDealRM = 0;
				var avgDealRMQuery = query.Where(x => x.AssUserId.HasValue).FirstOrDefault();
				if (avgDealRMQuery != null)
				{
					avgDealRM = query.GroupBy(m => m.AssUserId)
											.Select(group => new
											{
												GroupID = group.Key,
												Count = group.Count(),
											})
											.DefaultIfEmpty()
											.Average(a => a.Count);
				}
				dash_Avg_Number.AvgDealRM = (int)(avgDealRM);
			}
			catch { }

			return dash_Avg_Number;
		}

		public async Task<PaginationView<List<GroupByModel>>> GetListDealBranchById(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var dash_SalesPipeline = new Dash_SalesPipelineModel();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var query = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.BranchId.HasValue);

			query = await QueryArea(query, user);

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

			var queryUse = query.GroupBy(m => m.BranchId)
										 .Select(group => new GroupByModel()
										 {
											 GroupID = group.Key.ToString() ?? string.Empty,
											 Name = group.First().BranchName,
											 Value = group.Count()
										 });

			var pager = new Pager(queryUse.Count(), model.page, model.pagesize, null);

			var items = queryUse.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<GroupByModel>>()
			{
				Items = _mapper.Map<List<GroupByModel>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task<PaginationView<List<SaleGroupByModel>>> GetListDealRMById(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var dash_SalesPipeline = new Dash_SalesPipelineModel();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var query = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.AssUserId.HasValue);

			query = await QueryArea(query, user);

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

			if (!String.IsNullOrEmpty(model.searchtxt))
				query = query.Where(x => x.AssUserName != null && x.AssUserName.Contains(model.searchtxt));

			if (model.provinceid.HasValue)
				query = query.Where(x => x.ProvinceId.HasValue && x.ProvinceId == model.provinceid.Value);

			var queryUse = query.GroupBy(m => m.AssUserId)
					 .Select(group => new SaleGroupByModel()
					 {
						 GroupID = group.Key.ToString(),
						 Sales = _mapper.Map<List<SaleCustom>>(group.ToList())
					 });

			var pager = new Pager(queryUse.Count(), model.page, model.pagesize, null);

			var items = queryUse.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<SaleGroupByModel>>()
			{
				Items = _mapper.Map<List<SaleGroupByModel>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task<PaginationView<List<Dash_Map_ThailandCustom>>> GetTopSale(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var query = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.StatusSaleId == StatusSaleModel.CloseSale && x.LoanAmount > 0);

			query = await QueryArea(query, user);

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

			var queryGroup = query.GroupBy(m => m.ProvinceId)
											 .Select(group => new Dash_Map_ThailandCustom()
											 {
												 Type = 1,
												 ProvinceId = group.Key ?? 0,
												 ProvinceName = group.First().ProvinceName ?? string.Empty,
												 SalesAmount = group.Sum(s => s.LoanAmount) ?? 0
											 })
											 .Where(x => x.ProvinceId > 0)
											 .OrderByDescending(x => x.SalesAmount);

			if (!String.IsNullOrEmpty(model.sort))
			{
				if (model.sort == OrderByModel.ASC)
				{
					queryGroup = queryGroup.OrderBy(x => x.SalesAmount);
				}
				else if (model.sort == OrderByModel.DESC)
				{
					queryGroup = queryGroup.OrderByDescending(x => x.SalesAmount);
				}
			}

			var pager = new Pager(queryGroup.Count(), model.page, model.pagesize, null);

			var items = queryGroup.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Dash_Map_ThailandCustom>>()
			{
				Items = _mapper.Map<List<Dash_Map_ThailandCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task<PaginationView<List<Dash_Map_ThailandCustom>>> GetLostSale(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();


			var query = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active
			&& (x.StatusSaleId == StatusSaleModel.CloseSaleNotLoan
			 || x.StatusSaleId == StatusSaleModel.ResultsNotConsidered
			 || x.StatusSaleId == StatusSaleModel.CloseSaleFail));

			query = await QueryArea(query, user);

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

			var queryGroup = query.GroupBy(m => m.ProvinceId)
										 .Select(group => new Dash_Map_ThailandCustom()
										 {
											 Type = 1,
											 ProvinceId = group.Key ?? 0,
											 ProvinceName = group.First().ProvinceName ?? string.Empty,
											 SalesAmount = group.Sum(s => s.LoanAmount) ?? 0
										 })
										 .OrderBy(x => x.SalesAmount).AsQueryable();

			if (!String.IsNullOrEmpty(model.sort))
			{
				if (model.sort == OrderByModel.ASC)
				{
					queryGroup = queryGroup.OrderBy(x => x.SalesAmount);
				}
				else if (model.sort == OrderByModel.DESC)
				{
					queryGroup = queryGroup.OrderByDescending(x => x.SalesAmount);
				}
			}

			var pager = new Pager(queryGroup.Count(), model.page, model.pagesize, null);

			var items = queryGroup.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Dash_Map_ThailandCustom>>()
			{
				Items = _mapper.Map<List<Dash_Map_ThailandCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task<Dash_Avg_NumberOnStage> GetAvgOnStage(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var dash_Avg_NumberOnStage = new Dash_Avg_NumberOnStage();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var query = _repo.Context.Sale_Durations.Include(x => x.Sale)
												.Where(x => x.Status == StatusModel.Active)
												.OrderByDescending(x => x.CreateDate)
												.AsQueryable();

			query = await QueryAreaDuration(query, user);

			if (model.startdate.HasValue && !model.enddate.HasValue)
			{
				query = query.Where(x => x.Sale.CreateDate.Date >= model.startdate.Value.Date).OrderByDescending(x => x.CreateDate);
			}
			if (!model.startdate.HasValue && model.enddate.HasValue)
			{
				query = query.Where(x => x.Sale.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
			}
			if (model.startdate.HasValue && model.enddate.HasValue)
			{
				query = query.Where(x => x.Sale.CreateDate.Date >= model.startdate.Value.Date && x.Sale.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
			}

			//กิจการสาขาภาค[]
			if (model.DepBranchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToGuid(model.DepBranchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Sale.Master_Branch_RegionId.HasValue && idList.Contains(x.Sale.Master_Branch_RegionId));
				}
			}

			//จังหวัด[]
			if (model.Provinces?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Provinces);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Sale.ProvinceId.HasValue && idList.Contains(x.Sale.ProvinceId));
				}
			}

			//สาขา[]
			if (model.Branchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Branchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Sale.BranchId.HasValue && idList.Contains(x.Sale.BranchId));
				}
			}

			var avgContact = query.Select(x => (x.WaitContact + x.Contact))
												.DefaultIfEmpty()
												.Average();
			dash_Avg_NumberOnStage.Contact = double.Round(avgContact, 2, MidpointRounding.AwayFromZero);

			var avgMeet = query.Select(x => x.Meet)
											.DefaultIfEmpty()
											.Average();
			dash_Avg_NumberOnStage.Meet = double.Round(avgMeet, 2, MidpointRounding.AwayFromZero);

			var avgDocument = query.Select(x => x.Document)
											.DefaultIfEmpty()
											.Average();
			dash_Avg_NumberOnStage.Document = double.Round(avgDocument, 2, MidpointRounding.AwayFromZero);

			var avgResult = query.Select(x => x.Result)
											.DefaultIfEmpty()
											.Average();
			dash_Avg_NumberOnStage.Result = double.Round(avgResult, 2, MidpointRounding.AwayFromZero);

			var avgCloseSaleFail = query.Where(x => x.Sale.StatusSaleId == StatusSaleModel.RMReturnMCenter
													 || x.Sale.StatusSaleId == StatusSaleModel.ResultsNotConsidered
													 || x.Sale.StatusSaleId == StatusSaleModel.CloseSaleNotLoan)
											.Select(x => (x.WaitContact + x.Contact + x.Meet + x.Document + x.Result + x.CloseSale))
											.DefaultIfEmpty()
											.Average();
			dash_Avg_NumberOnStage.CloseSaleFail = double.Round(avgCloseSaleFail, 2, MidpointRounding.AwayFromZero);

			return dash_Avg_NumberOnStage;
		}

		public async Task<List<Dash_PieCustom>> GetPieCloseSaleReason(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var response = new List<Dash_PieCustom>();

			if (user.Role.Code != null && !user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				response = await GetDataPieCloseSale(model, response);
				response = await GetDataPieReasonFail(model, response);
			}

			return response;
		}

		public async Task<User_Target_SaleCustom> GetSumTargetActual(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var response = new User_Target_SaleCustom();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var queryTarget = _repo.Context.User_Target_Sales.Include(x => x.User).Where(x => x.Status == StatusModel.Active);

			var query = _repo.Context.Sales
				.Include(x => x.AssUser).ThenInclude(x => x.User_Target_SaleUsers)
				.Where(x => x.Status == StatusModel.Active && x.AssUser != null);

			query = await QueryArea(query, user);

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

			//var sumTarget = query.Sum(x=> x.AmountTarget);
			//var sumActual = query.Sum(x=>x.AmountActual);

			int _year = DateTime.Now.Year;
			if (model.startdate.HasValue)
			{
				_year = model.startdate.Value.Year;
			}

			//99999999-9999-9999-9999-999999999999 เห็นทั้งประเทศ
			if (!user.Role.IsAssignCenter && user.Master_Branch_RegionId != Guid.Parse("99999999-9999-9999-9999-999999999999"))
			{
				//9999 เห็นทุกจังหวัดในภาค
				if (user.Master_Branch_RegionId.HasValue && user_Areas.Any(x => x == 9999))
				{
					queryTarget = queryTarget.Where(x => x.User.Master_Branch_RegionId == user.Master_Branch_RegionId);
				}
				else
				{
					//เห็นเฉพาะจังหวัดที่ดูแล
					queryTarget = queryTarget.Where(x => x.User.ProvinceId.HasValue && user_Areas.Contains(x.User.ProvinceId.Value));
				}
			}

			var sumTarget = queryTarget.Where(x => x.Year == _year).Sum(s => s.AmountTarget);

			var sumActual = query.Sum(x => x.LoanAmount ?? 0);

			response.Status = StatusModel.Active;
			response.UserId = user.Id;
			response.Year = _year;
			response.AmountTarget = sumTarget;
			response.AmountActual = sumActual;

			return response;
		}

		private async Task<List<Dash_PieCustom>> GetDataPieCloseSale(allFilter model, List<Dash_PieCustom> response)
		{
			if (!model.userid.HasValue) return new();

			var dash_SalesPipeline = new Dash_SalesPipelineModel();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var query = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active);
			var queryCloseSale = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.StatusSaleId == StatusSaleModel.CloseSale);

			query = await QueryArea(query, user);
			queryCloseSale = await QueryArea(queryCloseSale, user);

			if (model.startdate.HasValue && !model.enddate.HasValue)
			{
				query = query.Where(x => x.CreateDate.Date >= model.startdate.Value.Date).OrderByDescending(x => x.CreateDate);
				queryCloseSale = queryCloseSale.Where(x => x.CreateDate.Date >= model.startdate.Value.Date).OrderByDescending(x => x.CreateDate);
			}
			if (!model.startdate.HasValue && model.enddate.HasValue)
			{
				query = query.Where(x => x.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
				queryCloseSale = queryCloseSale.Where(x => x.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
			}
			if (model.startdate.HasValue && model.enddate.HasValue)
			{
				query = query.Where(x => x.CreateDate.Date >= model.startdate.Value.Date && x.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
				queryCloseSale = queryCloseSale.Where(x => x.CreateDate.Date >= model.startdate.Value.Date && x.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
			}

			//กิจการสาขาภาค[]
			if (model.DepBranchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToGuid(model.DepBranchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Master_Branch_RegionId.HasValue && idList.Contains(x.Master_Branch_RegionId));
					queryCloseSale = queryCloseSale.Where(x => x.Master_Branch_RegionId.HasValue && idList.Contains(x.Master_Branch_RegionId));
				}
			}

			//จังหวัด[]
			if (model.Provinces?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Provinces);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.ProvinceId.HasValue && idList.Contains(x.ProvinceId));
					queryCloseSale = queryCloseSale.Where(x => x.ProvinceId.HasValue && idList.Contains(x.ProvinceId));
				}
			}

			//สาขา[]
			if (model.Branchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Branchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.BranchId.HasValue && idList.Contains(x.BranchId));
					queryCloseSale = queryCloseSale.Where(x => x.BranchId.HasValue && idList.Contains(x.BranchId));
				}
			}


			var salesAllCount = await query.CountAsync();
			var salesCloseSaleCount = await queryCloseSale.CountAsync();

			//salesAllCount = 50;
			//salesCloseSaleCount = 47;

			if (salesAllCount > 0 || salesCloseSaleCount > 0)
			{
				var sumSuccessFail = salesCloseSaleCount + salesAllCount;
				var perSuccess = ((decimal)salesCloseSaleCount) * 100 / sumSuccessFail;
				var perFail = ((decimal)salesAllCount) * 100 / sumSuccessFail;

				perSuccess = decimal.Round(perSuccess, 2, MidpointRounding.AwayFromZero);
				perFail = decimal.Round(perFail, 2, MidpointRounding.AwayFromZero);

				response.Add(new()
				{
					Status = StatusModel.Active,
					Code = Dash_PieCodeModel.ClosingSale,
					Name = "ปิดการขายสำเร็จ",
					Value = salesCloseSaleCount,
					//Percent = perSuccess
				});
				response.Add(new()
				{
					Status = StatusModel.Active,
					Code = Dash_PieCodeModel.ClosingSale,
					Name = "ปิดการขายไม่สำเร็จ",
					Value = salesAllCount,
					//Percent = perFail
				});

			}

			return response;
		}

		private async Task<List<Dash_PieCustom>> GetDataPieReasonFail(allFilter model, List<Dash_PieCustom> response)
		{
			if (!model.userid.HasValue) return new();

			var dash_SalesPipeline = new Dash_SalesPipelineModel();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var query = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.StatusSaleId == StatusSaleModel.CloseSaleNotLoan && x.Master_Reason_CloseSaleId.HasValue);

			query = await QueryArea(query, user);

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

			var queryUse = await query.GroupBy(m => m.Master_Reason_CloseSaleId)
										 .Select(group => new GroupByModel()
										 {
											 GroupID = group.Key.ToString() ?? string.Empty,
											 Name = group.First().StatusDescription,
											 Value = group.Count()
										 })
										 .OrderByDescending(x => x.Value).Take(6).ToListAsync();

			if (queryUse.Count > 0)
			{
				foreach (var item in queryUse)
				{
					response.Add(new()
					{
						Status = StatusModel.Active,
						Code = Dash_PieCodeModel.ReasonNotLoan,
						Name = item.Name,
						Value = item.Value
					});
				}
			}

			return response;
		}

		public async Task<List<Dash_PieCustom>> GetPieNumberCustomer(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var response = new List<Dash_PieCustom>();

			var query = _repo.Context.Sales.Include(s => s.Customer).Where(x => x.Status == StatusModel.Active);

			query = await QueryArea(query, user);

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

			var salesBusinessSize = query.GroupBy(m => m.Customer.Master_BusinessSizeId)
											 .Select(group => new { GroupID = group.Key, Customers = group.ToList() })
											 .Where(x => x.GroupID.HasValue)
											 .ToList();

			if (salesBusinessSize.Count > 0)
			{
				var useLoop = salesBusinessSize.OrderByDescending(x => x.Customers.Count).Take(6);
				foreach (var item in useLoop)
				{
					response.Add(new()
					{
						Status = StatusModel.Active,
						Code = Dash_PieCodeModel.NumCusSizeBusiness,
						TitleName = "จำนวนลูกค้าตามขนาดธุรกิจ",
						Name = $"{item.Customers.Select(x => x.Customer.Master_BusinessSizeName).FirstOrDefault()} ",
						Value = item.Customers.Count
					});
				}
			}

			var salesBusinessType = query.GroupBy(m => m.Customer.Master_BusinessTypeId)
										 .Select(group => new { GroupID = group.Key, Customers = group.ToList() })
										 .Where(x => x.GroupID.HasValue)
										 .ToList();
			if (salesBusinessType.Count > 0)
			{
				var useLoop = salesBusinessType.OrderByDescending(x => x.Customers.Count).Take(6);
				foreach (var item in useLoop)
				{
					response.Add(new()
					{
						Status = StatusModel.Active,
						Code = Dash_PieCodeModel.NumCusTypeBusiness,
						TitleName = "จำนวนลูกค้าตามประเภทกิจการ",
						Name = $"{item.Customers.Select(x => x.Customer.Master_BusinessTypeName).FirstOrDefault()} ",
						Value = item.Customers.Count
					});
				}
			}

			var salesISICCode = query.GroupBy(m => m.Customer.Master_ISICCodeId)
										 .Select(group => new { GroupID = group.Key, Customers = group.ToList() })
											 .Where(x => x.GroupID.HasValue)
										 .ToList();
			if (salesISICCode.Count > 0)
			{
				var useLoop = salesISICCode.OrderByDescending(x => x.Customers.Count).Take(6);
				foreach (var item in useLoop)
				{
					response.Add(new()
					{
						Status = StatusModel.Active,
						Code = Dash_PieCodeModel.NumCusISICCode,
						TitleName = "จำนวนลูกค้าตาม ISIC Code",
						Name = $"{item.Customers.Select(x => x.Customer.Master_ISICCodeName).FirstOrDefault()} ",
						Value = item.Customers.Count
					});
				}
			}

			var salesLoanType = query.GroupBy(m => m.Customer.Master_LoanTypeId)
										 .Select(group => new { GroupID = group.Key, Customers = group.ToList() })
										 .ToList();
			if (salesLoanType.Count > 0)
			{
				var useLoop = salesLoanType.OrderByDescending(x => x.Customers.Count).Take(6);
				foreach (var item in useLoop)
				{
					response.Add(new()
					{
						Status = StatusModel.Active,
						Code = Dash_PieCodeModel.NumCusLoanType,
						TitleName = "จำนวนลูกค้าตามประเภทสินเชื่อ",
						Name = $"{item.Customers.Select(x => x.Customer.Master_LoanTypeName).FirstOrDefault()} ",
						Value = item.Customers.Count
					});
				}
			}

			return response;
		}

		public async Task<List<Dash_PieCustom>> GetListNumberCustomer(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var response = new List<Dash_PieCustom>();

			var query = _repo.Context.Sales.Include(s => s.Customer).Where(x => x.Status == StatusModel.Active);

			query = await QueryArea(query, user);

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

			if (model.code == Dash_PieCodeModel.NumCusSizeBusiness)
			{
				var salesBusinessSize = query.GroupBy(m => m.Customer.Master_BusinessSizeId)
											 .Select(group => new { GroupID = group.Key, Customers = group.ToList() })
											 .Where(x => x.GroupID.HasValue)
											 .ToList();

				if (salesBusinessSize.Count > 0)
				{
					var useLoop = salesBusinessSize.OrderByDescending(x => x.Customers.Count);
					foreach (var item in useLoop)
					{
						response.Add(new()
						{
							Status = StatusModel.Active,
							Code = Dash_PieCodeModel.NumCusSizeBusiness,
							TitleName = "จำนวนลูกค้าตามขนาดธุรกิจ",
							Name = $"{item.Customers.Select(x => x.Customer.Master_BusinessSizeName).FirstOrDefault()} ",
							Value = item.Customers.Count
						});
					}
				}
			}
			else if (model.code == Dash_PieCodeModel.NumCusTypeBusiness)
			{
				var salesBusinessType = query.GroupBy(m => m.Customer.Master_BusinessTypeId)
											 .Select(group => new { GroupID = group.Key, Customers = group.ToList() })
											 .Where(x => x.GroupID.HasValue)
											 .ToList();
				if (salesBusinessType.Count > 0)
				{
					var useLoop = salesBusinessType.OrderByDescending(x => x.Customers.Count);
					foreach (var item in useLoop)
					{
						response.Add(new()
						{
							Status = StatusModel.Active,
							Code = Dash_PieCodeModel.NumCusTypeBusiness,
							TitleName = "จำนวนลูกค้าตามประเภทธุรกิจ",
							Name = $"{item.Customers.Select(x => x.Customer.Master_BusinessTypeName).FirstOrDefault()} ",
							Value = item.Customers.Count
						});
					}
				}
			}
			else if (model.code == Dash_PieCodeModel.NumCusISICCode)
			{
				var salesISICCode = query.GroupBy(m => m.Customer.Master_ISICCodeId)
											 .Select(group => new { GroupID = group.Key, Customers = group.ToList() })
											 .Where(x => x.GroupID.HasValue)
											 .ToList();
				if (salesISICCode.Count > 0)
				{
					var useLoop = salesISICCode.OrderByDescending(x => x.Customers.Count);
					foreach (var item in useLoop)
					{
						response.Add(new()
						{
							Status = StatusModel.Active,
							Code = Dash_PieCodeModel.NumCusISICCode,
							TitleName = "จำนวนลูกค้าตาม ISIC Code",
							Name = $"{item.Customers.Select(x => x.Customer.Master_ISICCodeName).FirstOrDefault()} ",
							Value = item.Customers.Count
						});
					}
				}

			}
			else if (model.code == Dash_PieCodeModel.NumCusLoanType)
			{
				var salesLoanType = query.GroupBy(m => m.Customer.Master_LoanTypeId)
											 .Select(group => new { GroupID = group.Key, Customers = group.ToList() })
											 .Where(x => x.GroupID.HasValue)
											 .ToList();
				if (salesLoanType.Count > 0)
				{
					var useLoop = salesLoanType.OrderByDescending(x => x.Customers.Count);
					foreach (var item in useLoop)
					{
						response.Add(new()
						{
							Status = StatusModel.Active,
							Code = Dash_PieCodeModel.NumCusLoanType,
							TitleName = "จำนวนลูกค้าตามประเภทสินเชื่อ",
							Name = $"{item.Customers.Select(x => x.Customer.Master_LoanTypeName).FirstOrDefault()} ",
							Value = item.Customers.Count
						});
					}
				}

			}

			return response;
		}

		public async Task<List<Dash_PieCustom>> GetPieLoanValue(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var response = new List<Dash_PieCustom>();

			var query = _repo.Context.Sales.Include(s => s.Customer).Where(x => x.Status == StatusModel.Active);

			query = await QueryArea(query, user);

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

			var salesBusinessSize = query.GroupBy(m => m.Customer.Master_BusinessSizeId)
												 .Select(group => new { GroupID = group.Key, Sales = group.ToList() })
												 .ToList();

			if (salesBusinessSize.Count > 0)
			{
				var useLoop = salesBusinessSize.OrderByDescending(x => x.Sales.Count).Take(6);
				foreach (var item in useLoop)
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

			response = await GetDataTypeBusiness(model, response);

			var salesISICCode = query.Where(x => x.LoanAmount > 0)
										 .GroupBy(m => m.Customer.Master_ISICCodeId)
										 .Select(group => new { GroupID = group.Key, Sales = group.ToList() })
										 .ToList();
			if (salesISICCode.Count > 0)
			{
				var useLoop = salesISICCode.OrderByDescending(x => x.Sales.Count).Take(6);
				foreach (var item in useLoop)
				{
					response.Add(new()
					{
						Status = StatusModel.Active,
						Code = Dash_PieCodeModel.ValueISICCode,
						TitleName = "มูลค่าสินเชื่อตาม ISIC Code",
						Name = $"{item.Sales.Select(x => x.Customer.Master_ISICCodeName).FirstOrDefault()} ",
						Value = item.Sales.Sum(s => s.LoanAmount)
					});
				}
			}

			var salesLoanType = query.GroupBy(m => m.Customer.Master_LoanTypeId)
										 .Select(group => new { GroupID = group.Key, Sales = group.ToList() })
										 .ToList();
			if (salesLoanType.Count > 0)
			{
				var useLoop = salesLoanType.OrderByDescending(x => x.Sales.Count).Take(6);
				foreach (var item in useLoop)
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

			return response;
		}

		private async Task<List<Dash_PieCustom>> GetDataTypeBusiness(allFilter model, List<Dash_PieCustom> response)
		{
			if (!model.userid.HasValue) return new();

			var dash_SalesPipeline = new Dash_SalesPipelineModel();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var query = _repo.Context.Sales.Include(s => s.Customer).Where(x => x.Status == StatusModel.Active);

			query = await QueryArea(query, user);

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


			var queryGroupBy = await query.GroupBy(m => m.Customer.Master_BusinessTypeId)
										 .Select(group => new { GroupID = group.Key, Sales = group.ToList() }).ToListAsync();

			//var salesBusinessTypeUse = await queryGroupBy.ToListAsync();
			if (queryGroupBy.Count > 0)
			{
				//var useLoop = salesBusinessTypeUse.OrderByDescending(x => x.Sales.Count).Take(6);
				foreach (var item in queryGroupBy)
				{
					response.Add(new()
					{
						Status = StatusModel.Active,
						Code = Dash_PieCodeModel.ValueTypeBusiness,
						//TitleName = "มูลค่าสินเชื่อตามประเภทธุรกิจ",
						Name = $"{item.Sales.Select(x => x.Customer.Master_BusinessTypeName).FirstOrDefault()} ",
						Value = item.Sales.Sum(s => s.LoanAmount)
					});
				}
			}

			return response;
		}

		public async Task<PaginationView<List<Sale_DurationCustom>>> GetDuration(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			if (user.Role.Code.StartsWith(RoleCodes.RM)) return new();

			IQueryable<Sale_Duration> query;

			query = _repo.Context.Sale_Durations.Include(x => x.Sale).ThenInclude(x => x.Customer)
												.Where(x => x.Status != StatusModel.Delete)
												.OrderByDescending(x => x.CreateDate)
												.AsQueryable();


			query = await QueryAreaDuration(query, user);

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
															 || x.Sale.StatusSaleId == StatusSaleModel.CloseSaleNotLoan);
				}
			}

			if (model.contactstartdate.HasValue)
			{
				query = query.Where(x => x.ContactStartDate.HasValue && (x.ContactStartDate.Value.Date == model.contactstartdate.Value.Date)).OrderByDescending(x => x.CreateDate);
			}

			if (int.TryParse(model.contact, out int _contact))
				query = query.Where(x => x.Contact == _contact);

			if (int.TryParse(model.meet, out int _meet))
				query = query.Where(x => x.Meet == _meet);

			if (int.TryParse(model.document, out int _document))
				query = query.Where(x => x.Document == _document);

			if (!String.IsNullOrEmpty(model.juristicnumber))
				query = query.Where(x => x.Sale.Customer.JuristicPersonRegNumber != null && x.Sale.Customer.JuristicPersonRegNumber.Contains(model.juristicnumber));

			if (!String.IsNullOrEmpty(model.searchtxt))
				query = query.Where(x => x.Sale.CompanyName != null && x.Sale.CompanyName.Contains(model.searchtxt));

			if (!String.IsNullOrEmpty(model.contact_name))
				query = query.Where(x => x.ContactName != null && x.ContactName.Contains(model.contact_name));

			if (!String.IsNullOrEmpty(model.assignrm_name))
				query = query.Where(x => x.Sale.AssUserName != null && x.Sale.AssUserName.Contains(model.assignrm_name));

			if (model.Branchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Branchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Sale.BranchId.HasValue && idList.Contains(x.Sale.BranchId));
				}
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Sale_DurationCustom>>()
			{
				Items = _mapper.Map<List<Sale_DurationCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task UpdateDurationById(allFilter model)
		{
			if (model.saleid.HasValue)
			{
				var sale_Durations = await _repo.Context.Sale_Durations.FirstOrDefaultAsync(x => x.SaleId == model.saleid.Value);

				int CRUD = CRUDModel.Update;

				if (sale_Durations == null)
				{
					CRUD = CRUDModel.Create;
					sale_Durations = new();
					sale_Durations.Status = StatusModel.Active;
					sale_Durations.CreateDate = DateTime.Now;
					sale_Durations.SaleId = model.saleid.Value;
				}

				//วันที่เริ่มติดต่อ
				DateTime contactFirst = DateTime.MinValue;
				var sales = await _repo.Context.Sales.Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == model.saleid.Value);
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
				sale_Durations.WaitMeet = 0;
				sale_Durations.Meet = 0;
				sale_Durations.Document = 0;
				sale_Durations.Result = 0;
				sale_Durations.CloseSale = 0;

				var sale_Status = await _repo.Context.Sale_Statuses.Where(x => x.SaleId == model.saleid.Value).ToListAsync();
				if (sale_Status.Count > 0)
				{

					#region "sale_Durations old"
					//var waitContactLast = sale_Status.Where(x => x.StatusId == StatusSaleModel.WaitContact).OrderByDescending(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
					//if (contactFirst != DateTime.MinValue && waitContactLast != DateTime.MinValue)
					//	sale_Durations.WaitContact = (int)(contactFirst - waitContactLast).TotalDays;

					//var contactLast = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Contact).OrderByDescending(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
					//var meetFirst = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Meet).OrderBy(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
					//if (meetFirst != DateTime.MinValue && contactLast != DateTime.MinValue)
					//	sale_Durations.Contact = (int)(meetFirst - contactLast).TotalDays;

					//var waitMeetLast = sale_Status.Where(x => x.StatusMainId == StatusSaleModel.WaitMeet).OrderByDescending(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
					//if (meetFirst != DateTime.MinValue && waitMeetLast != DateTime.MinValue)
					//	sale_Durations.WaitMeet = (int)(meetFirst - waitMeetLast).TotalDays;

					//var meetLast = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Meet).OrderByDescending(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
					//var documentFirst = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Document).OrderBy(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
					//if (documentFirst != DateTime.MinValue && meetLast != DateTime.MinValue)
					//	sale_Durations.Meet = (int)(documentFirst - meetLast).TotalDays;

					//var documentLast = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Document).OrderByDescending(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
					//var resultFirst = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Result).OrderBy(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
					//if (resultFirst != DateTime.MinValue && documentLast != DateTime.MinValue)
					//	sale_Durations.Document = (int)(resultFirst - documentLast).TotalDays;

					//var resultLast = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Result).OrderByDescending(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
					//var closeSaleFirst = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.CloseSale).OrderBy(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
					//if (closeSaleFirst != DateTime.MinValue && resultLast != DateTime.MinValue)
					//	sale_Durations.Result = (int)(closeSaleFirst - resultLast).TotalDays;
					#endregion

					//รอการติตต่อ
					var waitContact = sale_Status.Where(x => x.StatusId == StatusSaleModel.WaitContact).Select(x => x.CreateDate.Date).FirstOrDefault();
					//รอเข้าพบ
					var waitMeet = sale_Status.Where(x => x.StatusId == StatusSaleModel.WaitMeet).Select(x => x.CreateDate.Date).FirstOrDefault();
					//เริ่มต้นเข้าพบ
					var meetFirst = sale_Status.Where(x => x.StatusId == StatusSaleModel.Meet).OrderBy(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
					//เข้าพบล่าสุด
					var meetLast = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Meet).OrderByDescending(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
					//วันที่เริ่มยื่นเอกสาร
					var documentFirst = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.Document).OrderBy(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
					//รอบันทึกผลลัพธ์
					var waitResults = sale_Status.Where(x => x.StatusId == StatusSaleModel.WaitResults).Select(x => x.CreateDate.Date).FirstOrDefault();
					//วันที่เริ่มบันทึกผลลัพธ์
					var resultsFirst = sale_Status.Where(x => x.StatusId == StatusSaleModel.Results).OrderBy(x => x.CreateDate).Select(x => x.CreateDate.Date).FirstOrDefault();
					//รอปิดการขาย
					var waitcloseSale = sale_Status.Where(x => x.StatusId == StatusSaleModel.WaitCloseSale).Select(x => x.CreateDate.Date).FirstOrDefault();
					//ปิดการขาย
					var closeSale = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.CloseSale).Select(x => x.CreateDate.Date).FirstOrDefault();

					//WaitContact(รอติดต่อ) = รอการติตต่อ -> วันที่เริ่มติดต่อ  cal(วันที่เริ่มติดต่อ-รอการติตต่อ)
					if (waitContact != DateTime.MinValue && contactFirst != DateTime.MinValue)
						sale_Durations.WaitContact = (int)(contactFirst - waitContact).TotalDays;

					//Contact(ติดต่อ) = วันที่เริ่มติดต่อ -> รอเข้าพบ  cal(รอเข้าพบ-วันที่เริ่มติดต่อ)
					if (contactFirst != DateTime.MinValue && waitMeet != DateTime.MinValue)
						sale_Durations.Contact = (int)(waitMeet - contactFirst).TotalDays;

					//WaitMeet(รอเข้าพบ) = รอเข้าพบ -> เริ่มต้นเข้าพบ  cal(เริ่มต้นเข้าพบ-รอเข้าพบ)
					if (waitMeet != DateTime.MinValue && meetFirst != DateTime.MinValue)
						sale_Durations.WaitMeet = (int)(meetFirst - waitMeet).TotalDays;

					//Meet(เข้าพบ) = เริ่มต้นเข้าพบ -> วันที่เริ่มยื่นเอกสาร  cal(วันที่เริ่มยื่นเอกสาร-เริ่มต้นเข้าพบ)
					if (meetFirst != DateTime.MinValue && documentFirst != DateTime.MinValue)
						sale_Durations.Meet = (int)(documentFirst - meetFirst).TotalDays;

					//Document(ยื่นเอกสาร) = วันที่เริ่มยื่นเอกสาร -> รอบันทึกผลลัพธ์  cal(รอบันทึกผลลัพธ์-วันที่เริ่มยื่นเอกสาร)
					if (documentFirst != DateTime.MinValue && waitResults != DateTime.MinValue)
						sale_Durations.Document = (int)(waitResults - documentFirst).TotalDays;

					//Result(ผลลัพธ์) = รอบันทึกผลลัพธ์ -> รอปิดการขาย  cal(ปิดการขาย-รอบันทึกผลลัพธ์)
					if (waitResults != DateTime.MinValue && waitcloseSale != DateTime.MinValue)
						sale_Durations.Result = (int)(waitcloseSale - waitResults).TotalDays;

					//CloseSale(ปิดการขาย) = รอปิดการขาย -> ปิดการขาย  cal(ปิดการขาย-รอปิดการขาย)
					if (waitcloseSale != DateTime.MinValue && closeSale != DateTime.MinValue)
						sale_Durations.CloseSale = (int)(closeSale - waitcloseSale).TotalDays;

					sale_Durations.ContactStartDate = contactFirst;

					//cal(ปิดการขาย-รอการติตต่อ)
					sale_Durations.TotalDay = (int)(closeSale - waitContact).TotalDays;
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
		}

		public async Task<PaginationView<List<Sale_ActivityCustom>>> GetActivity(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			if (user.Role.Code.StartsWith(RoleCodes.RM)) return new();

			IQueryable<Sale_Activity> query;

			query = _repo.Context.Sale_Activities.Include(x => x.Sale)
												.Where(x => x.Status != StatusModel.Delete)
												.OrderByDescending(x => x.CreateDate)
												.AsQueryable();

			query = query.Where(x => x.Sale.StatusSaleId == StatusSaleModel.CloseSale);

			query = await QueryAreaActivity(query, user);

			if (!String.IsNullOrEmpty(model.searchtxt))
				query = query.Where(x => x.Sale.CompanyName != null && x.Sale.CompanyName.Contains(model.searchtxt));

			if (!String.IsNullOrEmpty(model.contact_name))
				query = query.Where(x => x.ContactName != null && x.ContactName.Contains(model.contact_name));

			if (!String.IsNullOrEmpty(model.assignrm_name))
				query = query.Where(x => x.Sale.AssUserName != null && x.Sale.AssUserName.Contains(model.assignrm_name));

			if (int.TryParse(model.contact, out int _contact))
				query = query.Where(x => x.Contact == _contact);

			if (int.TryParse(model.meet, out int _meet))
				query = query.Where(x => x.Meet == _meet);

			if (int.TryParse(model.document, out int _document))
				query = query.Where(x => x.Document == _document);

			if (model.Branchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Branchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Sale.BranchId.HasValue && idList.Contains(x.Sale.BranchId));
				}
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Sale_ActivityCustom>>()
			{
				Items = _mapper.Map<List<Sale_ActivityCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task UpdateActivityById(allFilter model)
		{
			if (model.saleid.HasValue)
			{
				var sales_Activities = await _repo.Context.Sale_Activities.FirstOrDefaultAsync(x => x.SaleId == model.saleid.Value);

				int CRUD = CRUDModel.Update;

				if (sales_Activities == null)
				{
					CRUD = CRUDModel.Create;
					sales_Activities = new();
					sales_Activities.Status = StatusModel.Active;
					sales_Activities.CreateDate = DateTime.Now;
					sales_Activities.SaleId = model.saleid.Value;
				}

				var sales = await _repo.Context.Sales.Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == model.saleid.Value);
				if (sales != null && sales.Customer != null)
				{
					sales_Activities.ContactName = sales.Customer.ContactName;
				}

				var sale_Contacts = await _repo.Context.Sale_Contacts.Where(x => x.SaleId == model.saleid.Value).ToListAsync();
				if (sale_Contacts.Count > 0)
				{
					sales_Activities.Contact = sale_Contacts.Count;
				}

				var sale_Meets = await _repo.Context.Sale_Meets.Where(x => x.SaleId == model.saleid.Value).ToListAsync();
				if (sale_Meets.Count > 0)
				{
					sales_Activities.Meet = sale_Meets.Count;
				}

				var sale_Documents = await _repo.Context.Sale_Documents.Where(x => x.SaleId == model.saleid.Value).ToListAsync();
				if (sale_Documents.Count > 0)
				{
					sales_Activities.Document = sale_Documents.Count;
				}

				var sale_Results = await _repo.Context.Sale_Results.Where(x => x.SaleId == model.saleid.Value).ToListAsync();
				if (sale_Results.Count > 0)
				{
					sales_Activities.Result = sale_Results.Count;
				}

				var sale_Close_Sales = await _repo.Context.Sale_Close_Sales.Where(x => x.SaleId == model.saleid.Value).ToListAsync();
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
		}

		public async Task<List<Dash_PieCustom>> GetGroupReasonNotLoan(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var dash_SalesPipeline = new Dash_SalesPipelineModel();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var query = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.StatusSaleId == StatusSaleModel.CloseSaleNotLoan && x.Master_Reason_CloseSaleId.HasValue);

			query = await QueryArea(query, user);

			var response = new List<Dash_PieCustom>();

			var queryUse = query.GroupBy(m => m.Master_Reason_CloseSaleId)
										 .Select(group => new { GroupID = group.Key, Sales = group.ToList() })
										 .ToList();

			if (queryUse.Count > 0)
			{
				foreach (var item in queryUse)
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

		public async Task<PaginationView<List<GroupByModel>>> GetGroupDealBranch(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var dash_SalesPipeline = new Dash_SalesPipelineModel();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var query = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.BranchId.HasValue && x.BranchId > 0);

			query = await QueryArea(query, user);

			var response = new List<Dash_PieCustom>();

			var queryUse = query.GroupBy(m => m.BranchId)
										 .Select(group => new GroupByModel()
										 {
											 GroupID = group.Key.ToString() ?? string.Empty,
											 Name = group.First().BranchName,
											 Value = group.Count()
										 });

			var pager = new Pager(queryUse.Count(), model.page, model.pagesize, null);

			var items = queryUse.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<GroupByModel>>()
			{
				Items = await items.ToListAsync(),
				Pager = pager
			};
		}

		public async Task<SalesFunnelModel> GetSalesFunnel(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();
			model.rolecode = user.Role.Code.ToUpper();

			var response = new SalesFunnelModel();

			var query = _repo.Context.Sales.Where(x => x.Status == StatusModel.Active);

			var statusTotal = new List<SaleStatusGroupByModel>();

			query = await QueryArea(query, user);

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

			response.Contact = 0;
			response.Meet = 0;
			response.Document = 0;
			response.CloseSale = 0;
			response.CloseSaleFail = 0;

			statusTotal = await query.GroupBy(info => info.StatusSaleId)
					   .Select(group => new SaleStatusGroupByModel()
					   {
						   StatusID = group.Key,
						   Count = group.Count()
					   }).OrderBy(x => x.StatusID).ToListAsync();

			if (statusTotal != null)
			{
				foreach (var item in statusTotal)
				{
					if (item.StatusID == (int)StatusSaleModel.Contact)
					{
						response.Contact = response.Contact + item.Count;
					}

					if (item.StatusID == (int)StatusSaleModel.Meet)
					{
						response.Meet = response.Meet + item.Count;
					}

					if (item.StatusID == (int)StatusSaleModel.SubmitDocument)
					{
						response.Document = response.Document + item.Count;
					}

					if (item.StatusID == (int)StatusSaleModel.CloseSale)
					{
						response.CloseSale = response.CloseSale + item.Count;
					}

					if (item.StatusID == (int)StatusSaleModel.CloseSaleFail
						|| item.StatusID == (int)StatusSaleModel.CloseSaleNotLoan)
					{
						response.CloseSaleFail = response.CloseSaleFail + item.Count;
					}
				}
			}

			return response;
		}

		public async Task<List<Dash_PieCustom>> GetPieRM(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();
			model.rolecode = user.Role.Code.ToUpper();

			var response = new List<Dash_PieCustom>();

			if (user.Role.Code != null && user.Role.Code.ToUpper().StartsWith(RoleCodes.RM))
			{
				response = await GetDataTypeBusiness(model, response);
				response = await GetDataPieCloseSale(model, response);
				response = await GetDataPieReasonFail(model, response);
			}

			return response;
		}

		public async Task<Dash_Avg_NumberOnStage> GetAvgDuration(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var response = new Dash_Avg_NumberOnStage();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var query = _repo.Context.Sale_Durations.Include(x => x.Sale).ThenInclude(x => x.Customer)
												.Where(x => x.Status == StatusModel.Active)
												.OrderByDescending(x => x.CreateDate)
												.AsQueryable();

			query = await QueryAreaDuration(query, user);

			if (model.startdate.HasValue && !model.enddate.HasValue)
			{
				query = query.Where(x => x.Sale.CreateDate.Date >= model.startdate.Value.Date).OrderByDescending(x => x.CreateDate);
			}
			if (!model.startdate.HasValue && model.enddate.HasValue)
			{
				query = query.Where(x => x.Sale.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
			}
			if (model.startdate.HasValue && model.enddate.HasValue)
			{
				query = query.Where(x => x.Sale.CreateDate.Date >= model.startdate.Value.Date && x.Sale.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
			}

			//ประเภทกิจการ
			if (!String.IsNullOrEmpty(model.businesstype))
			{
				if (Guid.TryParse(model.businesstype, out Guid id) && id != Guid.Empty)
				{
					query = query.Where(x => x.Sale.Customer != null && x.Sale.Customer.Master_BusinessTypeId == id);
				}
			}

			//ประเภทสินเชื่อ
			if (!String.IsNullOrEmpty(model.loantypeid))
			{
				if (Guid.TryParse(model.loantypeid, out Guid id) && id != Guid.Empty)
				{
					query = query.Where(x => x.Sale.Customer != null && x.Sale.Customer.Master_LoanTypeId == id);
				}
			}

			//กิจการสาขาภาค[]
			if (model.DepBranchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToGuid(model.DepBranchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Sale.Master_Branch_RegionId.HasValue && idList.Contains(x.Sale.Master_Branch_RegionId));
				}
			}

			//จังหวัด[]
			if (model.Provinces?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Provinces);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Sale.ProvinceId.HasValue && idList.Contains(x.Sale.ProvinceId));
				}
			}

			//สาขา[]
			if (model.Branchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Branchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Sale.BranchId.HasValue && idList.Contains(x.Sale.BranchId));
				}
			}

			var avgWaitContact = query.Select(x => x.WaitContact)
												.DefaultIfEmpty()
												.Average();
			response.WaitContact = double.Round(avgWaitContact, 2, MidpointRounding.AwayFromZero);

			var avgContact = query.Select(x => x.Contact)
												.DefaultIfEmpty()
												.Average();
			response.Contact = double.Round(avgContact, 2, MidpointRounding.AwayFromZero);

			var avgWaitMeet = query.Select(x => x.WaitMeet)
											.DefaultIfEmpty()
											.Average();
			response.WaitMeet = double.Round(avgWaitMeet, 2, MidpointRounding.AwayFromZero);

			var avgMeet = query.Select(x => x.Meet)
											.DefaultIfEmpty()
											.Average();
			response.Meet = double.Round(avgMeet, 2, MidpointRounding.AwayFromZero);

			var avgDocument = query.Select(x => x.Document)
											.DefaultIfEmpty()
											.Average();
			response.Document = double.Round(avgDocument, 2, MidpointRounding.AwayFromZero);

			var avgResult = query.Select(x => x.Result)
											.DefaultIfEmpty()
											.Average();
			response.Result = double.Round(avgResult, 2, MidpointRounding.AwayFromZero);

			return response;
		}

		public async Task<List<GroupByModel>> GetAvgTopBar(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();
			model.rolecode = user.Role.Code.ToUpper();

			var response = new List<GroupByModel>();

			var query = _repo.Context.Sales
									 .Where(x => x.Status == StatusModel.Active)
									 .OrderByDescending(x => x.CreateDate)
									 .AsQueryable();

			query = await QueryArea(query, user);

			var avgcountry = query.Sum(x => x.LoanAmount);
			if (avgcountry.HasValue)
			{
				response.Add(new()
				{
					GroupID = "country",
					Name = "ประเทศ",
					Value = decimal.Round(avgcountry.Value, 2, MidpointRounding.AwayFromZero)
				});
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

			if (model.DepBranchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToGuid(model.DepBranchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Master_Branch_RegionId.HasValue && idList.Contains(x.Master_Branch_RegionId));
				}
			}

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

			var queryRegion = await query.Where(x => x.LoanAmount > 0 && x.Master_Branch_RegionId.HasValue).GroupBy(g => g.Master_Branch_RegionId)
										 .Select(group => new
										 {
											 GroupID = group.Key.ToString(),
											 Value = group.Sum(s => s.LoanAmount)
										 }).ToListAsync();
			if (queryRegion.Count > 0)
			{
				var avg = queryRegion.Average(a => a.Value) ?? 0;
				response.Add(new()
				{
					GroupID = "region",
					Name = "ภูมิภาคทั้งหมด",
					Value = decimal.Round(avg, 2, MidpointRounding.AwayFromZero)
				});
			}

			var queryBranch = await query.Where(x => x.LoanAmount > 0 && x.BranchId.HasValue).GroupBy(g => g.BranchId)
										 .Select(group => new
										 {
											 GroupID = group.Key.ToString(),
											 Value = group.Sum(s => s.LoanAmount)
										 }).ToListAsync();
			if (queryBranch.Count > 0)
			{
				var avg = queryBranch.Average(a => a.Value) ?? 0;
				response.Add(new()
				{
					GroupID = "branch",
					Name = "ศูนย์สาขาทั้งหมด",
					Value = decimal.Round(avg, 2, MidpointRounding.AwayFromZero)
				});
			}

			var queryRM = await query.Where(x => x.LoanAmount > 0 && x.AssUserId.HasValue).GroupBy(g => g.AssUserId)
										 .Select(group => new
										 {
											 GroupID = group.Key.ToString(),
											 Value = group.Sum(s => s.LoanAmount)
										 }).ToListAsync();
			if (queryRM.Count > 0)
			{
				var avg = queryRM.Average(a => a.Value) ?? 0;
				response.Add(new()
				{
					GroupID = "rm",
					Name = "RM ทั้งหมด",
					Value = decimal.Round(avg, 2, MidpointRounding.AwayFromZero)
				});
			}


			return response;
		}

		public async Task<List<GroupByModel>> GetAvgRegionBar(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();
			model.rolecode = user.Role.Code.ToUpper();

			var response = new List<GroupByModel>();

			var query = _repo.Context.Sales
									 .Where(x => x.Status == StatusModel.Active)
									 .OrderByDescending(x => x.CreateDate)
									 .AsQueryable();

			query = await QueryArea(query, user);

			var avgcountry = query.Sum(x => x.LoanAmount);
			if (avgcountry.HasValue)
			{
				response.Add(new()
				{
					GroupID = "country",
					Name = "ประเทศ",
					Value = decimal.Round(avgcountry.Value, 2, MidpointRounding.AwayFromZero)
				});
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

			if (model.DepBranchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToGuid(model.DepBranchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Master_Branch_RegionId.HasValue && idList.Contains(x.Master_Branch_RegionId));
				}
			}

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

			var queryRegion = await query.Where(x => x.LoanAmount > 0 && x.Master_Branch_RegionId.HasValue).GroupBy(g => g.Master_Branch_RegionId)
										 .Select(group => new
										 {
											 GroupID = group.Key.ToString(),
											 Name = group.First().Master_Branch_RegionName,
											 Value = group.Sum(s => s.LoanAmount)
										 }).ToListAsync();
			if (queryRegion.Count > 0)
			{
				foreach (var item in queryRegion)
				{
					var avg = item.Value ?? 0;
					response.Add(new()
					{
						GroupID = "region",
						Name = item.Name,
						Value = decimal.Round(avg, 2, MidpointRounding.AwayFromZero)
					});
				}
			}


			return response;
		}

		public async Task<List<GroupByModel>> GetAvgBranchBar(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();
			model.rolecode = user.Role.Code.ToUpper();

			var response = new List<GroupByModel>();

			var query = _repo.Context.Sales
									 .Where(x => x.Status == StatusModel.Active)
									 .OrderByDescending(x => x.CreateDate)
									 .AsQueryable();

			query = await QueryArea(query, user);

			var avgcountry = query.Sum(x => x.LoanAmount);
			if (avgcountry.HasValue)
			{
				response.Add(new()
				{
					GroupID = "country",
					Name = "ประเทศ",
					Value = decimal.Round(avgcountry.Value, 2, MidpointRounding.AwayFromZero)
				});
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

			if (model.DepBranchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToGuid(model.DepBranchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Master_Branch_RegionId.HasValue && idList.Contains(x.Master_Branch_RegionId));
				}
			}

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

			Guid? branch_RegionIdDefault = null;
			if (model.DepBranchs == null || model.DepBranchs?.Count == 0)
			{
				//default กิจการสาขาภาคที่มีขอดขายสูงสูด
				var department_BranchMax = await query.Where(x => x.LoanAmount > 0 && x.Master_Branch_RegionId.HasValue).GroupBy(g => g.Master_Branch_RegionId)
										 .Select(group => new
										 {
											 GroupID = group.Key,
											 Name = group.First().Master_Branch_RegionName,
											 Value = group.Sum(s => s.LoanAmount)
										 }).OrderByDescending(x => x.Value).FirstOrDefaultAsync();


				if (department_BranchMax != null && department_BranchMax.GroupID.HasValue)
				{
					branch_RegionIdDefault = department_BranchMax.GroupID;
					query = query.Where(x => x.Master_Branch_RegionId == branch_RegionIdDefault);
				}
			}

			var queryRegion = await query.Where(x => x.LoanAmount > 0 && x.Master_Branch_RegionId.HasValue).GroupBy(g => g.Master_Branch_RegionId)
										 .Select(group => new
										 {
											 GroupID = group.Key.ToString(),
											 Name = group.First().Master_Branch_RegionName,
											 Value = group.Sum(s => s.LoanAmount)
										 }).ToListAsync();
			if (queryRegion.Count > 0)
			{
				foreach (var item in queryRegion)
				{
					var avg = item.Value ?? 0;
					response.Add(new()
					{
						GroupID = item.GroupID,
						Name = item.Name,
						Value = decimal.Round(avg, 2, MidpointRounding.AwayFromZero)
					});
				}
			}

			if (model.Branchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Branchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.BranchId.HasValue && idList.Contains(x.BranchId));
				}
			}
			else if (branch_RegionIdDefault.HasValue)
			{
				//default สาขาที่มีขอดขายสูงสูด 3 อันดับแรก
				var branchMax = await _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.Master_Branch_RegionId == branch_RegionIdDefault && x.BranchId.HasValue && x.BranchId > 0)
											 .GroupBy(m => m.BranchId)
											 .Select(group => new
											 {
												 GroupID = group.Key,
												 Name = group.First().BranchName,
												 Value = group.Sum(s => s.LoanAmount)
											 }).OrderByDescending(x => x.Value).Select(x => x.GroupID).Take(3).ToListAsync();
				if (branchMax.Count > 0)
				{
					query = query.Where(x => branchMax.Contains(x.BranchId));
				}
			}

			var queryBranch = await query.Where(x => x.LoanAmount > 0 && x.BranchId.HasValue).GroupBy(g => g.BranchId)
										 .Select(group => new
										 {
											 GroupID = group.Key.ToString(),
											 Name = group.First().BranchName,
											 Value = group.Sum(s => s.LoanAmount)
										 }).OrderByDescending(x => x.Value).ToListAsync();
			if (queryBranch.Count > 0)
			{
				foreach (var item in queryBranch)
				{
					var avg = item.Value ?? 0;
					response.Add(new()
					{
						GroupID = item.GroupID,
						Name = item.Name,
						Value = decimal.Round(avg, 2, MidpointRounding.AwayFromZero)
					});
				}
			}

			return response;
		}

		public async Task<List<GroupByModel>> GetAvgRMBar(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();
			model.rolecode = user.Role.Code.ToUpper();

			var response = new List<GroupByModel>();

			var query = _repo.Context.Sales
									 .Where(x => x.Status == StatusModel.Active)
									 .OrderByDescending(x => x.CreateDate)
									 .AsQueryable();

			query = await QueryArea(query, user);

			var avgcountry = query.Sum(x => x.LoanAmount);
			if (avgcountry.HasValue)
			{
				response.Add(new()
				{
					GroupID = "country",
					Name = "ประเทศ",
					Value = decimal.Round(avgcountry.Value, 2, MidpointRounding.AwayFromZero)
				});
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

			if (model.DepBranchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToGuid(model.DepBranchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Master_Branch_RegionId.HasValue && idList.Contains(x.Master_Branch_RegionId));
				}
			}

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

			int? branchIdDefault = null;
			if (model.Branchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Branchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.BranchId.HasValue && idList.Contains(x.BranchId));
				}
			}
			else
			{
				//default สาขาที่มีขอดขายสูงสูด
				var branchMax = await query.Where(x => x.LoanAmount > 0 && x.BranchId.HasValue).GroupBy(g => g.BranchId)
										 .Select(group => new
										 {
											 GroupID = group.Key,
											 Name = group.First().BranchName,
											 Value = group.Sum(s => s.LoanAmount)
										 }).OrderByDescending(x => x.Value).FirstOrDefaultAsync();


				if (branchMax != null && branchMax.GroupID.HasValue)
				{
					branchIdDefault = branchMax.GroupID;
					query = query.Where(x => x.BranchId == branchIdDefault);
				}
			}

			var queryBranch = await query.Where(x => x.LoanAmount > 0 && x.BranchId.HasValue).GroupBy(g => g.BranchId)
										 .Select(group => new
										 {
											 GroupID = group.Key.ToString(),
											 Name = group.First().BranchName,
											 Value = group.Sum(s => s.LoanAmount)
										 }).ToListAsync();
			if (queryBranch.Count > 0)
			{
				foreach (var item in queryBranch)
				{
					var avg = item.Value ?? 0;
					response.Add(new()
					{
						GroupID = item.GroupID,
						Name = item.Name,
						Value = decimal.Round(avg, 2, MidpointRounding.AwayFromZero)
					});
				}
			}

			if (model.AssUsers?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.AssUsers);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.AssUserId.HasValue && idList.Contains(x.AssUserId));
				}
			}
			else if (branchIdDefault.HasValue)
			{
				//default พนักงาน rm ที่มีขอดขายสูงสูด 3 อันดับแรก
				var rmMax = await _repo.Context.Sales.Where(x => x.Status == StatusModel.Active && x.BranchId == branchIdDefault)
											 .GroupBy(m => m.AssUserId)
											 .Select(group => new
											 {
												 GroupID = group.Key,
												 Name = group.First().BranchName,
												 Value = group.Sum(s => s.LoanAmount)
											 }).OrderByDescending(x => x.Value).Select(x => x.GroupID).Take(3).ToListAsync();
				if (rmMax.Count > 0)
				{
					query = query.Where(x => rmMax.Contains(x.AssUserId));
				}
			}

			var queryRM = await query.Where(x => x.LoanAmount > 0 && x.AssUserId.HasValue).GroupBy(g => g.AssUserId)
										 .Select(group => new
										 {
											 GroupID = group.Key.ToString(),
											 Name = group.First().AssUserName,
											 Value = group.Sum(s => s.LoanAmount)
										 }).OrderByDescending(x => x.Value).ToListAsync();
			if (queryRM.Count > 0)
			{
				foreach (var item in queryRM)
				{
					var avg = item.Value ?? 0;
					response.Add(new()
					{
						GroupID = item.GroupID,
						Name = item.Name,
						Value = decimal.Round(avg, 2, MidpointRounding.AwayFromZero)
					});
				}
			}

			return response;
		}

		public async Task<List<GroupByModel>> GetAvgRegionMonth12Bar(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			var response = new List<GroupByModel>();

			var query = _repo.Context.Sales.Include(i => i.Customer).Where(x => x.Status == StatusModel.Active && x.LoanAmount > 0);

			query = await QueryArea(query, user);

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

			if (model.loanamount > 0)
			{
				query = query.Where(x => x.LoanAmount.HasValue && x.LoanAmount == model.loanamount);
			}

			if (model.DepBranchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToGuid(model.DepBranchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Master_Branch_RegionId.HasValue && idList.Contains(x.Master_Branch_RegionId));
				}
			}

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

			//ประเภทกิจการ
			if (!String.IsNullOrEmpty(model.businesstype))
			{
				if (Guid.TryParse(model.businesstype, out Guid id))
				{
					query = query.Where(x => x.Customer != null && x.Customer.Master_BusinessTypeId == id);
				}
			}

			var sales = await query.GroupBy(g => g.CreateDate.Month)
											 .Select(group => new
											 {
												 GroupID = group.Key,
												 Value = group.Sum(s => s.LoanAmount)
											 }).ToListAsync();

			for (int i = 1; i <= 12; i++)
			{
				var _sales = sales.FirstOrDefault(x => x.GroupID == i);
				if (_sales != null)
				{
					response.Add(new() { GroupID = i.ToString(), Name = GeneralUtils.GetFullMonth(i), Value = _sales.Value ?? 0 });
				}
				else
				{
					response.Add(new() { GroupID = i.ToString(), Name = GeneralUtils.GetFullMonth(i) });
				}
			}

			return response;
		}

		public async Task<List<GroupByModel>> GetAvgComparePreMonth(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();
			model.rolecode = user.Role.Code.ToUpper();

			var response = new List<GroupByModel>();

			if (!model.startdate.HasValue || model.startdate.Value == DateTime.MinValue)
			{
				model.startdate = DateTime.Now;
			}
			DateTime pre_date = model.startdate.Value.AddMonths(-1);

			var query = _repo.Context.Sales
									 .Include(x => x.Customer)
									 .Where(x => x.Status == StatusModel.Active)
									 .OrderByDescending(x => x.CreateDate)
									 .AsQueryable();

			query = await QueryArea(query, user);

			//if (model.startdate.HasValue && !model.enddate.HasValue)
			//{
			//	query = query.Where(x => x.CreateDate.Date >= model.startdate.Value.Date).OrderByDescending(x => x.CreateDate);
			//}
			//if (!model.startdate.HasValue && model.enddate.HasValue)
			//{
			//	query = query.Where(x => x.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
			//}
			//if (model.startdate.HasValue && model.enddate.HasValue)
			//{
			//	query = query.Where(x => x.CreateDate.Date >= model.startdate.Value.Date && x.CreateDate.Date <= model.enddate.Value.Date).OrderByDescending(x => x.CreateDate);
			//}

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

			if (model.DepBranchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToGuid(model.DepBranchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Master_Branch_RegionId.HasValue && idList.Contains(x.Master_Branch_RegionId));
				}
			}

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

			var avgvalue = query.Where(x => x.CreateDate.Month == model.startdate.Value.Month).Select(s => s.LoanAmount).DefaultIfEmpty().Average() ?? 0;
			response.Add(new()
			{
				GroupID = "avgvalue",
				Name = "มูลค่าเฉลี่ย",
				Value = decimal.Round(avgvalue, 2, MidpointRounding.AwayFromZero)
			});

			var avgvalue_pre = query.Where(x => x.CreateDate.Month == pre_date.Month).Select(s => s.LoanAmount).DefaultIfEmpty().Average() ?? 0;
			response.Add(new()
			{
				GroupID = "avgvalue_pre",
				Name = "มูลค่าเฉลี่ย",
				Value = decimal.Round(avgvalue_pre, 2, MidpointRounding.AwayFromZero)
			});


			var closesale = query.Where(x => x.CreateDate.Month == model.startdate.Value.Month).Where(x => x.StatusSaleId == StatusSaleModel.CloseSale).Sum(s => s.LoanAmount) ?? 0;
			response.Add(new()
			{
				GroupID = "closesale",
				Name = "มูลค่าดีลที่ปิดได้",
				Value = decimal.Round(closesale, 2, MidpointRounding.AwayFromZero)
			});

			var closesale_pre = query.Where(x => x.CreateDate.Month == pre_date.Month && x.StatusSaleId == StatusSaleModel.CloseSale).Sum(s => s.LoanAmount) ?? 0;
			response.Add(new()
			{
				GroupID = "closesale_pre",
				Name = "มูลค่าดีลที่ปิดได้",
				Value = decimal.Round(closesale_pre, 2, MidpointRounding.AwayFromZero)
			});


			var alldeal = query.Where(x => x.CreateDate.Month == model.startdate.Value.Month).Count();
			response.Add(new()
			{
				GroupID = "alldeal",
				Name = "ดีลที่เปิดทั้งหมด",
				Value = decimal.Round(alldeal, 0, MidpointRounding.AwayFromZero)
			});

			var alldeal_pre = query.Count(x => x.CreateDate.Month == pre_date.Month);
			response.Add(new()
			{
				GroupID = "alldeal_pre",
				Name = "ดีลที่เปิดทั้งหมด",
				Value = decimal.Round(alldeal_pre, 0, MidpointRounding.AwayFromZero)
			});

			var alldealclose = query.Where(x => x.CreateDate.Month == model.startdate.Value.Month).Count(x => x.StatusSaleId == StatusSaleModel.CloseSale);
			response.Add(new()
			{
				GroupID = "alldealclose",
				Name = "ดีลที่ปิดได้ทั้งหมด",
				Value = decimal.Round(alldealclose, 0, MidpointRounding.AwayFromZero)
			});

			var alldealclose_pre = query.Count(x => x.CreateDate.Month == pre_date.Month && x.StatusSaleId == StatusSaleModel.CloseSale);
			response.Add(new()
			{
				GroupID = "alldealclose_pre",
				Name = "ดีลที่ปิดได้ทั้งหมด",
				Value = decimal.Round(alldealclose_pre, 0, MidpointRounding.AwayFromZero)
			});

			return response;
		}

		public async Task UpdateDeliverById(allFilter model)
		{
			if (model.saleid.HasValue)
			{
				var sale_Delivers = await _repo.Context.Sale_Delivers.FirstOrDefaultAsync(x => x.SaleId == model.saleid.Value);

				int CRUD = CRUDModel.Update;

				if (sale_Delivers == null)
				{
					CRUD = CRUDModel.Create;
					sale_Delivers = new();
					sale_Delivers.Status = StatusModel.Active;
					sale_Delivers.CreateDate = DateTime.Now;
					sale_Delivers.SaleId = model.saleid.Value;
				}

				sale_Delivers.LoanToBranchReg = 0;
				sale_Delivers.BranchRegToCenBranch = 0;
				sale_Delivers.CenBranchToRM = 0;
				sale_Delivers.CloseSale = 0;

				var sale_Status = await _repo.Context.Sale_Statuses.Where(x => x.SaleId == model.saleid.Value).ToListAsync();
				if (sale_Status.Count > 0)
				{
					//รอมอบหมาย(ผจศ) และ รอมอบหมาย(ผจศ.)(จาก ศูนย์ธุรกิจสินเชื่อ)
					DateTime _waitAssignCenterAll = DateTime.MinValue;

					//รอมอบหมาย(ผจศ.)(จาก ศูนย์ธุรกิจสินเชื่อ)
					var waitAssignCenterREG = sale_Status.Where(x => x.StatusId == StatusSaleModel.WaitAssignCenterREG).Select(x => x.CreateDate.Date).FirstOrDefault();
					//รอมอบหมาย(ผจศ)
					var waitAssignCenter = sale_Status.Where(x => x.StatusId == StatusSaleModel.WaitAssignCenter).Select(x => x.CreateDate.Date).FirstOrDefault();
					//รอมอบหมาย RM
					var waitAssign = sale_Status.Where(x => x.StatusId == StatusSaleModel.WaitAssign).Select(x => x.CreateDate.Date).FirstOrDefault();
					//รอการติตต่อ
					var waitContact = sale_Status.Where(x => x.StatusId == StatusSaleModel.WaitContact).Select(x => x.CreateDate.Date).FirstOrDefault();
					//ปิดการขาย
					var closeSale = sale_Status.Where(x => x.StatusMainId == StatusSaleMainModel.CloseSale).Select(x => x.CreateDate.Date).FirstOrDefault();

					_waitAssignCenterAll = waitAssignCenterREG;
					if (_waitAssignCenterAll == DateTime.MinValue)
					{
						_waitAssignCenterAll = waitAssignCenter;
					}

					//BranchRegToCenBranch กิจการสาขาภาคมอบหมายผู้จัดการศูนย์สาขา
					//รอมอบหมาย(ผจศ) -> รอมอบหมาย RM  cal(รอมอบหมาย RM-รอมอบหมาย(ผจศ))
					if (_waitAssignCenterAll != DateTime.MinValue && waitAssign != DateTime.MinValue)
						sale_Delivers.BranchRegToCenBranch = (int)(waitAssign - _waitAssignCenterAll).TotalDays;

					//CenBranchToRM ผู้จัดการศูนย์สาขามอบหมายพนักงาน RM
					//รอมอบหมาย RM -> รอการติตต่อ  cal(รอการติตต่อ-รอมอบหมาย RM)
					if (waitAssign != DateTime.MinValue && waitContact != DateTime.MinValue)
						sale_Delivers.CenBranchToRM = (int)(waitContact - waitAssign).TotalDays;

					//****** คำนวณที่หน้าบ้าน ******
					//CloseSale ปิดการขาย
					//รอการติตต่อ -> ปิดการขาย  cal(ปิดการขาย-รอการติตต่อ)
					//if (waitContact != DateTime.MinValue && closeSale != DateTime.MinValue)
					//	sale_Delivers.CloseSale = (int)(closeSale - waitContact).TotalDays;
				}

				if (CRUD == CRUDModel.Create)
				{
					await _db.InsterAsync(sale_Delivers);
				}
				else
				{
					_db.Update(sale_Delivers);
				}
				await _db.SaveAsync();
			}
		}

		public async Task<PaginationView<List<Sale_DeliverCustom>>> GetDeliver(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			if (user.Role.Code.StartsWith(RoleCodes.RM)) return new();

			IQueryable<Sale_Deliver> query;

			query = _repo.Context.Sale_Delivers.Include(x => x.Sale).ThenInclude(x => x.Customer)
												.Where(x => x.Status != StatusModel.Delete)
												.OrderByDescending(x => x.CreateDate)
												.AsQueryable();

			query = await QueryAreaDeliver(query, user);

			if (int.TryParse(model.loantobranchreg, out int _loantobranchreg))
				query = query.Where(x => x.LoanToBranchReg == _loantobranchreg);

			if (int.TryParse(model.branchregtocenbranch, out int _branchregtocenbranch))
				query = query.Where(x => x.BranchRegToCenBranch == _branchregtocenbranch);

			if (int.TryParse(model.cenbranchtorm, out int _cenbranchtorm))
				query = query.Where(x => x.CenBranchToRM == _cenbranchtorm);

			if (int.TryParse(model.closesale, out int _closesale))
				query = query.Where(x => x.CloseSale == _closesale);

			if (!String.IsNullOrEmpty(model.juristicnumber))
				query = query.Where(x => x.Sale.Customer.JuristicPersonRegNumber != null && x.Sale.Customer.JuristicPersonRegNumber.Contains(model.juristicnumber));

			if (!String.IsNullOrEmpty(model.searchtxt))
				query = query.Where(x => x.Sale.CompanyName != null && x.Sale.CompanyName.Contains(model.searchtxt));

			if (!String.IsNullOrEmpty(model.assignrm_name))
				query = query.Where(x => x.Sale.AssUserName != null && x.Sale.AssUserName.Contains(model.assignrm_name));

			if (model.provinceid.HasValue)
				query = query.Where(x => x.Sale.ProvinceId.HasValue && x.Sale.ProvinceId == model.provinceid.Value);

			if (model.Branchs?.Count > 0)
			{
				var idList = GeneralUtils.ListStringToInt(model.Branchs);
				if (idList.Count > 0)
				{
					query = query.Where(x => x.Sale.BranchId.HasValue && idList.Contains(x.Sale.BranchId));
				}
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Sale_DeliverCustom>>()
			{
				Items = _mapper.Map<List<Sale_DeliverCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

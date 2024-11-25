using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Index.HPRtree;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using System.Linq;
using System.Linq.Expressions;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class AssignmentCenter : IAssignmentCenter
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public AssignmentCenter(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<bool> CheckAssignmentByUserId(int id)
		{
			return await _repo.Context.Assignment_Centers.AnyAsync(x => x.UserId == id);
		}

		public async Task<Assignment_CenterCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Assignment_Centers
				.Where(x => x.Id == id)
				.Include(x => x.User).ThenInclude(x => x.Master_Branch_Region)
				.FirstOrDefaultAsync();
			return _mapper.Map<Assignment_CenterCustom>(query);
		}

		public async Task<Assignment_CenterCustom> GetByUserId(int id)
		{
			var query = await _repo.Context.Assignment_Centers
				.Where(x => x.UserId == id).FirstOrDefaultAsync();
			return _mapper.Map<Assignment_CenterCustom>(query);
		}

		public async Task<Assignment_CenterCustom> Create(Assignment_CenterCustom model)
		{
			if (await CheckAssignmentByUserId(model.UserId))
				throw new ExceptionCustom("assignment duplicate user");

			//if (model.BranchId.HasValue)
			//{
			//	if (await CheckAssignmentByBranchId(model.BranchId.Value))
			//		throw new ExceptionCustom("มีผู้จัดการศูนย์สาขานี้แล้ว");
			//}

			if (string.IsNullOrEmpty(model.EmployeeName))
			{
				model.EmployeeName = await _repo.User.GetFullNameById(model.UserId);
			}

			var assignment_Center = new Data.Entity.Assignment_Center();
			assignment_Center.Status = model.Status;
			assignment_Center.CreateDate = DateTime.Now;
			assignment_Center.UserId = model.UserId;
			assignment_Center.EmployeeId = model.EmployeeId;
			assignment_Center.EmployeeName = model.EmployeeName;
			assignment_Center.Tel = model.Tel;
			assignment_Center.RMNumber = model.RMNumber ?? 0;
			assignment_Center.CurrentNumber = model.CurrentNumber ?? 0;
			await _db.InsterAsync(assignment_Center);
			await _db.SaveAsync();

			//if (model.BranchId.HasValue)
			//{
			//	await _repo.AssignmentCenter.UpdateCurrentNumber(model.BranchId.Value);
			//}

			return _mapper.Map<Assignment_CenterCustom>(assignment_Center);
		}

		public async Task<Assignment_CenterCustom> Update(Assignment_CenterCustom model)
		{
			var assignment_Center = await _repo.Context.Assignment_Centers.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == model.Id);
			if (assignment_Center != null)
			{
				assignment_Center.Status = model.Status;
				assignment_Center.UserId = model.UserId;
				assignment_Center.EmployeeId = model.EmployeeId;
				assignment_Center.EmployeeName = model.EmployeeName;
				assignment_Center.Tel = model.Tel;
				_db.Update(assignment_Center);
				await _db.SaveAsync();

				//if (model.BranchId.HasValue)
				//{
				//	await _repo.AssignmentCenter.UpdateCurrentNumber(model.BranchId.Value);
				//}

			}
			return _mapper.Map<Assignment_CenterCustom>(assignment_Center);
		}

		public async Task DeleteByUserId(Guid id)
		{
			var assignment_Center = await _repo.Context.Assignment_Centers.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);
			if (assignment_Center != null)
			{
				_db.Delete(assignment_Center);
				await _db.SaveAsync();
			}
		}

		public async Task<PaginationView<List<Assignment_CenterCustom>>> GetListAutoAssign(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not role.");
			if (!user.Role.IsAssignCenter)
			{
				return new() { Items = new() };
			}

			//รูปแบบการมอบหมายตามเกณฑ์
			//1. ดึงข้อมูลผู้จัดการศูนย์ทั้งหมด
			//2. ดึงข้อมูลลูกค้าที่สร้างด้วย ธญ. ที่รอมอบหมาย และระบุพื้นที่จังหวัดแล้ว
			//3. มอบหมาย ผจธ. ตามพื้นที่ดูแล

			//1. ดึงข้อมูลผู้จัดการศูนย์ทั้งหมด
			var query = _repo.Context.Assignment_Centers.Where(x => x.Status == StatusModel.Active)
												 .Include(x => x.User).ThenInclude(t => t.User_Areas)
												 .OrderBy(x => x.CurrentNumber).ThenBy(x => x.CreateDate)
												 .AsQueryable();


			if (!String.IsNullOrEmpty(model.emp_id))
			{
				query = query.Where(x => x.EmployeeId != null && x.EmployeeId.Contains(model.emp_id));
			}

			if (!String.IsNullOrEmpty(model.emp_name))
			{
				query = query.Where(x => x.EmployeeName != null && x.EmployeeName.Contains(model.emp_name));
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);
			var userAssignment = await query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync();
			//var userAssignment = await query.ToListAsync();

			List<Assignment_CenterCustom> responseItems = new();

			//2. ดึงข้อมูลลูกค้าที่สร้างด้วย ธญ. ที่รอมอบหมาย และระบุพื้นที่จังหวัดแล้ว
			var salesQuery = _repo.Context.Sales
				.Include(x => x.Customer)
				.Where(x => x.Status == StatusModel.Active && x.ProvinceId > 0 && !x.AssUserId.HasValue && x.StatusSaleId == StatusSaleModel.WaitAssignCenter)
				.OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
				.AsQueryable();

			var salesCustomer = await salesQuery.ToListAsync();

			List<Guid> duplicateAssignment_Sales = new();

			if (salesCustomer.Count > 0 && userAssignment.Count > 0)
			{
				//3. มอบหมาย ผจธ. ตามพื้นที่ดูแล
				foreach (var item_center in userAssignment)
				{
					//if (item_center.UserId == 68)
					//{
					//}
					var areaList = item_center.User.User_Areas.Select(s => s.ProvinceId).ToList();

					//เห็นทุกภาค
					if (item_center.User.Master_Branch_RegionId == Guid.Parse("99999999-9999-9999-9999-999999999999"))
					{
						areaList = _repo.Context.InfoProvinces
									.Where(x => x.ProvinceID > 0)
									.Select(x => x.ProvinceID).ToList();
					}
					else
					{
						//9999 เห็นทุกจังหวัดในภาค
						if (areaList.Any(x => x == 9999))
						{
							var branch_RegionId = _repo.Context.Users.FirstOrDefault(x => x.Id == item_center.UserId)?.Master_Branch_RegionId;
							if (branch_RegionId.HasValue)
							{
								areaList = _repo.Context.InfoProvinces
									.Where(x => x.Master_Department_BranchId == branch_RegionId)
									.Select(x => x.ProvinceID).ToList();
							}
						}
					}

					var sales = salesCustomer.Where(x => x.ProvinceId.HasValue && areaList.Contains(x.ProvinceId.Value)).ToList();
					if (sales.Count > 0)
					{
						var assignment_Center = _mapper.Map<Assignment_CenterCustom>(item_center);
						assignment_Center.Tel = assignment_Center.User?.Tel;
						assignment_Center.EmployeeId = assignment_Center.User?.EmployeeId;
						assignment_Center.EmployeeName = assignment_Center.User?.FullName;

						if (assignment_Center.User?.User_Areas?.Count > 0)
						{
							string provinceNames = string.Join(",", assignment_Center.User.User_Areas.Select(x => x.ProvinceName));
							assignment_Center.AreaNameJoin = provinceNames;
						}

						assignment_Center.Assignment_Sales = new();
						foreach (var item_sale in sales)
						{
							if (!duplicateAssignment_Sales.Contains(item_sale.Id))
							{
								var _sale = _mapper.Map<SaleCustom>(item_sale);

								//ไม่ต้องตรวจสอบที่อยู่
								if (_sale.Customer != null)
								{
									_sale.Customer.IsExceptValidAddress = true;
								}

								assignment_Center.Assignment_Sales.Add(new()
								{
									SaleId = item_sale.Id,
									IsActive = StatusModel.Active,
									IsSelect = true,
									Sale = _sale
								});

								duplicateAssignment_Sales.Add(item_sale.Id);
							}
						}

						assignment_Center.User = null;
						responseItems.Add(assignment_Center);
					}
				}
			}

			return new PaginationView<List<Assignment_CenterCustom>>()
			{
				Items = responseItems,
				Pager = pager
			};
		}

		public async Task<PaginationView<List<Assignment_CenterCustom>>> GetListCenter(allFilter model)
		{
			var query = _repo.Context.Assignment_Centers.Where(x => x.Status != StatusModel.Delete)
												 .Include(x => x.User).ThenInclude(x => x.User_Areas)
												 .OrderBy(x => x.CurrentNumber).ThenBy(x => x.CreateDate)
												 .Where(x => x.User != null && x.User.User_Areas.Count > 0)
												 .AsQueryable();

			if (!String.IsNullOrEmpty(model.assignmentid))
			{
				if (Guid.TryParse(model.assignmentid, out Guid assignmentid))
				{
					query = query.Where(x => x.Id == assignmentid);
				}
			}

			if (!String.IsNullOrEmpty(model.emp_id))
			{
				query = query.Where(x => x.EmployeeId != null && x.EmployeeId.Contains(model.emp_id));
			}

			if (!String.IsNullOrEmpty(model.emp_name))
			{
				query = query.Where(x => x.EmployeeName != null && x.EmployeeName.Contains(model.emp_name));
			}

			if (model.provinceid.HasValue)
			{
				query = query.Where(x => x.User != null && x.User.ProvinceId == model.provinceid);
			}

			if (model.amphurid.HasValue)
			{
				query = query.Where(x => x.User != null && x.User.AmphurId == model.amphurid);
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Assignment_CenterCustom>>()
			{
				Items = _mapper.Map<List<Assignment_CenterCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task Assign(AssignModel model)
		{
			var assignment_CenterBranch = await _repo.Context.Assignment_Centers
				//.Include(x => x.User).ThenInclude(x => x.Master_Branch_Region) //Include จะทำให้ update sale บางตัวไม่ได้
				.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.Id == model.AssignMCenter.Id);
			if (assignment_CenterBranch != null)
			{
				//var salesCount = 0;
				foreach (var item in model.Sales)
				{
					var sale = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.Id == item.Id);
					if (sale != null)
					{
						sale.AssUser = null;
						sale.AssCenterUser = null;

						if (sale.AssCenterUserId.HasValue) throw new ExceptionCustom($"assignment duplicate {sale.CompanyName}");

						//ข้อมูลนี้จะได้มาตั้งแต่ตอนเพิ่มรายการ

						//sale.BranchId = assignment_CenterBranch.BranchId;
						//sale.BranchName = assignment_CenterBranch.BranchName;
						sale.AssCenterAlready = true;
						sale.AssCenterUserId = model.AssignMCenter.UserId;
						sale.AssCenterUserName = assignment_CenterBranch.EmployeeName;
						sale.AssCenterCreateBy = model.CurrentUserId;
						sale.AssCenterDate = DateTime.Now;
						//กรณีตีกลับบางจังหวะจะมี assUser ค้างไว้แสดงผล ต้องเคลียร์ออกกรณีมอบหมายใหม่
						sale.AssUserId = null;
						sale.AssUserName = null;
						_db.Update(sale);
						await _db.SaveAsync();

						var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

						await _repo.Sales.UpdateStatusOnly(new()
						{
							SaleId = sale.Id,
							StatusId = StatusSaleModel.WaitAssign,
							CreateBy = model.CurrentUserId,
							CreateByName = currentUserName,
						});

						await _repo.Dashboard.UpdateDeliverById(new() { saleid = item.Id });
						//salesCount++;
					}
				}

				int countCurrentNumber = await _repo.Context.Sales.CountAsync(x => x.Status == StatusModel.Active && x.AssCenterUserId == model.AssignMCenter.UserId);
				assignment_CenterBranch.CurrentNumber = countCurrentNumber;
				//assignment_CenterBranch.CurrentNumber = assignment_CenterBranch.CurrentNumber + salesCount;
				_db.Update(assignment_CenterBranch);
				await _db.SaveAsync();

			}
		}

		public async Task AssignCenter(List<Assignment_CenterCustom> model)
		{
			foreach (var item_center in model)
			{
				if (item_center.Assignment_Sales?.Count > 0)
				{
					var assignment_Center = await _repo.Context.Assignment_Centers
						.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.Id == item_center.Id);
					if (assignment_Center == null) throw new ExceptionCustom("ไม่พบข้อมูลผู้จัดการศูนย์");

					foreach (var item_sale in item_center.Assignment_Sales)
					{
						var sale = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.Id == item_sale.SaleId);
						if (sale == null) throw new ExceptionCustom("ไม่พบข้อมูลูกค้า");

						sale.AssUser = null;
						sale.AssCenterUser = null;
						sale.AssCenterAlready = true;
						sale.AssCenterUserId = item_center.UserId;
						sale.AssCenterUserName = item_center.EmployeeName;
						sale.AssCenterCreateBy = item_center.CurrentUserId;
						sale.AssCenterDate = DateTime.Now;
						_db.Update(sale);
						await _db.SaveAsync();

						var currentUserName = await _repo.User.GetFullNameById(item_center.CurrentUserId);

						await _repo.Sales.UpdateStatusOnly(new()
						{
							SaleId = sale.Id,
							StatusId = StatusSaleModel.WaitAssign,
							CreateBy = item_center.CurrentUserId,
							CreateByName = currentUserName,
						});

						await _repo.Dashboard.UpdateDeliverById(new() { saleid = item_sale.SaleId });
					}

					assignment_Center.CurrentNumber = item_center.NumberAfterAssignment;
					_db.Update(assignment_Center);
					await _db.SaveAsync();

				}
			}
		}

		public async Task AssignCenterUpdateRange(List<Assignment_CenterCustom> model)
		{
			foreach (var item_center in model)
			{
				if (item_center.Assignment_Sales?.Count > 0)
				{
					var assignment_Center = await _repo.Context.Assignment_Centers
						.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.Id == item_center.Id);
					if (assignment_Center == null) throw new ExceptionCustom("ไม่พบข้อมูลผู้จัดการศูนย์");

					var salesToWaitUpdate = new List<SaleCustom>();

					foreach (var item_sale in item_center.Assignment_Sales)
					{
						//var sale = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.Id == item_sale.SaleId);
						//if (sale == null) throw new ExceptionCustom("ไม่พบข้อมูลูกค้า");

						//sale.AssUser = null;
						//sale.AssCenterUser = null;
						//sale.AssCenterAlready = true;
						//sale.AssCenterUserId = item_center.UserId;
						//sale.AssCenterUserName = item_center.EmployeeName;
						//sale.AssCenterCreateBy = item_center.CurrentUserId;
						//sale.AssCenterDate = DateTime.Now;
						//_db.Update(sale);
						//await _db.SaveAsync();

						salesToWaitUpdate.Add(new()
						{
							Id = item_sale.SaleId,
							AssCenterUserId = item_center.UserId,
							AssCenterUserName = item_center.EmployeeName,
							AssCenterCreateBy = item_center.CurrentUserId,
						});

						var currentUserName = await _repo.User.GetFullNameById(item_center.CurrentUserId);

						await _repo.Sales.UpdateStatusOnly(new()
						{
							SaleId = item_sale.SaleId,
							StatusId = StatusSaleModel.WaitAssign,
							CreateBy = item_center.CurrentUserId,
							CreateByName = currentUserName,
						});

						await _repo.Dashboard.UpdateDeliverById(new() { saleid = item_sale.SaleId });
					}

					assignment_Center.CurrentNumber = item_center.NumberAfterAssignment;
					_db.Update(assignment_Center);
					await _db.SaveAsync();


					// รวบรวมการอัปเดตแล้วทำครั้งเดียว UpdateRange
					if (salesToWaitUpdate.Any())
					{
						var salesToUpdate = new List<Sale>();
						foreach (var item_upate in salesToWaitUpdate)
						{
							var sales = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.Id == item_upate.Id);
							if (sales != null)
							{
								sales.AssUser = null;
								sales.AssCenterUser = null;
								sales.AssCenterAlready = true;
								sales.AssCenterUserId = item_upate.AssCenterUserId;
								sales.AssCenterUserName = item_upate.AssCenterUserName;
								sales.AssCenterCreateBy = item_upate.AssCenterCreateBy;
								sales.AssCenterDate = DateTime.Now;
								salesToUpdate.Add(sales);
							}
						}
						if (salesToUpdate.Any())
						{
							_db.UpdateRange(salesToUpdate);
							await _db.SaveAsync();
						}
					}


				}
			}
		}

		public async Task UpdateCurrentNumber(int? userid = null)
		{
			var query = _repo.Context.Users.Include(x => x.Role)
										   .Include(x => x.User_Areas)
										   .Where(x => x.Status != StatusModel.Delete && x.Role != null && x.Role.IsAssignRM)
										   .OrderBy(x => x.Id)
										   .AsQueryable();

			if (userid.HasValue && userid > 0)
			{
				query = query.Where(x => x.Id == userid);
			}

			var usersCenter = await query.ToListAsync();

			if (usersCenter.Count > 0)
			{
				foreach (var user in usersCenter)
				{
					var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();


					var queryRM = _repo.Context.Users.Include(x => x.Role)
												   .Include(x => x.User_Areas)
												   .Where(x => x.Status != StatusModel.Delete && x.Role != null && x.Role.Code == RoleCodes.RM)
												   .AsQueryable();

					var querySale = _repo.Context.Sales.Where(x => x.Status != StatusModel.Delete)
														.Include(x => x.AssUser).ThenInclude(t => t.User_Areas)
														.Include(x => x.AssCenterUser).ThenInclude(s => s.Master_Branch_Region)
														.AsQueryable();
					//ผจธ. เห็นเฉพาะพนักงาน RM ภายใต้พื้นที่การดูแล และงานที่ถูกมอบหมายมาจาก ธญ
					Expression<Func<User, bool>> orExpressionRM = x => false;
					Expression<Func<Sale, bool>> orExpressionSale = x => false;
					foreach (var provinceId in user_Areas)
					{
						var tempProvinceId = provinceId; // ต้องใช้ตัวแปรแยกต่างหากสำหรับการใช้งานใน lambda
						orExpressionRM = orExpressionRM.Or(x => x.User_Areas.Any(s => s.ProvinceId == tempProvinceId));
						orExpressionSale = orExpressionSale.Or(x => x.AssUser != null && x.AssUser.User_Areas.Any(s => s.ProvinceId == tempProvinceId));
						orExpressionSale = orExpressionSale.Or(x => x.AssCenterUser != null && x.AssCenterUser.User_Areas.Any(s => s.ProvinceId == tempProvinceId));
					}
					// ใช้เงื่อนไข OR ที่สร้างขึ้นกับ query
					queryRM = queryRM.Where(orExpressionRM);
					querySale = querySale.Where(orExpressionSale);

					var assignments = await _repo.Context.Assignment_Centers.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Status == StatusModel.Active);
					if (assignments != null)
					{
						assignments.RMNumber = queryRM.Count();
						assignments.CurrentNumber = querySale.Count();
						_db.Update(assignments);
						await _db.SaveAsync();
					}
				}
			}
		}

		public async Task CreateAssignmentCenterAll(allFilter model)
		{
			await _repo.User.RemoveUserNotAssignment();

			var usersCenter = await _repo.Context.Users
											.Include(x => x.Role)
										   .Include(x => x.Assignment_Centers)
										   .Where(x => x.Status != StatusModel.Delete && x.Role != null && x.Role.Status == StatusModel.Active && x.Role.IsAssignRM && x.Assignment_Centers.Count == 0)
										   .OrderBy(x => x.Id)
										   .ToListAsync();

			if (usersCenter.Count > 0)
			{
				foreach (var item_center in usersCenter)
				{
					var assignmentCenter = await _repo.AssignmentCenter.Create(new()
					{
						Status = StatusModel.Active,
						UserId = item_center.Id,
						EmployeeId = item_center.EmployeeId,
						EmployeeName = item_center.FullName,
						Tel = item_center.Tel,
						RMNumber = 0,
						CurrentNumber = 0
					});
				}
			}



		}

	}
}

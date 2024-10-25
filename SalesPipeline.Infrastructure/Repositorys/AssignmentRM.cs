using AutoMapper;
using Hangfire.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Index.HPRtree;
using NPOI.SS.Formula.Functions;
using SalesPipeline.Infrastructure.Data.Context;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using System.Linq.Expressions;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class AssignmentRM : IAssignmentRM
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public AssignmentRM(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
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
				if (user.Master_Branch_RegionId != Guid.Parse("99999999-9999-9999-9999-999999999999"))
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

		private async Task<IQueryable<Assignment_RM>> QueryAreaAssignment_RM(IQueryable<Assignment_RM> query, UserCustom user)
		{
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();
			//var user_AreasRm = query.Select(x => x.User.User_Areas.Select(s => s.ProvinceId).ToList());

			//99999999-9999-9999-9999-999999999999 เห็นทั้งประเทศ
			if (user.Master_Branch_RegionId != Guid.Parse("99999999-9999-9999-9999-999999999999"))
			{
				Expression<Func<Assignment_RM, bool>> orExpression = x => false;
				//9999 เห็นทุกจังหวัดในภาค
				if (user.Master_Branch_RegionId.HasValue && user_Areas.Any(x => x == 9999))
				{
					var provinces = await _repo.Thailand.GetProvince(user.Master_Branch_RegionId);
					if (provinces?.Count > 0)
					{
						user_Areas = provinces.Select(x => x.ProvinceID).ToList();
					}
				}

				foreach (var provinceId in user_Areas)
				{
					var tempProvinceId = provinceId;
					orExpression = orExpression.Or(x =>
					(x.User.User_Areas.Any(s => s.ProvinceId == tempProvinceId))
					|| (x.User.User_Areas.Any(s => s.User.Master_Branch_RegionId == user.Master_Branch_RegionId && s.ProvinceId == 9999))
					);
				}

				orExpression = orExpression.Or(x => x.User.Master_Branch_RegionId == user.Master_Branch_RegionId && x.User.User_Areas.Any(s => s.ProvinceId == 9999));
				// ใช้เงื่อนไข OR ที่สร้างขึ้นกับ query
				query = query.Where(orExpression);
			}

			await Task.CompletedTask;

			return query;
		}

		private async Task<IQueryable<Assignment_RM>> QueryAreaAssignment_RM_Old(IQueryable<Assignment_RM> query, UserCustom user)
		{
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");
			var user_Areas = user.User_Areas?.Select(x => x.ProvinceId).ToList() ?? new();

			//99999999-9999-9999-9999-999999999999 เห็นทั้งประเทศ
			if (user.Master_Branch_RegionId != Guid.Parse("99999999-9999-9999-9999-999999999999"))
			{

				// สร้าง Expression<Func<MyEntity, bool>> สำหรับเงื่อนไข OR
				Expression<Func<Assignment_RM, bool>> orExpression = x => false; // เริ่มต้นด้วย false เพื่อให้ไม่มีผลกระทบในขั้นแรก

				foreach (var provinceId in user_Areas)
				{
					var tempProvinceId = provinceId; // ต้องใช้ตัวแปรแยกต่างหากสำหรับการใช้งานใน lambda
					orExpression = orExpression.Or(x =>
					(x.User.User_Areas.Any(s => s.ProvinceId == tempProvinceId))
					|| (x.User.User_Areas.Any(s => s.ProvinceId == 9999))
					);
				}

				orExpression = orExpression.Or(x => x.User.Master_Branch_RegionId == user.Master_Branch_RegionId && x.User.User_Areas.Any(s => s.ProvinceId == 9999));
				// ใช้เงื่อนไข OR ที่สร้างขึ้นกับ query
				query = query.Where(orExpression);
			}

			await Task.CompletedTask;

			return query;
		}

		public async Task<Assignment_RMCustom> Create(Assignment_RMCustom model)
		{
			if (await CheckAssignmentByUserId(model.UserId))
				throw new ExceptionCustom("assignmentRM duplicate user");

			if (string.IsNullOrEmpty(model.EmployeeName))
			{
				model.EmployeeName = await _repo.User.GetFullNameById(model.UserId);
			}

			var assignment_RM = new Data.Entity.Assignment_RM();
			assignment_RM.Status = model.Status;
			if (model.CreateDate != DateTime.MinValue)
			{
				assignment_RM.CreateDate = model.CreateDate;
			}
			else
			{
				assignment_RM.CreateDate = DateTime.Now;
			}
			assignment_RM.UserId = model.UserId;
			assignment_RM.EmployeeId = model.EmployeeId;
			assignment_RM.EmployeeName = model.EmployeeName;
			assignment_RM.CurrentNumber = model.CurrentNumber ?? 0;
			await _db.InsterAsync(assignment_RM);
			await _db.SaveAsync();

			//**** update ผู้จัดการศูนย์
			await _repo.AssignmentCenter.UpdateCurrentNumber(model.UserId);


			return _mapper.Map<Assignment_RMCustom>(assignment_RM);
		}

		public async Task<Assignment_RMCustom> Update(Assignment_RMCustom model)
		{
			//Guid? assignmentIdOriginal = null;
			var assignment_RM = await _repo.Context.Assignment_RMs.Where(x => x.UserId == model.UserId).FirstOrDefaultAsync();
			if (assignment_RM != null)
			{
				assignment_RM.Status = model.Status;
				assignment_RM.EmployeeId = model.EmployeeId;
				assignment_RM.EmployeeName = model.EmployeeName;
				_db.Update(assignment_RM);
				await _db.SaveAsync();
			}

			//**** มีการเปลี่ยนศูนย์ที่รับผิดชอบ  update ผู้จัดการศูนย์ 
			//if (assignmentIdOriginal.HasValue && model.AssignmentId.HasValue)
			//{
			//	await _repo.AssignmentCenter.UpdateCurrentNumber(model.AssignmentId.Value);
			//	await _repo.AssignmentCenter.UpdateCurrentNumber(assignmentIdOriginal.Value);
			//}

			return _mapper.Map<Assignment_RMCustom>(assignment_RM);
		}

		public async Task<Assignment_RM_SaleCustom> CreateSale(Assignment_RM_SaleCustom model)
		{
			if (string.IsNullOrEmpty(model.CreateByName))
			{
				model.CreateByName = await _repo.User.GetFullNameById(model.CreateBy);
			}

			var assignment_RM_Sale = new Data.Entity.Assignment_RM_Sale();
			assignment_RM_Sale.Status = StatusModel.Active;
			assignment_RM_Sale.CreateDate = DateTime.Now;
			assignment_RM_Sale.CreateBy = model.CreateBy;
			assignment_RM_Sale.CreateByName = model.CreateByName;
			assignment_RM_Sale.AssignmentRMId = model.AssignmentRMId;
			assignment_RM_Sale.IsActive = StatusModel.Active;
			assignment_RM_Sale.SaleId = model.SaleId;
			assignment_RM_Sale.Description = model.Description;
			await _db.InsterAsync(assignment_RM_Sale);
			await _db.SaveAsync();

			return _mapper.Map<Assignment_RM_SaleCustom>(assignment_RM_Sale);
		}

		public async Task<bool> CheckAssignmentByUserId(int id)
		{
			return await _repo.Context.Assignment_RMs.AnyAsync(x => x.UserId == id);
		}

		public async Task<bool> CheckAssignmentSaleById(Guid id)
		{
			return await _repo.Context.Assignment_RM_Sales.AnyAsync(x => x.SaleId == id && x.Status == StatusModel.Active && x.IsActive == StatusModel.Active);
		}

		//ใช้กรณีดึงไปเช็คก่อน update เพราะถ้าดึง GetByUserId ปกติจะมีการ join ทำให้บางฟิลด์ไม่ update
		public async Task<bool> GetAssignmentOnlyByUserId(int id)
		{
			return await _repo.Context.Assignment_RMs.AnyAsync(x => x.UserId == id);
		}

		public async Task<Assignment_RMCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Assignment_RMs
				.Include(x => x.Assignment_RM_Sales)
				.Include(x => x.User)
				.Where(x => x.Id == id).FirstOrDefaultAsync();
			return _mapper.Map<Assignment_RMCustom>(query);
		}

		public async Task<Assignment_RMCustom> GetByUserId(int id)
		{
			var query = await _repo.Context.Assignment_RMs
				.Include(x => x.Assignment_RM_Sales)
				.Include(x => x.User)
				.Where(x => x.UserId == id).FirstOrDefaultAsync();
			return _mapper.Map<Assignment_RMCustom>(query);
		}

		public async Task UpdateCurrentNumber(int? userid = null)
		{
			var query = _repo.Context.Users.Include(x => x.Role)
										   .Include(x => x.User_Areas)
										   .Where(x => x.Status != StatusModel.Delete && x.Role != null && x.Role.Code == RoleCodes.RM)
										   .OrderBy(x => x.Id)
										   .AsQueryable();

			if (userid.HasValue && userid > 0)
			{
				query = query.Where(x => x.Id == userid);
			}

			var usersRM = await query.ToListAsync();

			if (usersRM.Count > 0)
			{
				foreach (var user in usersRM)
				{
					var currentNumber = await _repo.Context.Sales.Where(x => x.Status == StatusModel.Active
																&& x.AssUserId == user.Id).CountAsync();
					var assignment_RMs = await _repo.Context.Assignment_RMs.FirstOrDefaultAsync(x => x.UserId == user.Id);
					if (assignment_RMs != null)
					{
						assignment_RMs.CurrentNumber = currentNumber;
						_db.Update(assignment_RMs);
						await _db.SaveAsync();

						//**** update ผู้จัดการศูนย์						
						//	await _repo.AssignmentCenter.UpdateCurrentNumber();						
					}
				}
			}

		}

		public async Task<PaginationView<List<Assignment_RMCustom>>> GetListAutoAssign(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not role.");
			if (!user.Role.IsAssignRM)
			{
				return new();
			}

			//รูปแบบการมอบหมายตามเกณฑ์
			//1. ข้อมูลพนักงาน RM เรียงจากลูกค้าที่ดูแลปัจจุบัน น้อย --> มาก
			//2. หาข้อมูลลูกค้าที่ยังไม่ถูกมอบหมาย
			//3. แยกรายการลูกค้าที่ยังไม่ถูกมอบหมายออกเป็นส่วนเท่าๆ กัน
			//4. มอบหมายให้พนักงานเท่าๆ กัน  (พนักงานที่ดูแลลูกค้าน้อยสุดจะถูกมอบหมายก่อนเรียงลำดับไปเรื่อยๆ)

			//Guid? assignmentCenterId = null;
			//int? assignmentCenterUserId = null;
			//if (model.assigncenter.HasValue)
			//{
			//	var assignment_MCenter = await _repo.Context.Assignment_Centers.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.UserId == model.assigncenter);
			//	if (assignment_MCenter != null)
			//	{
			//		assignmentCenterId = assignment_MCenter.Id;
			//		assignmentCenterUserId = assignment_MCenter.UserId;
			//	}
			//}

			//ข้อมูลพนักงาน RM เรียงจากลูกค้าที่ดูแลปัจจุบัน น้อย --> มาก
			var query = _repo.Context.Assignment_RMs.Where(x => x.Status != StatusModel.Delete)
												 .Include(x => x.User).ThenInclude(x => x.User_Areas)
												 .OrderBy(x => x.CurrentNumber).ThenBy(x => x.CreateDate)
												 .AsQueryable();

			if (user.Role.IsAssignRM)
			{
				query = await QueryAreaAssignment_RM(query, user);
			}

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

			List<Assignment_RMCustom> responseItems = new();

			//ข้อมูลลูกค้าที่ยังไม่ถูกมอบหมาย
			var salesQuery = _repo.Context.Sales
				.Include(x => x.Customer)
				.Where(x => x.Status != StatusModel.Delete && !x.AssUserId.HasValue && x.StatusSaleId == StatusSaleModel.WaitAssign)
				.OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
				.AsQueryable();

			//พื้นที่ดูแล
			if (user.Role.IsAssignRM)
			{
				salesQuery = await QueryArea(salesQuery, user);
			}

			var salesCustomer = await salesQuery.ToListAsync();

			if (salesCustomer.Count > 0 && userAssignment.Count > 0)
			{
				//แยกรายการลูกค้าที่ยังไม่ถูกมอบหมายออกเป็นส่วนเท่าๆ กัน
				var partitionCustomer = GeneralUtils.PartitionList(salesCustomer, userAssignment.Count);

				if (partitionCustomer.Length > 0)
				{
					int index_path = 0;
					foreach (var item_path in partitionCustomer)
					{
						//มอบหมายให้พนักงานเท่าๆ กัน
						var assignment_RM = _mapper.Map<Assignment_RMCustom>(userAssignment[index_path]);
						assignment_RM.Assignment_RM_Sales = new();
						assignment_RM.Tel = assignment_RM.User?.Tel;
						assignment_RM.ProvinceName = assignment_RM.User?.ProvinceName;
						assignment_RM.BranchName = assignment_RM.User?.BranchName;

						if (assignment_RM.User?.User_Areas?.Count > 0)
						{
							string provinceNames = string.Join(",", assignment_RM.User.User_Areas.Select(x => x.ProvinceName));
							assignment_RM.AreaNameJoin = provinceNames;
						}

						foreach (var item_sales in item_path)
						{
							assignment_RM.Assignment_RM_Sales.Add(new()
							{
								Id = Guid.NewGuid(),
								Status = StatusModel.Active,
								CreateDate = DateTime.Now.AddSeconds(1),
								AssignmentRMId = assignment_RM.Id,
								SaleId = item_sales.Id,
								IsActive = StatusModel.Active,
								IsSelect = true,
								IsSelectMove = false,
								Sale = _mapper.Map<SaleCustom>(item_sales)
							});
						}

						// OrderBy CreateDate
						assignment_RM.Assignment_RM_Sales = assignment_RM.Assignment_RM_Sales.OrderBy(x => x.CreateDate).ToList();

						assignment_RM.User = null;
						responseItems.Add(assignment_RM);
						index_path++;
					}
				}
			}

			return new PaginationView<List<Assignment_RMCustom>>()
			{
				Items = responseItems,
				Pager = pager
			};
		}

		public async Task<PaginationView<List<Assignment_RMCustom>>> GetListAutoAssign2(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not role.");
			if (!user.Role.IsAssignRM)
			{
				return new();
			}
			var user_Areas = user.User_Areas?.Select(x => new User_AreaCustom()
			{
				ProvinceId = x.ProvinceId,
				ProvinceName = x.ProvinceName
			}).ToList() ?? new();

			//Begin AI
			//ฟังก์ชัน AssignTasks จะทำการมอบหมายงานให้กับพนักงานที่มีสิทธิ์ในพื้นที่นั้นๆ โดยจะหมุนเวียนการมอบหมายงานไปเรื่อยๆ จนกว่างานจะหมด ทำให้แต่ละคนได้รับงานใกล้เคียงกันมากที่สุด
			//ผลลัพธ์ที่ได้จะแสดงรายชื่อพนักงาน พื้นที่รับผิดชอบ และงานที่ได้รับมอบหมาย ซึ่งจะทำให้งานถูกกระจายอย่างเท่าเทียมกันตามความรับผิดชอบของแต่ละคน

			//ยกตัวอย่างการทำงาน:
			//สมมติว่ามีพนักงานที่มีสิทธิ์รับงานในพื้นที่นี้ 3 คน(eligibleEmployees.Count = 3)

			//employeeIndex = (employeeIndex + 1) % eligibleEmployees.Count;
			//เริ่มต้น employeeIndex = 0
			//หลังจากมอบหมายงานชิ้นแรก: (0 + 1) % 3 = 1
			//หลังจากมอบหมายงานชิ้นที่สอง: (1 + 1) % 3 = 2
			//หลังจากมอบหมายงานชิ้นที่สาม: (2 + 1) % 3 = 0(วนกลับมาที่คนแรก)
			//หลังจากมอบหมายงานชิ้นที่สี่: (0 + 1) % 3 = 1
			//และวนซ้ำไปเรื่อยๆ

			//วิธีนี้ทำให้เราสามารถกระจายงานให้กับพนักงานทุกคนอย่างเท่าเทียมกัน โดยไม่ต้องกังวลว่าจะเกินขอบเขตของ list(เพราะ modulo จะทำให้ค่าวนกลับมาอยู่ในช่วงที่ถูกต้องเสมอ) และยังช่วยให้การกระจายงานเป็นไปอย่างต่อเนื่องแม้จะมีจำนวนงานมากกว่าจำนวนพนักงาน
			//End AI

			//ข้อมูลพนักงาน RM เรียงจากลูกค้าที่ดูแลปัจจุบัน น้อย --> มาก
			var query = _repo.Context.Assignment_RMs.Where(x => x.Status != StatusModel.Delete)
												 .Include(x => x.User).ThenInclude(x => x.User_Areas)
												 .OrderBy(x => x.CurrentNumber).ThenBy(x => x.CreateDate)
												 .AsQueryable();

			//var listRmAreas = query.ToList();

			if (user.Role.IsAssignRM)
			{
				query = await QueryAreaAssignment_RM(query, user);
			}

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

			List<Assignment_RMCustom> responseItems = new();

			//ข้อมูลลูกค้าที่ยังไม่ถูกมอบหมาย
			var salesQuery = _repo.Context.Sales
				.Include(x => x.Customer)
				.Where(x => x.Status != StatusModel.Delete && !x.AssUserId.HasValue && x.StatusSaleId == StatusSaleModel.WaitAssign)
				.OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
				.AsQueryable();

			//พื้นที่ดูแล
			if (user.Role.IsAssignRM)
			{
				salesQuery = await QueryArea(salesQuery, user);
			}

			var salesCustomer = await salesQuery.ToListAsync();


			//พื้นที่ดูแล ผจศ.
			List<User_AreaCustom> AreasList = user_Areas;
			//ลูกค้า
			List<Sale> JobsList = salesCustomer;
			//พนักงาน RM
			List<Assignment_RM> EmployeesList = userAssignment;

			foreach (var area in AreasList)
			{
				var jobs = JobsList.Where(j => j.ProvinceId == area.ProvinceId).ToList();
				var eligibleEmployees = EmployeesList.Where(e => e.User.User_Areas.Any(ua => ua.ProvinceId == area.ProvinceId)).ToList();

				if (eligibleEmployees.Count > 0)
				{
					int employeeIndex = 0;
					foreach (var job in jobs)
					{
						var currentEmployee = eligibleEmployees[employeeIndex];

						// ตรวจสอบว่าพนักงานนี้มีงานอยู่แล้วหรือไม่
						var existingAssignment = responseItems.FirstOrDefault(e => e.UserId == currentEmployee.UserId);

						var RM_Sales = new Assignment_RM_SaleCustom()
						{
							Id = Guid.NewGuid(),
							Status = StatusModel.Active,
							CreateDate = DateTime.Now.AddSeconds(1),
							//AssignmentRMId = existingAssignment.Id,
							SaleId = job.Id,
							IsActive = StatusModel.Active,
							IsSelect = true,
							IsSelectMove = false,
							Sale = _mapper.Map<SaleCustom>(job)
						};

						if (existingAssignment != null)
						{
							RM_Sales.AssignmentRMId = existingAssignment.Id;
							// ถ้าเป็นพนักงานคนเดิม ให้เพิ่มงานใน Assignment_RM_Sales
							if (existingAssignment.Assignment_RM_Sales == null) existingAssignment.Assignment_RM_Sales = new();
							existingAssignment.Assignment_RM_Sales.Add(RM_Sales);
						}
						else
						{
							// ถ้าเป็นพนักงานคนใหม่ ให้สร้าง Assignment_RM ใหม่
							var assignment_RM = _mapper.Map<Assignment_RMCustom>(currentEmployee);
							assignment_RM.Tel = assignment_RM.User?.Tel;
							if (assignment_RM.User?.User_Areas?.Count > 0)
							{
								string provinceNames = string.Join(",", assignment_RM.User.User_Areas.Select(x => x.ProvinceName));
								assignment_RM.AreaNameJoin = provinceNames;
							}

							if (assignment_RM.Assignment_RM_Sales == null) assignment_RM.Assignment_RM_Sales = new();
							assignment_RM.Assignment_RM_Sales.Add(RM_Sales);
							responseItems.Add(assignment_RM);
						}

						employeeIndex = (employeeIndex + 1) % eligibleEmployees.Count;
					}
				}
			}

			return new PaginationView<List<Assignment_RMCustom>>()
			{
				Items = responseItems,
				Pager = pager
			};
		}

		public async Task<PaginationView<List<Assignment_RMCustom>>> GetListAutoAssign3(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not role.");
			if (!user.Role.IsAssignRM)
			{
				return new() { Items = new() };
			}

			var user_Areas = user.User_Areas?.Select(x => new User_AreaCustom()
			{
				ProvinceId = x.ProvinceId,
				ProvinceName = x.ProvinceName
			}).ToList() ?? new();

			//Begin AI เลือกพนักงานแบบหมุนเวียน (round-robin)
			//ฟังก์ชัน AssignTasks จะทำการมอบหมายงานให้กับพนักงานที่มีสิทธิ์ในพื้นที่นั้นๆ โดยจะหมุนเวียนการมอบหมายงานไปเรื่อยๆ จนกว่างานจะหมด ทำให้แต่ละคนได้รับงานใกล้เคียงกันมากที่สุด
			//ผลลัพธ์ที่ได้จะแสดงรายชื่อพนักงาน พื้นที่รับผิดชอบ และงานที่ได้รับมอบหมาย ซึ่งจะทำให้งานถูกกระจายอย่างเท่าเทียมกันตามความรับผิดชอบของแต่ละคน

			//ยกตัวอย่างการทำงาน:
			//สมมติว่ามีพนักงานที่มีสิทธิ์รับงานในพื้นที่นี้ 3 คน(eligibleEmployees.Count = 3)

			//employeeIndex = (employeeIndex + 1) % eligibleEmployees.Count;
			//เริ่มต้น employeeIndex = 0
			//หลังจากมอบหมายงานชิ้นแรก: (0 + 1) % 3 = 1
			//หลังจากมอบหมายงานชิ้นที่สอง: (1 + 1) % 3 = 2
			//หลังจากมอบหมายงานชิ้นที่สาม: (2 + 1) % 3 = 0(วนกลับมาที่คนแรก)
			//หลังจากมอบหมายงานชิ้นที่สี่: (0 + 1) % 3 = 1
			//และวนซ้ำไปเรื่อยๆ

			//วิธีนี้ทำให้เราสามารถกระจายงานให้กับพนักงานทุกคนอย่างเท่าเทียมกัน โดยไม่ต้องกังวลว่าจะเกินขอบเขตของ list(เพราะ modulo จะทำให้ค่าวนกลับมาอยู่ในช่วงที่ถูกต้องเสมอ) และยังช่วยให้การกระจายงานเป็นไปอย่างต่อเนื่องแม้จะมีจำนวนงานมากกว่าจำนวนพนักงาน
			//End AI

			//ข้อมูลพนักงาน RM เรียงจากลูกค้าที่ดูแลปัจจุบัน น้อย --> มาก
			var query = _repo.Context.Assignment_RMs.Where(x => x.Status != StatusModel.Delete)
												 .Include(x => x.User).ThenInclude(x => x.User_Areas)
												 .OrderBy(x => x.CurrentNumber).ThenBy(x => x.CreateDate).ThenByDescending(x => x.UserId)
												 .AsQueryable();

			//var listRmAreas = query.ToList();

			if (user.Role.IsAssignRM)
			{
				query = await QueryAreaAssignment_RM(query, user);
			}

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

			List<Assignment_RMCustom> responseItems = new();

			//ข้อมูลลูกค้าที่ยังไม่ถูกมอบหมาย
			var salesQuery = _repo.Context.Sales
				.Include(x => x.Customer)
				.Where(x => x.Status != StatusModel.Delete && !x.AssUserId.HasValue && x.StatusSaleId == StatusSaleModel.WaitAssign)
				.OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
				.AsQueryable();

			//พื้นที่ดูแล
			if (user.Role.IsAssignRM)
			{
				salesQuery = await QueryArea(salesQuery, user);
			}

			var salesCustomer = await salesQuery.ToListAsync();

			//พื้นที่ดูแล ผจศ.
			List<User_AreaCustom> AreasList = user_Areas;
			//ลูกค้า
			List<Sale> JobsList = salesCustomer;
			//พนักงาน RM
			List<Assignment_RM> EmployeesList = userAssignment;

			//วนลูปผ่าน JobsList แทน AreasList
			//ทุกงานจะถูกพิจารณา ไม่ว่าจะอยู่ในพื้นที่ดูแลที่กำหนดไว้ใน AreasList หรือไม่
			if (JobsList.Count > 0 && EmployeesList.Count > 0)
			{
				int index_not_anyjob = 0;
				foreach (var job in JobsList)
				{
					var eligibleEmployees = EmployeesList.Where(e => e.User.User_Areas.Any(ua => ua.ProvinceId == job.ProvinceId)).ToList();
					Assignment_RM? selectedEmployee;

					if (eligibleEmployees.Count > 0)
					{
						// เลือกพนักงานแบบสุ่มจากผู้ที่มีสิทธิ์
						selectedEmployee = eligibleEmployees[new Random().Next(eligibleEmployees.Count)];
					}
					else
					{
						// ถ้าไม่มีพนักงานที่เหมาะสม จะวนมอบหมายให้พนักงานใน EmployeesList
						selectedEmployee = EmployeesList[index_not_anyjob];
						index_not_anyjob++;
						if (EmployeesList.Count == index_not_anyjob)
						{
							index_not_anyjob = 0;
						}
						if (selectedEmployee == null)
						{
							continue; // ข้ามไปงานถัดไป
						}
					}

					var RM_Sales = new Assignment_RM_SaleCustom()
					{
						Id = Guid.NewGuid(),
						Status = StatusModel.Active,
						CreateDate = DateTime.Now.AddSeconds(1),
						SaleId = job.Id,
						IsActive = StatusModel.Active,
						IsSelect = true,
						IsSelectMove = false,
						Sale = _mapper.Map<SaleCustom>(job)
					};

					var existingAssignment = responseItems.FirstOrDefault(e => e.UserId == selectedEmployee.UserId);
					if (existingAssignment != null)
					{
						RM_Sales.AssignmentRMId = existingAssignment.Id;
						if (existingAssignment.Assignment_RM_Sales == null) existingAssignment.Assignment_RM_Sales = new List<Assignment_RM_SaleCustom>();
						existingAssignment.Assignment_RM_Sales.Add(RM_Sales);
					}
					else
					{
						var assignment_RM = _mapper.Map<Assignment_RMCustom>(selectedEmployee);
						assignment_RM.Tel = assignment_RM.User?.Tel;
						if (assignment_RM.User?.User_Areas?.Count > 0)
						{
							string provinceNames = string.Join(",", assignment_RM.User.User_Areas.Select(x => x.ProvinceName));
							assignment_RM.AreaNameJoin = provinceNames;
						}
						assignment_RM.Assignment_RM_Sales = new List<Assignment_RM_SaleCustom> { RM_Sales };
						responseItems.Add(assignment_RM);
					}
				}
			}

			return new PaginationView<List<Assignment_RMCustom>>()
			{
				Items = responseItems,
				Pager = pager
			};
		}

		public async Task<PaginationView<List<Assignment_RMCustom>>> GetListAutoAssign4(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not role.");
			if (!user.Role.IsAssignRM)
			{
				return new() { Items = new() };
			}

			var user_Areas = user.User_Areas?.Select(x => new User_AreaCustom()
			{
				ProvinceId = x.ProvinceId,
				ProvinceName = x.ProvinceName
			}).ToList() ?? new();

			//ข้อมูลพนักงาน RM เรียงจากลูกค้าที่ดูแลปัจจุบัน น้อย --> มาก
			var query = _repo.Context.Assignment_RMs.Where(x => x.Status != StatusModel.Delete)
												 .Include(x => x.User).ThenInclude(x => x.User_Areas)
												 .OrderBy(x => x.CurrentNumber).ThenBy(x => x.CreateDate).ThenByDescending(x => x.UserId)
												 .AsQueryable();

			//var listRmAreas = query.ToList();

			if (user.Role.IsAssignRM)
			{
				query = await QueryAreaAssignment_RM(query, user);
			}

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

			List<Assignment_RMCustom> responseItems = new();

			//ข้อมูลลูกค้าที่ยังไม่ถูกมอบหมาย
			var salesQuery = _repo.Context.Sales
				.Include(x => x.Customer)
				.Where(x => x.Status != StatusModel.Delete && !x.AssUserId.HasValue && x.StatusSaleId == StatusSaleModel.WaitAssign)
				.OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
				.AsQueryable();

			//พื้นที่ดูแล
			if (user.Role.IsAssignRM)
			{
				salesQuery = await QueryArea(salesQuery, user);
			}

			var salesCustomer = await salesQuery.ToListAsync();

			//Begin AI เลือกพนักงานแบบหมุนเวียน (round-robin)
			//ฟังก์ชัน AssignTasks จะทำการมอบหมายงานให้กับพนักงานที่มีสิทธิ์ในพื้นที่นั้นๆ โดยจะหมุนเวียนการมอบหมายงานไปเรื่อยๆ จนกว่างานจะหมด ทำให้แต่ละคนได้รับงานใกล้เคียงกันมากที่สุด
			//ผลลัพธ์ที่ได้จะแสดงรายชื่อพนักงาน พื้นที่รับผิดชอบ และงานที่ได้รับมอบหมาย ซึ่งจะทำให้งานถูกกระจายอย่างเท่าเทียมกันตามความรับผิดชอบของแต่ละคน

			//index_not_anyjob
			//ตัวอย่างการทำงาน:
			//สมมติมีพนักงาน 3 คน และมีงาน 5 งานที่ไม่มีพนักงานที่มีสิทธิ์:
			//งานที่ 1->index = 0->เลือกพนักงานคนที่ 1
			//งานที่ 2->index = 1->เลือกพนักงานคนที่ 2
			//งานที่ 3->index = 2->เลือกพนักงานคนที่ 3
			//งานที่ 4->index รีเซ็ตเป็น 0->เลือกพนักงานคนที่ 1
			//งานที่ 5->index = 1->เลือกพนักงานคนที่ 2

			//วิธีนี้ทำให้เราสามารถกระจายงานให้กับพนักงานทุกคนอย่างเท่าเทียมกัน โดยไม่ต้องกังวลว่าจะเกินขอบเขตของ list(เพราะ modulo จะทำให้ค่าวนกลับมาอยู่ในช่วงที่ถูกต้องเสมอ) และยังช่วยให้การกระจายงานเป็นไปอย่างต่อเนื่องแม้จะมีจำนวนงานมากกว่าจำนวนพนักงาน
			//End AI

			//พื้นที่ดูแล ผจศ.
			List<User_AreaCustom> AreasList = user_Areas;
			//ลูกค้า
			List<Sale> JobsList = salesCustomer;
			//พนักงาน RM
			List<Assignment_RM> EmployeesList = userAssignment;

			if (JobsList.Count > 0 && EmployeesList.Count > 0)
			{
				int index_not_anyjob = 0;
				foreach (var job in JobsList)
				{
					var eligibleEmployees = EmployeesList.Where(e => e.User.User_Areas.Any(ua => ua.ProvinceId == job.ProvinceId)).ToList();
					Assignment_RM? selectedEmployee;

					if (eligibleEmployees.Count > 0)
					{
						// หาพนักงานที่มีงานน้อยที่สุดจากผู้ที่มีสิทธิ์
						selectedEmployee = eligibleEmployees
							.OrderBy(e => GetEmployeeJobCount(e, responseItems))
							.First();
					}
					else
					{
						// ถ้าไม่มีพนักงานที่เหมาะสม จะวนมอบหมายให้พนักงานใน EmployeesList
						selectedEmployee = EmployeesList[index_not_anyjob];
						index_not_anyjob++;
						if (EmployeesList.Count == index_not_anyjob)
						{
							index_not_anyjob = 0;
						}
						if (selectedEmployee == null)
						{
							continue; // ข้ามไปงานถัดไป
						}
					}

					var RM_Sales = new Assignment_RM_SaleCustom()
					{
						Id = Guid.NewGuid(),
						Status = StatusModel.Active,
						CreateDate = DateTime.Now.AddSeconds(1),
						SaleId = job.Id,
						IsActive = StatusModel.Active,
						IsSelect = true,
						IsSelectMove = false,
						Sale = _mapper.Map<SaleCustom>(job)
					};

					//ไม่ต้องตรวจสอบที่อยู่
					if (RM_Sales.Sale != null && RM_Sales.Sale.Customer != null)
					{
						RM_Sales.Sale.Customer.IsExceptValidAddress = true;
					}

					var existingAssignment = responseItems.FirstOrDefault(e => e.UserId == selectedEmployee.UserId);
					if (existingAssignment != null)
					{
						RM_Sales.AssignmentRMId = existingAssignment.Id;
						if (existingAssignment.Assignment_RM_Sales == null)
							existingAssignment.Assignment_RM_Sales = new List<Assignment_RM_SaleCustom>();
						existingAssignment.Assignment_RM_Sales.Add(RM_Sales);
					}
					else
					{
						var assignment_RM = _mapper.Map<Assignment_RMCustom>(selectedEmployee);
						assignment_RM.Tel = assignment_RM.User?.Tel;
						if (assignment_RM.User?.User_Areas?.Count > 0)
						{
							string provinceNames = string.Join(",", assignment_RM.User.User_Areas.Select(x => x.ProvinceName));
							assignment_RM.AreaNameJoin = provinceNames;
						}
						assignment_RM.Assignment_RM_Sales = new List<Assignment_RM_SaleCustom> { RM_Sales };
						responseItems.Add(assignment_RM);
					}
				}

				// เพิ่มพนักงานที่ยังไม่มีงานเข้าไปใน responseItems
				var remainingEmployees = EmployeesList
					.Where(employee => !responseItems.Any(r => r.UserId == employee.UserId))
					.ToList();

				foreach (var employee in remainingEmployees)
				{
					var assignment_RM = _mapper.Map<Assignment_RMCustom>(employee);
					assignment_RM.Tel = assignment_RM.User?.Tel;

					// เพิ่มข้อมูลพื้นที่รับผิดชอบ
					if (assignment_RM.User?.User_Areas?.Count > 0)
					{
						string provinceNames = string.Join(",",
							assignment_RM.User.User_Areas.Select(x => x.ProvinceName));
						assignment_RM.AreaNameJoin = provinceNames;
					}

					// สร้าง list งานเปล่า
					assignment_RM.Assignment_RM_Sales = new List<Assignment_RM_SaleCustom>();

					// เพิ่มเข้า responseItems
					responseItems.Add(assignment_RM);
				}

			}

			return new PaginationView<List<Assignment_RMCustom>>()
			{
				Items = responseItems,
				Pager = pager
			};
		}

		// เพิ่มเมธอดสำหรับนับจำนวนงานของพนักงาน
		private int GetEmployeeJobCount(Assignment_RM employee, List<Assignment_RMCustom> responseItems)
		{
			var existingAssignment = responseItems.FirstOrDefault(e => e.UserId == employee.UserId);
			return existingAssignment?.Assignment_RM_Sales?.Count ?? 0;
		}

		public async Task<PaginationView<List<Assignment_RMCustom>>> GetListRM(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not role.");
			if (!user.Role.IsAssignRM)
			{
				return new() { Items = new() };
			}

			var query = _repo.Context.Assignment_RMs.Where(x => x.Status != StatusModel.Delete)
												 .Include(x => x.User).ThenInclude(x => x.User_Areas)
												 .OrderBy(x => x.CurrentNumber).ThenBy(x => x.CreateDate)
												 .AsQueryable();

			query = await QueryAreaAssignment_RM(query, user);


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
				//ยังไม่ confirm เรื่องจังหวัดและอำเภอที่ดูแล
			}

			if (model.Branchs?.Count > 0)
			{
				//var idList = GeneralUtils.ListStringToInt(model.Branchs);
				//if (idList.Count > 0)
				//{
				//	query = query.Where(x => x.BranchId.HasValue && idList.Contains(x.BranchId));
				//}
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Assignment_RMCustom>>()
			{
				Items = _mapper.Map<List<Assignment_RMCustom>>(await items.ToListAsync()),
				Pager = pager
			};

		}

		public async Task Assign(List<Assignment_RMCustom> model)
		{
			foreach (var item in model)
			{
				var assignment_RM = await _repo.Context.Assignment_RMs.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == item.Id);

				var sales_select = item.Assignment_RM_Sales?.Where(x => x.IsSelect).ToList();

				if (assignment_RM != null && sales_select?.Count > 0)
				{
					var salesToWaitUpdate = new List<SaleCustom>();

					foreach (var item_sale in sales_select)
					{
						var currentUserName = await _repo.User.GetFullNameById(item.CurrentUserId);
						var assUserName = await _repo.User.GetFullNameById(assignment_RM.UserId);

						var assignmentRMSale = await CreateSale(new()
						{
							CreateBy = item.CurrentUserId,
							CreateByName = currentUserName,
							AssignmentRMId = assignment_RM.Id,
							SaleId = item_sale.SaleId
						});

						salesToWaitUpdate.Add(new()
						{
							Id = item_sale.SaleId,
							AssUserId = assignment_RM.UserId,
							AssUserName = assUserName
						});

						await _repo.Sales.UpdateStatusOnly(new()
						{
							SaleId = item_sale.SaleId,
							StatusId = StatusSaleModel.WaitContact,
							CreateBy = item.CurrentUserId,
							CreateByName = currentUserName
						});

						await _repo.Dashboard.UpdateDeliverById(new() { saleid = item_sale.SaleId });
					}

					// รวบรวมการอัปเดตแล้วทำครั้งเดียว UpdateRange
					if (salesToWaitUpdate.Any())
					{
						var salesToUpdate = new List<Sale>();
						foreach (var item_upate in salesToWaitUpdate)
						{
							var sales = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == item_upate.Id);
							if (sales != null)
							{
								sales.AssUserId = item_upate.AssUserId;
								sales.AssUserName = item_upate.AssUserName;
								salesToUpdate.Add(sales);
							}
						}
						if (salesToUpdate.Any())
						{
							_db.UpdateRange(salesToUpdate);
							await _db.SaveAsync();
						}
					}

					await UpdateCurrentNumber(assignment_RM.UserId);
				}
			}
		}

		public async Task AssignChange(AssignChangeModel model)
		{
			var assignments_RM_sales = await _repo.Context.Assignment_RM_Sales
				.Include(s => s.AssignmentRM)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.IsActive == StatusModel.Active && x.SaleId == model.Original.Id);

			if (assignments_RM_sales == null) throw new ExceptionCustom("Assignment_RM something went wrong!");

			assignments_RM_sales.Status = StatusModel.InActive;
			assignments_RM_sales.IsActive = StatusModel.InActive;
			assignments_RM_sales.AssignmentRM.CurrentNumber = assignments_RM_sales.AssignmentRM.CurrentNumber - 1;
			_db.Update(assignments_RM_sales);
			await _db.SaveAsync();

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);
			var assUserName = await _repo.User.GetFullNameById(model.New.UserId);

			var assignmentSale = await CreateSale(new()
			{
				CreateBy = model.CurrentUserId,
				CreateByName = currentUserName,
				AssignmentRMId = model.New.Id,
				SaleId = model.Original.Id
			});

			var sales = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == model.Original.Id);
			if (sales != null)
			{
				if (sales.StatusSaleId != StatusSaleModel.WaitContact)
				{
					throw new ExceptionCustom("ไม่อยู่ในเงื่อนไขการมอบหมายใหม่");
				}

				sales.AssUserId = model.New.UserId;
				sales.AssUserName = assUserName;
				_db.Update(sales);
				await _db.SaveAsync();
			}

			await UpdateCurrentNumber(model.New.UserId);

		}

		public async Task AssignReturnChange(AssignChangeModel model)
		{
			DateTime _dateNow = DateTime.Now;

			var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);
			var assUserName = await _repo.User.GetFullNameById(model.New.UserId);

			var assignments_RM_sales = await _repo.Context.Assignment_RM_Sales
				.Include(s => s.AssignmentRM)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.IsActive == StatusModel.Active && x.SaleId == model.Original.Id);

			//กรณีถูกมอบหมายงานแล้ว
			if (assignments_RM_sales != null)
			{
				assignments_RM_sales.Status = StatusModel.InActive;
				assignments_RM_sales.IsActive = StatusModel.InActive;
				assignments_RM_sales.AssignmentRM.CurrentNumber = assignments_RM_sales.AssignmentRM.CurrentNumber - 1;
				_db.Update(assignments_RM_sales);
				await _db.SaveAsync();

				var assignmentSale = await CreateSale(new()
				{
					CreateBy = model.CurrentUserId,
					CreateByName = currentUserName,
					AssignmentRMId = model.New.Id,
					SaleId = model.Original.Id
				});
			}

			var sales = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == model.Original.Id);
			if (sales != null)
			{
				var sale_Statuses = await _repo.Context.Sale_Statuses
					.Where(x => x.Status != StatusModel.Delete && x.SaleId == model.Original.Id)
					.ToListAsync();
				if (sale_Statuses != null && sale_Statuses.Count >= 2)
				{
					sales.AssUserAlready = true;
					sales.AssUserId = model.New.UserId;
					sales.AssUserName = assUserName;
					sales.AssUserDate = _dateNow;
					_db.Update(sales);
					await _db.SaveAsync();

					//สถานะล่าสุดก่อนส่งคืน
					var sale_beforelast = sale_Statuses.OrderByDescending(s => s.CreateDate).Take(2).Skip(1).FirstOrDefault();
					if (sale_beforelast != null)
					{
						sale_beforelast.Status = StatusModel.Delete;
						_db.Update(sale_beforelast);
						await _db.SaveAsync();

						await _repo.Sales.UpdateStatusOnly(new()
						{
							SaleId = sales.Id,
							StatusId = sale_beforelast.StatusId,
							CreateBy = model.CurrentUserId,
							CreateByName = currentUserName
						});
					}
				}

			}

			await UpdateCurrentNumber(model.New.UserId);

		}

		public async Task CreateAssignmentRMAll(allFilter model)
		{
			var usersRM = await _repo.Context.Users.Include(x => x.Role)
										   .Include(x => x.Assignment_RMs.Where(s => s.Status == StatusModel.Active))
										   .Where(x => x.Status == StatusModel.Active && x.Role != null && x.Role.Code == RoleCodes.RM && x.Assignment_RMs.Count == 0)
										   .OrderBy(x => x.Id)
										   .ToListAsync();

			if (usersRM.Count > 0)
			{
				int i = 1;
				foreach (var item_rm in usersRM)
				{
					var assignment = await _repo.AssignmentRM.Create(new()
					{
						Status = StatusModel.Active,
						CreateDate = DateTime.Now.AddSeconds(i),
						UserId = item_rm.Id,
						EmployeeId = item_rm.EmployeeId,
						EmployeeName = item_rm.FullName,
					});
					i++;
				}
			}

		}

	}
}

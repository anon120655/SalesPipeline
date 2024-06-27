using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

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

		public async Task<bool> CheckAssignmentByBranchId(int id)
		{
			return await _repo.Context.Assignment_Centers.AnyAsync(x => x.BranchId == id);
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

			if (model.BranchId.HasValue)
			{
				if (await CheckAssignmentByBranchId(model.BranchId.Value))
					throw new ExceptionCustom("มีผู้จัดการศูนย์สาขานี้แล้ว");
			}

			if (string.IsNullOrEmpty(model.EmployeeName))
			{
				model.EmployeeName = await _repo.User.GetFullNameById(model.UserId);
			}

			var assignment_Center = new Data.Entity.Assignment_Center();
			assignment_Center.Status = model.Status;
			assignment_Center.CreateDate = DateTime.Now;
			assignment_Center.BranchId = model.BranchId;
			assignment_Center.BranchCode = model.BranchCode;
			assignment_Center.BranchName = model.BranchName;
			assignment_Center.UserId = model.UserId;
			assignment_Center.EmployeeId = model.EmployeeId;
			assignment_Center.EmployeeName = model.EmployeeName;
			assignment_Center.Tel = model.Tel;
			assignment_Center.RMNumber = model.RMNumber ?? 0;
			assignment_Center.CurrentNumber = model.CurrentNumber ?? 0;
			await _db.InsterAsync(assignment_Center);
			await _db.SaveAsync();

			if (model.BranchId.HasValue)
			{
				await _repo.AssignmentCenter.UpdateCurrentNumber(model.BranchId.Value);
				//await _repo.AssignmentRM.UpdateAssignmentEmpty(model.BranchId.Value);
			}

			return _mapper.Map<Assignment_CenterCustom>(assignment_Center);
		}

		public async Task<Assignment_CenterCustom> Update(Assignment_CenterCustom model)
		{
			var assignment_Center = await _repo.Context.Assignment_Centers.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == model.Id);
			if (assignment_Center != null)
			{
				assignment_Center.Status = model.Status;
				assignment_Center.BranchId = model.BranchId;
				assignment_Center.BranchCode = model.BranchCode;
				assignment_Center.BranchName = model.BranchName;
				assignment_Center.UserId = model.UserId;
				assignment_Center.EmployeeId = model.EmployeeId;
				assignment_Center.EmployeeName = model.EmployeeName;
				assignment_Center.Tel = model.Tel;
				_db.Update(assignment_Center);
				await _db.SaveAsync();

				if (model.BranchId.HasValue)
				{
					await _repo.AssignmentCenter.UpdateCurrentNumber(model.BranchId.Value);
					//await _repo.AssignmentRM.UpdateAssignmentEmpty(model.BranchId.Value);
				}

			}
			return _mapper.Map<Assignment_CenterCustom>(assignment_Center);
		}

		public async Task<PaginationView<List<Assignment_CenterCustom>>> GetListAutoAssign(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not role.");
			if (!user.Role.Code.ToUpper().StartsWith(RoleCodes.LOAN))
			{
				return new();
			}

			//*************** ต้องเช็คพวก สาขา จังหวัด อำเภอ เพิ่มเติม ****************

			//รูปแบบการมอบหมายตามเกณฑ์
			//1. ผจศ. เรียงจากลูกค้าที่ดูแลปัจจุบัน น้อย --> มาก
			//2. หาข้อมูลลูกค้าที่ยังไม่ถูกมอบหมาย
			//3. แยกรายการลูกค้าที่ยังไม่ถูกมอบหมายออกเป็นส่วนเท่าๆ กัน
			//4. มอบหมายให้ ผจศ. เท่าๆ กัน  (ผจศ. ที่ดูแลลูกค้าน้อยสุดจะถูกมอบหมายก่อนเรียงลำดับไปเรื่อยๆ)

			//ข้อมูลพนักงาน  ผจศ. เรียงจากลูกค้าที่ดูแลปัจจุบัน น้อย --> มาก
			var query = _repo.Context.Assignment_Centers.Where(x => x.Status != StatusModel.Delete)
												 .Include(x => x.User)
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

			if (model.provinceid.HasValue)
			{
				//ยังไม่ confirm เรื่องจังหวัดและอำเภอที่ดูแล
			}

			if (model.amphurid.HasValue)
			{
				//ยังไม่ confirm เรื่องจังหวัดและอำเภอที่ดูแล
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var userAssignment = await query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToListAsync();

			List<Assignment_CenterCustom> responseItems = new();

			//ข้อมูลลูกค้าที่ยังไม่ถูกมอบหมาย
			var salesQuery = _repo.Context.Sales
				.Include(x => x.Customer)
				.Where(x => x.Status != StatusModel.Delete && !x.AssUserId.HasValue && x.StatusSaleId == StatusSaleModel.WaitAssignCenter)
				.OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
				.AsQueryable();

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
						//var assignment_Center = _mapper.Map<Assignment_CenterCustom>(userAssignment[index_path]);
						//assignment_Center.Assignment_RM_Sales = new();
						//assignment_Center.Tel = assignment_Center.User?.Tel;
						//assignment_Center.ProvinceName = assignment_Center.User?.ProvinceName;
						//assignment_Center.BranchName = assignment_Center.User?.BranchName;

						//foreach (var item_sales in item_path)
						//{
						//	assignment_Center.Assignment_RM_Sales.Add(new()
						//	{
						//		Id = Guid.NewGuid(),
						//		Status = StatusModel.Active,
						//		CreateDate = DateTime.Now.AddSeconds(1),
						//		AssignmentRMId = assignment_RM.Id,
						//		SaleId = item_sales.Id,
						//		IsActive = StatusModel.Active,
						//		IsSelect = true,
						//		IsSelectMove = false,
						//		Sale = _mapper.Map<SaleCustom>(item_sales)
						//	});
						//}

						//assignment_Center.Assignment_RM_Sales = assignment_Center.Assignment_RM_Sales.OrderBy(x => x.CreateDate).ToList();

						//assignment_Center.User = null;
						//responseItems.Add(assignment_Center);
						//index_path++;
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
												 .Include(x => x.Branch)
												 .OrderBy(x => x.CurrentNumber).ThenBy(x => x.CreateDate)
												 .AsQueryable();

			if (!String.IsNullOrEmpty(model.assignmentid))
			{
				if (Guid.TryParse(model.assignmentid, out Guid assignmentid))
				{
					query = query.Where(x => x.Id == assignmentid);
				}
			}

			if (!String.IsNullOrEmpty(model.mcenter_code))
			{
				query = query.Where(x => x.BranchCode != null && x.BranchCode.Contains(model.mcenter_code));
			}

			if (!String.IsNullOrEmpty(model.mcenter_name))
			{
				query = query.Where(x => x.BranchName != null && x.BranchName.Contains(model.mcenter_name));
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
						//if (assignment_MCenter.User != null)
						//{
						//	sale.Master_Branch_RegionId = assignment_MCenter.User.Master_Branch_RegionId;
						//	if (assignment_MCenter.User.Master_Branch_Region != null)
						//	{
						//		sale.Master_Branch_RegionName = assignment_MCenter.User.Master_Branch_Region.Name;
						//	}
						//	sale.ProvinceId = assignment_MCenter.User.ProvinceId;
						//	sale.ProvinceName = assignment_MCenter.User.ProvinceName;
						//}

						sale.BranchId = assignment_CenterBranch.BranchId;
						sale.BranchName = assignment_CenterBranch.BranchName;
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

		public async Task UpdateCurrentNumber(int id)
		{
			var assignment_RMs = await _repo.Context.Assignment_RMs
											  .Where(x => x.BranchId == id && x.Status == StatusModel.Active)
											  .ToListAsync();

			int countRm = assignment_RMs.Count;

			var assignments = await _repo.Context.Assignment_Centers.FirstOrDefaultAsync(x => x.BranchId == id && x.Status == StatusModel.Active);
			if (assignments != null)
			{
				assignments.RMNumber = countRm;

				var sales = await _repo.Context.Sales
												  .Where(x => x.AssCenterUserId == assignments.UserId && x.Status == StatusModel.Active)
												  .ToListAsync();

				assignments.CurrentNumber = sales.Count;
				_db.Update(assignments);
				await _db.SaveAsync();
			}

		}

		public async Task CreateAssignmentCenterAll(allFilter model)
		{
			var usersCenter = await _repo.Context.Users.Include(x => x.Role)
										   .Include(x => x.Assignment_Centers)
										   .Where(x => x.Status != StatusModel.Delete && x.BranchId.HasValue && x.Role != null && x.Role.Code == RoleCodes.CEN_BRANCH && x.Assignment_Centers.Count == 0)
										   .OrderBy(x => x.Id)
										   .ToListAsync();

			if (usersCenter.Count > 0)
			{
				foreach (var item_center in usersCenter)
				{
					if (item_center.BranchId.HasValue)
					{
						string? _code = null;
						string? _name = null;
						var branch = await _repo.Thailand.GetBranchByid(item_center.BranchId.Value);
						if (branch != null)
						{
							_code = branch.BranchCode;
							_name = branch.BranchName;
						}

						var assignmentCenter = await _repo.AssignmentCenter.Create(new()
						{
							Status = StatusModel.Active,
							BranchId = item_center.BranchId,
							BranchCode = _code,
							BranchName = _name,
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
}

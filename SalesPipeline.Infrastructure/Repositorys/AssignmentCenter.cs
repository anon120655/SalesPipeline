using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
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
			return await _repo.Context.Assignment_CenterBranches.AnyAsync(x => x.UserId == id);
		}

		public async Task<bool> CheckAssignmentByBranchId(int id)
		{
			return await _repo.Context.Assignment_CenterBranches.AnyAsync(x => x.BranchId == id);
		}

		public async Task<Assignment_CenterBranchCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Assignment_CenterBranches
				.Where(x => x.Id == id)
				.Include(x => x.User).ThenInclude(x => x.Master_Branch_Region)
				.FirstOrDefaultAsync();
			return _mapper.Map<Assignment_CenterBranchCustom>(query);
		}

		public async Task<Assignment_CenterBranchCustom> GetByUserId(int id)
		{
			var query = await _repo.Context.Assignment_CenterBranches
				.Where(x => x.UserId == id).FirstOrDefaultAsync();
			return _mapper.Map<Assignment_CenterBranchCustom>(query);
		}

		public async Task<Assignment_CenterBranchCustom> Create(Assignment_CenterBranchCustom model)
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

			var assignment_CenterBranch = new Data.Entity.Assignment_CenterBranch();
			assignment_CenterBranch.Status = model.Status;
			assignment_CenterBranch.CreateDate = DateTime.Now;
			assignment_CenterBranch.BranchId = model.BranchId;
			assignment_CenterBranch.BranchCode = model.BranchCode;
			assignment_CenterBranch.BranchName = model.BranchName;
			assignment_CenterBranch.UserId = model.UserId;
			assignment_CenterBranch.EmployeeId = model.EmployeeId;
			assignment_CenterBranch.EmployeeName = model.EmployeeName;
			assignment_CenterBranch.Tel = model.Tel;
			assignment_CenterBranch.RMNumber = model.RMNumber ?? 0;
			assignment_CenterBranch.CurrentNumber = model.CurrentNumber ?? 0;
			await _db.InsterAsync(assignment_CenterBranch);
			await _db.SaveAsync();

			if (model.BranchId.HasValue)
			{
				await _repo.AssignmentCenter.UpdateCurrentNumber(model.BranchId.Value);
				//await _repo.AssignmentRM.UpdateAssignmentEmpty(model.BranchId.Value);
			}

			return _mapper.Map<Assignment_CenterBranchCustom>(assignment_CenterBranch);
		}

		public async Task<Assignment_CenterBranchCustom> Update(Assignment_CenterBranchCustom model)
		{
			var assignment_CenterBranche = await _repo.Context.Assignment_CenterBranches.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == model.Id);
			if (assignment_CenterBranche != null)
			{
				assignment_CenterBranche.Status = model.Status;
				assignment_CenterBranche.BranchId = model.BranchId;
				assignment_CenterBranche.BranchCode = model.BranchCode;
				assignment_CenterBranche.BranchName = model.BranchName;
				assignment_CenterBranche.UserId = model.UserId;
				assignment_CenterBranche.EmployeeId = model.EmployeeId;
				assignment_CenterBranche.EmployeeName = model.EmployeeName;
				assignment_CenterBranche.Tel = model.Tel;
				_db.Update(assignment_CenterBranche);
				await _db.SaveAsync();

				if (model.BranchId.HasValue)
				{
					await _repo.AssignmentCenter.UpdateCurrentNumber(model.BranchId.Value);
					//await _repo.AssignmentRM.UpdateAssignmentEmpty(model.BranchId.Value);
				}

			}
			return _mapper.Map<Assignment_CenterBranchCustom>(assignment_CenterBranche);
		}

		public async Task<PaginationView<List<Assignment_CenterBranchCustom>>> GetListCenter(allFilter model)
		{
			var query = _repo.Context.Assignment_CenterBranches.Where(x => x.Status != StatusModel.Delete)
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

			return new PaginationView<List<Assignment_CenterBranchCustom>>()
			{
				Items = _mapper.Map<List<Assignment_CenterBranchCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task Assign(AssignModel model)
		{
			var assignment_CenterBranch = await _repo.Context.Assignment_CenterBranches
				//.Include(x => x.User).ThenInclude(x => x.Master_Branch_Region) //Include จะทำให้ update sale บางตัวไม่ได้
				.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.Id == model.AssignMCenter.Id);
			if (assignment_CenterBranch != null)
			{
				var salesCount = 0;
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

						salesCount++;
					}
				}

				assignment_CenterBranch.CurrentNumber = assignment_CenterBranch.CurrentNumber + salesCount;
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

			var assignments = await _repo.Context.Assignment_CenterBranches.FirstOrDefaultAsync(x => x.BranchId == id && x.Status == StatusModel.Active);
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
										   .Include(x => x.Assignment_CenterBranches)
										   .Where(x => x.Status != StatusModel.Delete && x.BranchId.HasValue && x.Role != null && x.Role.Code == RoleCodes.CEN_BRANCH && x.Assignment_CenterBranches.Count == 0)
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

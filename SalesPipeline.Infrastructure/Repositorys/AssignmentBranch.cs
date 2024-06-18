using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
    public class AssignmentBranch : IAssignmentBranch
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public AssignmentBranch(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<bool> CheckAssignmentByUserId(int id)
		{
			return await _repo.Context.Assignment_BranchRegs.AnyAsync(x => x.UserId == id);
		}

		public async Task<bool> CheckAssignmentByBranchId(int id)
		{
			return await _repo.Context.Assignment_BranchRegs.AnyAsync(x => x.BranchId == id);
		}

		public async Task<Assignment_BranchRegCustom> Create(Assignment_BranchRegCustom model)
		{
			if (await CheckAssignmentByUserId(model.UserId))
				throw new ExceptionCustom("assignment duplicate user");

			if (model.BranchId.HasValue)
			{
				if (await CheckAssignmentByBranchId(model.BranchId.Value))
					throw new ExceptionCustom("มีเจ้าหน้าที่กิจการสาขาภาคนี้แล้ว");
			}

			if (string.IsNullOrEmpty(model.EmployeeName))
			{
				model.EmployeeName = await _repo.User.GetFullNameById(model.UserId);
			}

			var assignment_Branch = new Data.Entity.Assignment_BranchReg();
			assignment_Branch.Status = model.Status;
			assignment_Branch.CreateDate = DateTime.Now;
			assignment_Branch.Master_Branch_RegionId = model.Master_Branch_RegionId;
			assignment_Branch.BranchId = model.BranchId;
			assignment_Branch.BranchCode = model.BranchCode;
			assignment_Branch.BranchName = model.BranchName;
			assignment_Branch.UserId = model.UserId;
			assignment_Branch.EmployeeId = model.EmployeeId;
			assignment_Branch.EmployeeName = model.EmployeeName;
			assignment_Branch.Tel = model.Tel;
			assignment_Branch.CurrentNumber = model.CurrentNumber ?? 0;
			await _db.InsterAsync(assignment_Branch);
			await _db.SaveAsync();

			return _mapper.Map<Assignment_BranchRegCustom>(assignment_Branch);
		}

		public async Task<PaginationView<List<Assignment_BranchRegCustom>>> GetListBranch(allFilter model)
		{
			var query = _repo.Context.Assignment_BranchRegs.Where(x => x.Status != StatusModel.Delete)
												 .Include(x => x.Branch)
												 .OrderBy(x => x.CreateDate)
												 .AsQueryable();

			//if (!String.IsNullOrEmpty(model.assignmentid))
			//{
			//	if (Guid.TryParse(model.assignmentid, out Guid assignmentid))
			//	{
			//		query = query.Where(x => x.Id == assignmentid);
			//	}
			//}

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

			return new PaginationView<List<Assignment_BranchRegCustom>>()
			{
				Items = _mapper.Map<List<Assignment_BranchRegCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task UpdateCurrentNumber(int id)
		{
			//var sales = await _repo.Context.Sales
			//								  .Where(x => x. == assignments.UserId && x.Status == StatusModel.Active)
			//								  .ToListAsync();

			//assignments.CurrentNumber = sales.Count;
			//_db.Update(assignments);
			//await _db.SaveAsync();
		}

		public async Task AutoAssignToMCenter(AssignModel model)
		{
			if (model.Sales.Count > 0)
			{
				foreach (var item in model.Sales)
				{
					var sale = await _repo.Context.Sales.Include(x=>x.Customer).FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == item.Id);
					if (sale != null && sale.Customer != null)
					{
						if (sale.StatusSaleId == StatusSaleModel.WaitVerifyBranch)
						{
							//ตรวจสอบว่าลูกค้าอยู่จังหวัดและอำเภอไหน และ assign ไปที่สาขาภาคนั้น **รอถามว่าตอนสร้างต้องระบุสาขาเลยไหม
							var provinceId = sale.Customer.ProvinceId;
							var amphurId = sale.Customer.AmphurId;

							//รูปแบบการมอบหมายตามเกณฑ์
							//1. เรียงจากลูกค้าที่ดูแลปัจจุบัน น้อย --> มาก
							//2. หาลูกค้าที่ยังไม่ถูกมอบหมาย
							//4. มอบหมายให้พนักงานเท่าๆ กัน  (พนักงานที่ดูแลลูกค้าน้อยสุดจะถูกมอบหมายก่อนเรียงลำดับไปเรื่อยๆ)

						}
					}
				}
			}
		}

		public async Task Assign(AssignModel model)
		{
			var assignment_BranchReg = await _repo.Context.Assignment_BranchRegs
				//.Include(x => x.User).ThenInclude(x => x.Master_Branch_Region) //Include จะทำให้ update sale บางตัวไม่ได้
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == model.AssignMBranch.Id);
			if (assignment_BranchReg != null)
			{
				foreach (var item in model.Sales)
				{
					var sale = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == item.Id);
					if (sale != null)
					{
						sale.AssCenterUser = null;
						sale.AssUser = null;

						//if (sale.AssCenterUserId.HasValue) throw new ExceptionCustom($"assignment duplicate {sale.CompanyName}");

						//ข้อมูลนี้จะได้มาตั้งแต่ตอนเพิ่มรายการ
						//if (assignment_Branch.User != null)
						//{
						//	sale.Master_Branch_RegionId = assignment_Branch.User.Master_Branch_RegionId;
						//	if (assignment_Branch.User.Master_Branch_Region != null)
						//	{
						//		sale.Master_Branch_RegionName = assignment_Branch.User.Master_Branch_Region.Name;
						//	}
						//	sale.ProvinceId = assignment_Branch.User.ProvinceId;
						//	sale.ProvinceName = assignment_Branch.User.ProvinceName;
						//}

						sale.Master_Branch_RegionId = assignment_BranchReg.Master_Branch_RegionId;
						sale.BranchId = assignment_BranchReg.BranchId;
						sale.BranchName = assignment_BranchReg.BranchName;
						sale.AssCenterUserId = null;
						sale.AssCenterUserName = null;
						sale.AssCenterCreateBy = null;
						sale.AssCenterDate = null;
						sale.AssUserId = null;
						sale.AssUserName = null;
						_db.Update(sale);
						await _db.SaveAsync();

						var currentUserName = await _repo.User.GetFullNameById(model.CurrentUserId);

						await _repo.Sales.UpdateStatusOnly(new()
						{
							SaleId = sale.Id,
							StatusId = StatusSaleModel.WaitAssignCenter,
							CreateBy = model.CurrentUserId,
							CreateByName = currentUserName,
						});

					}
				}
			}
		}

		public async Task CreateAssignmentBranchAll(allFilter model)
		{
			var usersBranch = await _repo.Context.Users.Include(x => x.Role)
										   .Include(x => x.Assignment_BranchRegs)
										   .Where(x => x.Status != StatusModel.Delete && x.BranchId.HasValue && x.Role != null && x.Role.Code == RoleCodes.BRANCH_REG_02 && x.Assignment_BranchRegs.Count == 0)
										   .OrderBy(x => x.Id)
										   .ToListAsync();

			if (usersBranch.Count > 0)
			{
				foreach (var item_branch in usersBranch)
				{
					if (item_branch.BranchId.HasValue)
					{
						string? _code = null;
						string? _name = null;
						var branch = await _repo.Thailand.GetBranchByid(item_branch.BranchId.Value);
						if (branch != null)
						{
							_code = branch.BranchCode;
							_name = branch.BranchName;
						}

						var assignmentCenter = await _repo.AssignmentBranch.Create(new()
						{
							Status = StatusModel.Active,
							Master_Branch_RegionId = item_branch.Master_Branch_RegionId,
							BranchId = item_branch.BranchId,
							BranchCode = _code,
							BranchName = _name,
							UserId = item_branch.Id,
							EmployeeId = item_branch.EmployeeId,
							EmployeeName = item_branch.FullName,
							Tel = item_branch.Tel
						});
					}

				}
			}
		}

	}
}

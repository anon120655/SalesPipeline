using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
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
			return await _repo.Context.Assignment_Branches.AnyAsync(x => x.UserId == id);
		}

		public async Task<bool> CheckAssignmentByBranchId(int id)
		{
			return await _repo.Context.Assignment_Branches.AnyAsync(x => x.BranchId == id);
		}

		public async Task<Assignment_BranchCustom> Create(Assignment_BranchCustom model)
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

			var assignment_Branch = new Data.Entity.Assignment_Branch();
			assignment_Branch.Status = model.Status;
			assignment_Branch.CreateDate = DateTime.Now;
			assignment_Branch.BranchId = model.BranchId;
			assignment_Branch.BranchCode = model.BranchCode;
			assignment_Branch.BranchName = model.BranchName;
			assignment_Branch.UserId = model.UserId;
			assignment_Branch.EmployeeId = model.EmployeeId;
			assignment_Branch.EmployeeName = model.EmployeeName;
			assignment_Branch.Tel = model.Tel;
			await _db.InsterAsync(assignment_Branch);
			await _db.SaveAsync();

			return _mapper.Map<Assignment_BranchCustom>(assignment_Branch);
		}

		public async Task CreateAssignmentBranchAll(allFilter model)
		{
			var usersBranch = await _repo.Context.Users.Include(x => x.Role)
										   .Include(x => x.Assignment_Branches)
										   .Where(x => x.Status != StatusModel.Delete && x.BranchId.HasValue && x.Role != null && x.Role.Code == RoleCodes.BRANCH02 && x.Assignment_Branches.Count == 0)
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

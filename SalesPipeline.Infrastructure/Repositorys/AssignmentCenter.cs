using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using SalesPipeline.Infrastructure.Data.Entity;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
			return await _repo.Context.Assignments.AnyAsync(x => x.UserId == id);
		}

		public async Task<AssignmentCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Assignments
				.Where(x => x.Id == id)
				.Include(x => x.User).ThenInclude(x => x.Master_Department_Center)
				.FirstOrDefaultAsync();
			return _mapper.Map<AssignmentCustom>(query);
		}

		public async Task<AssignmentCustom> GetByUserId(int id)
		{
			var query = await _repo.Context.Assignments
				.Where(x => x.UserId == id).FirstOrDefaultAsync();
			return _mapper.Map<AssignmentCustom>(query);
		}

		public async Task<AssignmentCustom> Create(AssignmentCustom model)
		{
			if (await CheckAssignmentByUserId(model.UserId))
				throw new ExceptionCustom("assignment duplicate user");

			if (string.IsNullOrEmpty(model.EmployeeName))
			{
				model.EmployeeName = await _repo.User.GetFullNameById(model.UserId);
			}

			var assignment = new Data.Entity.Assignment();
			assignment.Status = StatusModel.Active;
			assignment.CreateDate = DateTime.Now;
			assignment.Code = model.Code;
			assignment.Name = model.Name;
			assignment.UserId = model.UserId;
			assignment.EmployeeId = model.EmployeeId;
			assignment.EmployeeName = model.EmployeeName;
			assignment.Tel = model.Tel;
			assignment.RMNumber = model.RMNumber ?? 0;
			assignment.CurrentNumber = model.CurrentNumber ?? 0;
			await _db.InsterAsync(assignment);
			await _db.SaveAsync();

			return _mapper.Map<AssignmentCustom>(assignment);
		}

		public async Task<AssignmentCustom> Update(AssignmentCustom model)
		{
			var assignment = await _repo.Context.Assignments.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == model.Id);
			if (assignment != null)
			{
				assignment.Code = model.Code;
				assignment.Name = model.Name;
				assignment.UserId = model.UserId;
				assignment.EmployeeId = model.EmployeeId;
				assignment.EmployeeName = model.EmployeeName;
				assignment.Tel = model.Tel;
				_db.Update(assignment);
				await _db.SaveAsync();
			}
			return _mapper.Map<AssignmentCustom>(assignment);
		}

		public async Task<PaginationView<List<AssignmentCustom>>> GetListCenter(allFilter model)
		{
			var query = _repo.Context.Assignments.Where(x => x.Status != StatusModel.Delete)
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
				query = query.Where(x => x.Code != null && x.Code.Contains(model.mcenter_code));
			}

			if (!String.IsNullOrEmpty(model.mcenter_name))
			{
				query = query.Where(x => x.Name != null && x.Name.Contains(model.mcenter_name));
			}

			if (!String.IsNullOrEmpty(model.emp_id))
			{
				query = query.Where(x => x.EmployeeId != null && x.EmployeeId.Contains(model.emp_id));
			}

			if (!String.IsNullOrEmpty(model.emp_name))
			{
				query = query.Where(x => x.EmployeeName != null && x.EmployeeName.Contains(model.emp_name));
			}

			if (!String.IsNullOrEmpty(model.province))
			{
				if (int.TryParse(model.province, out var province))
				{
					query = query.Where(x => x.User != null && x.User.ProvinceId == province);
				}
			}

			if (!String.IsNullOrEmpty(model.amphur))
			{
				if (int.TryParse(model.amphur, out var amphur))
				{
					query = query.Where(x => x.User != null && x.User.AmphurId == amphur);
				}
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<AssignmentCustom>>()
			{
				Items = _mapper.Map<List<AssignmentCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task Assign(AssignCenterModel model)
		{
			var assignment = await _repo.Context.Assignments.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == model.Assign.Id);
			if (assignment != null)
			{
				var salesCount = 0;
				foreach (var item in model.Sales)
				{
					var sales = await _repo.Context.Sales.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == item.Id);
					if (sales != null)
					{
						if (sales.AssCenterUserId.HasValue) throw new ExceptionCustom($"assignment duplicate {sales.CompanyName}");

						sales.AssCenterUserId = model.Assign.UserId;
						sales.AssCenterUserName = assignment.EmployeeName;
						sales.AssCenterCreateBy = model.CurrentUserId;
						sales.AssCenterDate = DateTime.Now;
						_db.Update(sales);
						await _db.SaveAsync();

						salesCount++;
					}
				}

				assignment.CurrentNumber = assignment.CurrentNumber + salesCount;
				_db.Update(assignment);
				await _db.SaveAsync();
			}
		}

		public async Task UpdateCurrentNumber(Guid id)
		{
			var assignment_RMs = await _repo.Context.Assignment_RMs
											  .Where(x => x.AssignmentId == id && x.Status == StatusModel.Active)
											  .ToListAsync();

			int countRm = assignment_RMs.Count;

			var assignments = await _repo.Context.Assignments.FirstOrDefaultAsync(x => x.Id == id);
			if (assignments != null)
			{
				assignments.RMNumber = countRm;
				//assignments.CurrentNumber = assignment_RMs.Sum(x => x.CurrentNumber);

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
										   .Include(x => x.Assignments)
										   .Where(x => x.Status != StatusModel.Delete && x.Master_Department_CenterId.HasValue && x.Role != null && x.Role.Code == RoleCodes.MCENTER && x.Assignments.Count == 0)
										   .OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
										   .ToListAsync();

			if (usersCenter.Count > 0)
			{
				foreach (var item_center in usersCenter)
				{
					if (item_center.Master_Department_CenterId.HasValue)
					{
						var depCenter = await _repo.MasterDepCenter.GetById(item_center.Master_Department_CenterId.Value);
						if (depCenter != null)
						{
							int rMNumber = 0;
							int currentNumber = 0;

							var assignmentCenter = await _repo.AssignmentCenter.Create(new()
							{
								Code = depCenter.Code,
								Name = depCenter.Name,
								UserId = item_center.Id,
								EmployeeId = item_center.EmployeeId,
								EmployeeName = item_center.FullName,
								Tel = item_center.Tel,
								RMNumber = rMNumber,
								CurrentNumber = currentNumber
							});
						}
					}

				}
			}
		}

	}
}

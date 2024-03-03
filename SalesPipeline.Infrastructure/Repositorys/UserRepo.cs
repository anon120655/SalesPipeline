using AutoMapper;
using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NPOI.SS.Formula.Functions;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class UserRepo : IUserRepo
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public UserRepo(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<UserCustom> Validate(UserCustom model, bool isThrow = true)
		{
			string errorMessage = string.Empty;
			model.IsValidate = true;

			string? roleCode = null;
			if (model.RoleId.HasValue)
			{
				roleCode = await GetRoleCodeById(model.RoleId.Value);
			}

			if (model.Id == 0)
			{
				if (model.ValidateError == null) model.ValidateError = new();

				if (model.EmployeeId != null && await UserExists(model.EmployeeId))
				{
					errorMessage = $"มีรหัสพนักงาน {model.EmployeeId} อยู่แล้ว";
					model.IsValidate = false;
					model.ValidateError.Add(errorMessage);
					if (isThrow) throw new ExceptionCustom(errorMessage);
				}
				if (model.Email != null && _repo.Context.Users.Any(x => x.Email == model.Email))
				{
					errorMessage = $"มีผู้ใช้ Email {model.Email} แล้ว";
					model.IsValidate = false;
					model.ValidateError.Add(errorMessage);
					if (isThrow) throw new ExceptionCustom(errorMessage);
				}
			}

			if (roleCode != null)
			{
				if (roleCode.ToUpper().StartsWith(RoleCodes.BRANCH))
				{
					model.Master_Department_CenterId = null;
					//model.AssignmentId = null;
				}
				else if (roleCode.ToUpper().StartsWith(RoleCodes.MCENTER))
				{
					//model.AssignmentId = null;
					model.LevelId = null;
					//model.Master_Department_BranchId = null;
					model.ProvinceId = null;
					model.AmphurId = null;
				}
				else if (roleCode.ToUpper().StartsWith(RoleCodes.RM))
				{
					model.LevelId = null;
					//model.Master_Department_BranchId = null;
					//model.Master_Department_CenterId = null;
				}
			}

			return model;
		}

		public async Task<List<UserCustom>> ValidateUpload(List<UserCustom> model)
		{
			for (int i = 0; i < model.Count; i++)
			{
				model[i] = await Validate(model[i], false);
			}
			return model;
		}

		public async Task<UserCustom> Create(UserCustom model)
		{
			await Validate(model);

			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				string passwordHashGen = BCrypt.Net.BCrypt.EnhancedHashPassword("password", hashType: HashType.SHA384);

				int id = _repo.Context.Users.Max(u => u.Id) + 1;

				string? provinceName = null;
				string? branchName = null;
				if (model.ProvinceId.HasValue)
				{
					provinceName = await _repo.Thailand.GetProvinceNameByid(model.ProvinceId.Value);
				}
				if (model.BranchId.HasValue)
				{
					branchName = await _repo.Thailand.GetBranchNameByid(model.BranchId.Value);
				}

				var user = new Data.Entity.User();
				user.Id = id;
				user.Status = StatusModel.Active;
				user.CreateDate = _dateNow;
				user.CreateBy = model.CurrentUserId;
				user.UpdateDate = _dateNow;
				user.UpdateBy = model.CurrentUserId;
				user.EmployeeId = model.EmployeeId;
				user.TitleName = model.TitleName;
				user.FirstName = model.FirstNames;
				user.LastName = model.LastNames;
				user.FullName = model.FullName;
				user.Email = model.Email;
				user.Tel = model.Tel;
				user.BranchId = model.BranchId;
				user.Master_DepartmentId = model.Master_DepartmentId;
				user.Master_Department_BranchId = model.Master_Department_BranchId;
				user.Master_Department_CenterId = model.Master_Department_CenterId;
				user.ProvinceId = model.ProvinceId;
				user.ProvinceName = provinceName;
				user.BranchId = model.BranchId;
				user.BranchName = branchName;
				user.PositionId = model.PositionId;
				user.LevelId = model.LevelId;
				user.RoleId = model.RoleId;
				user.PasswordHash = passwordHashGen;
				await _db.InsterAsync(user);
				await _db.SaveAsync();

				//RM Role Create Default Assignment
				if (model.RoleId.HasValue)
				{
					var roleCode = await GetRoleCodeById(model.RoleId.Value);
					if (roleCode != null)
					{
						if (user.Master_Department_BranchId.HasValue)
						{
							if (roleCode.ToUpper().StartsWith(RoleCodes.MCENTER))
							{
								string? _code = null;
								string? _name = null;
								var depCenter = await _repo.MasterDepCenter.GetByBranchId(user.Master_Department_BranchId.Value);
								if (depCenter != null)
								{
									_code = depCenter.Code;
									_name = depCenter.Name;
								}
								var assignmentCenter = await _repo.AssignmentCenter.Create(new()
								{
									Master_Department_BranchId = user.Master_Department_BranchId,
									Code = _code,
									Name = _name,
									UserId = user.Id,
									EmployeeId = model.EmployeeId,
									EmployeeName = model.FullName,
									Tel = model.Tel,
									RMNumber = 0,
									CurrentNumber = 0
								});
							}
							else if (roleCode.ToUpper().StartsWith(RoleCodes.RM))
							{
								//เช็คว่ายังไม่เคยบันทึกข้อมูลใน AssignmentRM
								if (!await _repo.AssignmentRM.CheckAssignmentByUserId(user.Id))
								{
									int? _assignmentUserId = null;
									string? _assignmentName = null;
									var userMcenter = await this.GetMcencerByBranchId(user.Master_Department_BranchId.Value);
									if (userMcenter != null)
									{
										_assignmentUserId = userMcenter.Id;
										_assignmentName = userMcenter.FullName;
									}

									var assignment = await _repo.AssignmentRM.Create(new()
									{
										AssignmentUserId = _assignmentUserId,
										AssignmentName = _assignmentName,
										Master_Department_BranchId = user.Master_Department_BranchId,
										UserId = user.Id,
										EmployeeId = model.EmployeeId,
										EmployeeName = model.FullName,
									});
								}
							}
						}
					}
				}


				_transaction.Commit();

				return _mapper.Map<UserCustom>(user);
			}
		}

		public async Task<UserCustom> Update(UserCustom model)
		{
			await Validate(model);

			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				string? provinceName = null;
				string? branchName = null;
				if (model.ProvinceId.HasValue)
				{
					provinceName = await _repo.Thailand.GetProvinceNameByid(model.ProvinceId.Value);
				}
				if (model.BranchId.HasValue)
				{
					branchName = await _repo.Thailand.GetBranchNameByid(model.BranchId.Value);
				}

				string? roleCode = null;
				if (model.RoleId.HasValue)
				{
					roleCode = await GetRoleCodeById(model.RoleId.Value);
				}

				var user = await _repo.Context.Users.Include(x => x.Assignment_RMs.Where(x => x.Status != StatusModel.Delete))
					.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == model.Id);
				if (user != null)
				{
					if (roleCode != null)
					{
						if (roleCode.ToUpper().StartsWith(RoleCodes.MCENTER))
						{
							if (model.Master_Department_CenterId != user.Master_Department_CenterId)
							{
								var assignments = await _repo.Context.Assignments.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.UserId == model.Id);
								if (assignments != null && assignments.RMNumber > 0)
								{
									throw new ExceptionCustom("ไม่สามารถเปลี่ยนศูนย์ที่รับผิดชอบได้ เนื่องจากมีพนักงานที่ดูแล");
								}
							}
						}
						else if (roleCode.ToUpper().StartsWith(RoleCodes.RM))
						{

							//if (model.AssignmentId != _AssignmentId)
							//{
							//	var assignment_RM = await _repo.Context.Assignment_RMs.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.UserId == model.Id);
							//	if (assignment_RM != null && assignment_RM.CurrentNumber > 0)
							//	{
							//		throw new ExceptionCustom("ไม่สามารถเปลี่ยนผู้จัดการศูนย์ที่ดูแลได้ เนื่องจากมีการมอบหมายแล้ว");
							//	}
							//	else
							//	{
							//		var salesCount = await _repo.Context.Sales.CountAsync(x => x.Status != StatusModel.Delete && x.AssUserId == model.Id);
							//		if (salesCount > 0)
							//		{
							//			throw new ExceptionCustom("ไม่สามารถเปลี่ยนผู้จัดการศูนย์ที่ดูแลได้ เนื่องจากมีลูกค้าอยู่ระหว่างการดำเนินการ");
							//		}
							//	}
							//}
						}
					}

					user.UpdateDate = _dateNow;
					user.UpdateBy = model.CurrentUserId;
					user.TitleName = model.TitleName;
					user.FirstName = model.FirstNames;
					user.LastName = model.LastNames;
					user.FullName = model.FullName;
					user.Email = model.Email;
					user.Tel = model.Tel;
					user.BranchId = model.BranchId;
					user.Master_DepartmentId = model.Master_DepartmentId;
					user.Master_Department_BranchId = model.Master_Department_BranchId;
					user.Master_Department_CenterId = model.Master_Department_CenterId;
					user.ProvinceId = model.ProvinceId;
					user.ProvinceName = provinceName;
					user.BranchId = model.BranchId;
					user.BranchName = branchName;
					user.PositionId = model.PositionId;
					user.LevelId = model.LevelId;
					user.RoleId = model.RoleId;
					_db.Update(user);
					await _db.SaveAsync();

					//RM Role Create Default Assignment
					if (roleCode != null)
					{
						if (roleCode.ToUpper().StartsWith(RoleCodes.MCENTER) && user.Master_Department_CenterId.HasValue)
						{
							var depCenter = await _repo.MasterDepCenter.GetById(user.Master_Department_CenterId.Value);
							if (depCenter != null)
							{
								AssignmentCustom assignmentCenterModel = new()
								{
									Master_Department_BranchId = user.Master_Department_BranchId,
									Code = depCenter.Code,
									Name = depCenter.Name,
									UserId = user.Id,
									EmployeeId = model.EmployeeId,
									EmployeeName = model.FullName,
									Tel = user.Tel,
									RMNumber = 0,
									CurrentNumber = 0
								};

								var checkAssignment = await _repo.AssignmentCenter.GetByUserId(user.Id);
								if (checkAssignment == null)
								{
									await _repo.AssignmentCenter.Create(assignmentCenterModel);
								}
								else
								{
									assignmentCenterModel.Id = checkAssignment.Id;
									await _repo.AssignmentCenter.Update(assignmentCenterModel);
								}
							}
						}
						else if (roleCode.ToUpper().StartsWith(RoleCodes.RM))
						{
							//เช็คว่ายังไม่เคยบันทึกข้อมูลใน AssignmentRM
							//Assignment_RMCustom assignmentRMModel = new()
							//{
							//	AssignmentId = model.AssignmentId.Value,
							//	UserId = user.Id,
							//	EmployeeId = model.EmployeeId,
							//	EmployeeName = model.FullName
							//};

							//if (!await _repo.AssignmentRM.GetAssignmentOnlyByUserId(user.Id))
							//{
							//	await _repo.AssignmentRM.Create(assignmentRMModel);
							//}
							//else
							//{
							//	await _repo.AssignmentRM.Update(assignmentRMModel);
							//}
						}
					}

					_transaction.Commit();
				}

				return _mapper.Map<UserCustom>(user);
			}
		}

		public async Task DeleteById(UpdateModel model)
		{
			int id = int.Parse(model.id);
			var query = await _repo.Context.Users.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
			if (query != null)
			{
				query.UpdateDate = DateTime.Now;
				query.UpdateBy = model.userid;
				query.Status = StatusModel.Delete;
				_db.Update(query);
				await _db.SaveAsync();
			}
		}

		public async Task UpdateStatusById(UpdateModel model)
		{
			if (model != null && Boolean.TryParse(model.value, out bool parsedValue))
			{
				var _status = parsedValue ? (short)1 : (short)0;
				int id = int.Parse(model.id);
				var query = await _repo.Context.Users.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
				if (query != null)
				{
					query.UpdateBy = model.userid;
					query.Status = _status;
					_db.Update(query);
					await _db.SaveAsync();
				}
			}
		}

		public async Task<UserCustom> GetById(int id)
		{
			var query = await _repo.Context.Users
				.Include(x => x.Master_Department_Branch)
				.Include(x => x.Master_Department_Center)
				.Include(x => x.Position)
				.Include(x => x.Role)
				.Include(x => x.Assignment_RMs.Where(x => x.Status == StatusModel.Active))
				.Where(x => x.Id == id).FirstOrDefaultAsync();
			return _mapper.Map<UserCustom>(query);
		}

		public async Task<UserCustom> GetMcencerByBranchId(Guid id)
		{
			//7=ผู้จัดการศูนย์
			var query = await _repo.Context.Users.Where(x => x.Status == StatusModel.Active && x.RoleId == 7 && x.Master_Department_BranchId == id).FirstOrDefaultAsync();
			return _mapper.Map<UserCustom>(query);
		}

		public async Task<string?> GetFullNameById(int id)
		{
			var fullName = await _repo.Context.Users.Where(x => x.Id == id).Select(x => x.FullName).FirstOrDefaultAsync();
			return fullName;
		}

		public async Task<bool> UserExists(string employeeid)
		{
			return await _repo.Context.Users.AnyAsync(x => x.EmployeeId == employeeid);
		}

		public async Task<PaginationView<List<UserCustom>>> GetList(UserFilter model)
		{
			var query = _repo.Context.Users.Include(x => x.Role)
										   .Include(x => x.Master_Department_Branch)
										   .Include(x => x.Master_Department_Center)
										   .Where(x => x.Status != StatusModel.Delete && x.Role != null && x.Role.Code != RoleCodes.SUPERADMIN && x.Role.Code != RoleCodes.ADMIN)
										   .OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
										   .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (!String.IsNullOrEmpty(model.type))
			{
				if (model.type == UserTypes.Admin)
				{
					query = query.Where(x => x.Role != null && x.Role.Code.Contains(RoleCodes.LOAN));
				}
				else if (model.type == UserTypes.User)
				{
					query = query.Where(x => x.Role != null && !x.Role.Code.Contains(RoleCodes.LOAN));
				}
			}

			if (!String.IsNullOrEmpty(model.employeeid))
				query = query.Where(x => x.EmployeeId != null && x.EmployeeId.Contains(model.employeeid));

			if (!String.IsNullOrEmpty(model.employeeid))
				query = query.Where(x => x.EmployeeId != null && x.EmployeeId.Contains(model.employeeid));

			if (!String.IsNullOrEmpty(model.fullname))
				query = query.Where(x => (x.FirstName != null && x.FirstName.Contains(model.fullname))
									|| (x.LastName != null && x.LastName.Contains(model.fullname)));

			if (model.spositions != null)
			{
				var SelectedList = model.spositions.Split(',').ToList<string>();
				if (SelectedList != null && SelectedList.Count > 0)
				{
					query = query.Where(x => x.PositionId.HasValue && SelectedList.Contains(x.PositionId.Value.ToString()));
				}
			}

			if (model.suserlevels != null)
			{
				var SelectedList = model.suserlevels.Split(',').ToList<string>();
				if (SelectedList != null && SelectedList.Count > 0)
				{
					query = query.Where(x => x.LevelId.HasValue && SelectedList.Contains(x.LevelId.Value.ToString()));
				}
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<UserCustom>>()
			{
				Items = _mapper.Map<List<UserCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task<List<User_LevelCustom>> GetListLevel(allFilter model)
		{
			var query = _repo.Context.User_Levels.Where(x => x.Status != StatusModel.Delete)
												 .OrderBy(x => x.Id)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (!String.IsNullOrEmpty(model.searchtxt))
				query = query.Where(x => (x.Name != null && x.Name.Contains(model.searchtxt)));

			return _mapper.Map<List<User_LevelCustom>>(await query.ToListAsync());
		}

		public async Task<User_RoleCustom> CreateRole(User_RoleCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				int id = _repo.Context.User_Roles.Max(u => u.Id) + 1;

				var userRole = new Data.Entity.User_Role()
				{
					Id = id,
					Status = StatusModel.Active,
					CreateDate = _dateNow,
					CreateBy = model.CurrentUserId,
					UpdateDate = _dateNow,
					UpdateBy = model.CurrentUserId,
					Code = model.Code,
					Name = model.Name,
					Description = model.Description,
					IsModify = model.IsModify,
				};
				await _db.InsterAsync(userRole);
				await _db.SaveAsync();

				if (model.User_Permissions?.Count > 0)
				{
					foreach (var item in model.User_Permissions)
					{
						var userPermissions = new Data.Entity.User_Permission()
						{
							Status = item.Status,
							CreateDate = _dateNow,
							RoleId = userRole.Id,
							MenuNumber = item.MenuNumber,
							IsView = item.IsView
						};
						_db.Inster(userPermissions);
						await _db.SaveAsync();
					}
				}

				_transaction.Commit();

				return _mapper.Map<User_RoleCustom>(userRole);
			}
		}

		public async Task<User_RoleCustom> UpdateRole(User_RoleCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var userRole = await _repo.Context.User_Roles.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == model.Id);
				if (userRole != null)
				{
					userRole.UpdateDate = _dateNow;
					userRole.UpdateBy = model.CurrentUserId;
					userRole.Code = model.Code;
					userRole.Name = model.Name;
					userRole.Description = model.Description;
					userRole.IsModify = model.IsModify;
					_db.Update(userRole);
					await _db.SaveAsync();

					if (model.User_Permissions?.Count > 0)
					{
						foreach (var item in model.User_Permissions)
						{
							var userPermissions = await _repo.Context.User_Permissions.FirstOrDefaultAsync(x => x.RoleId == userRole.Id
																											&& x.MenuNumber == item.MenuNumber);
							if (userPermissions != null)
							{
								userPermissions.IsView = item.IsView;
								_db.Update(userPermissions);
								await _db.SaveAsync();
							}
							else
							{
								userPermissions = new Data.Entity.User_Permission()
								{
									Status = item.Status,
									CreateDate = _dateNow,
									RoleId = userRole.Id,
									MenuNumber = item.MenuNumber,
									IsView = item.IsView
								};
								_db.Inster(userPermissions);
								await _db.SaveAsync();
							}
						}
					}

					_transaction.Commit();
				}

				return _mapper.Map<User_RoleCustom>(userRole);
			}
		}

		public async Task DeleteRoleById(UpdateModel model)
		{
			int id = int.Parse(model.id);
			var query = await _repo.Context.User_Roles.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
			if (query != null)
			{
				query.UpdateDate = DateTime.Now;
				query.UpdateBy = model.userid;
				query.Status = StatusModel.Delete;
				_db.Update(query);
				await _db.SaveAsync();
			}
		}

		public async Task UpdateIsModifyRoleById(UpdateModel model)
		{
			if (model != null && Boolean.TryParse(model.value, out bool val))
			{
				int id = int.Parse(model.id);
				var query = await _repo.Context.User_Roles.Where(x => x.Status != StatusModel.Delete && x.Id == id).FirstOrDefaultAsync();
				if (query != null)
				{
					query.UpdateBy = model.userid;
					query.IsModify = val;
					_db.Update(query);
					await _db.SaveAsync();
				}
			}
		}

		public async Task<User_RoleCustom> GetRoleById(int id)
		{
			var query = await _repo.Context.User_Roles
									.Where(x => x.Id == id)
									.Include(x => x.User_Permissions.Where(x => x.Status == StatusModel.Active))
									.FirstOrDefaultAsync();

			return _mapper.Map<User_RoleCustom>(query);
		}

		public async Task<string?> GetRoleCodeById(int id)
		{
			var code = await _repo.Context.User_Roles.Where(x => x.Id == id).Select(x => x.Code).FirstOrDefaultAsync();
			return code;
		}

		public async Task<User_RoleCustom?> GetRoleByUserId(int id)
		{
			var roleId = await _repo.Context.Users.Where(x => x.Id == id).Select(x => x.RoleId).FirstOrDefaultAsync();
			if (roleId.HasValue)
			{
				var user_Roles = await _repo.Context.User_Roles
										.Where(x => x.Id == roleId)
										.FirstOrDefaultAsync();

				return _mapper.Map<User_RoleCustom>(user_Roles);
			}

			return null;
		}

		public async Task<PaginationView<List<User_RoleCustom>>> GetListRole(allFilter model)
		{
			var query = _repo.Context.User_Roles.Where(x => x.Status != StatusModel.Delete && x.Code != RoleCodes.SUPERADMIN && x.Code != RoleCodes.ADMIN)
												 .Include(x => x.User_Permissions)
												 .OrderBy(x => x.CreateDate)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (!String.IsNullOrEmpty(model.searchtxt))
				query = query.Where(x => (x.Name != null && x.Name.Contains(model.searchtxt))
									|| (x.Description != null && x.Description.Contains(model.searchtxt)));

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<User_RoleCustom>>()
			{
				Items = _mapper.Map<List<User_RoleCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}

using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;
using System.Text.RegularExpressions;

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

		public async Task<UserCustom> Validate(UserCustom model, bool isThrow = true, bool? isSetMaster = false)
		{
			string errorMessage = string.Empty;
			model.IsValidate = true;
			if (model.ValidateError == null) model.ValidateError = new();

			string? roleCode = null;
			if (model.RoleId.HasValue)
			{
				roleCode = await GetRoleCodeById(model.RoleId.Value);
			}

			if (model.Id == 0)
			{
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

			if (model.BranchId.HasValue)
			{
				if (roleCode != null && !roleCode.ToUpper().StartsWith(RoleCodes.RM))
				{
					var usersBranch = await _repo.Context.Users.Where(x => x.Status == StatusModel.Active
							&& x.RoleId == model.RoleId
							&& x.BranchId == model.BranchId && x.Id != model.Id).FirstOrDefaultAsync();
					if (usersBranch != null)
					{
						errorMessage = $"มีพนักงานสาขานี้แล้ว";
						model.IsValidate = false;
						model.ValidateError.Add(errorMessage);
						if (isThrow) throw new ExceptionCustom(errorMessage);
					}
				}
			}

			if (model.RoleId == 3 || model.RoleId == 5)
			{
				if (model.LevelId < 10 || model.LevelId > 12)
				{
					errorMessage = $"ระดับไม่ถูกต้อง ต้องอยู่ในช่วงระหว่าง(10-12)";
					model.IsValidate = false;
					model.ValidateError.Add(errorMessage);
					if (isThrow) throw new ExceptionCustom(errorMessage);
				}
			}
			else if (model.RoleId == 4 || model.RoleId == 6)
			{
				if (model.LevelId < 4 || model.LevelId > 9)
				{
					errorMessage = $"ระดับไม่ถูกต้อง ต้องอยู่ในช่วงระหว่าง(4-9)";
					model.IsValidate = false;
					model.ValidateError.Add(errorMessage);
					if (isThrow) throw new ExceptionCustom(errorMessage);
				}
			}

			if (model.PositionId.HasValue)
			{
				if (model.RoleId == 3 || model.RoleId == 4)
				{
					var positionsList = await _repo.Master.Positions(new() { type = "1" });
					if (!positionsList.Select(x => x.Id).Contains(model.PositionId.Value))
					{
						errorMessage = $"ระบุตำแหน่งไม่ถูกต้อง";
						model.IsValidate = false;
						model.ValidateError.Add(errorMessage);
						if (isThrow) throw new ExceptionCustom(errorMessage);
					}
				}
				if (model.RoleId >= 5 && model.RoleId <= 8)
				{
					var positionsList = await _repo.Master.Positions(new() { type = "2" });
					if (!positionsList.Select(x => x.Id).Contains(model.PositionId.Value))
					{
						errorMessage = $"ระบุตำแหน่งไม่ถูกต้อง";
						model.IsValidate = false;
						model.ValidateError.Add(errorMessage);
						if (isThrow) throw new ExceptionCustom(errorMessage);
					}
				}
			}

			if (model.Master_Branch_RegionId.HasValue)
			{
				var provinceList = await _repo.Thailand.GetProvince(model.Master_Branch_RegionId.Value);
				if (provinceList.Count > 0)
				{
					if (model.ProvinceId.HasValue)
					{
						if (!provinceList.Select(x => x.ProvinceID).Contains(model.ProvinceId.Value))
						{
							errorMessage = $"ไม่พบจังหวัดภายใต้ กิจการสาขาภาคที่ระบุ";
							model.IsValidate = false;
							model.ValidateError.Add(errorMessage);
							if (isThrow) throw new ExceptionCustom(errorMessage);
						}
						else
						{
							if (model.BranchId.HasValue)
							{
								var branchList = await _repo.Thailand.GetBranch(model.ProvinceId.Value);
								if (!branchList.Select(x => x.BranchID).Contains(model.BranchId.Value))
								{
									errorMessage = $"ไม่พบสาขาภายใต้ จังหวัดที่ระบุ";
									model.IsValidate = false;
									model.ValidateError.Add(errorMessage);
									if (isThrow) throw new ExceptionCustom(errorMessage);
								}
							}
						}
					}
				}
			}

			if (isSetMaster == true)
			{
				if (model.Master_DepartmentId.HasValue)
				{
					model.Master_DepartmentName = await _repo.MasterDepartment.GetNameById(model.Master_DepartmentId.Value);
				}
				if (model.PositionId.HasValue)
				{
					model.PositionName = await _repo.Context.Master_Positions.Where(x => x.Id == model.PositionId.Value).Select(x => x.Name).FirstOrDefaultAsync();
				}
				if (model.RoleId.HasValue)
				{
					var _role = await _repo.User.GetRoleById(model.RoleId.Value);
					if (_role != null)
					{
						model.RoleName = _role.Name;
					}
				}
				if (model.Master_Branch_RegionId.HasValue)
				{
					model.Master_Branch_RegionName = await _repo.MasterBranchReg.GetNameById(model.Master_Branch_RegionId.Value);
				}
				if (model.ProvinceId.HasValue)
				{
					model.ProvinceName = await _repo.Thailand.GetProvinceNameByid(model.ProvinceId.Value);
				}
				if (model.BranchId.HasValue)
				{
					model.BranchName = await _repo.Thailand.GetBranchNameByid(model.BranchId.Value);
				}
			}

			if (roleCode != null)
			{
				if (roleCode.ToUpper().StartsWith(RoleCodes.BRANCH_REG))
				{
				}
				else if (roleCode.ToUpper().StartsWith(RoleCodes.CEN_BRANCH))
				{
					model.LevelId = null;
				}
				else if (roleCode.ToUpper().StartsWith(RoleCodes.RM))
				{
				}
			}

			return model;
		}

		public async Task<List<UserCustom>> ValidateUpload(List<UserCustom> model)
		{
			for (int i = 0; i < model.Count; i++)
			{
				model[i] = await Validate(model[i], false, true);
			}
			return model;
		}

		public async Task<UserCustom> Create(UserCustom model)
		{
			await Validate(model);

			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				//นำไปไว้ตอนส่งเมล เพราะถ้าส่งเมลไม่ผ่านแล้วต้องส่งใหม่ password จะถูก gen ใหม่ทุกครั้ง
				//string defaultPassword = GeneralUtils.RandomString(8);
				//string passwordHashGen = BCrypt.Net.BCrypt.EnhancedHashPassword(defaultPassword, hashType: HashType.SHA384);

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
				user.Status = model.Status;
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
				user.Master_Branch_RegionId = model.Master_Branch_RegionId;
				user.ProvinceId = model.ProvinceId;
				user.ProvinceName = provinceName;
				user.BranchId = model.BranchId;
				user.BranchName = branchName;
				user.PositionId = model.PositionId;
				user.LevelId = model.LevelId;
				user.RoleId = model.RoleId;
				//user.PasswordHash = passwordHashGen;
				await _db.InsterAsync(user);
				await _db.SaveAsync();

				//RM Role Create Default Assignment
				if (model.RoleId.HasValue)
				{
					var roleCode = await GetRoleCodeById(model.RoleId.Value);
					if (roleCode != null && user.BranchId.HasValue)
					{
						if (roleCode.ToUpper().StartsWith(RoleCodes.BRANCH_REG))
						{
							string? _code = null;
							string? _name = null;
							var branch = await _repo.Thailand.GetBranchByid(user.BranchId.Value);
							if (branch != null)
							{
								_code = branch.BranchCode;
								_name = branch.BranchName;
							}

							var assignmentCenter = await _repo.AssignmentBranch.Create(new()
							{
								Status = StatusModel.Active,
								Master_Branch_RegionId = user.Master_Branch_RegionId,
								BranchId = user.BranchId,
								BranchCode = _code,
								BranchName = _name,
								UserId = user.Id,
								EmployeeId = user.EmployeeId,
								EmployeeName = user.FullName,
								Tel = user.Tel,
								CurrentNumber = 0
							});
						}
						else if (roleCode.ToUpper().StartsWith(RoleCodes.CEN_BRANCH))
						{
							string? _code = null;
							string? _name = null;
							var branch = await _repo.Thailand.GetBranchByid(user.BranchId.Value);
							if (branch != null)
							{
								_code = branch.BranchCode;
								_name = branch.BranchName;
							}

							var assignmentCenter = await _repo.AssignmentCenter.Create(new()
							{
								Status = model.Status,
								BranchId = user.BranchId,
								BranchCode = _code,
								BranchName = _name,
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
								var assignment = await _repo.AssignmentRM.Create(new()
								{
									Status = model.Status,
									BranchId = user.BranchId,
									UserId = user.Id,
									EmployeeId = model.EmployeeId,
									EmployeeName = model.FullName,
								});
							}
						}
					}
				}

				_transaction.Commit();

				var response = _mapper.Map<UserCustom>(user);

				//response.DefaultPassword = defaultPassword;

				return response;
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
					if (roleCode != null && user.BranchId.HasValue)
					{
						if (roleCode.ToUpper().StartsWith(RoleCodes.CEN_BRANCH))
						{
							if (model.BranchId != user.BranchId)
							{
								var assignments = await _repo.Context.Assignment_CenterBranches.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.UserId == model.Id);
								if (assignments != null && assignments.RMNumber > 0)
								{
									throw new ExceptionCustom("ไม่สามารถเปลี่ยนสาขาที่รับผิดชอบได้ เนื่องจากมีพนักงานที่ดูแล");
								}
							}
						}
						else if (roleCode.ToUpper().StartsWith(RoleCodes.RM))
						{
							if (model.BranchId != user.BranchId)
							{
								var assignment_RM = await _repo.Context.Assignment_RMs.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.UserId == model.Id);
								if (assignment_RM != null && assignment_RM.CurrentNumber > 0)
								{
									throw new ExceptionCustom("ไม่สามารถเปลี่ยนสาขาได้ เนื่องจากมีการมอบหมายแล้ว");
								}
								else
								{
									var salesCount = await _repo.Context.Sales.CountAsync(x => x.Status != StatusModel.Delete && x.AssUserId == model.Id);
									if (salesCount > 0)
									{
										throw new ExceptionCustom("ไม่สามารถเปลี่ยนสาขาได้ เนื่องจากมีลูกค้าอยู่ระหว่างการดำเนินการ");
									}
								}
							}
						}
					}

					user.Status = model.Status;
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
					user.Master_Branch_RegionId = model.Master_Branch_RegionId;
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
						if (user.BranchId.HasValue)
						{
							if (roleCode.ToUpper().StartsWith(RoleCodes.CEN_BRANCH))
							{

								string? _code = null;
								string? _name = null;
								var branch = await _repo.Thailand.GetBranchByid(user.BranchId.Value);
								if (branch != null)
								{
									_code = branch.BranchCode;
									_name = branch.BranchName;
								}

								Assignment_CenterBranchCustom assignmentCenterModel = new()
								{
									Status = model.Status,
									BranchId = user.BranchId,
									BranchCode = _code,
									BranchName = _name,
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
							else if (roleCode.ToUpper().StartsWith(RoleCodes.RM))
							{
								Assignment_RMCustom assignment_RM = new()
								{
									Status = model.Status,
									BranchId = user.BranchId,
									UserId = user.Id,
									EmployeeId = model.EmployeeId,
									EmployeeName = model.FullName,
								};

								//เช็คว่ายังไม่เคยบันทึกข้อมูลใน AssignmentRM
								if (!await _repo.AssignmentRM.CheckAssignmentByUserId(user.Id))
								{
									var assignment = await _repo.AssignmentRM.Create(assignment_RM);
								}
								else
								{
									var assignment = await _repo.AssignmentRM.Update(assignment_RM);
								}
							}
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

					if (query.RoleId == 7)
					{
						var assignment = await _repo.Context.Assignment_CenterBranches.Where(x => x.UserId == query.Id).FirstOrDefaultAsync();
						if (assignment != null)
						{
							assignment.Status = _status;
							_db.Update(assignment);
							await _db.SaveAsync();
						}
					}
					else if (query.RoleId == 8)
					{
						var assignment_RM = await _repo.Context.Assignment_RMs.Where(x => x.UserId == query.Id).FirstOrDefaultAsync();
						if (assignment_RM != null)
						{
							assignment_RM.Status = _status;
							_db.Update(assignment_RM);
							await _db.SaveAsync();
						}
					}

				}
			}
		}

		public async Task<UserCustom> GetById(int id)
		{
			var query = await _repo.Context.Users
				.Include(x => x.Master_Branch_Region)
				.Include(x => x.Position)
				.Include(x => x.Role)
				.Where(x => x.Id == id).FirstOrDefaultAsync();
			return _mapper.Map<UserCustom>(query);
		}

		public async Task<UserCustom> GetByBranchRegionId(Guid id, int role)
		{
			var query = _repo.Context.Users.Where(x => x.Status == StatusModel.Active);

			var queryUse = await query.FirstOrDefaultAsync(x => x.RoleId == role && x.Master_Branch_RegionId == id);

			return _mapper.Map<UserCustom>(queryUse);
		}

		public async Task<UserCustom> GetByBranchId(int id, int role)
		{
			var query = _repo.Context.Users.Where(x => x.Status == StatusModel.Active);

			var queryUse = await query.FirstOrDefaultAsync(x => x.RoleId == role && x.BranchId == id);

			return _mapper.Map<UserCustom>(queryUse);
		}

		public async Task<UserCustom> GetUserRMByProvinceId(int id)
		{
			var query = await _repo.Context.Users.Where(x => x.Status == StatusModel.Active && x.RoleId == 8 && x.ProvinceId == id).FirstOrDefaultAsync();
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
										   .Include(x => x.Master_Branch_Region)
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

			if (model.branchid > 0)
			{
				query = query.Where(x => x.BranchId != null && x.BranchId == model.branchid);
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

		public async Task<PaginationView<List<UserCustom>>> GetUserTargetList(allFilter model)
		{
			if (!model.userid.HasValue) return new();

			var user = await _repo.User.GetById(model.userid.Value);
			if (user == null || user.Role == null) throw new ExceptionCustom("userid not map role.");

			int _year = DateTime.Now.Year;
			if (!string.IsNullOrEmpty(model.year))
			{
				if (int.TryParse(model.year, out int year))
				{
					_year = year;
				}
			}

			var query = _repo.Context.Users.Include(x => x.Role)
										   .Include(x => x.User_Target_SaleUsers.Where(w => w.Year == _year))
										   .Include(x => x.Master_Branch_Region)
										   .Where(x => x.Status != StatusModel.Delete && x.Role != null && x.Role.Code == RoleCodes.RM)
										   .OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
										   .AsQueryable();

			if (user.Role.Code.ToUpper().StartsWith(RoleCodes.CEN_BRANCH))
			{
				query = query.Where(x => x.BranchId == user.BranchId);
			}
			else if (user.Role.Code.ToUpper().StartsWith(RoleCodes.BRANCH_REG))
			{
				query = query.Where(x => x.Master_Branch_RegionId == user.Master_Branch_RegionId);
			}
			else if (user.Role.Code.ToUpper().StartsWith(RoleCodes.LOAN) || user.Role.Code.ToUpper().Contains(RoleCodes.ADMIN))
			{
			}

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

			if (!String.IsNullOrEmpty(model.emp_id))
				query = query.Where(x => x.EmployeeId != null && x.EmployeeId.Contains(model.emp_id));

			if (!String.IsNullOrEmpty(model.emp_name))
				query = query.Where(x => x.FullName != null && x.FullName.Contains(model.emp_name));

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

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<UserCustom>>()
			{
				Items = _mapper.Map<List<UserCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task UpdateUserTarget(User_Main model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				foreach (var item in model.ItemsTarget)
				{
					int CRUD = CRUDModel.Update;
					var user_Target_Sales = await _repo.Context.User_Target_Sales.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.UserId == item.UserId && x.Year == item.Year);
					if (user_Target_Sales == null)
					{
						CRUD = CRUDModel.Create;
						user_Target_Sales = new();
						user_Target_Sales.CreateDate = _dateNow;
						user_Target_Sales.CreateBy = model.CurrentUserId;
					}
					user_Target_Sales.UpdateDate = _dateNow;
					user_Target_Sales.UpdateBy = model.CurrentUserId;

					user_Target_Sales.UserId = item.UserId;
					user_Target_Sales.Year = item.Year;
					user_Target_Sales.AmountTarget = item.AmountTarget;

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

				_transaction.Commit();
			}
		}

		public async Task LogLogin(User_Login_LogCustom model)
		{
			try
			{
				var users = await _repo.Context.Users.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.Id == model.UserId);
				if (users != null)
				{
					var _DateNow = DateTime.Now;
					var logLogins = await _repo.Context.User_Login_Logs.FirstOrDefaultAsync(x => x.UserId == model.UserId && x.IPAddress == model.IPAddress
					&& x.CreateDate.Date == _DateNow.Date
					&& x.CreateDate.Hour == _DateNow.Hour
					&& x.CreateDate.Minute == _DateNow.Minute);

					if (logLogins == null)
					{
						var logLogin = new Infrastructure.Data.Entity.User_Login_Log()
						{
							UserId = model.UserId,
							FullName = users.FullName,
							IPAddress = model.IPAddress
						};
						_db.Inster(logLogin);
						await _db.SaveAsync();
					}

				}
			}
			catch
			{
			}
		}

		public async Task<List<UserCustom>> GetNewUserSendMail(int? id)
		{
			var query = _repo.Context.Users.Where(x => x.Status == StatusModel.Active && x.IsSentMail != 1);

			if (id.HasValue)
			{
				query = query.Where(x => x.Id == id);
			}

			return _mapper.Map<List<UserCustom>>(await query.ToListAsync());
		}

		public async Task<UserCustom> UpdateNewUserSendMail(int id)
		{
			string defaultPassword = GeneralUtils.RandomString(8);
			string passwordHashGen = BCrypt.Net.BCrypt.EnhancedHashPassword(defaultPassword, hashType: HashType.SHA384);

			var user = await _repo.Context.Users.FirstOrDefaultAsync(x => x.Status == StatusModel.Active && x.IsSentMail != 1 && x.Id == id);
			if (user != null)
			{
				user.IsSentMail = 1;
				user.PasswordHash = passwordHashGen;
				_db.Update(user);
				await _db.SaveAsync();
			}

			var response = _mapper.Map<UserCustom>(user); ;

			response.DefaultPassword = defaultPassword;

			return response;
		}

		public async Task ChangePassword(ChangePasswordModel model)
		{
			var user = await _repo.Context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
			if (user == null) throw new ExceptionCustom($"Id not found.");

			if (user.PasswordHash == null) throw new ExceptionCustom("อีเมล์หรือรหัสผ่านของท่านไม่ถูกต้อง");

			bool verified = BCrypt.Net.BCrypt.EnhancedVerify(model.OldPassword, user.PasswordHash, hashType: HashType.SHA384);
			if (!verified)
				throw new ExceptionCustom($"Old Password incorrect");

			if (string.IsNullOrEmpty(model.NewPassword))
				throw new ExceptionCustom($"New Password required");

			//Regex vaildate_password = GeneralUtils.PasswordValidation();

			if (!GeneralUtils.IsValidPassword(model.NewPassword))
				throw new ExceptionCustom($"Password must be atleast 8 to 15 characters. It contains atleast one character and numbers.");

			if (string.IsNullOrEmpty(model.ConfirmPassword))
				throw new ExceptionCustom($"Confirm Password required");

			if (model.NewPassword != model.ConfirmPassword)
				throw new ExceptionCustom($"The password and confirmation password do not match.");

			string passwordHashGen = BCrypt.Net.BCrypt.EnhancedHashPassword(model.NewPassword, hashType: HashType.SHA384);

			user.PasswordHash = passwordHashGen;
			_db.Update(user);
			await _db.SaveAsync();

		}

	}
}

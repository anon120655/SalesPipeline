using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;
using System.Drawing;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using static NPOI.HSSF.Util.HSSFColor;

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
				if (model.UserName != null && _repo.Context.Users.Any(x => x.UserName == model.UserName))
				{
					errorMessage = $"มีผู้ใช้ UserName {model.UserName} แล้ว";
					model.IsValidate = false;
					model.ValidateError.Add(errorMessage);
					if (isThrow) throw new ExceptionCustom(errorMessage);
				}
				if (model.EmployeeId != null && await UserExists(model.EmployeeId))
				{
					errorMessage = $"มีรหัสพนักงาน {model.EmployeeId} อยู่แล้ว";
					model.IsValidate = false;
					model.ValidateError.Add(errorMessage);
					if (isThrow) throw new ExceptionCustom(errorMessage);
				}
				//if (model.Email != null && _repo.Context.Users.Any(x => x.Email == model.Email))
				//{
				//	errorMessage = $"มีผู้ใช้ Email {model.Email} แล้ว";
				//	model.IsValidate = false;
				//	model.ValidateError.Add(errorMessage);
				//	if (isThrow) throw new ExceptionCustom(errorMessage);
				//}
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

			//if (model.RoleId == 3 || model.RoleId == 5)
			//{
			//	if (model.LevelId < 10 || model.LevelId > 12)
			//	{
			//		errorMessage = $"ระดับไม่ถูกต้อง ต้องอยู่ในช่วงระหว่าง(10-12)";
			//		model.IsValidate = false;
			//		model.ValidateError.Add(errorMessage);
			//		if (isThrow) throw new ExceptionCustom(errorMessage);
			//	}
			//}
			//else if (model.RoleId == 4 || model.RoleId == 6)
			//{
			//	if (model.LevelId < 4 || model.LevelId > 9)
			//	{
			//		errorMessage = $"ระดับไม่ถูกต้อง ต้องอยู่ในช่วงระหว่าง(4-9)";
			//		model.IsValidate = false;
			//		model.ValidateError.Add(errorMessage);
			//		if (isThrow) throw new ExceptionCustom(errorMessage);
			//	}
			//}

			//if (model.PositionId.HasValue)
			//{
			//	if (model.RoleId == 3 || model.RoleId == 4)
			//	{
			//		var positionsList = await _repo.Master.Positions(new() { type = "1" });
			//		if (!positionsList.Select(x => x.Id).Contains(model.PositionId.Value))
			//		{
			//			errorMessage = $"ระบุตำแหน่งไม่ถูกต้อง";
			//			model.IsValidate = false;
			//			model.ValidateError.Add(errorMessage);
			//			if (isThrow) throw new ExceptionCustom(errorMessage);
			//		}
			//	}
			//	if (model.RoleId >= 5 && model.RoleId <= 8)
			//	{
			//		var positionsList = await _repo.Master.Positions(new() { type = "2" });
			//		if (!positionsList.Select(x => x.Id).Contains(model.PositionId.Value))
			//		{
			//			errorMessage = $"ระบุตำแหน่งไม่ถูกต้อง";
			//			model.IsValidate = false;
			//			model.ValidateError.Add(errorMessage);
			//			if (isThrow) throw new ExceptionCustom(errorMessage);
			//		}
			//	}
			//}

			if (model.Master_Branch_RegionId.HasValue)
			{
				var provinceList = await _repo.Thailand.GetProvince(model.Master_Branch_RegionId.Value);
				if (provinceList.Count > 0)
				{
					//if (model.ProvinceId > 0)
					//{
					//	if (!provinceList.Select(x => x.ProvinceID).Contains(model.ProvinceId.Value))
					//	{
					//		errorMessage = $"ไม่พบจังหวัดภายใต้ กิจการสาขาภาคที่ระบุ";
					//		model.IsValidate = false;
					//		model.ValidateError.Add(errorMessage);
					//		if (isThrow) throw new ExceptionCustom(errorMessage);
					//	}
					//	else
					//	{
					//		if (model.BranchId > 0)
					//		{
					//			var branchList = await _repo.Thailand.GetBranch(model.ProvinceId.Value);
					//			if (!branchList.Select(x => x.BranchID).Contains(model.BranchId.Value))
					//			{
					//				errorMessage = $"ไม่พบสาขาภายใต้ จังหวัดที่ระบุ";
					//				model.IsValidate = false;
					//				model.ValidateError.Add(errorMessage);
					//				if (isThrow) throw new ExceptionCustom(errorMessage);
					//			}
					//		}
					//	}
					//}
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
				user.UserName = model.UserName;
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
				user.Create_Type = model.Create_Type;
				user.authen_fail_time = model.authen_fail_time;
				user.branch_code = model.branch_code;
				user.branch_name = model.branch_name;
				user.cbs_id = model.cbs_id;
				user.change_password_url = model.change_password_url;
				user.create_password_url = model.create_password_url;
				user.email_baac = model.email_baac;
				user.employee_id = model.employee_id;
				user.employee_position_id = model.employee_position_id;
				user.employee_position_level = model.employee_position_level;
				user.employee_position_name = model.employee_position_name;
				user.employee_status = model.employee_status;
				user.first_name_th = model.first_name_th;
				user.image_existing = model.image_existing;
				user.job_field_id = model.job_field_id;
				user.job_field_name = model.job_field_name;
				user.job_id = model.job_id;
				user.job_name = model.job_name;
				user.last_name_th = model.last_name_th;
				user.lastauthen_timestamp = model.lastauthen_timestamp;
				user.mobile_no = model.mobile_no;
				user.name_en = model.name_en;
				user.org_id = model.org_id;
				user.org_name = model.org_name;
				user.organization_48 = model.organization_48;
				user.organization_abbreviation = model.organization_abbreviation;
				user.organization_upper_id = model.organization_upper_id;
				user.organization_upper_id2 = model.organization_upper_id2;
				user.organization_upper_id3 = model.organization_upper_id3;
				user.organization_upper_name = model.organization_upper_name;
				user.organization_upper_name2 = model.organization_upper_name2;
				user.organization_upper_name3 = model.organization_upper_name3;
				user.password_unexpire = model.password_unexpire;
				user.requester_active = model.requester_active;
				user.requester_existing = model.requester_existing;
				user.timeresive = model.timeresive;
				user.timesend = model.timesend;
				user.title_th = model.title_th;
				user.title_th_2 = model.title_th_2;
				user.user_class = model.user_class;
				user.username_active = model.username_active;
				user.username_existing = model.username_existing;
				user.working_status = model.working_status;
				await _db.InsterAsync(user);
				await _db.SaveAsync();

				//Area
				if (model.User_Areas?.Count > 0)
				{
					foreach (var item in model.User_Areas)
					{
						var provinceName_area = await _repo.Thailand.GetProvinceNameByid(item.ProvinceId);

						var user_Area = new Data.Entity.User_Area();
						user_Area.Status = model.Status;
						user_Area.UserId = user.Id;
						user_Area.CreateDate = _dateNow;
						user_Area.ProvinceId = item.ProvinceId;
						user_Area.ProvinceName = provinceName_area;
						await _db.InsterAsync(user_Area);
						await _db.SaveAsync();
					}
				}

				//RM Role Create Default Assignment
				if (model.RoleId.HasValue)
				{
					var userRole = await GetRoleById(model.RoleId.Value);
					if (userRole != null && user.BranchId.HasValue)
					{
						if (userRole.IsAssignRM)
						{
							var assignmentCenter = await _repo.AssignmentCenter.Create(new()
							{
								Status = model.Status,
								UserId = user.Id,
								EmployeeId = model.EmployeeId,
								EmployeeName = model.FullName,
								Tel = model.Tel,
								RMNumber = 0,
								CurrentNumber = 0
							});
						}
						else if (model.RoleId == 8)
						{
							//เช็คว่ายังไม่เคยบันทึกข้อมูลใน AssignmentRM
							if (!await _repo.AssignmentRM.CheckAssignmentByUserId(user.Id))
							{
								var assignment = await _repo.AssignmentRM.Create(new()
								{
									Status = model.Status,
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

				User_RoleCustom? userRole = null;
				if (model.RoleId.HasValue)
				{
					userRole = await GetRoleById(model.RoleId.Value);
				}

				var user = await _repo.Context.Users.Include(x => x.Assignment_RMs.Where(x => x.Status != StatusModel.Delete))
					.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == model.Id);
				if (user != null)
				{
					user.Status = model.Status;
					user.UpdateDate = _dateNow;
					user.UpdateBy = model.CurrentUserId;
					user.EmployeeId = model.EmployeeId;
					user.TitleName = model.TitleName;
					user.FirstName = model.FirstNames;
					user.LastName = model.LastNames;
					user.FullName = model.FullName;
					user.Email = model.Email;
					user.Tel = model.Tel;
					if (model.UpdateChannel != 1)
					{
						user.BranchId = model.BranchId;
						user.Master_DepartmentId = model.Master_DepartmentId;
						user.Master_Branch_RegionId = model.Master_Branch_RegionId;
						user.ProvinceId = model.ProvinceId;
						user.ProvinceName = provinceName;
						user.BranchId = model.BranchId;
						user.BranchName = branchName;
						user.PositionId = model.PositionId;
						user.LevelId = model.LevelId;
					}
					user.RoleId = model.RoleId;
					//user.Create_Type = model.Create_Type;
					user.authen_fail_time = model.authen_fail_time;
					user.branch_code = model.branch_code;
					user.branch_name = model.branch_name;
					user.cbs_id = model.cbs_id;
					user.change_password_url = model.change_password_url;
					user.create_password_url = model.create_password_url;
					user.email_baac = model.email_baac;
					user.employee_id = model.employee_id;
					user.employee_position_id = model.employee_position_id;
					user.employee_position_level = model.employee_position_level;
					user.employee_position_name = model.employee_position_name;
					user.employee_status = model.employee_status;
					user.first_name_th = model.first_name_th;
					user.image_existing = model.image_existing;
					user.job_field_id = model.job_field_id;
					user.job_field_name = model.job_field_name;
					user.job_id = model.job_id;
					user.job_name = model.job_name;
					user.last_name_th = model.last_name_th;
					user.lastauthen_timestamp = model.lastauthen_timestamp;
					user.mobile_no = model.mobile_no;
					user.name_en = model.name_en;
					user.org_id = model.org_id;
					user.org_name = model.org_name;
					user.organization_48 = model.organization_48;
					user.organization_abbreviation = model.organization_abbreviation;
					user.organization_upper_id = model.organization_upper_id;
					user.organization_upper_id2 = model.organization_upper_id2;
					user.organization_upper_id3 = model.organization_upper_id3;
					user.organization_upper_name = model.organization_upper_name;
					user.organization_upper_name2 = model.organization_upper_name2;
					user.organization_upper_name3 = model.organization_upper_name3;
					user.password_unexpire = model.password_unexpire;
					user.requester_active = model.requester_active;
					user.requester_existing = model.requester_existing;
					user.timeresive = model.timeresive;
					user.timesend = model.timesend;
					user.title_th = model.title_th;
					user.title_th_2 = model.title_th_2;
					user.user_class = model.user_class;
					user.username_active = model.username_active;
					user.username_existing = model.username_existing;
					user.working_status = model.working_status;
					_db.Update(user);
					await _db.SaveAsync();

					//Area
					var user_AreaR = _repo.Context.User_Areas.Where(x => x.UserId == user.Id).ToList();
					if (user_AreaR.Count > 0)
					{
						//กรณี iauth จะไม่ได้ส่งมาจะ default ค่าเดิม
						if (model.User_Areas == null || model.User_Areas?.Count == 0)
						{
							model.User_Areas = _mapper.Map<List<User_AreaCustom>>(user_AreaR);
						}

						_db.DeleteRange(user_AreaR);
						await _db.SaveAsync();
					}
					if (model.User_Areas?.Count > 0)
					{
						var userRegion = _repo.Context.Users
							.Include(x => x.User_Areas)
							.FirstOrDefault(x => x.Status == StatusModel.Active && x.RoleId == 7
							&& x.Master_Branch_RegionId == model.Master_Branch_RegionId);

						if (userRegion != null && userRegion.User_Areas.Select(x => x.ProvinceId).Any(x => x == 9999))
						{
							throw new ExceptionCustom("มี ผจศ. ที่ดูแลพื้นที่ในกิจการสาขาภาคนี้ทั้งหมดแล้ว");
						}

						foreach (var item in model.User_Areas)
						{
							if (userRole != null && userRole.IsAssignRM)
							{
								var user_AreaCheck = _repo.Context.Users
															.Include(x => x.User_Areas)
															.Any(x => x.Status == StatusModel.Active
															&& x.Id != model.Id
															&& x.RoleId == 7
															&& x.Master_Branch_RegionId == model.Master_Branch_RegionId
															&& x.User_Areas.Any(a => a.ProvinceId == item.ProvinceId));
								if (user_AreaCheck) throw new ExceptionCustom("มี ผจศ. ที่ดูแลพื้นที่นี้แล้ว");
							}

							var provinceName_area = await _repo.Thailand.GetProvinceNameByid(item.ProvinceId);
							if (item.ProvinceId == 9999) provinceName_area = "ทั้งหมด";

							var user_Area = new Data.Entity.User_Area();
							user_Area.Status = model.Status;
							user_Area.UserId = user.Id;
							user_Area.CreateDate = _dateNow;
							user_Area.ProvinceId = item.ProvinceId;
							user_Area.ProvinceName = provinceName_area;
							await _db.InsterAsync(user_Area);
							await _db.SaveAsync();
						}
					}

					//RM Role Create Default Assignment
					if (userRole != null)
					{
						if (userRole.IsAssignRM)
						{
							Assignment_CenterCustom assignmentCenterModel = new()
							{
								Status = model.Status,
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
						else if (userRole.Id == 8)
						{
							Assignment_RMCustom assignment_RM = new()
							{
								Status = model.Status,
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

				var user_AreaR = _repo.Context.User_Areas.Where(x => x.UserId == query.Id).ToList();
				if (user_AreaR.Count > 0)
				{
					foreach (var item in user_AreaR)
					{
						item.Status = StatusModel.Delete;
						_db.Update(item);
					}
					await _db.SaveAsync();
				}
			}
		}

		public async Task UpdateStatusById(UpdateModel model)
		{
			using (var _transaction = _repo.BeginTransaction())
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
							var assignment = await _repo.Context.Assignment_Centers.Where(x => x.UserId == query.Id).FirstOrDefaultAsync();
							if (assignment != null)
							{
								assignment.Status = _status;
								_db.Update(assignment);
								await _db.SaveAsync();
							}

							var user_AreaR = _repo.Context.User_Areas.Where(x => x.UserId == query.Id).ToList();
							if (user_AreaR.Count > 0)
							{
								if (_status == StatusModel.Active)
								{
									var userRegion = _repo.Context.Users
									.Include(x => x.User_Areas)
									.FirstOrDefault(x => x.Status == StatusModel.Active && x.RoleId == 7
									&& x.Master_Branch_RegionId == query.Master_Branch_RegionId);

									if (userRegion != null && userRegion.User_Areas.Select(x => x.ProvinceId).Any(x => x == 9999))
									{
										throw new ExceptionCustom("มี ผจศ. ที่ดูแลพื้นที่ในกิจการสาขาภาคนี้ทั้งหมดแล้ว");
									}
								}

								foreach (var item in user_AreaR)
								{
									if (_status == StatusModel.Active)
									{
										var user_AreaCheck = _repo.Context.User_Areas.Any(x => x.Status == StatusModel.Active
											&& x.UserId != model.userid
											&& x.ProvinceId == item.ProvinceId);
										if (user_AreaCheck) throw new ExceptionCustom("มี ผจศ. ที่ดูแลพื้นที่นี้แล้ว");
									}

									item.Status = _status;
									_db.Update(item);
								}
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

				_transaction.Commit();
			}
		}

		public async Task<UserCustom> GetById(int id)
		{
			var query = await _repo.Context.Users.AsNoTracking()
                .Include(x => x.User_Areas)
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

		public async Task<List<User_AreaCustom>> GetAreaByUserId(int id)
		{
			var query = await _repo.Context.User_Areas
				.Where(x => x.UserId == id).ToListAsync();
			return _mapper.Map<List<User_AreaCustom>>(query);
		}

		public async Task<string?> GetFullNameById(int id)
		{
			var fullName = await _repo.Context.Users.AsNoTracking().Where(x => x.Id == id).Select(x => x.FullName).FirstOrDefaultAsync();
			return fullName;
		}

		public async Task<bool> UserExists(string employeeid)
		{
			return await _repo.Context.Users.AnyAsync(x => x.EmployeeId == employeeid);
		}

		public async Task<PaginationView<List<UserCustom>>> GetList(UserFilter model)
		{
			var query = _repo.Context.Users.Include(x => x.Role)
										   .Include(x => x.User_Areas.OrderBy(o => o.ProvinceId))
										   .Include(x => x.Master_Branch_Region)
										   .Where(x => x.Status != StatusModel.Delete && x.Role != null && x.Role.Code != RoleCodes.SUPERADMIN && x.Role.Code != RoleCodes.ADMIN)
										   .OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
										   .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			//*** ไม่มีส่วนนี้ ยุบเมนูจัดการ user มารวมกับ จัดการระบบผู้ใช้งาน
			//if (!String.IsNullOrEmpty(model.type))
			//{
			//	if (model.type == UserTypes.Admin)
			//	{
			//		query = query.Where(x => x.Role != null && x.Role.Code.Contains(RoleCodes.LOAN));
			//	}
			//	else if (model.type == UserTypes.User)
			//	{
			//		query = query.Where(x => x.Role != null && !x.Role.Code.Contains(RoleCodes.LOAN));
			//	}
			//}

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

			if (model.roleid > 0)
			{
				query = query.Where(x => x.RoleId != null && x.RoleId == model.roleid);
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
					iAuthenRoleCode = model.iAuthenRoleCode,
					iAuthenRoleName = model.iAuthenRoleName,
					IsModify = model.IsModify,
					IsAssignCenter = model.IsAssignCenter,
					IsAssignRM = model.IsAssignRM,
					org_id = model.org_id,
					org_name = model.org_name,
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
					userRole.iAuthenRoleCode = model.iAuthenRoleCode;
					userRole.iAuthenRoleName = model.iAuthenRoleName;
					userRole.IsModify = model.IsModify;
					userRole.IsAssignCenter = model.IsAssignCenter;
					userRole.IsAssignRM = model.IsAssignRM;
					userRole.org_id = model.org_id;
					userRole.org_name = model.org_name;
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

		public async Task<List<UserCustom>> GetUserByRole(int roleId)
		{
			var user = await _repo.Context.Users.Where(x => x.Status == StatusModel.Active && x.RoleId == roleId).ToListAsync();
			return _mapper.Map<List<UserCustom>>(user);
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
										   .Include(x => x.User_Target_SaleUsers.Where(w => w.Status == StatusModel.Active && w.Year == _year))
										   .Include(x => x.Master_Branch_Region)
										   .Where(x => x.Status != StatusModel.Delete && x.Role != null && x.Role.Code == RoleCodes.RM)
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
					query = query.Where(x => x.Role != null && x.Role.IsAssignCenter);
				}
				else if (model.type == UserTypes.User)
				{
					query = query.Where(x => x.Role != null && !x.Role.IsAssignCenter);
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
					var user_Target_SalesRemove = await _repo.Context.User_Target_Sales.Where(x => x.Status == StatusModel.Active && x.UserId == item.UserId && x.Year == item.Year).ToListAsync();

					if (user_Target_SalesRemove != null && user_Target_SalesRemove.Count > 0)
					{
						foreach (var item_remove in user_Target_SalesRemove)
						{
							item_remove.Status = StatusModel.InActive;
							_db.Update(item_remove);
						}
						await _db.SaveAsync();
					}

					User_Target_Sale user_Target_Sales = new();
					user_Target_Sales.Status = StatusModel.Active;
					user_Target_Sales.CreateDate = _dateNow;
					user_Target_Sales.CreateBy = model.CurrentUserId;
					user_Target_Sales.UpdateDate = _dateNow;
					user_Target_Sales.UpdateBy = model.CurrentUserId;
					user_Target_Sales.UserId = item.UserId;
					user_Target_Sales.Year = item.Year;
					user_Target_Sales.AmountTarget = item.AmountTarget;
					await _db.InsterAsync(user_Target_Sales);
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
							IPAddress = model.IPAddress,
							DeviceId = model.DeviceId,
							DeviceVersion = model.DeviceVersion,
							SystemVersion = model.SystemVersion,
							AppVersion = model.AppVersion,
							tokenNoti = model.tokenNoti
						};
						_db.Inster(logLogin);
						await _db.SaveAsync();

						try
						{
							//string filePath = @"C:\inetpub\wwwroot\logs\user_login_log_file.txt";
							if (!string.IsNullOrWhiteSpace(_appSet.LogLoginPath))
							{
								var yearEn = GeneralUtils.GetYearEn(DateTime.Now.Year);
								var Month = DateTime.Now.ToString("MM");
								var Day = DateTime.Now.ToString("dd");
								string formattedDate = $"{yearEn}_{Month}_{Day}";
								Console.WriteLine(formattedDate);
								string filefullpath = $"{_appSet.LogLoginPath}\\user_login_log_file_{formattedDate}.txt";

								FileInfo fileInfo = new FileInfo(filefullpath);
								if (!fileInfo.Exists && fileInfo.Directory != null && !fileInfo.Directory.Exists)
									Directory.CreateDirectory(fileInfo.Directory.FullName);

								//string logMessage = $"[{DateTime.Now}] UserId: {logLogin.UserId}, FullName: {logLogin.FullName}, IPAddress: {logLogin.IPAddress}, DeviceId: {logLogin.DeviceId}, DeviceVersion: {logLogin.DeviceVersion}, SystemVersion: {logLogin.SystemVersion}, AppVersion: {logLogin.AppVersion}, tokenNoti: {logLogin.tokenNoti}{Environment.NewLine}";

								//// อ่านข้อมูลเดิมจากไฟล์ (ถ้ามี)
								//string existingData = File.Exists(filefullpath) ? File.ReadAllText(filefullpath) : "[]"; // เริ่มด้วย array ว่างถ้าไม่มีไฟล์
								//var logList = JsonConvert.DeserializeObject<List<User_Login_Log>>(existingData); // แปลงเป็น List เพื่อเพิ่มข้อมูล

								//// เพิ่มข้อมูลใหม่
								//string dataJson = JsonConvert.SerializeObject(logLogin);
								//logList.Add(JsonConvert.DeserializeObject<User_Login_Log>(dataJson)); // เพิ่ม object ใหม่เข้าไปใน List

								//// เขียนข้อมูลทั้งหมดกลับไปที่ไฟล์
								////string updatedJson = JsonConvert.SerializeObject(logList, Formatting.Indented); // ทำให้ JSON อ่านง่าย
								//string updatedJson = JsonConvert.SerializeObject(logList); // ทำให้ JSON อ่านง่าย
								//File.WriteAllText(filefullpath, updatedJson); // เขียนทับไฟล์ด้วยข้อมูลใหม่
								// อ่านข้อมูลเดิมจากไฟล์ (ถ้ามี)

								string existingData = File.Exists(filefullpath) ? File.ReadAllText(filefullpath) : "{}"; // เริ่มด้วย object ว่างถ้าไม่มีไฟล์
								var logDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(existingData); // แปลงเป็น Dictionary

								// สร้าง key สำหรับ log ใหม่ (เช่น ใช้ timestamp)
								string key = DateTime.Now.ToString("yyyyMMddHHmmssfff"); // key ไม่ซ้ำกันจาก timestamp
								string dataJson = JsonConvert.SerializeObject(logLogin);
								logDict[key] = JsonConvert.DeserializeObject<object>(dataJson); // เพิ่ม log ใหม่เข้า Dictionary

								// เขียนข้อมูลทั้งหมดกลับไปที่ไฟล์
								string updatedJson = JsonConvert.SerializeObject(logDict); // ทำให้ JSON อ่านง่าย
								File.WriteAllText(filefullpath, updatedJson); // เขียนทับไฟล์ด้วยข้อมูลใหม่
							}
						}
						catch (Exception ex)
						{
						}
					}

					if (!String.IsNullOrEmpty(model.DeviceId) && !String.IsNullOrEmpty(model.tokenNoti))
					{
						var user_Login_TokenNotis = await _repo.Context.User_Login_TokenNotis.FirstOrDefaultAsync(x => x.UserId == model.UserId && x.DeviceId == model.DeviceId);
						if (user_Login_TokenNotis == null)
						{
							user_Login_TokenNotis = new Infrastructure.Data.Entity.User_Login_TokenNoti()
							{
								UserId = model.UserId,
								DeviceId = model.DeviceId,
								tokenNoti = model.tokenNoti
							};
							await _db.InsterAsync(user_Login_TokenNotis);
							await _db.SaveAsync();
						}
						else
						{
							user_Login_TokenNotis.tokenNoti = model.tokenNoti;
							_db.Update(user_Login_TokenNotis);
							await _db.SaveAsync();
						}
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

			if (user.PasswordHash == null) throw new ExceptionCustom("อีเมลหรือรหัสผ่านของท่านไม่ถูกต้อง");

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

		public async Task RemoveUserNotAssignment()
		{
			try
			{
				var assignment_Center = await _repo.Context.Assignment_Centers.Where(x => x.UserId > 0).ToListAsync();
				if (assignment_Center.Count > 0)
				{
					List<int> itemDelete = new();
					foreach (var item in assignment_Center)
					{
						var users = await _repo.Context.Users
							.AsTracking()
							.Include(x => x.Role)
							.FirstOrDefaultAsync(x => x.Id == item.UserId);
						if (users != null && users.Role != null)
						{
							//ผู้ที่ไม่มีสิทธ์มอบหมาย RM หรือมีสิทธิ์มอบหมายแต่สิทธิ์ถูกลบไปแล้ว
							if (!users.Role.IsAssignRM || (users.Role.IsAssignRM && users.Role.Status != StatusModel.Active))
							{
								_db.Delete(item);
								await _db.SaveAsync();
								itemDelete.Add(item.UserId);
							}
						}
					}
					if (itemDelete.Count > 0)
					{

					}
				}
			}
			catch (Exception ex)
			{

			}
		}


	}
}

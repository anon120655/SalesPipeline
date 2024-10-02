using NPOI.Util;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IUserRepo
	{
		Task<UserCustom> Validate(UserCustom model, bool isThrow = false, bool? isSetMaster = false);
		Task<List<UserCustom>> ValidateUpload(List<UserCustom> model);
		Task<UserCustom> Create(UserCustom model);
		Task<UserCustom> Update(UserCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<UserCustom> GetById(int id);
		Task<UserCustom> GetByBranchRegionId(Guid id, int role);
		Task<UserCustom> GetByBranchId(int id,int role);
		Task<UserCustom> GetUserRMByProvinceId(int id);
		Task<List<User_AreaCustom>> GetAreaByUserId(int id);
		Task<string?> GetFullNameById(int id);
		Task<bool> UserExists(string employeeid);
		Task<PaginationView<List<UserCustom>>> GetList(UserFilter model);
		Task<List<User_LevelCustom>> GetListLevel(allFilter model);
		Task<User_RoleCustom> CreateRole(User_RoleCustom model);
		Task<User_RoleCustom> UpdateRole(User_RoleCustom model);
		Task DeleteRoleById(UpdateModel model);
		Task UpdateIsModifyRoleById(UpdateModel model);
		Task<User_RoleCustom> GetRoleById(int id);
		Task<string?> GetRoleCodeById(int id);
		Task<User_RoleCustom?> GetRoleByUserId(int id);
		Task<PaginationView<List<User_RoleCustom>>> GetListRole(allFilter model);
		Task<List<UserCustom>> GetUserByRole(int roleId);
		Task<PaginationView<List<UserCustom>>> GetUserTargetList(allFilter model);
		Task UpdateUserTarget(User_Main model);
		Task LogLogin(User_Login_LogCustom model);
		Task<List<UserCustom>> GetNewUserSendMail(int? id);
		Task<UserCustom> UpdateNewUserSendMail(int id);
		Task ChangePassword(ChangePasswordModel model);
		Task RemoveUserNotAssignment();
	}
}

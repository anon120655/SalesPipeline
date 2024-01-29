using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IUserRepo
	{
		Task<UserCustom> Validate(UserCustom model, bool isThrow = false);
		Task<List<UserCustom>> ValidateUpload(List<UserCustom> model);
		Task<UserCustom> Create(UserCustom model);
		Task<UserCustom> Update(UserCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<UserCustom> GetById(int id);
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
		//Task<PaginationView<List<User_BranchCustom>>> GetUsersRM(allFilter model);
	}
}

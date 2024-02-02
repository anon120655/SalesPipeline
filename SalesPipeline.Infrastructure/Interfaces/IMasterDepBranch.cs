using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterDepBranch
	{
		//ฝ่ายกิจการสาขา
		Task<Master_Department_BranchCustom> Create(Master_Department_BranchCustom model);
		Task<Master_Department_BranchCustom> Update(Master_Department_BranchCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_Department_BranchCustom> GetById(Guid id);
		Task<PaginationView<List<Master_Department_BranchCustom>>> GetBranchs(allFilter model);

	}
}

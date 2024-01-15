using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterDivBranch
	{
		//ฝ่ายกิจการสาขา
		Task<Master_Division_BranchCustom> Create(Master_Division_BranchCustom model);
		Task<Master_Division_BranchCustom> Update(Master_Division_BranchCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_Division_BranchCustom> GetById(Guid id);
		Task<PaginationView<List<Master_Division_BranchCustom>>> GetBranchs(allFilter model);

	}
}

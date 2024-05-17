using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterBranchReg
	{
		//กิจการสาขาภาค
		Task<Master_Branch_RegionCustom> Create(Master_Branch_RegionCustom model);
		Task<Master_Branch_RegionCustom> Update(Master_Branch_RegionCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_Branch_RegionCustom> GetById(Guid id);
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Master_Branch_RegionCustom>>> GetBranchRegs(allFilter model);

	}
}

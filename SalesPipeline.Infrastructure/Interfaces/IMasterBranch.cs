using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterBranch
	{
		//สาขา
		Task<InfoBranchCustom> Create(InfoBranchCustom model);
		Task<InfoBranchCustom> Update(InfoBranchCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<InfoBranchCustom> GetById(int id);
		Task<string?> GetNameById(int id);
		Task<PaginationView<List<InfoBranchCustom>>> GetBranchs(allFilter model);
	}
}

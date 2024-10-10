using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterYields
	{
		Task<List<Master_YieldCustom>> ValidateUpload(List<Master_YieldCustom> model);
		Task<Master_YieldCustom> Create(Master_YieldCustom model);
		Task<Master_YieldCustom> Update(Master_YieldCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_YieldCustom> GetById(Guid id);
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Master_YieldCustom>>> GetList(allFilter model);
	}
}

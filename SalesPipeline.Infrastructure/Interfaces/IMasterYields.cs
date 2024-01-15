using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterYields
	{
		Task<Master_YieldCustom> Create(Master_YieldCustom model);
		Task<Master_YieldCustom> Update(Master_YieldCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_YieldCustom> GetById(Guid id);
		Task<PaginationView<List<Master_YieldCustom>>> GetYields(allFilter model);
	}
}

using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterBusinessType
	{
		Task<Master_BusinessTypeCustom> Create(Master_BusinessTypeCustom model);
		Task<Master_BusinessTypeCustom> Update(Master_BusinessTypeCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_BusinessTypeCustom> GetById(Guid id);
		Task<PaginationView<List<Master_BusinessTypeCustom>>> GetList(allFilter model);
	}
}

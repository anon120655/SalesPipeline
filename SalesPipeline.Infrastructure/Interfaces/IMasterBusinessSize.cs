using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterBusinessSize
	{
		Task<Master_BusinessSizeCustom> Create(Master_BusinessSizeCustom model);
		Task<Master_BusinessSizeCustom> Update(Master_BusinessSizeCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_BusinessSizeCustom> GetById(Guid id);
		Task<string?> GetNameById(Guid id);
		Task<Guid?> GetIdByName(string name);
		Task<PaginationView<List<Master_BusinessSizeCustom>>> GetList(allFilter model);
	}
}

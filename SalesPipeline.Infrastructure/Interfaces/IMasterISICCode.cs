using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterISICCode
	{
		Task<Master_ISICCodeCustom> Create(Master_ISICCodeCustom model);
		Task<Master_ISICCodeCustom> Update(Master_ISICCodeCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_ISICCodeCustom> GetById(Guid id);
		Task<Guid?> GetIDByCode(string code);
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Master_ISICCodeCustom>>> GetList(allFilter model);
	}
}

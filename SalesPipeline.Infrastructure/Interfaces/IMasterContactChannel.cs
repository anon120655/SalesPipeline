using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterContactChannel
	{
		Task<Master_ContactChannelCustom> Create(Master_ContactChannelCustom model);
		Task<Master_ContactChannelCustom> Update(Master_ContactChannelCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_ContactChannelCustom> GetById(Guid id);
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Master_ContactChannelCustom>>> GetList(allFilter model);
	}
}

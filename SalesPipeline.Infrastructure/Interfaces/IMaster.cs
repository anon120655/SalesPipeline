using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMaster
	{
		Task<IList<Master_ListCustom>> MasterLists(allFilter model);
		Task<IList<Master_PositionCustom>> Positions(allFilter model);
		Task<IList<Master_RegionCustom>> Regions(allFilter model);
		Task<IList<MenuItemCustom>> MenuItem(allFilter model);
	}
}

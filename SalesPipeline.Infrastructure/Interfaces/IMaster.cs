using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMaster
	{
		Task<IList<Master_ProductProgramBankCustom>> ProductProgramBanks(allFilter model);
		Task<IList<Master_TypeLoanRequestCustom>> TypeLoanRequests(allFilter model);
		Task<IList<Master_ProceedCustom>> Proceeds(allFilter model);
		Task<IList<Master_ListCustom>> MasterLists(allFilter model);
		Task<IList<Master_PositionCustom>> Positions(allFilter model);
		Task<IList<Master_YearCustom>> Year(allFilter model);
		Task<IList<MenuItemCustom>> MenuItem(allFilter model);
	}
}

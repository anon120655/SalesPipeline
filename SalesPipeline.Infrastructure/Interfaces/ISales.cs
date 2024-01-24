using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface ISales
	{
		Task<SaleCustom> Create(SaleCustom model);
		Task<SaleCustom> Update(SaleCustom model);
		Task UpdateStatusOnly(Sale_StatusCustom model);
		Task<SaleCustom> GetById(Guid id);
		Task<PaginationView<List<SaleCustom>>> GetList(allFilter model);
	}
}

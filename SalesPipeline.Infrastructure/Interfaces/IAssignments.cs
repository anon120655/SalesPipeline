using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IAssignments
	{
		Task<PaginationView<List<SaleCustom>>> GetList(allFilter model);
		Task<AssignmentCustom> Create(AssignmentCustom model);
		Task<Assignment_SaleCustom> CreateSale(Assignment_SaleCustom model);
		Task<bool> CheckAssignmentByUserId(int id);
		Task<AssignmentCustom> GetById(Guid id);
		Task<AssignmentCustom> GetByUserId(int id);
		Task UpdateCurrentNumber(Guid id);
	}
}

using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IAssignments
	{
		Task<AssignmentCustom> Create(AssignmentCustom model);
		Task<Assignment_SaleCustom> CreateSale(Assignment_SaleCustom model);
		Task<bool> CheckAssignmentByUserId(int id);
		Task<AssignmentCustom> GetById(Guid id);
		Task<AssignmentCustom> GetByUserId(int id);
		Task UpdateCurrentNumber(Guid id);
		Task<PaginationView<List<AssignmentCustom>>> GetListAutoAssign(allFilter model);
		Task Assign(List<AssignmentCustom> model);
	}
}

using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IAssignmentRM
	{
		Task<Assignment_RMCustom> Create(Assignment_RMCustom model);
		Task<Assignment_RM_SaleCustom> CreateSale(Assignment_RM_SaleCustom model);
		Task<bool> CheckAssignmentByUserId(int id);
		Task<bool> CheckAssignmentSaleById(Guid id);
		Task<Assignment_RMCustom> GetById(Guid id);
		Task<Assignment_RMCustom> GetByUserId(int id);
		Task UpdateCurrentNumber(Guid id);
		Task<PaginationView<List<Assignment_RMCustom>>> GetListAutoAssign(allFilter model);
		Task<PaginationView<List<Assignment_RMCustom>>> GetListRM(allFilter model);
		Task Assign(List<Assignment_RMCustom> model);
		Task AssignChange(AssignChangeModel model);
		Task CreateAssignmentRMAll(allFilter model);
	}
}

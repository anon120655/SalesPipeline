using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Customers;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IAssignments
	{
		Task<AssignmentCustom> Create(AssignmentCustom model);
		Task<Assignment_SaleCustom> CreateSale(Assignment_SaleCustom model);
		Task<bool> CheckUserId(int id);
		Task<AssignmentCustom> GetById(Guid id);
		Task<AssignmentCustom> GetByUserId(int id);
		Task UpdateCurrentNumber(Guid id);
	}
}

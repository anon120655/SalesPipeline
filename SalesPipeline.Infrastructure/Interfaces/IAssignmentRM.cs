using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IAssignmentRM
	{
		Task<Assignment_RMCustom> Create(Assignment_RMCustom model);
		Task<Assignment_RMCustom> Update(Assignment_RMCustom model);
		Task<Assignment_RM_SaleCustom> CreateSale(Assignment_RM_SaleCustom model);
		Task<bool> CheckAssignmentByUserId(int id);
		Task<bool> CheckAssignmentSaleById(Guid id);
		/// <summary>
		/// ใช้กรณีดึงไปเช็คก่อน update เพราะถ้าดึง GetByUserId ปกติจะมีการ join ทำให้บางฟิลด์ไม่ update
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<bool> GetAssignmentOnlyByUserId(int id);
		Task<Assignment_RMCustom> GetById(Guid id);
		Task<Assignment_RMCustom> GetByUserId(int id);
		Task UpdateCurrentNumber(int? userid = null);
		Task<PaginationView<List<Assignment_RMCustom>>> GetListAutoAssign(allFilter model);
		Task<PaginationView<List<Assignment_RMCustom>>> GetListAutoAssign2(allFilter model);
		Task<PaginationView<List<Assignment_RMCustom>>> GetListAutoAssign3(allFilter model);
		Task<PaginationView<List<Assignment_RMCustom>>> GetListAutoAssign4(allFilter model);
		Task<PaginationView<List<Assignment_RMCustom>>> GetListRM(allFilter model);
		Task Assign(List<Assignment_RMCustom> model);
		Task AssignChange(AssignChangeModel model);
		Task AssignReturnChange(AssignChangeModel model);
		Task CreateAssignmentRMAll(allFilter model);
	}
}

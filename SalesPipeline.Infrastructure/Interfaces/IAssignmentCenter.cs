using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IAssignmentCenter
	{
		Task<bool> CheckAssignmentByUserId(int id);
		Task<bool> CheckAssignmentByBranchId(int id);
		Task<Assignment_MCenterCustom> GetById(Guid id);
		Task<Assignment_MCenterCustom> GetByUserId(int id);
		Task<Assignment_MCenterCustom> Create(Assignment_MCenterCustom model);
		Task<Assignment_MCenterCustom> Update(Assignment_MCenterCustom model);
		Task<PaginationView<List<Assignment_MCenterCustom>>> GetListCenter(allFilter model);
		Task Assign(AssignModel model);
		Task UpdateCurrentNumber(int id);
		Task CreateAssignmentCenterAll(allFilter model);
	}
}

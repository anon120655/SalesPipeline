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
		Task<AssignmentCustom> GetById(Guid id);
		Task<AssignmentCustom> GetByUserId(int id);
		Task<AssignmentCustom> Create(AssignmentCustom model);
		Task<AssignmentCustom> Update(AssignmentCustom model);
		Task<PaginationView<List<AssignmentCustom>>> GetListCenter(allFilter model);
		Task Assign(AssignCenterModel model);
		Task UpdateCurrentNumber(int id);
		Task CreateAssignmentCenterAll(allFilter model);
	}
}

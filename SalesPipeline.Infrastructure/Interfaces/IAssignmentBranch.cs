using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IAssignmentBranch
	{
		Task<bool> CheckAssignmentByUserId(int id);
		Task<bool> CheckAssignmentByBranchId(int id);
		Task<Assignment_BranchCustom> Create(Assignment_BranchCustom model);
		Task<PaginationView<List<Assignment_BranchCustom>>> GetListBranch(allFilter model);
		Task Assign(AssignModel model);
		Task CreateAssignmentBranchAll(allFilter model);
	}
}

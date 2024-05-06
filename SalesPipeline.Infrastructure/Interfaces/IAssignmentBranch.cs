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
		Task<Assignment_BranchRegCustom> Create(Assignment_BranchRegCustom model);
		Task<PaginationView<List<Assignment_BranchRegCustom>>> GetListBranch(allFilter model);
		Task AutoAssignToMCenter(AssignModel model);
		Task Assign(AssignModel model);
		Task CreateAssignmentBranchAll(allFilter model);
	}
}

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
		Task<Assignment_CenterBranchCustom> GetById(Guid id);
		Task<Assignment_CenterBranchCustom> GetByUserId(int id);
		Task<Assignment_CenterBranchCustom> Create(Assignment_CenterBranchCustom model);
		Task<Assignment_CenterBranchCustom> Update(Assignment_CenterBranchCustom model);
		Task<PaginationView<List<Assignment_CenterBranchCustom>>> GetListCenter(allFilter model);
		Task Assign(AssignModel model);
		Task UpdateCurrentNumber(int id);
		Task CreateAssignmentCenterAll(allFilter model);
	}
}

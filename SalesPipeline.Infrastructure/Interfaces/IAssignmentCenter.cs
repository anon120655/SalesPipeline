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
		Task<Assignment_CenterCustom> GetById(Guid id);
		Task<Assignment_CenterCustom> GetByUserId(int id);
		Task<Assignment_CenterCustom> Create(Assignment_CenterCustom model);
		Task<Assignment_CenterCustom> Update(Assignment_CenterCustom model);
		Task DeleteByUserId(Guid id);
		Task<PaginationView<List<Assignment_CenterCustom>>> GetListAutoAssign(allFilter model);
		Task<PaginationView<List<Assignment_CenterCustom>>> GetListCenter(allFilter model);
		Task Assign(AssignModel model);
		Task AssignCenter(List<Assignment_CenterCustom> model);
		Task AssignCenterUpdateRange(List<Assignment_CenterCustom> model);
		Task UpdateCurrentNumber(int? userid = null);
		Task CreateAssignmentCenterAll(allFilter model);
	}
}
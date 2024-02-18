using SalesPipeline.Utils.Resources.Assignments;
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
		Task<AssignmentCustom> Create(AssignmentCustom model);
	}
}

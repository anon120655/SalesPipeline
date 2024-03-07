using SalesPipeline.Utils.Resources.Assignments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IReturnRepo
	{
		Task RMToMCenter(ReturnModel model);
		Task MCenterToBranch(ReturnModel model);
		Task BranchToLCenter(ReturnModel model);
	}
}

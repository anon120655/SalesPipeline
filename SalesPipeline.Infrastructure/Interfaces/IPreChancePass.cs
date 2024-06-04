using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IPreChancePass
	{
		Task<PaginationView<List<Pre_ChancePassCustom>>> GetList(allFilter model);
	}
}

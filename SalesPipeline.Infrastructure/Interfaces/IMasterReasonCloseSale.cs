using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterReasonCloseSale
	{
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Master_Reason_CloseSaleCustom>>> GetReasonCloseSale(allFilter model);
	}
}

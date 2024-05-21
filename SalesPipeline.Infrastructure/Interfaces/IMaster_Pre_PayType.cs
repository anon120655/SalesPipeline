using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMaster_Pre_PayType
	{
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Master_Pre_Interest_PayTypeCustom>>> GetList(allFilter model);
	}
}

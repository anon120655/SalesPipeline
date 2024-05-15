using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMaster_Pre_RateType
	{
		Task<Master_Pre_Interest_RateTypeCustom> Update(Master_Pre_Interest_RateTypeCustom model);
		Task<Master_Pre_Interest_RateTypeCustom> GetById(Guid id);
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Master_Pre_Interest_RateTypeCustom>>> GetList(allFilter model);
	}
}

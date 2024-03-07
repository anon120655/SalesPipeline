using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterReasonReturns
	{
		Task<Master_ReasonReturnCustom> Create(Master_ReasonReturnCustom model);
		Task<Master_ReasonReturnCustom> Update(Master_ReasonReturnCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_ReasonReturnCustom> GetById(Guid id);
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Master_ReasonReturnCustom>>> GetList(allFilter model);
	}
}

using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterSLAOperations
	{
		Task<Master_SLAOperationCustom> Create(Master_SLAOperationCustom model);
		Task<Master_SLAOperationCustom> Update(Master_SLAOperationCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_SLAOperationCustom> GetById(Guid id);
		Task<PaginationView<List<Master_SLAOperationCustom>>> GetSLAOperations(allFilter model);
	}
}

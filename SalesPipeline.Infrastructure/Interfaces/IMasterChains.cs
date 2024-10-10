using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterChains
	{
		Task<List<Master_ChainCustom>> ValidateUpload(List<Master_ChainCustom> model);
		Task<Master_ChainCustom> Create(Master_ChainCustom model);
		Task<Master_ChainCustom> Update(Master_ChainCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_ChainCustom> GetById(Guid id);
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Master_ChainCustom>>> GetList(allFilter model);
	}
}

using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterTSIC
	{
		Task<Master_TSICCustom> Create(Master_TSICCustom model);
		Task<Master_TSICCustom> Update(Master_TSICCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_TSICCustom> GetById(Guid id);
		Task<Guid?> GetIDByCode(string code);
		Task<string?> GetNameById(Guid id);
		Task<Guid?> GetIdByName(string name);
		Task<PaginationView<List<Master_TSICCustom>>> GetList(allFilter model);
	}
}

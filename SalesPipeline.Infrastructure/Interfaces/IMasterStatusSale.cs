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
	public interface IMasterStatusSale
	{
		Task<Master_StatusSaleCustom> Create(Master_StatusSaleCustom model);
		Task<Master_StatusSaleCustom> Update(Master_StatusSaleCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_StatusSaleCustom> GetById(int id);
		Task<string?> GetNameById(int id);
		Task<PaginationView<List<Master_StatusSaleCustom>>> GetList(allFilter model);
	}
}

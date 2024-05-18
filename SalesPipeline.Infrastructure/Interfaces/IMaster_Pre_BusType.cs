using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMaster_Pre_BusType
	{
		Task<Master_Pre_BusinessTypeCustom> Create(Master_Pre_BusinessTypeCustom model);
		Task<Master_Pre_BusinessTypeCustom> Update(Master_Pre_BusinessTypeCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_Pre_BusinessTypeCustom> GetById(Guid id);
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Master_Pre_BusinessTypeCustom>>> GetList(allFilter model);
	}
}

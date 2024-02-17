using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterDepCenter
	{
		//ศูนย์ธุรกิจสินเชื่อ
		Task<Master_Department_CenterCustom> Create(Master_Department_CenterCustom model);
		Task<Master_Department_CenterCustom> Update(Master_Department_CenterCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_Department_CenterCustom> GetById(Guid id);
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Master_Department_CenterCustom>>> GetList(allFilter model);
	}
}

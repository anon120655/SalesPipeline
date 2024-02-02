using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterDepLoan
	{
		//ฝ่ายธุรกิจสินเชื่อ
		Task<Master_Department_LoanCustom> Create(Master_Department_LoanCustom model);
		Task<Master_Department_LoanCustom> Update(Master_Department_LoanCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_Department_LoanCustom> GetById(Guid id);
		Task<PaginationView<List<Master_Department_LoanCustom>>> GetList(allFilter model);
	}
}

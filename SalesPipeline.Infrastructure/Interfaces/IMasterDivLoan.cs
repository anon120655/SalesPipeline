using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterDivLoan
	{
		//ฝ่ายธุรกิจสินเชื่อ
		Task<Master_Division_LoanCustom> Create(Master_Division_LoanCustom model);
		Task<Master_Division_LoanCustom> Update(Master_Division_LoanCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_Division_LoanCustom> GetById(Guid id);
		Task<PaginationView<List<Master_Division_LoanCustom>>> GetLoans(allFilter model);
	}
}

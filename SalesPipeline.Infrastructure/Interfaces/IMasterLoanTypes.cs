using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterLoanTypes
	{
		Task<Master_LoanTypeCustom> Create(Master_LoanTypeCustom model);
		Task<Master_LoanTypeCustom> Update(Master_LoanTypeCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_LoanTypeCustom> GetById(Guid id);
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Master_LoanTypeCustom>>> GetList(allFilter model);
	}
}

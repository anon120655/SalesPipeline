using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMaster_Pre_App_Loan
	{
		Task<Master_Pre_Applicant_LoanCustom> Create(Master_Pre_Applicant_LoanCustom model);
		Task<Master_Pre_Applicant_LoanCustom> Update(Master_Pre_Applicant_LoanCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_Pre_Applicant_LoanCustom> GetById(Guid id);
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Master_Pre_Applicant_LoanCustom>>> GetList(allFilter model);
	}
}

using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMaster_Pre_Loan_App
	{
		Task<Master_Pre_Loan_ApplicantCustom> Create(Master_Pre_Loan_ApplicantCustom model);
		Task<Master_Pre_Loan_ApplicantCustom> Update(Master_Pre_Loan_ApplicantCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Master_Pre_Loan_ApplicantCustom> GetById(Guid id);
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Master_Pre_Loan_ApplicantCustom>>> GetList(allFilter model);
	}
}

using SalesPipeline.Utils.Resources.Loans;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface ILoanRepo
	{
		Task<LoanCustom> Create(LoanCustom model);
		Task<LoanCustom> Update(LoanCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<LoanCustom> GetById(Guid id);
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<LoanCustom>>> GetList(allFilter model);
	}
}

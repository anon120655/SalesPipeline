using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IPreCreditScore
	{
		Task<Pre_CreditScoreCustom> Create(Pre_CreditScoreCustom model);
		Task<Pre_CreditScoreCustom> Update(Pre_CreditScoreCustom model);
		Task DeleteById(UpdateModel model);
		Task<Pre_CreditScoreCustom> GetById(Guid id);
		Task<PaginationView<List<Pre_CreditScoreCustom>>> GetList(allFilter model);
	}
}

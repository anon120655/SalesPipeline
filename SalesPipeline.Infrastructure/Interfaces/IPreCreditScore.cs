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
		Task<PaginationView<List<Pre_CreditScoreCustom>>> GetList(allFilter model);
	}
}

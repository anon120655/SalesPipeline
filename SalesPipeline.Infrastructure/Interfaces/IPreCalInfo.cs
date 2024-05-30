using SalesPipeline.Utils.Resources.Loans;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IPreCalInfo
	{
		Task<Pre_Cal_InfoCustom> Create(Pre_Cal_InfoCustom model);
		Task<Pre_Cal_InfoCustom> Update(Pre_Cal_InfoCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Pre_Cal_InfoCustom> GetById(Guid id);
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Pre_Cal_InfoCustom>>> GetList(allFilter model);
	}
}

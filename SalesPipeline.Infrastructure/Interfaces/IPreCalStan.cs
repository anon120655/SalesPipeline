using SalesPipeline.Utils.Resources.PreApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IPreCalStan
	{
		Task<Pre_Cal_Fetu_StanCustom> Create(Pre_Cal_Fetu_StanCustom model);
		Task<Pre_Cal_Fetu_StanCustom> Update(Pre_Cal_Fetu_StanCustom model);
		Task<Pre_Cal_Fetu_StanCustom> GetById(Guid id);
	}
}

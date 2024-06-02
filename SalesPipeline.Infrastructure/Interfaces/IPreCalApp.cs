using SalesPipeline.Utils.Resources.PreApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IPreCalApp
	{
		Task<Pre_Cal_Fetu_AppCustom> Create(Pre_Cal_Fetu_AppCustom model);
		Task<Pre_Cal_Fetu_AppCustom> Update(Pre_Cal_Fetu_AppCustom model);
		Task<Pre_Cal_Fetu_AppCustom> GetById(Guid id);
	}
}

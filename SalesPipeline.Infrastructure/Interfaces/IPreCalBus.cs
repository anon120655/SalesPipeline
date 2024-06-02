using SalesPipeline.Utils.Resources.PreApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IPreCalBus
	{
		Task<Pre_Cal_Fetu_BuCustom> Create(Pre_Cal_Fetu_BuCustom model);
		Task<Pre_Cal_Fetu_BuCustom> Update(Pre_Cal_Fetu_BuCustom model);
		Task<Pre_Cal_Fetu_BuCustom> GetById(Guid id);
	}
}

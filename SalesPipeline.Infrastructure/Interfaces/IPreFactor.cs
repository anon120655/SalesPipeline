using SalesPipeline.Utils.Resources.PreApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IPreFactor
	{
		Task<Pre_FactorCustom> Create(Pre_FactorCustom model);
		Task<Pre_FactorCustom> GetById(Guid id);
	}
}

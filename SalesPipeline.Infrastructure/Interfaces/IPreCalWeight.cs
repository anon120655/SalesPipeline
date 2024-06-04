using SalesPipeline.Utils.Resources.PreApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IPreCalWeight
	{
		Task Validate(List<Pre_Cal_WeightFactorCustom> model);
		Task RemoveAllPreCall(Guid id);
		Task<Pre_Cal_WeightFactorCustom> Create(Pre_Cal_WeightFactorCustom model);
		Task<Pre_Cal_WeightFactorCustom> Update(Pre_Cal_WeightFactorCustom model);
		Task<Pre_Cal_WeightFactorCustom> GetById(Guid id);
		Task<List<Pre_Cal_WeightFactorCustom>> GetAllPreCalById(Guid id);
	}
}

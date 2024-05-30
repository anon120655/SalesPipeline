
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IPreCalInfo
	{
		Task<Pre_Cal_InfoCustom> Create(Pre_Cal_InfoCustom model);
		Task<Pre_Cal_InfoCustom> Update(Pre_Cal_InfoCustom model);
		Task<Pre_Cal_InfoCustom> GetById(Guid id);
	}
}

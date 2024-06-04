
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IPreCal
	{
		Task<Pre_CalCustom> Create(Pre_CalCustom model);
		Task<Pre_CalCustom> Update(Pre_CalCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<Pre_CalCustom> GetById(Guid id);
		Task<Pre_CalCustom> GetIncludeAllById(Guid id);
		Task<Pre_CalCustom> GetCalByAppBusId(Guid appid, Guid busid);
		Task<PaginationView<List<Pre_CalCustom>>> GetList(allFilter model);
	}
}

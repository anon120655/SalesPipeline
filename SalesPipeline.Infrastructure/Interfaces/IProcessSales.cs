using SalesPipeline.Utils.Resources.ProcessSales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IProcessSales
	{
		Task<ProcessSaleCustom> GetById(Guid id);
		Task<ProcessSaleCustom> Update(ProcessSaleCustom model);
		Task<PaginationView<List<ProcessSaleCustom>>> GetProcessSales(allFilter model);
		Task<ProcessSale_ReplyCustom> CreateReply(ProcessSale_ReplyCustom model);
		Task<ProcessSale_ReplyCustom> UpdateReply(ProcessSale_ReplyCustom model);
		Task<ProcessSale_ReplyCustom> GetReplyById(Guid id);
		Task<PaginationView<List<ProcessSale_ReplyCustom>>> GetReplys(allFilter model);
	}
}

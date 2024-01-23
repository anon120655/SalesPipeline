using SalesPipeline.Utils.Resources.ProcessSales;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
    public interface IProcessSales
	{
		Task<ProcessSaleCustom> GetById(Guid id);
		Task<ProcessSaleCustom> Update(ProcessSaleCustom model);
		Task<PaginationView<List<ProcessSaleCustom>>> GetList(allFilter model);
		Task<Sale_ReplyCustom> CreateReply(Sale_ReplyCustom model);
		Task<Sale_ReplyCustom> UpdateReply(Sale_ReplyCustom model);
		Task<Sale_ReplyCustom> GetReplyById(Guid id);
		Task<PaginationView<List<Sale_ReplyCustom>>> GetListReply(allFilter model);
	}
}

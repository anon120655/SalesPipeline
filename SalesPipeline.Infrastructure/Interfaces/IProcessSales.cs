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
		Task<Sale_ContactCustom> CreateContact(Sale_ContactCustom model);
		Task<Sale_MeetCustom> CreateMeet(Sale_MeetCustom model);
		Task<Sale_DocumentCustom> CreateDocument(Sale_DocumentCustom model);
		Task<Sale_ResultCustom> CreateResult(Sale_ResultCustom model);
		Task<Sale_Close_SaleCustom> CreateCloseSale(Sale_Close_SaleCustom model);
		Task<Sale_Contact_HistoryCustom> CreateContactHistory(Sale_Contact_HistoryCustom model);
		Task<List<Sale_DocumentCustom>> GetListDocument(allFilter model);
		Task<PaginationView<List<Sale_Contact_HistoryCustom>>> GetListContactHistory(allFilter model);
		Task<List<Sale_Contact_HistoryCustom>> GetListCalendar(allFilter model);
		Task<Sale_ContactCustom> CreateContactDiscard(Sale_ContactCustom model);
	}
}

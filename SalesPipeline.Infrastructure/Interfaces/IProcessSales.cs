using SalesPipeline.Utils.Resources.Phoenixs;
using SalesPipeline.Utils.Resources.ProcessSales;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using static SalesPipeline.Utils.AppSettings;

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
		Task UpdateScheduledJob(Guid id);
		Task UpdateScheduledJobSucceed(Guid id);
		Task<Sale_ContactCustom> CreateContactDiscard(Sale_ContactCustom model);
		Task<List<Sale_PhoenixCustom>?> GetPhoenixBySaleId(Guid id);
		Task UpdatePhoenix(PhoenixModel model, List<Sale_PhoenixCustom>? phoenix);
		Task SyncPhoenixBySaleId(Guid id, List<Sale_PhoenixCustom>? phoenix);
		Task<Sale_Document_UploadCustom> CreateDocumentFile(Sale_Document_UploadCustom model);
		Task<Sale_Document_UploadCustom> UpdateDocumentFile(Sale_Document_UploadCustom model);
		Task<Sale_Document_UploadCustom> GetDocumentFileById(Guid id);
		Task<Sale_Document_UploadCustom> GetDocumentFileSaleType(Guid saleid, short type);
		Task DocumentFileById(UpdateModel model);
		Task<List<Sale_Document_UploadCustom>> GetListDocumentFile(allFilter model);
		Task DocumentFileDeleteById(UpdateModel model);
	}
}

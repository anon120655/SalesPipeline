using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface ISales
	{
		Task<SaleCustom> Create(SaleCustom model);
		Task<SaleCustom> Update(SaleCustom model);
		Task UpdateStatusOnly(Sale_StatusCustom model, SaleCustom? modelSale = null);
		Task<bool> CheckStatusById(Guid id, int statusid);
		Task<SaleCustom> GetById(Guid id); 
		Task<SaleCustom> GetByCustomerId(Guid id); 
		Task<SaleCustom> GetStatusById(Guid id);
		Task<bool> IsViewSales(Guid id, int userid);
		Task<PaginationView<List<SaleCustom>>> GetList(allFilter model);
		Task<List<Sale_StatusCustom>> GetListStatusById(Guid id);
		Task<Sale_ReturnCustom> CreateReturn(Sale_ReturnCustom model);
		Task<PaginationView<List<Sale_ReturnCustom>>> GetListReturn(allFilter model);
		Task SetIsUpdateStatusTotal(int id);
		Task<int> GetOverdueCount(allFilter model);
		Task<Sale_Status_TotalCustom> GetStatusTotalById(int id);

		//ข้อมูลผู้ติดต่อ
		Task<Sale_Contact_InfoCustom> CreateInfo(Sale_Contact_InfoCustom model);
		Task<Sale_Contact_InfoCustom> UpdateInfo(Sale_Contact_InfoCustom model);
		Task<Sale_Contact_InfoCustom> GetInfoById(Guid id);
		Task<PaginationView<List<Sale_Contact_InfoCustom>>> GetListInfo(allFilter model);

		//คู่ค้า
		Task<Sale_PartnerCustom> CreatePartner(Sale_PartnerCustom model);
		Task<Sale_PartnerCustom> UpdatePartner(Sale_PartnerCustom model);
		Task<Sale_PartnerCustom> GetPartnerById(Guid id);
		Task<PaginationView<List<Sale_PartnerCustom>>> GetListPartner(allFilter model);
		Task<List<HistoryLoanModel>> GetListHistoryLoan(allFilter model);

		Task<CustomerCustom> RePurpose(RePurposeModel model);
	}
}

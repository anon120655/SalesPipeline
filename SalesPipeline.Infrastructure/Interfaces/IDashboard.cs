using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IDashboard
	{
		//Status_Total
		Task<Dash_Status_TotalCustom> GetStatus_TotalById(int userid);
		Task UpdateStatus_TotalById(int userid);
		//มูลค่าเฉลี่ยต่อหนึ่งดีล ,ระยะเวลาเฉลี่ยที่ใช้ในการปิดการขาย ,ระยะเวลาเฉลี่ยที่ใช้ในการขายที่แพ้ให้กับคู่แข่ง
		Task<Dash_Avg_NumberCustom> GetAvgTop_NumberById(int userid);
		Task UpdateAvg_NumberById(int userid);
		Task<List<Dash_Map_ThailandCustom>> GetMap_ThailandById(int userid);
		Task UpdateMap_ThailandById(int userid);
		Task<List<Dash_PieCustom>> GetPieCloseSaleReason(int userid);
		Task<List<Dash_PieCustom>> GetPieNumberCustomer(int userid);
		Task<List<Dash_PieCustom>> GetPieLoanValue(int userid);
		Task<PaginationView<List<Sale_DurationCustom>>> GetDuration(allFilter model);
		Task UpdateDurationById(Guid saleid);
		Task UpdateActivityById(Guid saleid);
		Task<List<Dash_PieCustom>> GetGroupReasonNotLoan(int userid);
	}
}

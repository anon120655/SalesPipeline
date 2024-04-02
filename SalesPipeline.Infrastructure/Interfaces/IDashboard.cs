using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IDashboard
	{
		//Status_Total
		Task<Dash_Status_TotalCustom> GetStatus_TotalById(int userid);
		Task UpdateStatus_TotalById(int userid);
		Task<Dash_Avg_NumberCustom> GetAvg_NumberById(int userid);
		Task UpdateAvg_NumberById(int userid);
		Task<List<Dash_Map_ThailandCustom>> GetMap_ThailandById(int userid);
		Task UpdateMap_ThailandById(int userid);
		Task<List<Dash_PieCustom>> GetPieCloseSaleReason(int userid);
		Task<List<Dash_PieCustom>> GetPieNumberCustomer(int userid);
		Task<List<Dash_PieCustom>> GetPieLoanValue(int userid);
		Task UpdateDurationById(int userid);
	}
}

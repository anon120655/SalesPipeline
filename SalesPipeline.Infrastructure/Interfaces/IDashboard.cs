using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IDashboard
	{
		//Status_Total
		Task<Dash_Status_TotalCustom> GetStatus_Total(allFilter model);
		Task<Dash_Avg_NumberCustom> GetAvg_Number(allFilter model);
	}
}

using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.ViewModels
{
	public class DashboardViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public DashboardViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<Dash_Status_TotalCustom>?> GetStatus_TotalById(int userid)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Dashboard/GetStatus_TotalById?userid={userid}");
				var dataMap = JsonConvert.DeserializeObject<Dash_Status_TotalCustom>(content);
				return new ResultModel<Dash_Status_TotalCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Dash_Status_TotalCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Dash_Avg_NumberCustom>?> GetAvg_NumberById(int userid)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Dashboard/GetAvg_NumberById?userid={userid}");
				var dataMap = JsonConvert.DeserializeObject<Dash_Avg_NumberCustom>(content);
				return new ResultModel<Dash_Avg_NumberCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Dash_Avg_NumberCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}



	}
}

using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Sales;
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

		public async Task<ResultModel<Dash_Avg_NumberCustom>?> GetAvgTop_NumberById(int userid)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Dashboard/GetAvgTop_NumberById?userid={userid}");
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

		public async Task<ResultModel<List<Dash_Map_ThailandCustom>>?> GetMap_ThailandById(int userid)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Dashboard/GetMap_ThailandById?userid={userid}");
				var dataMap = JsonConvert.DeserializeObject<List<Dash_Map_ThailandCustom>>(content);
				return new ResultModel<List<Dash_Map_ThailandCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<Dash_Map_ThailandCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<Dash_PieCustom>>?> GetPieCloseSaleReason(int userid)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Dashboard/GetPieCloseSaleReason?userid={userid}");
				var dataMap = JsonConvert.DeserializeObject<List<Dash_PieCustom>>(content);
				return new ResultModel<List<Dash_PieCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<Dash_PieCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Sale_DurationCustom>>>> GetDuration(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Dashboard/GetDuration", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Sale_DurationCustom>>>(content);

				return new ResultModel<PaginationView<List<Sale_DurationCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Sale_DurationCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Sales_ActivityCustom>>>> GetActivity(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Dashboard/GetActivity", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Sales_ActivityCustom>>>(content);

				return new ResultModel<PaginationView<List<Sales_ActivityCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Sales_ActivityCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<Dash_PieCustom>>?> GetPieNumberCustomer(int userid)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Dashboard/GetPieNumberCustomer?userid={userid}");
				var dataMap = JsonConvert.DeserializeObject<List<Dash_PieCustom>>(content);
				return new ResultModel<List<Dash_PieCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<Dash_PieCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<Dash_PieCustom>>?> GetPieLoanValue(int userid)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Dashboard/GetPieLoanValue?userid={userid}");
				var dataMap = JsonConvert.DeserializeObject<List<Dash_PieCustom>>(content);
				return new ResultModel<List<Dash_PieCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<Dash_PieCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<Dash_PieCustom>>?> GetGroupReasonNotLoan(int userid)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Dashboard/GetGroupReasonNotLoan?userid={userid}");
				var dataMap = JsonConvert.DeserializeObject<List<Dash_PieCustom>>(content);
				return new ResultModel<List<Dash_PieCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<Dash_PieCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}




	}
}

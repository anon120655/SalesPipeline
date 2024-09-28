using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.ViewModels
{
	public class SalesViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public SalesViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<SaleCustom>?> GetById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Sales/GetById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<SaleCustom>(content);
				return new ResultModel<SaleCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<SaleCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<SaleCustom>?> GetByCustomerId(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Sales/GetByCustomerId?id={id}");
				var dataMap = JsonConvert.DeserializeObject<SaleCustom>(content);
				return new ResultModel<SaleCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<SaleCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> IsViewSales(Guid id, int userid)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Sales/IsViewSales?id={id}&userid={userid}");
				var dataMap = JsonConvert.DeserializeObject<bool>(content);
				return new ResultModel<bool>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<SaleCustom>>>> GetList(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Sales/GetList", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<SaleCustom>>>(content);

				return new ResultModel<PaginationView<List<SaleCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<SaleCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<Sale_StatusCustom>>?> GetListStatusById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Sales/GetListStatusById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<List<Sale_StatusCustom>>(content);
				return new ResultModel<List<Sale_StatusCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<Sale_StatusCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Boolean>> UpdateStatusOnly(Sale_StatusCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Sales/UpdateStatusOnly", dataJson, token: tokenJwt);
				
				return new ResultModel<Boolean>()
				{
					Data = true
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Boolean>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Boolean>> UpdateStatusOnlyList(List<Sale_StatusCustom> model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Sales/UpdateStatusOnlyList", dataJson, token: tokenJwt);

				return new ResultModel<Boolean>()
				{
					Data = true
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Boolean>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<int>> GetOverdueCount(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Sales/GetOverdueCount", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<int>(content);

				return new ResultModel<int>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<int>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Sale_Contact_InfoCustom>>>> GetListInfo(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Sales/GetListInfo", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Sale_Contact_InfoCustom>>>(content);

				return new ResultModel<PaginationView<List<Sale_Contact_InfoCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Sale_Contact_InfoCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Sale_PartnerCustom>>>> GetListPartner(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Sales/GetListPartner", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Sale_PartnerCustom>>>(content);

				return new ResultModel<PaginationView<List<Sale_PartnerCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Sale_PartnerCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<CustomerCustom>> RePurpose(RePurposeModel model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Sales/RePurpose", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<CustomerCustom>(content);

				return new ResultModel<CustomerCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<CustomerCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}
	}
}

using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.ManageSystems;

namespace SalesPipeline.ViewModels
{
	public class SystemViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public SystemViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<System_SignatureCustom>> CreateSignature(System_SignatureCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/System/CreateSignature", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<System_SignatureCustom>(content);
				return new ResultModel<System_SignatureCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<System_SignatureCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<System_SignatureCustom>?> GetSignatureLast()
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/System/GetSignatureLast");
				var dataMap = JsonConvert.DeserializeObject<System_SignatureCustom>(content);
				return new ResultModel<System_SignatureCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<System_SignatureCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<System_SLACustom>> CreateSLA(System_SLACustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/System/CreateSLA", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<System_SLACustom>(content);
				return new ResultModel<System_SLACustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<System_SLACustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<System_SLACustom>> UpdateSLA(System_SLACustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/System/UpdateSLA", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<System_SLACustom>(content);
				return new ResultModel<System_SLACustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<System_SLACustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> DeleteSLAById(UpdateModel model)
		{
			try
			{
				await _httpClient.DeleteAsync($"/v1/System/DeleteSLAById?{model.SetParameter(true)}");
				return new ResultModel<bool>();
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
				
		public async Task<ResultModel<System_SLACustom>?> GetSLAById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/System/GetSLAById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<System_SLACustom>(content);
				return new ResultModel<System_SLACustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<System_SLACustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<System_SLACustom>>>> GetListSLA(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/System/GetListSLA?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<System_SLACustom>>>(content);

				return new ResultModel<PaginationView<List<System_SLACustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<System_SLACustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}


	}
}

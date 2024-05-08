using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.ViewModels
{
	public class CustomerViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public CustomerViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<ResponseDefaultModel>?> VerifyByNumber(string juristicNumber)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Customer/VerifyByNumber?juristicNumber={juristicNumber}");
				var dataMap = JsonConvert.DeserializeObject<ResponseDefaultModel>(content);
				return new ResultModel<ResponseDefaultModel>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<ResponseDefaultModel>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<CustomerCustom>>> ValidateUpload(List<CustomerCustom>? model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Customer/ValidateUpload", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<List<CustomerCustom>>(content);
				return new ResultModel<List<CustomerCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<CustomerCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<CustomerCustom>> Create(CustomerCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Customer/Create", dataJson, token: tokenJwt);
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

		public async Task<ResultModel<CustomerCustom>> Update(CustomerCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/Customer/Update", dataJson, token: tokenJwt);
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

		public async Task<ResultModel<bool>?> DeleteById(UpdateModel model)
		{
			try
			{
				await _httpClient.DeleteAsync($"/v1/Customer/DeleteById?{model.SetParameter(true)}");
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

		public async Task<ResultModel<bool>?> UpdateStatusById(UpdateModel model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				await _httpClient.PutAsync($"/v1/Customer/UpdateStatusById", dataJson, token: tokenJwt);
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

		public async Task<ResultModel<CustomerCustom>?> GetById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Customer/GetById?id={id}");
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

		public async Task<ResultModel<PaginationView<List<CustomerCustom>>>> GetList(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Customer/GetList?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<CustomerCustom>>>(content);

				return new ResultModel<PaginationView<List<CustomerCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<CustomerCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

	}
}

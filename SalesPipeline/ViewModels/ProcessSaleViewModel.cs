using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.ProcessSales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.ViewModels
{
	public class ProcessSaleViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public ProcessSaleViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<ProcessSaleCustom>?> GetById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/ProcessSale/GetById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<ProcessSaleCustom>(content);
				return new ResultModel<ProcessSaleCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<ProcessSaleCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<ProcessSaleCustom>> Update(ProcessSaleCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/ProcessSale/Update", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<ProcessSaleCustom>(content);
				return new ResultModel<ProcessSaleCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<ProcessSaleCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<ProcessSaleCustom>>>> GetList(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/ProcessSale/GetList?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<ProcessSaleCustom>>>(content);

				return new ResultModel<PaginationView<List<ProcessSaleCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<ProcessSaleCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<ProcessSale_ReplyCustom>> CreateReply(ProcessSale_ReplyCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/ProcessSale/CreateReply", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<ProcessSale_ReplyCustom>(content);
				return new ResultModel<ProcessSale_ReplyCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<ProcessSale_ReplyCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<ProcessSale_ReplyCustom>> UpdateReply(ProcessSale_ReplyCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/ProcessSale/UpdateReply", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<ProcessSale_ReplyCustom>(content);
				return new ResultModel<ProcessSale_ReplyCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<ProcessSale_ReplyCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<ProcessSale_ReplyCustom>?> GetReplyById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/ProcessSale/GetReplyById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<ProcessSale_ReplyCustom>(content);
				return new ResultModel<ProcessSale_ReplyCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<ProcessSale_ReplyCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<ProcessSale_ReplyCustom>>>> GetListReply(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/ProcessSale/GetListReply?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<ProcessSale_ReplyCustom>>>(content);

				return new ResultModel<PaginationView<List<ProcessSale_ReplyCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<ProcessSale_ReplyCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}


	}
}

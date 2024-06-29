using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.ProcessSales;
using SalesPipeline.Utils.Resources.Sales;
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

		public async Task<ResultModel<Sale_ReplyCustom>> CreateReply(Sale_ReplyCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/ProcessSale/CreateReply", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Sale_ReplyCustom>(content);
				return new ResultModel<Sale_ReplyCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Sale_ReplyCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Sale_ReplyCustom>> UpdateReply(Sale_ReplyCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/ProcessSale/UpdateReply", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Sale_ReplyCustom>(content);
				return new ResultModel<Sale_ReplyCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Sale_ReplyCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Sale_ReplyCustom>?> GetReplyById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/ProcessSale/GetReplyById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<Sale_ReplyCustom>(content);
				return new ResultModel<Sale_ReplyCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Sale_ReplyCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Sale_ReplyCustom>>>> GetListReply(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/ProcessSale/GetListReply?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Sale_ReplyCustom>>>(content);

				return new ResultModel<PaginationView<List<Sale_ReplyCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Sale_ReplyCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<Sale_DocumentCustom>>> GetListDocument(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/ProcessSale/GetListDocument?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<List<Sale_DocumentCustom>>(content);

				return new ResultModel<List<Sale_DocumentCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<Sale_DocumentCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Sale_Contact_HistoryCustom>>>> GetListContactHistory(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/ProcessSale/GetListContactHistory?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Sale_Contact_HistoryCustom>>>(content);

				return new ResultModel<PaginationView<List<Sale_Contact_HistoryCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Sale_Contact_HistoryCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Sale_Document_UploadCustom>> CreateDocumentFile(Sale_Document_UploadCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/ProcessSale/CreateDocumentFile", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Sale_Document_UploadCustom>(content);
				return new ResultModel<Sale_Document_UploadCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Sale_Document_UploadCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<Sale_Document_UploadCustom>>> GetListDocumentFile(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/ProcessSale/GetListDocumentFile?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<List<Sale_Document_UploadCustom>>(content);

				return new ResultModel<List<Sale_Document_UploadCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<Sale_Document_UploadCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}


	}
}

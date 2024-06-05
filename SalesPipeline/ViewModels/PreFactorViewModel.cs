using Newtonsoft.Json;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using SalesPipeline.Helpers;
using Microsoft.Extensions.Options;

namespace SalesPipeline.ViewModels
{
	public class PreFactorViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public PreFactorViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<Pre_FactorCustom>> Create(Pre_FactorCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/PreFactor/Create", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Pre_FactorCustom>(content);
				return new ResultModel<Pre_FactorCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Pre_FactorCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Pre_FactorCustom>?> GetById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/PreFactor/GetById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<Pre_FactorCustom>(content);
				return new ResultModel<Pre_FactorCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Pre_FactorCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

	}
}

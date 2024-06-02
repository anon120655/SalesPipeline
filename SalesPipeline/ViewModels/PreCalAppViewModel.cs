using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.ViewModels
{
	public class PreCalAppViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public PreCalAppViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<Pre_Cal_Fetu_AppCustom>> Create(Pre_Cal_Fetu_AppCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/PreCalApp/Create", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Pre_Cal_Fetu_AppCustom>(content);
				return new ResultModel<Pre_Cal_Fetu_AppCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Pre_Cal_Fetu_AppCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Pre_Cal_Fetu_AppCustom>> Update(Pre_Cal_Fetu_AppCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/PreCalApp/Update", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Pre_Cal_Fetu_AppCustom>(content);
				return new ResultModel<Pre_Cal_Fetu_AppCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Pre_Cal_Fetu_AppCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Pre_Cal_Fetu_AppCustom>?> GetById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/PreCalApp/GetById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<Pre_Cal_Fetu_AppCustom>(content);
				return new ResultModel<Pre_Cal_Fetu_AppCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Pre_Cal_Fetu_AppCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

	}
}

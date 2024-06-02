using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;

namespace SalesPipeline.ViewModels
{
	public class PreCalBusViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public PreCalBusViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<Pre_Cal_Fetu_BuCustom>> Create(Pre_Cal_Fetu_BuCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/PreCalBus/Create", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Pre_Cal_Fetu_BuCustom>(content);
				return new ResultModel<Pre_Cal_Fetu_BuCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Pre_Cal_Fetu_BuCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Pre_Cal_Fetu_BuCustom>> Update(Pre_Cal_Fetu_BuCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/PreCalBus/Update", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Pre_Cal_Fetu_BuCustom>(content);
				return new ResultModel<Pre_Cal_Fetu_BuCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Pre_Cal_Fetu_BuCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Pre_Cal_Fetu_BuCustom>?> GetById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/PreCalBus/GetById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<Pre_Cal_Fetu_BuCustom>(content);
				return new ResultModel<Pre_Cal_Fetu_BuCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Pre_Cal_Fetu_BuCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

	}
}

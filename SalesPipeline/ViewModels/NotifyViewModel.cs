using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Loans;
using SalesPipeline.Utils.Resources.Notifications;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.ViewModels
{
	public class NotifyViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public NotifyViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<NotificationMobileResponse>> NotiMobile(NotificationMobile model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Notify/NotiMobile", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<NotificationMobileResponse>(content);
				return new ResultModel<NotificationMobileResponse>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<NotificationMobileResponse>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<User_Login_TokenNotiCustom>>> GetUserSendNotiById(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Notify/GetUserSendNotiById", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<List<User_Login_TokenNotiCustom>>(content);

				return new ResultModel<List<User_Login_TokenNotiCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<User_Login_TokenNotiCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}


	}
}

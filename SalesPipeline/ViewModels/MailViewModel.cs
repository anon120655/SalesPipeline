using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using static System.Net.WebRequestMethods;

namespace SalesPipeline.ViewModels
{
	public class MailViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public MailViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<Boolean>> SendMail(SendMailModel model)
		{
			try
			{
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Mail/SendMail", dataJson);
				var dataMap = JsonConvert.DeserializeObject<ResultModel<Boolean>>(content);

				return new ResultModel<Boolean>
				{
					Status = true
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

	}
}

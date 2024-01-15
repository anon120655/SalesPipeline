using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Shares;
using static System.Net.WebRequestMethods;

namespace SalesPipeline.ViewModels
{
	public class FileViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public FileViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<FormFileResponse>> Upload(FileModel model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				var contentfile = await _httpClient.PostFileAsync($"/v1/File/Upload", model, tokenJwt);
				var dataMapfile = JsonConvert.DeserializeObject<FormFileResponse>(contentfile);
				return new ResultModel<FormFileResponse>()
				{
					Data = dataMapfile
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<FormFileResponse>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

	}
}

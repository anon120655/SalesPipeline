using Microsoft.Extensions.Options;
using SalesPipeline.Helpers;
using SalesPipeline.Utils;

namespace SalesPipeline.ViewModels
{
	public class DashboardViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public DashboardViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}



	}
}

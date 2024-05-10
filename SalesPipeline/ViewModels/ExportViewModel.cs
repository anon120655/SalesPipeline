using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.ViewModels
{
	public class ExportViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public ExportViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<byte[]?>> ExcelTotalImport(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelTotalImport", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<byte[]?>> ExcelReturnedItems(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelReturnedItems", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<byte[]?>> ExcelNotAchievedTarget(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelNotAchievedTarget", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<byte[]?>> ExcelAvgDurationCloseSale(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelAvgDurationCloseSale", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<byte[]?>> ExcelAvgDurationLostSale(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelAvgDurationLostSale", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<byte[]?>> ExcelClosingSale(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelClosingSale", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<byte[]?>> ExcelTargetSales(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelTargetSales", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<byte[]?>> ExcelTopSales(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelTopSales", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<byte[]?>> ExcelLostSales(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelLostSales", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<byte[]?>> ExcelDurationOnStage(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelDurationOnStage", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<byte[]?>> ExcelAvgDealBranch(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelAvgDealBranch", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<byte[]?>> ExcelAvgSaleActCloseDeal(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelAvgSaleActCloseDeal", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<byte[]?>> ExcelAvgDeliveryDuration(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelAvgDeliveryDuration", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<byte[]?>> ExcelAvgDealRm(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelAvgDealRm", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<byte[]?>> ExcelNumCusTypeBusiness(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelNumCusTypeBusiness", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<byte[]?>> ExcelNumCusSizeBusiness(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostByteAsync($"/v1/Export/ExcelNumCusSizeBusiness", dataJson, token: tokenJwt);
				return new ResultModel<byte[]?>()
				{
					Data = content
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<byte[]?>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}


	}
}

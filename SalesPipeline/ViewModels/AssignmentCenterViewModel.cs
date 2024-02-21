using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.ViewModels
{
	public class AssignmentCenterViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public AssignmentCenterViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<AssignmentCustom>?> GetById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/AssignmentCenter/GetById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<AssignmentCustom>(content);
				return new ResultModel<AssignmentCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<AssignmentCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<AssignmentCustom>>>> GetListCenter(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/AssignmentCenter/GetListCenter", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<AssignmentCustom>>>(content);

				return new ResultModel<PaginationView<List<AssignmentCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<AssignmentCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> CreateAssignmentCenterAll()
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/AssignmentCenter/CreateAssignmentCenterAll");
				
				return new ResultModel<bool>()
				{
					Data = true
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

	}
}

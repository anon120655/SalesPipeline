using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.ViewModels
{
	public class AssignmentBranchViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public AssignmentBranchViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<PaginationView<List<Assignment_BranchRegCustom>>>> GetListBranch(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/AssignmentBranch/GetListBranch", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Assignment_BranchRegCustom>>>(content);

				return new ResultModel<PaginationView<List<Assignment_BranchRegCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Assignment_BranchRegCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Boolean>> Assign(AssignModel model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/AssignmentBranch/Assign", dataJson, token: tokenJwt);

				return new ResultModel<Boolean>()
				{
					Data = true
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

		public async Task<ResultModel<bool>?> CreateAssignmentBranchAll()
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/AssignmentBranch/CreateAssignmentBranchAll");

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

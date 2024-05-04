using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;

namespace SalesPipeline.ViewModels
{
    public class UserViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public UserViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<UserCustom>> Create(UserCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/User/Create", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<UserCustom>(content);
				return new ResultModel<UserCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<UserCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<UserCustom>> Update(UserCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/User/Update", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<UserCustom>(content);
				return new ResultModel<UserCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<UserCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> DeleteById(UpdateModel model)
		{
			try
			{
				await _httpClient.DeleteAsync($"/v1/User/DeleteById?{model.SetParameter(true)}");
				return new ResultModel<bool>();
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

		public async Task<ResultModel<bool>?> UpdateStatusById(UpdateModel model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				await _httpClient.PutAsync($"/v1/User/UpdateStatusById", dataJson, token: tokenJwt);
				return new ResultModel<bool>();
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

		public async Task<ResultModel<UserCustom>?> GetById(int id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/User/GetById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<UserCustom>(content);
				return new ResultModel<UserCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<UserCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<UserCustom>>>> GetList(UserFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/User/GetList?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<UserCustom>>>(content);

				return new ResultModel<PaginationView<List<UserCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<UserCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<User_LevelCustom>>> GetListLevel(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/User/GetListLevel?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<List<User_LevelCustom>>(content);

				return new ResultModel<List<User_LevelCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<User_LevelCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<User_RoleCustom>>>> GetListRole(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/User/GetListRole?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<User_RoleCustom>>>(content);

				return new ResultModel<PaginationView<List<User_RoleCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<User_RoleCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<User_RoleCustom>> CreateRole(User_RoleCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/User/CreateRole", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<User_RoleCustom>(content);
				return new ResultModel<User_RoleCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<User_RoleCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<User_RoleCustom>> UpdateRole(User_RoleCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/User/UpdateRole", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<User_RoleCustom>(content);
				return new ResultModel<User_RoleCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<User_RoleCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> DeleteRoleById(UpdateModel model)
		{
			try
			{
				await _httpClient.DeleteAsync($"/v1/User/DeleteRoleById?{model.SetParameter(true)}");
				return new ResultModel<bool>();
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

		public async Task<ResultModel<bool>?> UpdateIsModifyRoleById(UpdateModel model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				await _httpClient.PutAsync($"/v1/User/UpdateIsModifyRoleById", dataJson, token: tokenJwt);
				return new ResultModel<bool>();
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

		public async Task<ResultModel<User_RoleCustom>?> GetRoleById(int id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/User/GetRoleById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<User_RoleCustom>(content);
				return new ResultModel<User_RoleCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<User_RoleCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<UserCustom>>> ValidateUpload(List<UserCustom>? model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/User/ValidateUpload", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<List<UserCustom>>(content);
				return new ResultModel<List<UserCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<UserCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<UserCustom>>>> GetUserTargetList(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/User/GetUserTargetList?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<UserCustom>>>(content);

				return new ResultModel<PaginationView<List<UserCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<UserCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}


	}
}

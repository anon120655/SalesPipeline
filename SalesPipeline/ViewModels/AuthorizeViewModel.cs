using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Auths;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace SalesPipeline.ViewModels
{
	public class AuthorizeViewModel : AuthenticationStateProvider
	{
		private readonly ProtectedLocalStorage _protectedLocalStorage;
		private NavigationManager _Nav;
		private readonly HttpClient _httpClient;
		private readonly AppSettings _appSet;

		public AuthorizeViewModel(ProtectedLocalStorage protectedLocalStorage
			, HttpClient httpClient
			, IOptions<AppSettings> appset
			, NavigationManager Nav)
		{
			_protectedLocalStorage = protectedLocalStorage;
			_httpClient = httpClient;
			_appSet = appset.Value;
			_Nav = Nav;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var principal = new ClaimsPrincipal();
			try
			{
				var storedPrincipal = await _protectedLocalStorage.GetAsync<string>("identity");
				if (storedPrincipal.Success && storedPrincipal.Value != null)
				{
					var user = JsonConvert.DeserializeObject<LoginResponseModel>(storedPrincipal.Value);
					if (user != null)
					{
						var identity = CreateIdentityFromUser(user);
						principal = new(identity);
					}
				}
			}
			catch
			{
			}
			return new AuthenticationState(principal);
		}

		public async Task<String> GetAccessToken()
		{
			var access_token = String.Empty;
			try
			{
				var storedPrincipal = await _protectedLocalStorage.GetAsync<string>("identity");
				if (storedPrincipal.Success && storedPrincipal.Value != null)
				{
					var user = JsonConvert.DeserializeObject<LoginResponseModel>(storedPrincipal.Value);
					if (user != null)
					{
						if (user.access_token != null)
						{
							access_token = user.access_token;
						}
					}
				}
			}
			catch
			{
			}
			return access_token;
		}

		public async Task<ResultModel<LoginResponseModel>> LoginAsync(LoginRequestModel user)
		{
			try
			{
				var userInDatabase = await this.Authenticate(new LoginRequestModel() { Username = user.Username, Password = user.Password });
				var principal = new ClaimsPrincipal();

				if (userInDatabase.Status && userInDatabase.Data != null)
				{
					var identity = CreateIdentityFromUser(userInDatabase.Data);
					principal = new ClaimsPrincipal(identity);
					await _protectedLocalStorage.SetAsync("identity", JsonConvert.SerializeObject(userInDatabase.Data));
				}

				NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
				return userInDatabase;
			}
			catch (Exception ex)
			{
				return new ResultModel<LoginResponseModel>()
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task LogoutAsync()
		{
			await _protectedLocalStorage.DeleteAsync("identity");
			var principal = new ClaimsPrincipal();
			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
		}

		private ClaimsIdentity CreateIdentityFromUser(LoginResponseModel user)
		{
			var result = new ClaimsIdentity(new Claim[]
			{
				new ("Id", user.Id.ToString()),
				new ("FullName", user.FullName ?? string.Empty),
				new ("TitleName", user.TitleName ?? string.Empty),
				new ("FirstName", user.FirstName ?? string.Empty),
				new ("LastName", user.LastName ?? string.Empty),
				new ("access_token", user.access_token ?? string.Empty)
			}, "SalesPipelineAuthentication");

			return result;
		}

		public async Task<Boolean> IsAuth()
		{
			var authState = await GetAuthenticationStateAsync();
			var user = authState.User;
			if (user.Identity == null || !user.Identity.IsAuthenticated)
			{
				_Nav.NavigateTo("/signin", true);
				return false;
			}
			else
			{
				try
				{
					string token = await GetAccessToken();
					if (!string.IsNullOrEmpty(token))
					{
						var responseClient = await _httpClient.GetAsync($"{_appSet.baseUriApi}/v1/Authorize/ExpireToken?token={token}");
						if (responseClient.IsSuccessStatusCode && responseClient.StatusCode == System.Net.HttpStatusCode.OK)
						{
							var content = await responseClient.Content.ReadAsStringAsync();
							var dataMap = JsonConvert.DeserializeObject<Boolean>(content);
							if (dataMap != true)
							{
								_Nav.NavigateTo("/signin", true);
								return false;
							}
						}
					}
				}
				catch (Exception ex)
				{
					_Nav.NavigateTo("/signin", true);
					return false;
				}
			}

			return true;
		}

		public async Task<LoginResponseModel?> GetUserInfo()
		{
			try
			{
				var userInDatabase = new LoginResponseModel();
				var storedPrincipal = await _protectedLocalStorage.GetAsync<string>("identity");
				if (storedPrincipal.Success && storedPrincipal.Value != null)
				{
					userInDatabase = JsonConvert.DeserializeObject<LoginResponseModel>(storedPrincipal.Value);
				}

				return userInDatabase;
			}
			catch
			{
				_Nav.NavigateTo("/backoffice/signin", true);
				return null;
			}
		}

		public async Task<ResultModel<LoginResponseModel>> Authenticate(LoginRequestModel model)
		{
			try
			{
				var response = await _httpClient.PostAsJsonAsync($"{_appSet.baseUriApi}/v1/Authorize", model);
				if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
				{
					var data = JsonConvert.DeserializeObject<LoginResponseModel>(await response.Content.ReadAsStringAsync());
					if (data != null)
					{
						_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", data.access_token);
						var datauser = await _httpClient.GetAsync($"{_appSet.baseUriApi}/v1/User/GetById?id={data.Id}");
						if (datauser.StatusCode == System.Net.HttpStatusCode.OK && datauser.Content != null)
						{
							var datauserMap = JsonConvert.DeserializeObject<UserCustom>(await datauser.Content.ReadAsStringAsync());
							if (datauserMap != null)
							{
								data.Master_Department_BranchId = datauserMap.Master_Department_BranchId;
								data.ProvinceId = datauserMap.ProvinceId;
								data.ProvinceName = datauserMap.ProvinceName;
								data.BranchId = datauserMap.BranchId;
								data.BranchName = datauserMap.BranchName;
								data.PositionId = datauserMap.PositionId;
								data.RoleId = datauserMap.RoleId;

								if (datauserMap.Master_Department_Branch != null)								
									data.Master_Department_BranchName = datauserMap.Master_Department_Branch.Name;

								if (datauserMap.LevelId != null)
									data.LevelName = datauserMap.LevelId.ToString();

								if (datauserMap.Position != null)
									data.PositionName = datauserMap.Position.Name;

								if (datauserMap.Role != null)
								{
									data.RoleCode = datauserMap.Role.Code;
									data.RoleName = datauserMap.Role.Name;
								}

								var dataRole = await _httpClient.GetAsync($"{_appSet.baseUriApi}/v1/User/GetRoleById?id={datauserMap.RoleId}");
								if (dataRole.StatusCode == System.Net.HttpStatusCode.OK && dataRole.Content != null)
								{
									var dataRoleMap = JsonConvert.DeserializeObject<User_RoleCustom>(await dataRole.Content.ReadAsStringAsync());
									if (dataRoleMap != null && dataRoleMap.User_Permissions != null)
									{
										data.User_Permissions = dataRoleMap.User_Permissions.ToList();
									}
								}
							}
						}
					}

					return new ResultModel<LoginResponseModel>()
					{
						Data = data
					};
				}
				else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				{
					throw new ExceptionCustom(GeneralTxt.ExceptionTxtDefault);
				}
				else
				{
					if (response.Content != null)
					{
						throw new ExceptionCustom(GeneralUtils.MapErrorModel(await response.Content.ReadAsStringAsync()));
					}
				}

				throw new ExceptionCustom($"{response.StatusCode} กรุณาติดต่อผู้ดูแลระบบ");
			}
			catch (Exception ex)
			{
				return new ResultModel<LoginResponseModel>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

	}
}

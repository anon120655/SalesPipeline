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
using SalesPipeline.Utils.Resources.iAuthen;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using static Dapper.SqlMapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SalesPipeline.ViewModels
{
	public class AuthorizeViewModel : AuthenticationStateProvider
	{
		private readonly ProtectedLocalStorage _protectedLocalStorage;
		private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
		private readonly NavigationManager _Nav;
		private readonly HttpClient _httpClient;
		private readonly AppSettings _appSet;

		public AuthorizeViewModel(ProtectedLocalStorage protectedLocalStorage
			, HttpClient httpClient
			, IOptions<AppSettings> appset
			, NavigationManager Nav)
		{
			_protectedLocalStorage = protectedLocalStorage;
			_appSet = appset.Value;
			_Nav = Nav;

			// สร้าง HttpClientHandler ที่ข้ามการตรวจสอบ Certificate
			var handler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = (message, certificate, chain, errors) => true
			};

			// กำหนด HttpClient ที่ใช้ Handler นี้
			_httpClient = new HttpClient(handler);
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var principal = new ClaimsPrincipal();
			try
			{

				var storedPrincipal = await _protectedLocalStorage.GetAsync<string>("identity");
				var identityData = storedPrincipal.Success ? storedPrincipal.Value : null;

				if (string.IsNullOrEmpty(identityData))
				{
					return new AuthenticationState(_anonymous);
				}

				var user = JsonConvert.DeserializeObject<LoginResponseModel>(identityData);
				if (user == null) return new AuthenticationState(_anonymous);

				var identity = CreateIdentityFromUser(user);

				var identityUser = new ClaimsPrincipal(identity);

				return new AuthenticationState(identityUser);
			}
			catch (Exception ex)
			{
				//อาจจะเกิดขึ้นได้กรณี AppKey หายไปตอน restart server
				await LogoutAsync();
				return new AuthenticationState(principal);
			}
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
				var userInDatabase = await this.Authenticate(new LoginRequestModel()
				{
					Username = user.Username,
					Password = user.Password,
					IPAddress = user.IPAddress
				});
				if (!userInDatabase.Status || userInDatabase.Data == null) throw new Exception(userInDatabase.errorMessage);

				List<MenuItemCustom> MenuItem = new();
				var dataMenuItem = await _httpClient.GetAsync($"{_appSet.baseUriApi}/v1/Master/MenuItem?status=1");
				if (dataMenuItem.StatusCode == System.Net.HttpStatusCode.OK && dataMenuItem.Content != null)
				{
					var dataMenuItemMap = JsonConvert.DeserializeObject<List<MenuItemCustom>>(await dataMenuItem.Content.ReadAsStringAsync());
					if (dataMenuItemMap != null)
					{
						MenuItem = dataMenuItemMap;
					}
				}

				var identity = CreateIdentityFromUser(userInDatabase.Data);
				var principal = new ClaimsPrincipal(identity);
				await _protectedLocalStorage.SetAsync("identity", JsonConvert.SerializeObject(userInDatabase.Data));
				await _protectedLocalStorage.SetAsync("identityMenu", JsonConvert.SerializeObject(MenuItem));

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
			await _protectedLocalStorage.DeleteAsync("identityMenu");
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
				_Nav.NavigateTo($"/signin?p=isauth", true);
				return false;
			}
			else
			{
				try
				{
					var isExpire = await IsTokenExpired();
					if (isExpire)
					{
						_Nav.NavigateTo("/signin?p=token", true);
						return false;
					}
				}
				catch (Exception)
				{
					_Nav.NavigateTo("/signin?p=token", true);
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
				_Nav.NavigateTo("/signin?p=user", true);
				return null;
			}
		}

		public async Task<List<MenuItemCustom>?> GetMenuItem()
		{
			try
			{
				var data = new List<MenuItemCustom>();
				var storedPrincipal = await _protectedLocalStorage.GetAsync<string>("identityMenu");
				if (storedPrincipal.Success && storedPrincipal.Value != null)
				{
					data = JsonConvert.DeserializeObject<List<MenuItemCustom>>(storedPrincipal.Value);
				}

				return data;
			}
			catch
			{
				_Nav.NavigateTo("/signin?p=user", true);
				return null;
			}
		}

		public async Task<ResultModel<LoginResponseModel>> Authenticate(LoginRequestModel model)
		{
			try
			{
				var fullUrl = $"{_appSet.baseUriApi}/v1/Authorize";
				var response = await _httpClient.PostAsJsonAsync($"{_appSet.baseUriApi}/v1/Authorize", model);
				if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK && response.Content != null)
				{
					var data = JsonConvert.DeserializeObject<LoginResponseModel>(await response.Content.ReadAsStringAsync());
					if (data != null)
					{
						if (data.Id > 0 && !string.IsNullOrEmpty(data.access_token))
						{
							_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", data.access_token);
							var datauser = await _httpClient.GetAsync($"{_appSet.baseUriApi}/v1/User/GetById?id={data.Id}");
							if (datauser.StatusCode == System.Net.HttpStatusCode.OK && datauser.Content != null)
							{
								var datauserMap = JsonConvert.DeserializeObject<UserCustom>(await datauser.Content.ReadAsStringAsync());
								if (datauserMap != null)
								{
									data.CheckData = data.access_token;
									data.Master_Department_BranchId = datauserMap.Master_Branch_RegionId;
									data.ProvinceId = datauserMap.ProvinceId;
									data.ProvinceName = datauserMap.ProvinceName;
									data.BranchId = datauserMap.BranchId;
									data.BranchName = datauserMap.BranchName;
									data.PositionId = datauserMap.PositionId;
									data.RoleId = datauserMap.RoleId;
									data.EmployeeId = datauserMap.EmployeeId;
									data.TitleName = datauserMap.TitleName;
									data.FirstName = datauserMap.FirstName;
									data.LastName = datauserMap.LastName;
									data.LevelId = datauserMap.LevelId;

									if (datauserMap.Master_Branch_Region != null)
										data.Master_Department_BranchName = datauserMap.Master_Branch_Region.Name;

									if (datauserMap.Position != null)
										data.PositionName = datauserMap.Position.Name;

									if (datauserMap.Role != null)
									{
										data.RoleCode = datauserMap.Role.Code;
										data.RoleName = datauserMap.Role.Name;
										data.IsAssignCenter = datauserMap.Role.IsAssignCenter;
										data.IsAssignRM = datauserMap.Role.IsAssignRM;
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

									data.User_Areas = datauserMap.User_Areas;
								}
							}
						}
						else
						{
							if (data.iauthen != null)
							{
								var base64redirecturl = Convert.ToBase64String(Encoding.UTF8.GetBytes(_appSet?.baseUriWeb ?? string.Empty));
								if (data.iauthen.response_data != null)
								{
									if (!data.iauthen.response_data.password_unexpire && !string.IsNullOrEmpty(data.iauthen.response_data.change_password_url))
									{
										string? _url = $"{data.iauthen.response_data.change_password_url}&redirecturl={base64redirecturl}";
										_Nav.NavigateTo(_url, true);
									}
									else if (!data.iauthen.response_data.username_existing && !string.IsNullOrEmpty(data.iauthen.response_data.create_password_url))
									{
										string? _url = $"{data.iauthen.response_data.create_password_url}&redirecturl={base64redirecturl}";
										_Nav.NavigateTo(_url, true);
									}
									else
									{
										throw new ExceptionCustom($"เชื่อมต่อ iAuth ไม่สำเร็จ กรุณาติดต่อผู้ดูแลระบบ");
									}
								}
								else if (data.iauthen.response_status != "pass")
								{
									throw new Exception($"iAuth : {data.iauthen.response_message}");
								}
								else
								{
									throw new ExceptionCustom($"เชื่อมต่อ iAuth ไม่สำเร็จ กรุณาติดต่อผู้ดูแลระบบ");
								}
							}
							else
							{
								throw new ExceptionCustom($"เชื่อมต่อ iAuth ไม่สำเร็จ กรุณาติดต่อผู้ดูแลระบบ");
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
					throw new Exception(GeneralTxt.ExceptionTxtDefault);
				}
				else
				{
					if (response.Content != null)
					{
						throw new Exception(GeneralUtils.MapErrorModel(await response.Content.ReadAsStringAsync()));
					}
				}

				throw new Exception($"{response.StatusCode} กรุณาติดต่อผู้ดูแลระบบ");
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

		public async Task<bool> IsTokenExpired()
		{
			var token = await GetAccessToken();
			if (string.IsNullOrEmpty(token))
			{
				return true;
			}

			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token);
			var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

			if (expClaim == null)
			{
				return true;
			}

			var exp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value));
			return exp < DateTimeOffset.UtcNow;
		}

	}
}

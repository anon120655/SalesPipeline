using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Auths;
using SalesPipeline.Utils.Resources.Sales;

namespace SalesPipeline.Pages.Login
{
	public partial class SignIn
	{
		string? _errorMessage = null;
		bool isLoading = false;
		LoginRequestModel loginModel = new LoginRequestModel();

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await _authorizeViewModel.LogoutAsync();

			if (firstRender)
			{
				if (_appSet.Value.ServerSite != ServerSites.PRO)
				{
					loginModel.Username = "superadmin@gmail.com";
					loginModel.Password = "password";
					StateHasChanged();
				}
				firstRender = false;
			}
		}

		//protected override async Task OnInitializedAsync()
		//{
		//	await _authApi.LogoutAsync();
		//}

		protected async Task SubmitLogin()
		{
			var remoteIpAddress = _accessor.HttpContext?.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

			isLoading = true;
			await Task.Delay(100);
			loginModel.IPAddress = $"{remoteIpAddress}";

			var authenticate = await _authorizeViewModel.LoginAsync(loginModel);
			if (authenticate.Status)
			{
				_Navs.NavigateTo("/");
			}
			else
			{
				isLoading = false;
				_errorMessage = authenticate.errorMessage;
			}

		}

		protected void OnRememberMe(object? checkedValue)
		{
			if (checkedValue != null && (bool)checkedValue)
			{
				loginModel.IsRememberMe = true;
			}
			else
			{
				loginModel.IsRememberMe = false;
			}
		}


	}
}
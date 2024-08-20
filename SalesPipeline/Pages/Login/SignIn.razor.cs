using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Auths;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Sales;
using System.Text;

namespace SalesPipeline.Pages.Login
{
	public partial class SignIn
	{
		[Inject] protected IOptions<AppSettings> _appSet { get; set; } = null!;

		string? _errorMessage = null;
		bool isLoading = false;
		bool toggleEye = false;
		string customIconName = "fa-regular fa-eye-slash field-icon";
		LoginRequestModel loginModel = new();
		string base64redirecturl = string.Empty;

		//[CascadingParameter] protected List<System_ConfigCustom>? ItemConfig { get; set; } = default!;

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await _authorizeViewModel.LogoutAsync();

			if (firstRender)
			{
				//if (ItemConfig != null)
				//{

				//}
				//if (_appSet.Value.ServerSite != ServerSites.PRO)
				//{
				//	loginModel.Username = "superadmin@gmail.com";
				//	loginModel.Password = "password";
				//	StateHasChanged();
				//}
				firstRender = false;
			}
		}

		protected override async Task OnInitializedAsync()
		{
			base64redirecturl = Convert.ToBase64String(Encoding.UTF8.GetBytes(_appSet.Value?.baseUriWeb ?? string.Empty));
			await Task.Delay(1);
		}

		protected async Task SubmitLogin()
		{
			var remoteIpAddress = _accessor.HttpContext?.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

			isLoading = true;
			await Task.Delay(100);
			loginModel.IPAddress = $"{remoteIpAddress}";

			var authenticate = await _authorizeViewModel.LoginAsync(loginModel);
			if (authenticate.Status)
			{
				isLoading = false;
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

		protected void TogglePassword()
		{
			toggleEye = !toggleEye;
			if (toggleEye)
			{
				customIconName = "fa-regular fa-eye-slash field-icon";
			}
			else
			{
				customIconName = "fa-regular fa-eye field-icon";
			}
			StateHasChanged();
		}


	}
}
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using SalesPipeline.Utils.Resources.Authorizes.Auths;

namespace SalesPipeline.Pages.Login
{
	public partial class SignIn
	{
		[Inject] protected IHttpContextAccessor _accessor { get; set; } = null!;

		string? _errorMessage = null;
		bool isLoading = false;
		LoginRequestModel loginModel = new LoginRequestModel();

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await _authorizeViewModel.LogoutAsync();
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
			// loginModel.IPAddress = $"{remoteIpAddress}";
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


	}
}
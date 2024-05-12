
using Microsoft.AspNetCore.Http.Features;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Auths;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.ViewModels;

namespace SalesPipeline.Pages.Login
{
	public partial class ChangePassword
	{
		string? _errorMessage = null;
		bool isLoading = false;
		bool isSuccess = false;
		private Boolean? isAuthorize { get; set; }
		ChangePasswordModel changeModel = new();

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				isAuthorize = await _authorizeViewModel.IsAuth();
				if (firstRender)
				{
					if (isAuthorize == true)
					{						
						UserInfo = await _authorizeViewModel.GetUserInfo() ?? new();
					}
					else
					{
						_Navs.NavigateTo("/signin", true);
					}

					StateHasChanged();
					firstRender = false;
				}

				await Task.Delay(1);
				firstRender = false;
			}
		}

		protected async Task SubmitChange()
		{
			isLoading = true;
			await Task.Delay(100);

			changeModel.UserId = UserInfo.Id;
			var response = await _userViewModel.ChangePassword(changeModel);

			if (response.Status)
			{
				_errorMessage = null;
				isSuccess = true;
				isLoading = false;
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
			}
			else
			{
				isLoading = false;
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}

	}
}
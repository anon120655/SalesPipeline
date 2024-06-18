using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.ManageSystems
{
	public partial class SystemConfig
	{
		string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private List<System_ConfigCustom> Items = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SystemConfig) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetModel();

				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetModel()
		{
			var data = await _systemViewModel.GetConfig();
			if (data != null && data.Status && data.Data != null)
			{
				Items = data.Data;				
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task Save()
		{
			_errorMessage = null;
			ShowLoading();

			var response = await _systemViewModel.UpdateConfig(Items);
			if (response.Status)
			{
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
				HideLoading();
				await SetModel();
			}
			else
			{
				HideLoading();
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}

		protected void ShowLoading()
		{
			isLoading = true;
			StateHasChanged();
		}

		protected void HideLoading()
		{
			isLoading = false;
			StateHasChanged();
		}

	}
}
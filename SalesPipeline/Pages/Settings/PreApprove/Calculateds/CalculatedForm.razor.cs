using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.PreApprove;

namespace SalesPipeline.Pages.Settings.PreApprove.Calculateds
{
	public partial class CalculatedForm
	{
		[Parameter]
		public Guid id { get; set; }

		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private Pre_CalCustom formModel = new();
		private bool IsShowTabInfo = false;
		private bool IsShowTabStan = false;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetPreApprove) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");

				firstRender = false;
			}

		}

		protected void TabAllClosed()
		{
			IsShowTabInfo = false;
			IsShowTabStan = false;
		}

		protected void ShowTabInfo()
		{
			TabAllClosed();
			IsShowTabInfo = true;
		}

		protected void ShowTabStan()
		{
			TabAllClosed();
			IsShowTabStan = true;
		}


	}
}
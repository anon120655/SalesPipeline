using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;

namespace SalesPipeline.Pages.Settings.PreApprove.Calculateds
{
	public partial class CalculatedForm
	{
		[Parameter]
		public Guid? id { get; set; }

		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();

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

	}
}
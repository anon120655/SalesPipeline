using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Dashboards
{
	public partial class NumCusTypeBusiness
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private List<Dash_PieCustom> Items = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Dashboard) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetModel();

				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetModel()
		{
			var data = await _dashboarViewModel.GetListNumberCustomer(new() { userid = UserInfo.Id, code = Dash_PieCodeModel.NumCusTypeBusiness });
			if (data != null && data.Status && data.Data != null)
			{
				Items = data.Data;
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

	}
}